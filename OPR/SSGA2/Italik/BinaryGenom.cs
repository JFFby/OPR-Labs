namespace OPR.SSGA2.Italik
{
    public sealed class BinaryGenom : Genom, Interfaces.IGenom
    {
        public BinaryGenom()
        {
            cromosomeCreator = new BinaryCromosome();
        }

        public string Code { get; set; }

        public void Initializae(EntityArgs args)
        {
            _code = cromosomeCreator.EntityArgsToCode(args);
            Code = string.Join("", _code);
        }
    }
}
