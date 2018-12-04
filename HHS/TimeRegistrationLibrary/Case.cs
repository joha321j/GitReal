using System.Collections.Generic;

namespace TimeRegistrationLibrary
{
    public  class Case
    {
        public int CaseId { get; set; }
        public string CaseName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public Address CustomerAddress { get; }

        private readonly List<KeyValuePair<string, int>> _workTypeList;
        private readonly List<TimeSheet> timeSheets;

        public Case(Address customerAddress, string customerName, string customerEmail, List<KeyValuePair<string, int>> workTypeList)
        {
            CustomerAddress = customerAddress;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            CaseName = customerName + customerEmail;
            _workTypeList = workTypeList;
        }

        public Case(Address customerAddress, string customerName, string customerEmail, string caseName, List<KeyValuePair<string, int>> workTypeList)
        {
            CaseName = caseName;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            _workTypeList = workTypeList;
        }

        public List<KeyValuePair<string, int>> GetWorkTypeList()
        {
            return _workTypeList;
        }

        public void EnterWorkHours(KeyValuePair<string, int> workType, double userInput, Employee employee)
        {
            TimeSheet timeSheetForEmployee = FindTimeSheetForEmployee(employee);

            timeSheetForEmployee.EnterWorkHours(workType, userInput);
        }

        private TimeSheet FindTimeSheetForEmployee(Employee employee)
        {
            return timeSheets.Find(t => t.EmployeeId == employee.EmployeeId);
        }
    }
}