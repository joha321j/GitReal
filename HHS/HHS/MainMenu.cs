﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRegistrationLibrary;
using System.Globalization;

namespace HHS
{
    class MainMenu : IMenu
    {
        private readonly Controller _controller;
        private TimeRegistrationMenu timeRegistrationMenu;

        public MainMenu()
        {
            _controller = new Controller();
            timeRegistrationMenu = new TimeRegistrationMenu(_controller);
        }
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
                        timeRegistrationMenu.Show();
                        break;
                    case "2":
                        CreateNewStandardCase();
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.ReadLine();
                        break;
                }

            }

        }

        private void CreateNewStandardCase()
        {
            Console.WriteLine("Angiv et navn på sagen: ");
            string caseName = Console.ReadLine();
            Console.WriteLine("Angiv et kunde nummer");
            int.TryParse(Console.ReadLine(), out int custoId);
            Console.WriteLine("Angiv en Adresse");
            int.TryParse(Console.ReadLine(), out int addressId);

            _controller.CreateNewStandardCase(caseName, custoId, addressId);

            Console.WriteLine("Din sag er nu oprettet");
        }

        /// <summary>
        /// Print the menu to the console.
        /// </summary>
        private void showMenu()
        {
            Console.Clear();
            Console.WriteLine("Hovedmenu");
            Console.WriteLine();
            Console.WriteLine("Vælg hvad du vil gøre:");
            Console.WriteLine("1. Timeregistrering.");
            Console.WriteLine("2. Oprat ny sag");
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
    }

    internal interface IMenu
    {
        void Show();
    }
}