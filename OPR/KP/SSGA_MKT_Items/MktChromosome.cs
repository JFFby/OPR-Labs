using System;
using System.Linq;
using OPR.KP.MKT_Items;
using OPR.SSGA2;
using OPR.SSGA2.Extension;
using OPR.SSGA2.Interfaces;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class MktChromosome : IChromosome
    {
        public int[] EntityArgsToCode(EntityArgs args)
        {
            var config = GetConfig(args);
            return config.ToArray();
        }

        public ValidationResult CodeToEntityArgs(int[] code)
        {
            var config = MKT_Config.ParseFromArray(code);

            if (config != null)
            {
                var isValid = ValidationService.ValidateBounds(
                    config.n, RnadomMKTConfigGenerator.nBounds[0], RnadomMKTConfigGenerator.nBounds[1])
                              && ValidationService.ValidateBounds(
                                  config.Lambda, RnadomMKTConfigGenerator.nBounds[0],
                                  RnadomMKTConfigGenerator.nBounds[1])
                              && ValidationService.ValidateBounds(
                                  config.N, RnadomMKTConfigGenerator.NBounds[0], RnadomMKTConfigGenerator.NBounds[1]);
                return new ValidationResult
                {
                    Args = config,
                    IsValid = isValid
                };
            }

            return null;
        }

        public int[] Mutate(int[] code)
        {
            return new RnadomMKTConfigGenerator()
                .GenerateEntityArgs(1)
                .Cast<MKT_Config>()
                .First()
                .ToArray();
        }

        private MKT_Config GetConfig(EntityArgs args)
        {
            var config = args as MKT_Config;
            if (config == null)
            {
                throw new ArgumentException();
            }

            return config;
        }
    }
}
