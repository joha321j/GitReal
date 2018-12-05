using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TimeRegistrationLibrary
{
    public class CaseRepo
    {
        private List<Case> cases = new List<Case>();
        //private List<KeyValuePair<int, string>> _standardWorkTypeList = new List<KeyValuePair<int, string>>();


        public CaseRepo(string loginInformation)
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
                            
                            List<KeyValuePair<int, string>> workTypeList = new List<KeyValuePair<int, string>>();


                            while (reader.Read())
                            {
                                getWorkTypeOfCases.Parameters.Clear();
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
                Console.WriteLine("Something goofed "+ e.Message);
            }
        }

        /// <summary>
        /// Returns all case names and ids that are to be displayed.
        /// </summary>
        public List<KeyValuePair<int, string>> GetCaseNameAndId()
        {
            List<KeyValuePair<int, string>> caseNameIdPairs = new List<KeyValuePair<int, string>>();

            foreach (Case caseCase in cases)
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
            return cases.Find(caseToFind => caseToFind.CaseId == caseId);
        }

        /// <summary>
        /// Create new case.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerAddress"></param>
        //public void Createcase(int caseid, string casename, string customername, string customeremail, Address customeraddress)
        //{
        //    Case newcase = new Case(caseid, casename, customername, customeremail, customeraddress, _standardWorkTypeList);

        //    AddCase(newcase);
        //}

        /// <summary>
        /// Create new case.
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerAddress"></param>
        /// <param name="workTypeList"></param>
        public void CreateCase(int caseId, string caseName, string customerName, string customerEmail, Address customerAddress, List<KeyValuePair<int, string>> workTypeList)
        {
            Case newStandardCase = new Case(customerAddress, customerName, customerEmail, caseName, workTypeList);

            AddCase(newStandardCase);
        }
        /// <summary>
        /// Adds a case to the list of cases
        /// </summary>
        /// <param name="caseToAdd"></param>
        private void AddCase(Case caseToAdd)
        {
            cases.Add(caseToAdd);
        }
    }
}
