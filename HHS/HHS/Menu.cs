using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegistrationLibrary;

namespace HHS
{
    class Menu
    {
        readonly Controller _controller = new Controller();
        /// <summary>
        /// Show the menu and handle the user choice.
        /// </summary>
        public void Show()
        {
            bool running = true;

            ShowAndGetEmployee();

            while (running)
            {
                showMenu();
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        running = false;
                        break;
                    case "1":
                        TimeRegistration();
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.ReadLine();
                        break;
                }

            }

        }
        /// <summary>
        /// Allow user to do time registration.
        /// </summary>
        private void TimeRegistration()
        {
            List<KeyValuePair<string, int>> caseList = _controller.GetCaseList();
            ShowCaseList(caseList);
            ChooseCase(caseList);
            KeyValuePair<string, int> userChoice = ShowAndSelectWorkType();
            EnterWorkHours(userChoice);

        }

        /// <summary>
        /// Shows and allows the user to select the work type.
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<string, int> ShowAndSelectWorkType()
        {
            List<KeyValuePair<string, int>> workTypeList = _controller.GetWorkTypeList();
            ShowWorkTypes(workTypeList);
            return SelectWorkType(workTypeList);
        }

        /// <summary>
        /// Allows user to enter the hours spent on a given work type.
        /// </summary>
        /// <param name="workType"></param>
        private void EnterWorkHours(KeyValuePair<string, int> workType)
        {
            double userInput;
            bool input;
            printTimeSheet();
            Console.WriteLine("Hvor mange timer har du brugt på {0} for {1}?", workType.Key,
                _controller.GetCaseName());
            do
            {
                input = double.TryParse(Console.ReadLine(), out userInput);

            } while (!input);

            _controller.EnterWorkHours(userInput, workType);
        }

        /// <summary>
        /// Shows the time sheet for the user for week.
        /// </summary>
        private void printTimeSheet()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes the user select what work type to work with.
        /// </summary>
        /// <param name="workTypeList"></param>
        /// <returns></returns>
        private KeyValuePair<string, int> SelectWorkType(List<KeyValuePair<string, int>> workTypeList)
        {
            bool input;
            int userChoice;

            Console.WriteLine("Skriv nummeret, der står ud for den arbejdstype du ønsker og tryk Enter.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out userChoice);
                if (userChoice > workTypeList.Count && userChoice < 0)
                {
                    input = false;
                }
            } while (!input);

            return workTypeList[userChoice-1];  
        }

        /// <summary>
        /// Prints the KeyValuePair list.
        /// </summary>
        /// <param name="workTypeList"></param>
        private void ShowWorkTypes(List<KeyValuePair<string, int>> workTypeList)
        {
            int i = 1;
            

            Console.WriteLine("Tidsregistrering");

            foreach (KeyValuePair<string, int> workType in workTypeList)
            {
                Console.WriteLine(i + ". " + workType.Key);
            }

        }

        /// <summary>
        /// Choose what case you are working on.
        /// </summary>
        /// <param name="caseList"></param>
        private void ChooseCase(List<KeyValuePair<string, int>> caseList)
        {
            bool input;
            int result = 0;
            Console.WriteLine("Vælg hvilken sag, du skal tidsregistrere for.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out result);
                if (result > caseList.Count && result < 0)
                {
                    input = false;
                }

            } while (!input);

            _controller.ChooseCase(caseList[result - 1].Value);
        }

        /// <summary>
        /// Print the menu to the console.
        /// </summary>
        private void showMenu()
        {
            Console.Clear();
            Console.WriteLine("Velkommen til HHS - Håndværkernes HåndteringsSystem");
            Console.WriteLine();
            Console.WriteLine("1. Begynd timeregistrering.");
            Console.WriteLine("0. Afslut program.");
        }

        /// <summary>
        /// Shows the list of employees and makes the user choose one.
        /// </summary>
        private void ShowAndGetEmployee()
        {
            List<Employee> employeeListToChooseFrom = ShowAndGetListOfUsers();
            GetEmployee(employeeListToChooseFrom);
        }

        /// <summary>
        /// Makes user choose the employee.
        /// </summary>
        /// <param name="employeeList"></param>
        private void GetEmployee(List<Employee> employeeList)
        {
            bool input;
            int userChoice;

            Console.WriteLine("Skriv nummeret, der står ud for dit navn og tryk Enter.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out userChoice);
                if (userChoice > employeeList.Count && userChoice < 0)
                {
                    input = false;
                }

            } while (!input);

            _controller.SetEmployee(employeeList, userChoice);
        }

        /// <summary>
        /// Gets the list of all users and shows them.
        /// </summary>
        /// <returns></returns>
        private List<Employee> ShowAndGetListOfUsers()
        {
            Console.WriteLine("Velkommen til HHS - Håndværkernes HåndteringsSystem");
            Console.WriteLine("Vælg hvem du er fra denne liste:");

            List<Employee> employeeListToShow = _controller.GetListOfUsers();
            employeeListToShow.OrderBy(e => (int) e.PositionInCompany);
            int i = 1;
            foreach (Employee employee in employeeListToShow)
            {
                Console.WriteLine(i + ". " + employee);
                i++;
            }
            return employeeListToShow;
        }

        /// <summary>
        /// Shows all the cases in the given list.
        /// </summary>
        /// <param name="caseList"></param>
        private void ShowCaseList(List<KeyValuePair<string, int>> caseList)
        {
            foreach (KeyValuePair<string, int> nameIdPair in caseList)
            {
                Console.WriteLine(nameIdPair.Key);
            }
        }
        private void SendTimeSheets()
        {
            _controller.SendTimeSheets();
        }
    }
}
