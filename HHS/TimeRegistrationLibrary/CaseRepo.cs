using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TimeRegistrationLibrary
{
    public class CaseRepo
    {
        private readonly List<Case> _cases = new List<Case>();
        private readonly List<KeyValuePair<int, string>> _standardWorkTypeList = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(1,"Andet"),
            new KeyValuePair<int, string>(2,"Sygdom"),
            new KeyValuePair<int, string>(3, "Ferie")
        };


        public CaseRepo(string loginInformation)
        {
            UpdateCaseRepo(loginInformation);
        }

        /// <summary>
        /// Connects to the database and inserts the standard worktypes for a given caseid.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        public void AddStandardWorkTypes(int id, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spInsertCase_WorkType", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < 3; i++)
                    {
                        command.Parameters.AddWithValue("@CaseId", id);
                        command.Parameters.AddWithValue("@WorktypeId", _standardWorkTypeList[i].Key);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                }
            }
            catch (Exception e)
            {
                /// TODO: Actually handle the exception!
                throw e;
            }
        }

        /// <summary>
        /// Connects to the database and adds the given parametres.
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="custoId"></param>
        /// <param name="addressId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public int CreateNewStandardCase(string caseName, int custoId, int addressId, string connectionString)
        {
            int output = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand insertNewCase = new SqlCommand("spInsertCaseGetId", connection);
                    insertNewCase.CommandType = CommandType.StoredProcedure;
                    insertNewCase.Parameters.AddWithValue("@CustomerId", custoId);
                    insertNewCase.Parameters.AddWithValue("@CaseName", caseName);
                    insertNewCase.Parameters.AddWithValue("@Case_AddressId", addressId);

                    using (SqlDataReader reader = insertNewCase.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            output = reader.GetInt32(0);
                        }

                    }
                                       
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Something goofed" + e.Message);
            }
            return output;

        }


        /// <summary>
        /// Returns all case names and ids that are to be displayed.
        /// </summary>
        public List<KeyValuePair<int, string>> GetCaseNameAndId()
        {
            List<KeyValuePair<int, string>> caseNameIdPairs = new List<KeyValuePair<int, string>>();

            foreach (Case caseCase in _cases)
            {
                KeyValuePair<int, string> valuePair = new KeyValuePair<int, string>(caseCase.CaseId, caseCase.CaseName);
                caseNameIdPairs.Add(valuePair);
            }

            return caseNameIdPairs;
        }
        /// <summary>
        /// Return case with given caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public Case GetCase(int caseId)
        {
            return _cases.Find(caseToFind => caseToFind.CaseId == caseId);
        }

        /// <summary>
        /// Create new case.
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="caseName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerAddress"></param>
        /// <param name="workTypeList"></param>
        private void CreateCase(int caseId, string caseName, string customerName, string customerEmail, Address customerAddress, List<KeyValuePair<int, string>> workTypeList)
        {
            Case newStandardCase = new Case(caseId, caseName, customerName, customerEmail, customerAddress, workTypeList);

            AddCase(newStandardCase);
        }
        /// <summary>
        /// Adds a case to the list of cases
        /// </summary>
        /// <param name="caseToAdd"></param>
        private void AddCase(Case caseToAdd)
        {
            _cases.Add(caseToAdd);
        }

        /// <summary>
        /// Connect to the database given in loginInformation,
        /// and update the caseRepo with all the cases in the database.
        /// </summary>
        /// <param name="loginInformation"></param>
        private void UpdateCaseRepo(string loginInformation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(loginInformation))
                {
                    try
                    {
                        connection.Open();


                        SqlCommand getAllCaseSqlCommand = new SqlCommand("spGetCasesAddressCustomer", connection);
                        getAllCaseSqlCommand.CommandType = CommandType.StoredProcedure;

                        SqlCommand getWorkTypeOfCases = new SqlCommand("spGetWorkTypesForCase", connection);
                        getWorkTypeOfCases.CommandType = CommandType.StoredProcedure;



                        using (SqlDataReader reader = getAllCaseSqlCommand.ExecuteReader())
                        {


                            while (reader.Read())
                            {
                                List<KeyValuePair<int, string>> workTypeList = new List<KeyValuePair<int, string>>();

                                getWorkTypeOfCases.Parameters.AddWithValue("@CaseId", reader.GetInt32(4));
                                Address caseAddress = new Address(
                                    reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());

                                using (SqlDataReader workTypeReader = getWorkTypeOfCases.ExecuteReader())
                                {

                                    while (workTypeReader.Read())
                                    {
                                        workTypeList.Add(new KeyValuePair<int, string>(workTypeReader.GetInt32(0), workTypeReader[1].ToString()));
                                    }
                                }

                                CreateCase(reader.GetInt32(4), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), caseAddress, workTypeList);
                                getWorkTypeOfCases.Parameters.Clear();                               
                            }

                        }


                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Another thing goofed" + e.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something goofed " + e.Message);
            }
        }

        /// <summary>
        /// Establish connection to database given in string loginInformation, and send all timesheets for employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="loginInformation"></param>
        /// <exception cref="EmployeeHourNormNotFulfilledException"></exception>
        /// <exception cref="SqlException"></exception>
        public void SendTimeSheets(Employee employee, string loginInformation)
        {
            List<TimeSheet> timeSheetsToSend = GetTimeSheets(employee);
            double totalHours = 0;
            foreach (TimeSheet timeSheet in timeSheetsToSend)
            {
                totalHours += timeSheet.GetTotalHours();

            }

            timeSheetsToSend.RemoveAll(timeSheet => timeSheet.GetTotalHours() < 0.0001);

            if (totalHours < employee.HourNorm)
            {
                throw new EmployeeHourNormNotFulfilledException(employee.HourNorm, totalHours);
            }

            using (SqlConnection connection = new SqlConnection(loginInformation))
            {
                try
                {
                    connection.Open();
                    SqlCommand insertTimeSheet = new SqlCommand("spInsertTimeSheet", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlCommand insertWork = new SqlCommand("spInsertWork", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    foreach (TimeSheet timeSheet in timeSheetsToSend)
                    {
                        int timeSheetId = 0;
                        insertTimeSheet.Parameters.AddWithValue("@CaseId", timeSheet.CaseId);
                        insertTimeSheet.Parameters.AddWithValue("@EmployeeId", timeSheet.EmployeeId);
                        insertTimeSheet.Parameters.AddWithValue("@Comment", timeSheet.Comment);
                        insertTimeSheet.Parameters.AddWithValue("@Date", timeSheet.Date);

                        using (SqlDataReader reader = insertTimeSheet.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                timeSheetId = reader.GetInt32(0);
                            }
                        }
                        foreach (TimeSheet.Work work in timeSheet.WorkList)
                        {
                            insertWork.Parameters.AddWithValue("@TimeSheetId", timeSheetId);
                            insertWork.Parameters.AddWithValue("@Hours", work.Hours);
                            insertWork.Parameters.AddWithValue("@Block", work.Block);
                            insertWork.Parameters.AddWithValue("@workTypeId", work.WorkType.Key);

                            insertWork.ExecuteNonQuery();

                            insertWork.Parameters.Clear();
                        }
                        insertTimeSheet.Parameters.Clear();
                    }
                }
                catch (SqlException exception)
                {
                    Console.WriteLine(exception.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets a list of timesheets for the given employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private List<TimeSheet> GetTimeSheets(Employee employee)
        {
            List<TimeSheet> resultList = new List<TimeSheet>();
            foreach (Case caseCase in _cases)
            {
                resultList.Add(caseCase.GetTimeSheet(employee));
            }

            return resultList;
        }

        /// <summary>
        /// returns the total hours registered for the given employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public double GetTotalHoursRegisteredForEmployee(Employee employee)
        {
            double totalHours = 0;

            List<TimeSheet> timeSheets = GetTimeSheets(employee);

            foreach (TimeSheet timeSheet in timeSheets)
            {
                totalHours += timeSheet.GetTotalHours();
            }

            return totalHours;
        }
    }
}
