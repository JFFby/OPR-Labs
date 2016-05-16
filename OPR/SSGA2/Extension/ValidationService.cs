namespace OPR.SSGA2.Extension
{
    public class ValidationService
    {
        public static bool ValidateBounds(float value, float minBound, float maxBound)
        {
            return value > minBound && value < maxBound;
        }
    }
}
