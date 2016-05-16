using System;
using OPR.SSGA2.Interfaces;
using OPR.SSGA2.Italik;

namespace OPR.SSGA2.Extension
{
    public static class BinaryEntityExtenstion
    {
        public static float X<TValueService, TGenom>(this Entity<TValueService, TGenom> entity)
            where TValueService : IValueService, new()
        where TGenom : IGenom, new()
        {
            return GetBunatyArgs(entity.Args).X;
        }

        public static float Y<TValueService, TGenom>(this Entity<TValueService, TGenom> entity)
            where TValueService : IValueService, new()
        where TGenom : IGenom, new()
        {
            return GetBunatyArgs(entity.Args).Y;
        }

        private static BinaryEntityArgs GetBunatyArgs(EntityArgs args)
        {
            var binatyArgs = args as BinaryEntityArgs;
            if (binatyArgs == null)
            {
                throw new ArgumentException();
            }

            return binatyArgs;
        }
    }
}
