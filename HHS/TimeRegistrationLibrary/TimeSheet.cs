using System;
using System.Collections.Generic;

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
        internal string Comment { get; set; }

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
    }
}