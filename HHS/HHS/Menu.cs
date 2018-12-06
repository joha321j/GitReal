using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegistrationLibrary;
using System.Globalization;

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
            bool timeRegistrationRunning = true;
            while (timeRegistrationRunning)
            {
                List<KeyValuePair<int, string>> caseList = _controller.GetCaseList();
                ShowCaseList(caseList);
                timeRegistrationRunning = ChooseCase(caseList);
                if (timeRegistrationRunning)
                {
                    bool keepRunning;
                    do
                    {
                        KeyValuePair<int, string> userChoice = ShowAndSelectWorkType(out keepRunning);
                        if (userChoice.Key != 0)
                        {
                            EnterWorkHours(userChoice);
                        }


                    } while (keepRunning);
                }
            }
        }

        /// <summary>
        /// Shows and allows the user to select the work type.
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<int, string> ShowAndSelectWorkType(out bool keepRunning)
        {
            List<KeyValuePair<int, string>> workTypeList = _controller.GetWorkTypeList();
            ShowWorkTypes(workTypeList);
            return SelectWorkType(workTypeList, out keepRunning);
        }

        /// <summary>
        /// Allows user to enter the hours spent on a given work type.
        /// </summary>
        /// <param name="workType"></param>
        private void EnterWorkHours(KeyValuePair<int, string> workType)
        {
            double userInput;
            bool input;
            Console.WriteLine("Hvor mange timer har du brugt på {0} for {1}?", workType.Value,
                _controller.GetCaseName());
            do
            {
                input = double.TryParse(Console.ReadLine(), out userInput);

            } while (!input);

            _controller.EnterWorkHours(userInput, workType);
        }

        /// <summary>
        /// Makes the user select what work type to work with.
        /// </summary>
        /// <param name="workTypeList"></param>
        /// <returns></returns>
        private KeyValuePair<int, string> SelectWorkType(List<KeyValuePair<int, string>> workTypeList, out bool keepRunning)
        {
            bool input;
            int userChoice;
            keepRunning = true;

            Console.WriteLine("Skriv nummeret, der står ud for den arbejdstype du ønsker og tryk Enter.");
            Console.WriteLine("Tryk 0 for at afslutte og vende tilbage til sagsoversigten.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out userChoice);
                if (userChoice > workTypeList.Count)
                {
                    input = false;
                }
                else if (userChoice == 0)
                {
                    keepRunning = false;
                    return new KeyValuePair<int, string>(0,string.Empty);
                }
            } while (!input);

            return workTypeList[userChoice-1];  
        }

        /// <summary>
        /// Prints the KeyValuePair list.
        /// </summary>
        /// <param name="workTypeList"></param>
        private void ShowWorkTypes(List<KeyValuePair<int, string>> workTypeList)
        {
            Console.Clear();
            PrintTop();
            PrintDateAndWeek();
            PrintCaseName();
            PrintWorkTypes();
            PrintCommentArea();
            PrintTotalHours();

        }

        private void PrintTotalHours()
        {
            string bar = @"+---------------------+-------------------------+";
            string totalHoursString = @"| Samlet antal timer: |                       ";
            double totalHours = _controller.GetTimeSheet().GetTotalHours();

            Console.WriteLine(bar);
            Console.WriteLine("{0}{1,-2}|", totalHoursString, totalHours);
            Console.WriteLine(bar);

        }

        private void PrintCommentArea()
        {
            TimeSheet timeSheet = _controller.GetTimeSheet();
            string commentTitle = @"|Kommentar:|                                    |";
            string commentBoxBar = @"+----------+                                    |";
            string commentBox = @"|                                               |";

            Console.WriteLine(commentTitle);
            Console.WriteLine(commentBoxBar);
            string comment = timeSheet.GetComment();
            if (Equals(comment, null))
            {
                Console.WriteLine(commentBox);
            }
            else
            {
                List<string> commentList = EnsureCommentWrapping(comment);

                foreach (string line in commentList)
                {
                    Console.WriteLine("|{0, -47}|", line);
                }
            }
        }

        private List<string> EnsureCommentWrapping(string comment)
        {
            string[] words = comment.Split(' ');
            List<string> lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
            {
                if (l.Last().Length + w.Length >= 47)
                    l.Add(w);
                else
                    l[l.Count - 1] += " " + w;
                return l;
            });

            return lines;
        }

        private void PrintWorkTypes()
        {
            TimeSheet timeSheet = _controller.GetTimeSheet();
            int i = 1;
            string barTitles = @"|          Entrepriser           | Blok | Timer |";
            string bar = @"+--------------------------------+------+-------+";
            List<KeyValuePair<int, string>> workTypeList = _controller.GetWorkTypeList();
            Console.WriteLine(barTitles);
            Console.WriteLine(bar);

            foreach (KeyValuePair<int, string> workType in workTypeList)
            {
                string workTypeString = EnsureWorkTypeLength(workType);
                Console.WriteLine("|{0}.{1}|    {2, -2}|     {3, -2}|", i, workTypeString, 
                    timeSheet.GetBlockForWorkType(workType), timeSheet.GetHoursRegisteredForWorkType(workType));
                Console.WriteLine(bar);
                i++;

            }
        }

        private string EnsureWorkTypeLength(KeyValuePair<int, string> workType)
        {
            string result;
            result = " " + workType.Value;
            for (int i = 0; i < 29 - workType.Value.Length; i++)
            {
                result = result + " ";
            }
            return result;
        }

        private void PrintCaseName()
        {
            string bar = @"+--------------------------------+------+-------+";
            string caseName = _controller.GetCaseName();
            caseName = EnsureNameLength(caseName);
            string caseNameBar = @"|Sag/Kunde:";

            Console.WriteLine(caseNameBar + caseName + "|");
            Console.WriteLine(bar);
        }

        private string EnsureNameLength(string caseName)
        {
            string result = string.Empty;
            for (int i = 0; i < 36 - caseName.Length; i++)
            {
                result = result + " ";
            }
            result += caseName + " ";
            return result;
        }

        private void PrintDateAndWeek()
        {
            DateTime today = DateTime.Now;
            int weekNumber = GetIso8601WeekOfYear(today);
            string weekNumberString = checkWeekNumberLength(weekNumber);
            string bar = @"+-----------------------+-----------------------+";
            string date = @"|Dato:       " + today.ToString("dd/MM/yyyy ");
            string week = @"|Uge:                " + weekNumberString +" |";

            Console.WriteLine(bar);
            Console.WriteLine(date + week);
            Console.WriteLine(bar);
        }

        private string checkWeekNumberLength(int weekNumber)
        {
            if (weekNumber < 10)
            {
                return " " + weekNumber;
            }
            else
            {
                return weekNumber.ToString();
            }
        }

        private static int GetIso8601WeekOfYear(DateTime today)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(today);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                today = today.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private void PrintTop()
        {
            string title = @"  _______  _      _                         _       _                     _               
 |__   __|(_)    | |                       (_)     | |                   (_)              
    | |    _   __| | ___  _ __  ___   __ _  _  ___ | |_  _ __  ___  _ __  _  _ __    __ _ 
    | |   | | / _` |/ __|| '__|/ _ \ / _` || |/ __|| __|| '__|/ _ \| '__|| || '_ \  / _` |
    | |   | || (_| |\__ \| |  |  __/| (_| || |\__ \| |_ | |  |  __/| |   | || | | || (_| |
    |_|   |_| \__,_||___/|_|   \___| \__, ||_||___/ \__||_|   \___||_|   |_||_| |_| \__, |
                                      __/ |                                          __/ |
                                     |___/                                          |___/ ";
            Console.WriteLine(title);
        }

        /// <summary>
        /// Choose what case you are working on.
        /// </summary>
        /// <param name="caseList"></param>
        private bool ChooseCase(List<KeyValuePair<int, string>> caseList)
        {
            bool input;
            int result = 0;
            Console.WriteLine("Vælg hvilken sag, du skal tidsregistrere for.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out result);
                if (result > caseList.Count || result < 0)
                {
                    input = false;
                }

            } while (!input);
            if (result == 0)
            {
                return false;
            }
            else
            {
                _controller.ChooseCase(caseList[result - 1].Key);
                return true;
            }
        }

        /// <summary>
        /// Print the menu to the console.
        /// </summary>
        private void showMenu()
        {
            Console.Clear();
            Console.WriteLine("Velkommen til HHS - Håndværkernes HåndteringsSystem");
            Console.WriteLine();
            Console.WriteLine("Hovedmenu");
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
                if (userChoice > employeeList.Count || userChoice < 1)
                {
                    input = false;
                    Console.WriteLine("Ugyldigt valg.");
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
            Console.WriteLine();
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
        private void ShowCaseList(List<KeyValuePair<int, string>> caseList)
        {
            Console.Clear();
            Console.WriteLine("Liste af oprettede sager");
            foreach (KeyValuePair<int, string> nameIdPair in caseList)
            {
                Console.WriteLine("{0}. " + nameIdPair.Value, nameIdPair.Key);
            }
            Console.WriteLine("0. Afslut timeregistering og gå tilbage til hovedmenu");
        }
        private void SendTimeSheets()
        {
            _controller.SendTimeSheets();
        }
    }
}
