﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TimeRegistrationLibrary;
using System.Data.SqlClient;

namespace TimeRegistrationLibrary
{
    public class EmployeeRepo
    {
        private readonly List<Employee> _employees = new List<Employee>();
        /// <summary>
        /// Connects to the database and adds all employees to a list.
        /// </summary>
        /// <param name="loginInformation"></param>
        public EmployeeRepo(string loginInformation)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(loginInformation))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand getEmployees = new SqlCommand("spGetEmployees", connection);
                        getEmployees.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = getEmployees.ExecuteReader();
                        while (reader.Read())
                        {
                            CompanyPosition position = (CompanyPosition) Enum.Parse(typeof(CompanyPosition), reader.GetString(3));
                            Employee employee = new Employee(reader.GetInt32(0), reader.GetString(1), position);
                            _employees.Add(employee);
                        }
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("Could not find the login info file." + e.Message);
            }
        }

        /// <summary>
        /// Returns a list of all employees.
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployeeList()
        {
            return _employees;
        }
    }
}
