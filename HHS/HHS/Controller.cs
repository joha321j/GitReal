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
        private Employee employee;
        private Case caseToUse;
        private CaseRepo caseRepo = new CaseRepo("Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30;");
        private EmployeeRepo employeeRepo = new EmployeeRepo("Server=EALSQL1.eal.local; Database = B_DB30_2018; User Id = B_STUDENT30; Password = B_OPENDB30;"); 
        public void SendTimeSheets()
        {
           // workerBillable.SendTimeSheets();
        }

        public List<KeyValuePair<int, string>> GetCaseList()
        {
           return caseRepo.GetCaseNameAndId();
        }

        public List<Employee> GetListOfUsers()
        {
            return employeeRepo.GetEmployeeList();
        }

        public void SetEmployee(List<Employee> employeeList, int userChoice)
        {
            employee = employeeList[userChoice - 1];
        }

        public void ChooseCase(int caseId)
        {
            caseToUse = caseRepo.GetCase(caseId);
        }

        public List<KeyValuePair<int, string>> GetWorkTypeList()
        {
            return caseToUse.GetWorkTypeList();
        }

        public object GetCaseName()
        {
            return caseToUse.CaseName;
        }

        public void EnterWorkHours(double userInput, KeyValuePair<int, string> workType)
        {
            caseToUse.EnterWorkHours(workType, userInput, employee);
        }
    }
}
    