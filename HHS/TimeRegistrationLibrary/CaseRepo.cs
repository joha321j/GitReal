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
        private List<KeyValuePair<string, int>> _standardWorkTypeList;

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

                        SqlDataReader reader = getAllCaseSqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            Address caseAddress = new Address(
                                reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());
                            CreateCase(reader.GetInt32(4), reader[5].ToString(),reader[6].ToString(),reader[7].ToString(),caseAddress);
                        }

                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Returns all case names and ids that are to be displayed.
        /// </summary>
        public List<KeyValuePair<string, int>> GetCaseNameAndId()
        {
            List<KeyValuePair<string, int>> caseNameIdPairs = new List<KeyValuePair<string, int>>();

            foreach (Case caseCase in cases)
            {
                KeyValuePair<string, int> valuePair = new KeyValuePair<string, int>(caseCase.CaseName, caseCase.CaseId);
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
        public void CreateCase(int caseId, string caseName, string customerName, string customerEmail, Address customerAddress)
        {
            Case newCase = new Case(caseId, caseName, customerName, customerEmail, customerAddress, _standardWorkTypeList);

            AddCase(newCase);
        }

        /// <summary>
        /// Create new case.
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerAddress"></param>
        /// <param name="workTypeList"></param>
        public void CreateNewStandardCase(string caseName, string customerName, string customerEmail, Address customerAddress, List<KeyValuePair<string, int>> workTypeList)
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
