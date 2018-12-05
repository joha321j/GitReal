using System;
using System.Collections.Generic;

namespace TimeRegistrationLibrary
{
    public class TimeSheet
    {
        internal class Work
        {
            internal KeyValuePair<int, string> workType { get; private set; }
            internal int Block { get; private set; }
            internal int Hours { get; private set; }

            public Work(KeyValuePair<int, string> workType, int block = 0, int hours = 0)
            {
                this.workType = workType;
                Block = block;
                Hours = hours;
            }
        }
        
        internal int EmployeeId { get; }

        internal List<Work> WorkList;
        internal string Comment { get; private set; }

        public TimeSheet(int employeeId, List<KeyValuePair<int, string>> workTypes)
        {
            EmployeeId = employeeId;
            WorkList = new List<Work>();
            foreach (KeyValuePair<int, string> workType in workTypes)
            {
                Work workToAdd = new Work(workType);
                WorkList.Add(workToAdd);
            }
           
        }

        public void EnterWorkHours(KeyValuePair<int, string> workType, double userInput)
        {
            throw new System.NotImplementedException();
        }

        public int GetBlockForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.workType.Key == workType.Key).Block;
        }

        public int GetHoursRegisteredForWorkType(KeyValuePair<int, string> workType)
        {
            return WorkList.Find(work => work.workType.Key == workType.Key).Hours;
        }

        public string GetComment()
        {
            return Comment;
        }
    }
}