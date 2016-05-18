using System;
using OPR.KP.MKT_Items;
using OPR.SSGA2;
using OPR.SSGA2.Interfaces;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class MktValueService : IValueService
    {
        public float Value(EntityArgs args)
        {
            var config = GetConfig(args);
            return new MKT(config).Solve().Value;
        }

        private MKT_Config GetConfig(EntityArgs args)
        {
            var config = args as MKT_Config;
            if (config == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return config;
        }
    }
}
