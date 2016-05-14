﻿namespace OPR.SSGA2.Interfaces
{
    public interface IChromosome
    {
        int[] EntityArgsToCode(EntityArgs args);

        EntityArgs CodeToEntityArgs(int[] cdoe);

        int[] Mutate(int[] code);
    }
}
