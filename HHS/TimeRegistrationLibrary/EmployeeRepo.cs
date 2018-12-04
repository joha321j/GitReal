using System;
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
        public EmployeeRepo(string loginInformation)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(loginInformation))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand getEmployees = new SqlCommand("spGetAllEmployees", connection);
                        getEmployees.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = getEmployees.ExecuteReader();
                        while (reader.Read())
                        {
                            CompanyPosition position = (CompanyPosition) Enum.Parse(typeof(CompanyPosition), reader.GetString(1));
                            Employee employee = new Employee(reader.GetInt32(0), reader.GetString(2), position);
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

        public List<Employee> GetEmployeeList()
        {
            return _employees;
        }
    }
}
