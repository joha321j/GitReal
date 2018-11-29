using System;
using System.Collections.Generic;
using System.Text;
using TimeRegistrationLibrary;

namespace TimeRegistrationLibrary
{
    public class Carpenter: Employee, IBillable
    {
        private double NumberOfWorkHours { get; }
        public Carpenter(CompanyPosition positionInCompany, int employeeId, string name, double numberOfWorkHours) : base(employeeId, name, positionInCompany)
        {
            NumberOfWorkHours = numberOfWorkHours;
        }

        public void SendTimeSheets()
        {
            
        }

    }
}
