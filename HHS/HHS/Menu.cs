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
        Controller controller = new Controller();

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

        private void TimeRegistration()
        {
            List<KeyValuePair<string, int>> caseList = controller.GetCaseList();
            ShowCaseList(caseList);
            ChooseCase(caseList);
            SelectWorkType();

        }

        private void SelectWorkType()
        {
            ShowWorkTypes();
        }

        private void ShowWorkTypes()
        {
            int i = 1;
            List<KeyValuePair<string, int>> workTypeList = controller.GetWorkTypeList();

            Console.WriteLine("Tidsregistrering");

            foreach (KeyValuePair<string, int> workType in workTypeList)
            {
                Console.WriteLine(i + ". " + workType.Key);
            }

        }

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

            controller.ChooseCase(caseList[result - 1].Value);
        }

        private void showMenu()
        {
            Console.Clear();
            Console.WriteLine("Velkommen til HHS - Håndværkernes HåndteringsSystem");
            Console.WriteLine();
            Console.WriteLine("1. Begynd timeregistrering.");
            Console.WriteLine("0. Afslut program.");
        }

        private void ShowAndGetEmployee()
        {
            List<Employee> employeeListToChooseFrom = ShowAndGetListOfUsers();
            GetEmployee(employeeListToChooseFrom);
        }

        private void GetEmployee(List<Employee> employeeList)
        {
            bool input;
            int userChoice = 0;

            Console.WriteLine("Skriv nummeret, der står ud for dit navn og tryk Enter.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out userChoice);
                if (userChoice > employeeList.Count && userChoice < 0)
                {
                    input = false;
                }

            } while (!input);

            controller.SetEmployee(employeeList, userChoice);
        }

        private List<Employee> ShowAndGetListOfUsers()
        {
            Console.WriteLine("Velkommen til HHS - Håndværkernes HåndteringsSystem");
            Console.WriteLine("Vælg hvem du er fra denne liste:");

            List<Employee> employeeListToShow = controller.GetListOfUsers();
            employeeListToShow.OrderBy(e => (int) e.PositionInCompany);
            int i = 1;
            foreach (Employee employee in employeeListToShow)
            {
                Console.WriteLine(i + ". " + employee);
                i++;
            }
            return employeeListToShow;
        }

        private void ShowCaseList(List<KeyValuePair<string, int>> caseList)
        {
            foreach (KeyValuePair<string, int> nameIdPair in caseList)
            {
                Console.WriteLine(nameIdPair.Key);
            }
        }
        private void SendTimeSheets()
        {
            controller.SendTimeSheets();
        }
    }
}
