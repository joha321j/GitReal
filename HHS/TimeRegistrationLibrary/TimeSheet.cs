using System.Collections.Generic;

namespace TimeRegistrationLibrary
{
    internal class TimeSheet
    {
        
        internal int EmployeeId { get; }

        internal List<KeyValuePair<string, int>> workTypes;

        public TimeSheet(int employeeId, List<KeyValuePair<string, int>> workTypes)
        {
            EmployeeId = employeeId;
            this.workTypes = workTypes;
        }

        public void EnterWorkHours(KeyValuePair<string, int> workType, double userInput)
        {
            throw new System.NotImplementedException();
        }
    }
}