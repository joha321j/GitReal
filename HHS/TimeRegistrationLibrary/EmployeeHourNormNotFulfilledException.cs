using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRegistrationLibrary
{
    public class EmployeeHourNormNotFulfilledException : Exception
    {
        public EmployeeHourNormNotFulfilledException(double hourNorm, double hoursEntered )
        {
            HourNorm = hourNorm;
            HoursEntered = hoursEntered;
        }

        public EmployeeHourNormNotFulfilledException(string message) : base(message)
        {
        }

        public EmployeeHourNormNotFulfilledException(string message, Exception innerException) : base(message,
            innerException)
        {
        }

        public double HourNorm { get; private set; }
        public double HoursEntered { get; private set; }
    }
}
