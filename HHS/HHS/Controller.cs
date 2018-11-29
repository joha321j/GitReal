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
        private CaseRepo caseRepo = new CaseRepo();
        private EmployeeRepo employeeRepo = new EmployeeRepo(); 
        public void SendTimeSheets()
        {
           // workerBillable.SendTimeSheets();
        }

        public List<KeyValuePair<string, int>> GetCaseList()
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

        public List<KeyValuePair<string, int>> GetWorkTypeList()
        {
            return caseToUse.GetWorkTypeList();
        }
    }
}
    