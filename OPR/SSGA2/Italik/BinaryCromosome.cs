using System;
using System.Collections.Generic;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Italik
{
    public sealed class BinaryCromosome :  IChromosome
    {
        private readonly  int intValue = 4; // TODO: move this into Globals
        private readonly int integerPatLength;
        private const int frictionLength = 4;

        public BinaryCromosome()
        {
            integerPatLength = Convert.ToString(intValue, 2).Length;
        }

        public int[] EntityArgsToCode(EntityArgs args)
        {
            var binaryArgs = GetArgs(args);
            var result = new List<int>();
            result.AddRange(ConvertFloatToCodePart(binaryArgs.X));
            result.AddRange(ConvertFloatToCodePart(binaryArgs.Y));
            return result.ToArray();
        }

        public EntityArgs CodeToEntityArgs(int[] code)
        {
            //TODO: Add validation
            var oneNumberLength = code.Length/2;
            var firstPart = new int[oneNumberLength];
            Array.Copy(code,firstPart,oneNumberLength);
            var x = ConvertCodePartToFloat(firstPart);
            var secondPart = new int[oneNumberLength];
            Array.Copy(code,oneNumberLength, secondPart,0, oneNumberLength);
            var y = ConvertCodePartToFloat(secondPart);

            return new BinaryEntityArgs {X = x, Y = y};
        }

        public int[] Mutate(int[] code)
        {
            var result = new int[code.Length];
            for (int i = 0; i < code.Length; i++)
            {
                result[i] = code[i] == 1 ? 0 : 1;
            }

            return result;
        }

        private BinaryEntityArgs GetArgs(EntityArgs args)
        {
            var binaryArgs = args as BinaryEntityArgs;
            if (binaryArgs == null)
            {
                throw new System.ArgumentException();
            }

            return binaryArgs;
        }

        private float ConvertCodePartToFloat(int[] codePart)
        {
             var sign = codePart[0];
            var integer = new int[integerPatLength];
            var fraction = new int[frictionLength];
            for (int i = 1, j = 0; j < integerPatLength; ++i, ++j)
            {
                integer[j] = codePart[i];
            }

            for (int i = integerPatLength + 1, j = 0; i < codePart.Length; i++, ++j)
            {
                fraction[j] = codePart[i];
            }

            string signValue = sign > 0 ? "-" : string.Empty;
            int integerValue = 0;
            for (int i = integer.Length - 1, j = 0; i >= 0; i--, ++j)
            {
                integerValue += integer[i] * (int)Math.Pow(2, j);
            }

            int fractionValue = 0;
            for (int i = fraction.Length - 1, j = 0; i >= 0; i--, ++j)
            {
                fractionValue += fraction[i] * (int)Math.Pow(2, j);
            }

            var stringValue = string.Format("{0}{1},{2}", signValue,
                integerValue.ToString(), fractionValue.ToString());
             return (float)Decimal.Parse(stringValue);
        }

        private List<int> ConvertFloatToCodePart(float value)
        {
            var integer = new int[integerPatLength];
            var fraction = new int[frictionLength];
            var result = new List<int>();
            var sign = (int)(value > 0 ? 0 : 1);
            result.Add(sign);
            var stringInteget = Convert.ToString((int)Math.Abs(value), 2);
            for (int i = stringInteget.Length - 1, j = integer.Length - 1; i >= 0; --i, j--)
            {
                integer[j] = Byte.Parse(stringInteget[i].ToString());
            }

            result.AddRange(integer);
            var stringfraction = Convert.ToString(RoundUp(Math.Abs(value), 1), 2);
            for (int i = stringfraction.Length - 1, j = fraction.Length - 1; i >= 0; --i, j--)
            {
                fraction[j] = Byte.Parse(stringfraction[i].ToString());
            }

            result.AddRange(fraction);
            return result;
        }

        private int RoundUp(float number, int digits)
        {
            var factor = (float)Convert.ToDecimal(Math.Pow(10, digits));
            var roundValue = Math.Round(number * factor) / factor;
            return (int)Math.Floor((float)(roundValue * factor - Math.Floor(roundValue) * factor));
        }
    }
}
