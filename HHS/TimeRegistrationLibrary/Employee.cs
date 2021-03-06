﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRegistrationLibrary
{
    public enum CompanyPosition
    {
        Carpenter = 3,
        MasterCarpenter = 1,
        Secretary = 2
    }
    public class Employee
    {
        public CompanyPosition PositionInCompany { get; }
        private string Name { get; }
        internal int EmployeeId { get; }
        public double HourNorm { get; set; }

        public Employee(int employeeId, string name, CompanyPosition positionInCompany)
        {
            PositionInCompany = positionInCompany;
            EmployeeId = employeeId;
            Name = name;
        }

        public override string ToString()
        {
            return PositionInCompany +  " - " + Name;
        }
    }
}
