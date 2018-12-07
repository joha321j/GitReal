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
        private CaseRepo _caseRepo = new CaseRepo(_connectionString);
        private EmployeeRepo _employeeRepo = new EmployeeRepo("Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30;"); 
        public void SendTimeSheets()
        {
            _caseRepo.SendTimeSheets(_employee, _connectionString);
        }

        public List<KeyValuePair<int, string>> GetCaseList()
        {
           return _caseRepo.GetCaseNameAndId();
        }

        public List<Employee> GetListOfUsers()
        {
            return _employeeRepo.GetEmployeeList();
        }

        public void SetEmployee(List<Employee> employeeList, int userChoice)
        {
            _employee = employeeList[userChoice - 1];
        }

        public void ChooseCase(int caseId)
        {
            _caseToUse = _caseRepo.GetCase(caseId);
        }

        public List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return _caseToUse.GetWorkTypeList();
        }

        internal void CreateNewStandardCase(string caseName, int custoId, int addressId)
        {
            int id = _caseRepo.CreateNewStandardCase(caseName,custoId,addressId,_connectionString);
            _caseRepo.AddStandardWorkTypes(id,_connectionString);
        }

        public string GetCaseName()
        {
            return _caseToUse.CaseName;
        }

        public void EnterWorkHours(double userInput, KeyValuePair<int, string> workType)
        {
            _caseToUse.EnterWorkHours(workType, userInput, _employee);
        }

        internal TimeSheet GetTimeSheet()
        {
            return _caseToUse.GetTimeSheet(_employee);
        }

        internal void EnterWorkComment(string userComment)
        {
            _caseToUse.EnterWorkComment(userComment, _employee);
        }
    }
}
    