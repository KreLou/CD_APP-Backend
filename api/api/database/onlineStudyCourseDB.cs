﻿using api.Interfaces;
using api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace api.database
{
    public class onlineStudyCourseDB : IStudyCourseDB
    {
        SqlConnection sqlConnection = null;
        public StudyCourse[] getAllCourses()
        {
            sqlConnection = null;
            sqlConnection = TimeTableDatabase.getConnection();
            try
            {
                using (sqlConnection)
                {

                    StudyCourse SQLItem = new StudyCourse();
                    List<StudyCourse> list = new List<StudyCourse>();


                    string SQL = "SELECT [coursetypid],[typname],[typnameshort]" +
                    " FROM [coursetyp];";

                sqlConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(SQL, sqlConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                        SQLItem.ID = Convert.ToInt32(myReader["coursetypid"].ToString());
                        SQLItem.LongText = myReader["typname"].ToString();
                        SQLItem.ShortText = myReader["typnameshort"].ToString();
                        list.Add(SQLItem);
                        SQLItem = new StudyCourse();
                    }

                    sqlConnection.Close();
                    sqlConnection = null;
                    return list.ToArray();
                }

            }
            catch (System.Exception)
            {
                return null;
            }
}

        public StudyCourse getCourseById(int id)
        {
            sqlConnection = null;
            sqlConnection = TimeTableDatabase.getConnection();
            try
            {
                using (sqlConnection)
                {

                    StudyCourse SQLItem = new StudyCourse();

                    string SQL = "SELECT [coursetypid],[typname],[typnameshort]" +
                    " FROM [coursetyp] WHERE [coursetypid] = '" + id.ToString()+"';";

                    sqlConnection.Open();
                    SqlDataReader myReader = null;
                    SqlCommand myCommand = new SqlCommand(SQL, sqlConnection);
                    myReader = myCommand.ExecuteReader();

                    if (myReader.Read())
                    {
                        SQLItem.ID = Convert.ToInt32(myReader["coursetypid"].ToString());
                        SQLItem.LongText = myReader["typname"].ToString();
                        SQLItem.ShortText = myReader["typnameshort"].ToString();
                        sqlConnection.Close();
                        sqlConnection = null;
                        return SQLItem;
                    }
                    else
                    {
                        sqlConnection.Close();
                        sqlConnection = null;
                        return null;
                    }
                }

            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
