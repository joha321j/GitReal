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

        public void EnterWorkHours(KeyValuePair<int, string> workType, double userInput, int userInputBlock)
        {
            Work work = GetWork(workType.Key);
            work.Block = userInputBlock;
            work.Hours += userInput;
        }

        private Work GetWork(int workTypeKey)
        {
            return WorkList.Find(work => work.WorkType.Key == workTypeKey);
        }

        public int GetBlockForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.WorkType.Key == workType.Key).Block;
        }

        public double GetHoursRegisteredForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.WorkType.Key == workType.Key).Hours;
        }

        public string GetComment()
        {
            return Comment;
        }

        public double GetTotalHours()
        {
            double sum = 0;
            foreach (Work work in WorkList)
            {
                sum += work.Hours;
            }
            return sum;
        }

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