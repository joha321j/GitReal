using System;
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

        private readonly List<KeyValuePair<int, string>> _workTypeList;
        private readonly List<TimeSheet> timeSheets;

        public Case(Address customerAddress, string customerName, string customerEmail, List<KeyValuePair<int, string>> workTypeList)
        {
            CustomerAddress = customerAddress;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            CaseName = customerName + customerEmail;
            _workTypeList = workTypeList;
        }

        public Case(Address customerAddress, string customerName, string customerEmail, string caseName, List<KeyValuePair<int, string>> workTypeList)
        {
            CaseName = caseName;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            _workTypeList = workTypeList;
        }

        public Case(int caseId, string caseName, string customerName, string customerEmail, Address customerAddress, List<KeyValuePair<int, string>> workTypeList)
        {
            CaseId = caseId;
            CaseName = caseName;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            _workTypeList = workTypeList;
        }

        public List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return _workTypeList;
        }

        public TimeSheet GetTimeSheet(Employee employee)
        {
            try
            {
                return timeSheets.Find(timeSheet => timeSheet.EmployeeId == employee.EmployeeId);
            }
            catch (ArgumentNullException)
            {
                TimeSheet newTimeSheet = new TimeSheet(employee.EmployeeId, _workTypeList);
                timeSheets.Add(newTimeSheet);
                return newTimeSheet;
            }
        }

        public void EnterWorkHours(KeyValuePair<int, string> workType, double userInput, Employee employee)
        {
            TimeSheet timeSheetForEmployee = GetTimeSheet(employee);

            timeSheetForEmployee.EnterWorkHours(workType, userInput);
        }
    }
}