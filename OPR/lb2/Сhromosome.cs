using System;
using System.Collections.Generic;
using System.Text;

namespace OPR.lb2
{
    [Obsolete]
    public sealed class BinaryСhromosome
    {
        public static void SetUp(int maxValue = 4, float accuracy = 0.1f, byte mutationChance = 7)
        {
            var intValue = maxValue;
            IntegerPatLength = Convert.ToString(intValue, 2).Length;
            BinaryСhromosome.accuracy = accuracy;
            BinaryСhromosome.mutationChance = mutationChance;
        }

        private static int IntegerPatLength;
        private static float accuracy = 0.1f;
        private static byte mutationChance;

        private readonly byte sign;
        private readonly byte[] integer;
        private readonly byte[] fraction;
        private const byte frictionLength = 4;

        public BinaryСhromosome(float value)
        {
            this.Value = value;
            integer = new byte[IntegerPatLength];
            fraction = new byte[frictionLength];
            sign = (byte)(value > 0 ? 0 : 1);
            var stringInteget = Convert.ToString((int)Math.Abs(value), 2);
            for (int i = stringInteget.Length - 1, j = integer.Length - 1; i >= 0; --i, j--)
            {
                integer[j] = Byte.Parse(stringInteget[i].ToString());
            }

            var stringfraction = Convert.ToString(RoundUp(Math.Abs(value), 1), 2);
            for (int i = stringfraction.Length - 1, j = fraction.Length - 1; i >= 0; --i, j--)
            {
                fraction[j] = Byte.Parse(stringfraction[i].ToString());
            }

            Code = GetCode();
        }

        public BinaryСhromosome(string code)
        {
            this.Code = code;
            sign = Byte.Parse(code[0].ToString());
            integer = new byte[IntegerPatLength];
            fraction = new byte[frictionLength];
            for (int i = 1, j = 0; j < IntegerPatLength; ++i, ++j)
            {
                integer[j] = Byte.Parse(code[i].ToString());
            }

            for (int i = IntegerPatLength + 1, j = 0; i < code.Length; i++, ++j)
            {
                fraction[j] = Byte.Parse(code[i].ToString());
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
            Value = (float)Decimal.Parse(stringValue);
        }
        
        public static string Mutate(string code)
        {
            string stringResult = null;
            if (mutationChance >= RandomHelper.Random(0, 100))
            {
                var result = new char[code.Length];
                for (int i = 0; i < code.Length; i++)
                {
                    result[i] = code[i] == '1' ? '0' : '1';
                }

                stringResult = new string(result);
            }

            return stringResult;
        }

        public string Code { get; private set; }

        public float Value { get; private set; }

        private int RoundUp(float number, int digits)
        {
            var factor = (float)Convert.ToDecimal(Math.Pow(10, digits));
            var roundValue = Math.Round(number * factor) / factor;
            return (int)Math.Floor((float)(roundValue * factor - Math.Floor(roundValue) * factor));
        }

        private string GetCode()
        {
            var listCode = new List<byte>() { sign };
            listCode.AddRange(integer);
            listCode.AddRange(fraction);
            var code = new StringBuilder();
            listCode.ForEach(x => code.Append(x));
            return code.ToString();
        }
    }
}
