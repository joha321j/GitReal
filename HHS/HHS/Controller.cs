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
        private Employee _employee;
        private Case _caseToUse;
        private static string _connectionString = "Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30; MultipleActiveResultSets=True;";
        private readonly CaseRepo _caseRepo = new CaseRepo(_connectionString);
        private readonly CustomerRepo _custoRepo = new CustomerRepo();
        private readonly EmployeeRepo _employeeRepo = new EmployeeRepo("Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30;"); 
        internal void SendTimeSheets()
        {
            _caseRepo.SendTimeSheets(_employee, _connectionString);
        }

        internal List<KeyValuePair<int, string>> GetCaseList()
        {
           return _caseRepo.GetCaseNameAndId();
        }

        internal List<Employee> GetListOfUsers()
        {
            return _employeeRepo.GetEmployeeList();
        }

        internal void SetEmployee(List<Employee> employeeList, int userChoice)
        {
            _employee = employeeList[userChoice - 1];
        }

        internal void ChooseCase(int caseId)
        {
            _caseToUse = _caseRepo.GetCase(caseId);
        }

        internal List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return _caseToUse.GetWorkTypeList();
        }

        internal void CreateNewStandardCase(string caseName, int custoId, int addressId)
        {
            int id = _caseRepo.CreateNewStandardCase(caseName,custoId,addressId,_connectionString);
            _caseRepo.AddStandardWorkTypes(id,_connectionString);
        }

        internal List<KeyValuePair<int,string>> GetAllCustomers()
        {
            return _custoRepo.GetallCustomers(_connectionString);
        }

        internal string GetCaseName()
        {
            return _caseToUse.CaseName;
        }

        internal List<KeyValuePair<int, string>> GetallCustomersAddresses(int custoId)
        {
            return _custoRepo.GetallCustomersAddresses(custoId, _connectionString);
        }

        internal void EnterWorkHours(double userInput, int userInputBlock, KeyValuePair<int, string> workType)
        {
            _caseToUse.EnterWorkHours(workType, userInputBlock, userInput, _employee);
        }

        internal TimeSheet GetTimeSheet()
        {
            return _caseToUse.GetTimeSheet(_employee);
        }

        internal void EnterWorkComment(string userComment)
        {
            _caseToUse.EnterWorkComment(userComment, _employee);
        }

        internal double GetTotalHoursRegisteredForEmployee()
        {
            return _caseRepo.GetTotalHoursRegisteredForEmployee(_employee);
        }

        public List<string> GetRegisteredTimeSheets()
        {
            List<string> timeSheets = TimeSheet.GetRegisteredTimeSheets(_connectionString);

            return timeSheets;
        }
    }
}
    