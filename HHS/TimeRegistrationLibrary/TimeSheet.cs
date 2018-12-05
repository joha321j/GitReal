using System;
using System.Collections.Generic;

namespace TimeRegistrationLibrary
{
    public class TimeSheet
    {
        
        internal int EmployeeId { get; }

        internal List<KeyValuePair<int, string>> workTypes;

        public TimeSheet(int employeeId, List<KeyValuePair<int, string>> workTypes)
        {
            EmployeeId = employeeId;
            this.workTypes = workTypes;
        }

        public void EnterWorkHours(KeyValuePair<int, string> workType, double userInput)
        {
            throw new System.NotImplementedException();
        }

        public int GetBlockForWorkType(KeyValuePair<int, string> workType)
        {
            throw new System.NotImplementedException();
        }

        public object GetHoursRegisteredForWorkType(KeyValuePair<int, string> workType)
        {
            throw new NotImplementedException();
        }
    }
}