using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegistrationLibrary;

namespace HHS
{
    internal class TimeRegistrationMenu: IMenu
    {
        private readonly Controller _controller;

        public TimeRegistrationMenu(Controller controller)
        {
            _controller = controller;
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
        /// Shows all the cases in the given list.
        /// </summary>
        /// <param name="caseList"></param>
        private void ShowCaseList(List<KeyValuePair<int, string>> caseList)
        {
            Console.Clear();
            Console.WriteLine("Liste af oprettede sager");
            Console.WriteLine();
            Console.WriteLine("Vælg hvilken sag, du skal tidsregistrere for:");

            foreach (KeyValuePair<int, string> nameIdPair in caseList)
            {
                Console.WriteLine("{0}. " + nameIdPair.Value, nameIdPair.Key);
            }
            Console.WriteLine();
            Console.WriteLine("0. Afslut timeregistering og gå tilbage til hovedmenu");
        }

        /// <summary>
        /// Choose what case you are working on.
        /// </summary>
        /// <param name="caseList"></param>
        private bool ChooseCase(List<KeyValuePair<int, string>> caseList)
        {
            bool input;
            int result;

            do
            {
                input = int.TryParse(Console.ReadLine(), out result);
                if (result > caseList.Count || result < 0 || input == false)
                {
                    Console.WriteLine("Ugyldigt valg.");
                    input = false;
                }

            } while (!input);
            if (result == 0)
            {
                return false;
            }
            _controller.ChooseCase(caseList[result - 1].Key);
            return true;
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

            Console.WriteLine();
            Console.WriteLine("Skriv nummeret, der står ud for den arbejdstype du ønsker og tryk Enter.");
            Console.WriteLine("Tryk 0 for at afslutte og vende tilbage til sagsoversigten.");

            do
            {
                input = int.TryParse(Console.ReadLine(), out userChoice);
                if (userChoice > workTypeList.Count || input == false)
                {
                    Console.WriteLine("Ugyldigt valg.");
                    input = false;
                }
                else if (userChoice == 0)
                {
                    keepRunning = false;
                    return new KeyValuePair<int, string>(0, string.Empty);
                }
            } while (!input);

            return workTypeList[userChoice - 1];
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
            if (workType.Key == 1)
            {
                Console.WriteLine("Tilføj kommentar til, hvad du har lavet.");
                string userComment = Console.ReadLine();
                _controller.EnterWorkComment(userComment);
            }
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

        private void PrintDateAndWeek()
        {
            DateTime today = DateTime.Now;
            int weekNumber = GetIso8601WeekOfYear(today);
            string weekNumberString = checkWeekNumberLength(weekNumber);
            string bar = @"+-----------------------+-----------------------+";
            string date = @"|Dato:       " + today.ToString("dd/MM/yyyy ");
            string week = @"|Uge:                " + weekNumberString + " |";

            Console.WriteLine(bar);
            Console.WriteLine(date + week);
            Console.WriteLine(bar);
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

        private void PrintTotalHours()
        {
            string bar = @"+---------------------+-------------------------+";
            string totalHoursString = @"| Samlet antal timer: |                       ";
            double totalHours = _controller.GetTimeSheet().GetTotalHours();

            Console.WriteLine(bar);
            Console.WriteLine("{0}{1,-2}|", totalHoursString, totalHours);
            Console.WriteLine(bar);

        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                ShowMenu();

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        TimeRegistration();
                        break;
                    case "2":
                        SendTimeSheets();
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Timeregistreringsmenu");
            Console.WriteLine();
            Console.WriteLine("Vælg hvad du vil gøre:");
            Console.WriteLine("1. Indtast timer");
            Console.WriteLine("2. Send timeseddel");
        }

        private void SendTimeSheets()
        {
            bool running = true;
            Console.Clear();
            Console.WriteLine("Ønsker du at indsende dine timesedler?");
            Console.WriteLine("1. Ja");
            Console.WriteLine("2. Hvor mange timer har jeg registreret?");
            Console.WriteLine("0. Vend tilbage til timeregistreringsmenuen.");

            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        _controller.SendTimeSheets();
                        break;
                    case "2":
                        ShowTotalHoursRegisteredForEmployee();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.ReadLine();
                        break;
                }
            } while (running);
        }

        private void ShowTotalHoursRegisteredForEmployee()
        {
            throw new NotImplementedException();
        }
    }
}
