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
        private readonly List<TimeSheet> _timeSheets = new List<TimeSheet>();


        //public Case(Address customerAddress, string customerName, string customerEmail, List<KeyValuePair<int, string>> workTypeList)
        //{
        //    CustomerAddress = customerAddress;
        //    CustomerEmail = customerEmail;
        //    CustomerName = customerName;
        //    CaseName = customerName + customerEmail;
        //    _workTypeList = workTypeList;
        //}

        //public Case(Address customerAddress, string customerName, string customerEmail, string caseName, List<KeyValuePair<int, string>> workTypeList)
        //{
        //    CaseName = caseName;
        //    CustomerName = customerName;
        //    CustomerEmail = customerEmail;
        //    CustomerAddress = customerAddress;
        //    _workTypeList = workTypeList;
        //}

        /// <summary>
        /// Constructs a case with given parametres.
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="caseName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerAddress"></param>
        /// <param name="workTypeList"></param>
        public Case(int caseId, string caseName, string customerName, string customerEmail, Address customerAddress, List<KeyValuePair<int, string>> workTypeList)
        {
            CaseId = caseId;
            CaseName = caseName;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            _workTypeList = workTypeList;
        }

        /// <summary>
        /// Returns a lost of worktypes.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return _workTypeList;
        }

        /// <summary>
        /// Gets the timesheet for the employee, creates and adds one if it doesn't exists.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public TimeSheet GetTimeSheet(Employee employee)
        {
            TimeSheet timeSheetToReturn = _timeSheets.Find(timeSheet => timeSheet.EmployeeId == employee.EmployeeId);
            if (timeSheetToReturn == null)
            {
                timeSheetToReturn = new TimeSheet(CaseId, employee.EmployeeId, _workTypeList);
                _timeSheets.Add(timeSheetToReturn);
            }
            return timeSheetToReturn;
        }

        /// <summary>
        /// Adds the userComment to the timesheet.
        /// </summary>
        /// <param name="userComment"></param>
        /// <param name="employee"></param>
        public void EnterWorkComment(string userComment, Employee employee)
        {
            TimeSheet timeSheetForEmployee = GetTimeSheet(employee);
            timeSheetForEmployee.Comment = userComment;
        }

        /// <summary>
        /// Finds a timesheet based on employee and adds entered workhours.
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="userInputBlock"></param>
        /// <param name="userInput"></param>
        /// <param name="employee"></param>
        public void EnterWorkHours(KeyValuePair<int, string> workType, int userInputBlock, double userInput, Employee employee)
        {
            TimeSheet timeSheetForEmployee = GetTimeSheet(employee);

            timeSheetForEmployee.EnterWorkHours(workType, userInput, userInputBlock);
        }
    }
}