using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRegistrationLibrary
{
    public class EmployeeHourNormNotFulfilledException : Exception
    {
        public double HourNorm { get; }
        public double HoursEntered { get; }

        public EmployeeHourNormNotFulfilledException(double hourNorm, double hoursEntered )
        {
            HourNorm = hourNorm;
            HoursEntered = hoursEntered;
        }
    }
}
