namespace Sat.Recruitment.Core.Entities
{
    public class PremiumUser : User
    {
        public override void CalculateMoney()
        {
            if (Money > 100)
            {
                Money += Money * 2;
            }
        }
    }
}
