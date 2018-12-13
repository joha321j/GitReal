using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TimeRegistrationLibrary
{
    public class TimeSheet
    {
        internal class Work
        {
            internal KeyValuePair<int, string> WorkType { get; private set; }
            internal int Block { get; set; }
            internal double Hours { get; set; }

            public Work(KeyValuePair<int, string> workType, int block = 0, int hours = 0)
            {
                WorkType = workType;
                Block = block;
                Hours = hours;
            }
        }
        
        internal int EmployeeId { get; }

        internal int CaseId { get; }

        internal List<Work> WorkList;
        internal string Comment { get; set; } = " ";

        internal DateTime Date { get; }

        /// <summary>
        /// Constructs a timesheet.
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="employeeId"></param>
        /// <param name="workTypes"></param>
        public TimeSheet(int caseId, int employeeId, List<KeyValuePair<int, string>> workTypes)
        {
            CaseId = caseId;
            EmployeeId = employeeId;
            WorkList = new List<Work>();
            Date = DateTime.Today;
            foreach (KeyValuePair<int, string> workType in workTypes)
            {
                Work workToAdd = new Work(workType);
                WorkList.Add(workToAdd);
            }
           
        }

        /// <summary>
        /// Adds the inputted hours to the work.
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="userInput"></param>
        /// <param name="userInputBlock"></param>
        public void EnterWorkHours(KeyValuePair<int, string> workType, double userInput, int userInputBlock)
        {
            Work work = GetWork(workType.Key);
            work.Block = userInputBlock;
            work.Hours += userInput;
        }

        /// <summary>
        /// Returns the given work.
        /// </summary>
        /// <param name="workTypeKey"></param>
        /// <returns></returns>
        private Work GetWork(int workTypeKey)
        {
            return WorkList.Find(work => work.WorkType.Key == workTypeKey);
        }

        /// <summary>
        /// Returns the block number for given worktype.
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        public int GetBlockForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.WorkType.Key == workType.Key).Block;
        }

        /// <summary>
        /// Returns 5hours registered for the given worktype.
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        public double GetHoursRegisteredForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.WorkType.Key == workType.Key).Hours;
        }

        /// <summary>
        /// Returns the comment.
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return Comment;
        }

        /// <summary>
        /// Returns the sum of all hours.
        /// </summary>
        /// <returns></returns>
        public double GetTotalHours()
        {
            double sum = 0;
            foreach (Work work in WorkList)
            {
                sum += work.Hours;
            }
            return sum;
        }

        /// <summary>
        /// Connects to the database and returns a list of all registered timesheets.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<string> GetRegisteredTimeSheets(string connectionString)
        {
            List<string> timeSheets = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand getTimeSheets = new SqlCommand("spGetTimeSheets", connection)
                    {
                        CommandType = CommandType.StoredProcedure

                    };

                    using (SqlDataReader reader = getTimeSheets.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string employeeName = reader.GetString(0);
                            string caseName = reader.GetString(1);
                            string workType = reader.GetString(2);
                            int block = reader.GetInt32(3);
                            double hours = reader.GetDouble(4);
                            string comment = reader[5].ToString();
                            DateTime date = reader.GetDateTime(6);
                            string timeSheetString = employeeName + " " + caseName + " " + workType + " "
                                                     + block + " " + hours + " " + comment + " " + date.Date.ToShortDateString();

                            timeSheets.Add(timeSheetString);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }

            return timeSheets;
        }
    }
}