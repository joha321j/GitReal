using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegistrationLibrary;


namespace HHS
{
    class Controller
    {
        /// <summary>
        /// Creates instances of all objects.
        /// </summary>
        private Employee _employee;
        private Case _caseToUse;
        private static string _connectionString = "Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30; MultipleActiveResultSets=True;";
        private readonly CaseRepo _caseRepo = new  CaseRepo(_connectionString);
        private readonly CustomerRepo _custoRepo = new CustomerRepo();
        private readonly EmployeeRepo _employeeRepo = new EmployeeRepo("Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30;"); 
        /// <summary>
        /// Calls on caseRepo SendTimeSheet method.
        /// </summary>
        internal void SendTimeSheets()
        {
            _caseRepo.SendTimeSheets(_employee, _connectionString);
        }
        /// <summary>
        /// returns a list of casename and id from caseRepo.
        /// </summary>
        /// <returns></returns>
        internal List<KeyValuePair<int, string>> GetCaseList()
        {
           return _caseRepo.GetCaseNameAndId();
        }
        /// <summary>
        /// Returns a list of employees from employeeRepo.
        /// </summary>
        /// <returns></returns>
        internal List<Employee> GetListOfUsers()
        {
            return _employeeRepo.GetEmployeeList();
        }
        /// <summary>
        /// Looks at a list of employees and picks one based on userChoice
        /// </summary>
        /// <param name="employeeList"></param>
        /// <param name="userChoice"></param>
        internal void SetEmployee(List<Employee> employeeList, int userChoice)
        {
            _employee = employeeList[userChoice - 1];
        }

        /// <summary>
        /// Finds a case in caseRepo using caseId.
        /// </summary>
        /// <param name="caseId"></param>
        internal void ChooseCase(int caseId)
        {
            _caseToUse = _caseRepo.GetCase(caseId);
        }

        /// <summary>
        /// Returns a list of worktypes for a case.
        /// </summary>
        /// <returns></returns>
        internal List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return _caseToUse.GetWorkTypeList();
        }

        /// <summary>
        /// Creates a new standardcase from caseName, custoId, addressId, gives it an id
        /// and then adds the standard worktypes.
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="custoId"></param>
        /// <param name="addressId"></param>
        internal void CreateNewStandardCase(string caseName, int custoId, int addressId)
        {
            int id = _caseRepo.CreateNewStandardCase(caseName,custoId,addressId,_connectionString);
            _caseRepo.AddStandardWorkTypes(id,_connectionString);
        }

        /// <summary>
        /// Returns list of all customers in customerRepo.
        /// </summary>
        /// <returns></returns>
        internal List<KeyValuePair<int,string>> GetAllCustomers()
        {
            return _custoRepo.GetallCustomers(_connectionString);
        }

        /// <summary>
        /// Returns string with the case name.
        /// </summary>
        /// <returns></returns>
        internal string GetCaseName()
        {
            return _caseToUse.CaseName;
        }

        /// <summary>
        /// Returns all the addresses in customerRepo for the given customerId.
        /// </summary>
        /// <param name="custoId"></param>
        /// <returns></returns>
        internal List<KeyValuePair<int, string>> GetallCustomersAddresses(int custoId)
        {
            return _custoRepo.GetallCustomersAddresses(custoId, _connectionString);
        }

        /// <summary>
        /// Inserts the given data about hours worked, which block and what worktype.
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="userInputBlock"></param>
        /// <param name="workType"></param>
        internal void EnterWorkHours(double userInput, int userInputBlock, KeyValuePair<int, string> workType)
        {
            _caseToUse.EnterWorkHours(workType, userInputBlock, userInput, _employee);
        }

        /// <summary>
        /// Returns a timesheet for case.
        /// </summary>
        /// <returns></returns>
        internal TimeSheet GetTimeSheet()
        {
            return _caseToUse.GetTimeSheet(_employee);
        }
        /// <summary>
        /// Inserts the userComment into the case.
        /// </summary>
        /// <param name="userComment"></param>
        internal void EnterWorkComment(string userComment)
        {
            _caseToUse.EnterWorkComment(userComment, _employee);
        }

        /// <summary>
        /// Returns the total hours for the employee.
        /// </summary>
        /// <returns></returns>
        internal double GetTotalHoursRegisteredForEmployee()
        {
            return _caseRepo.GetTotalHoursRegisteredForEmployee(_employee);
        }

        /// <summary>
        /// Returns a string list of timesheets that have been sent.
        /// </summary>
        /// <returns></returns>
        public List<string> GetRegisteredTimeSheets()
        {
            List<string> timeSheets = TimeSheet.GetRegisteredTimeSheets(_connectionString);

            return timeSheets;
        }
    }
}
    