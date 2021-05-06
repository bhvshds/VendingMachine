using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrisdrankAutomaat
{
    public class NotEnougMoneyException : Exception
    {
        private double budget;

        public NotEnougMoneyException(double budget)
        {
            this.budget = budget;
        }

        public double Budget { get=> budget;  }
    }
}
