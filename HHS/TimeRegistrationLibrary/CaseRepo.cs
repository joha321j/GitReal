﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TimeRegistrationLibrary
{
    public class CaseRepo
    {
        private List<Case> cases = new List<Case>();
        private List<KeyValuePair<int, string>> _standardWorkTypeList = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(1,"Andet"),
            new KeyValuePair<int, string>(2,"Sygdom"),
            new KeyValuePair<int, string>(3, "Ferie")
        };


        public CaseRepo(string loginInformation)
        {
            UpdateCaseRepo(loginInformation);
        }

        public void addStandardWorkTypes(int id, string connectionString)
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
                throw e;
            }
        }

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
                    insertNewCase.ExecuteScalar();

                    using (SqlDataReader reader = insertNewCase.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            output = reader.GetInt32(0);
                        }

                    }
                                       
                }

            }
            catch (Exception)
            {

                throw;
            }
            return output;

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
            Case newStandardCase = new Case(caseId, caseName, customerName, customerEmail, customerAddress, workTypeList);

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
                                //workTypeList.Clear();
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
    }
}
