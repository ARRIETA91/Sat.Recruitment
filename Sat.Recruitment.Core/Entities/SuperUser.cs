using System;

namespace Sat.Recruitment.Core.Entities
{
    public class SuperUser : User
    {
        public override void CalculateMoney()
        {
            if (Money > 100)
            {
                var percentage = Convert.ToDecimal(0.20);
                Money += Money * percentage;
            }
        }
    }
}
