using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace TimeRegistrationLibrary
{
    public class CustomerRepo
    {
        /// <summary>
        /// Connects to the database and makes a list of all customers.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<KeyValuePair<int,string>> GetallCustomers(string connectionString)
        {
            List<KeyValuePair<int, string>> listOfcustomers = new List<KeyValuePair<int, string>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand getCustomers = new SqlCommand("spGetCustomers", connection);
                    getCustomers.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = getCustomers.ExecuteReader();
                    while (reader.Read())
                    {
                        listOfcustomers.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }                    
                }

                return listOfcustomers;
            }
            catch (Exception e)
            {                
                Console.WriteLine("Something goofed" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Connects to the database and makes a list of all addresses for the given customerId
        /// </summary>
        /// <param name="custoId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetallCustomersAddresses(int custoId, string connectionString)
        {
            List<KeyValuePair<int, string>> listOfcustomersAddresses = new List<KeyValuePair<int, string>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand getCustomersAddreses = new SqlCommand("spGetAddressesByCustomerId", connection);
                    getCustomersAddreses.CommandType = CommandType.StoredProcedure;
                    getCustomersAddreses.Parameters.AddWithValue("@CustomerId", custoId);

                    SqlDataReader reader = getCustomersAddreses.ExecuteReader();
                    while (reader.Read())
                    {
                        string address = "Vejnavn: " +     reader[1].ToString() + " " + "Vejnummer: " + reader[2].ToString() + " " + "Sal: " + reader[3].ToString() + " "+ "PostNummer: "  + reader[4].ToString() + " " + "By: " + reader[5].ToString();
                        listOfcustomersAddresses.Add(new KeyValuePair<int, string>(reader.GetInt32(0), address));
                    }
                }

                return listOfcustomersAddresses;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something goofed" + e.Message);
                return null;
            }
        }
    }
}
