using BasicFunctions.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BasicFunctions
{
    internal class StudentRepository
    {
        private const string ConnectionString = "Server=localhost;Database=University;Trusted_Connection=True;";

        public List<StudentModel> GetAllDisconnected()
        {
            var students = new List<StudentModel>();

            // create a unique instance of a database connection
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM dbo.Students", connection);
                connection.Open();

                var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();

                // populate the data set
                adapter.Fill(dataSet);

                // cast DataRowCollection to DataRow for further mapping
                foreach (var dataRow in dataSet.Tables[0].Rows.Cast<DataRow>())
                {
                    // map DataRows into Student instances
                    students.Add(CreateStudent(dataRow));
                }
            }
            return students;
        }

        public List<StudentModel> GetAllConnected()
        {
            var students = new List<StudentModel>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM dbo.Students", connection);
                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // map SqlDataReader into Student instances
                    students.Add(CreateStudent(reader));
                }
            }
            return students;
        }

        public List<StudentModel> GetAllStoredViaProcedure()
        {
            var students = new List<StudentModel>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                // execute an existing stored procedure by name
                var command = new SqlCommand("sp_SelectAllStudents", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                foreach (var dataRow in dataSet.Tables[0].Rows.Cast<DataRow>())
                {
                    students.Add(CreateStudent(dataRow));
                }
            }
            return students;
        }

        public StudentModel Get(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand($"SELECT * FROM dbo.Students WHERE Id={id}", connection);
                connection.Open();

                var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                var dataRow = dataSet.Tables[0].Rows.Cast<DataRow>();
                return CreateStudent(dataRow.FirstOrDefault());
            }
        }

        public string GetFacultyNameViaFucntion(int studentId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                //execute an existing user-defined function by name
                var command = new SqlCommand("fk_GetFacultyName", connection);
                //both functions and stored procedures use the StoredProcedure command type
                command.CommandType = CommandType.StoredProcedure;

                //define input parameters
                command.Parameters.AddWithValue("@StudentId", studentId);

                //define return parameters
                var returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                returnValue.SqlDbType = SqlDbType.NVarChar;
                command.Parameters.Add(returnValue);

                connection.Open();
                command.ExecuteNonQuery();

                return (string)returnValue.Value;
            }
        }

        public int Add(StudentModel student)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("sp_AddStudent", connection);
                command.CommandType = CommandType.StoredProcedure;

                //define input parameters
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@AverageGrade", student.AverageGrade);
                command.Parameters.AddWithValue("@FacultyId", student.FacultyId);

                //define output parameters
                var outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@Id";
                outputParameter.Direction = ParameterDirection.Output;
                outputParameter.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(outputParameter);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputParameter.Value;
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("sp_deleteStudent", connection);
                command.CommandType = CommandType.StoredProcedure;

                //define input parameters
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private StudentModel CreateStudent(DataRow tableRow)
        {
            var student = new StudentModel()
            {
                Id = (int)tableRow["Id"],
                FirstName = (string)tableRow["FirstName"],
                LastName = (string)tableRow["LastName"],
                Email = (string)tableRow["Email"],
                PhoneNumber = (string)tableRow["PhoneNumber"],
                AverageGrade = (double)tableRow["AverageGrade"],
                FacultyId = tableRow["FacultyId"] != DBNull.Value
                    ? (int)tableRow["FacultyId"]
                    : null
            };
            return student;
        }

        private StudentModel CreateStudent(SqlDataReader reader)
        {
            var student = new StudentModel()
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Email = (string)reader["Email"],
                PhoneNumber = (string)reader["PhoneNumber"],
                AverageGrade = (double)reader["AverageGrade"],
                FacultyId = reader["FacultyId"] != DBNull.Value
                    ? (int)reader["FacultyId"]
                    : null
            };
            return student;
        }
    }
}
