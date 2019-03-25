using System.Collections.Generic;
using System.Data.SqlClient;
using StudentExcercise4.Models;

namespace StudentExcercise4.Data
{
    /// <summary>
    ///  An object to contain all database interactions.
    /// </summary>
    public class Repository
    {
        /// <summary>
        ///  Represents a connection to the database.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Server = ANUPAMA\\SQLEXPRESS; Database =StudentExcerciseDB ; Trusted_Connection = True; ";
                return new SqlConnection(_connectionString);
            }
        }


        //Query the database for all the Exercises.-------------------------------------

        public List<Excercise> GetAllExercises()
        {

            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, ExcerciseName FROM Excercise";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the departments we retrieve from the database.
                    List<Excercise> excercises = new List<Excercise>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "DeptName" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int ExcerciseNameColumnPosition = reader.GetOrdinal("ExcerciseName");
                        string ExcerciseNameValue = reader.GetString(ExcerciseNameColumnPosition);

                        // Now let's create a new department object using the data from the database.
                        Excercise excercise = new Excercise
                        {
                            Id = idValue,
                            ExcerciseName = ExcerciseNameValue
                        };

                        // ...and add that department object to our list.
                        excercises.Add(excercise);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of departments who whomever called this method.
                    return excercises;
                }
            }
        }

        //Find all the exercises in the database where the language is JavaScript.--------------------------------

        public List<Excercise> GetAllJavaScriptExcercises()
        {

            using (SqlConnection conn = Connection)
            {

                conn.Open();


                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT Id, ExcerciseName,ExcerciseLanguage FROM Excercise WHERE ExcerciseLanguage = 'Java-Script'";


                    SqlDataReader reader = cmd.ExecuteReader();


                    List<Excercise> javaScriptExercises = new List<Excercise>();


                    while (reader.Read())
                    {

                        int idColumnPosition = reader.GetOrdinal("Id");


                        int idValue = reader.GetInt32(idColumnPosition);

                        int ExcerciseNameColumnPosition = reader.GetOrdinal("ExcerciseName");
                        string ExcerciseNameValue = reader.GetString(ExcerciseNameColumnPosition);

                        int ExcerciseLanguageColumnPosition = reader.GetOrdinal("ExcerciseLanguage");
                        string ExcerciseLanguageValue = reader.GetString(ExcerciseLanguageColumnPosition);


                        Excercise javaScriptExercise = new Excercise
                        {
                            Id = idValue,
                            ExcerciseName = ExcerciseNameValue,
                            ExcerciseLanguage = ExcerciseLanguageValue
                        };


                        javaScriptExercises.Add(javaScriptExercise);
                    }


                    reader.Close();


                    return javaScriptExercises;
                }
            }
        }

        //Insert a new exercise into the database.----------------------------------------------------------------------

        public void AddExercise(Excercise excercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = $"INSERT INTO Excercise (ExcerciseName,ExcerciseLanguage) Values (@ExcerciseName,@ExcerciseLanguage)";

                    cmd.Parameters.Add(new SqlParameter("@ExcerciseName", excercise.ExcerciseName));
                    cmd.Parameters.Add(new SqlParameter("@ExcerciseLanguage", excercise.ExcerciseLanguage));

                 
                    cmd.ExecuteNonQuery();
                }
            }
        }


            // Find all instructors in the database. Include each instructor's cohort.-----------------------------


         public List<Instructor> GetInstructorsAndTheirCohort()
            {

                using (SqlConnection conn = Connection)
                {

                    conn.Open();


                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = @"SELECT Instructor.Id,Instructor.FirstName, Instructor.LastName , Cohort.CohortName 
                                                                                  FROM Instructor
                                                                                  LEFT JOIN Cohort
                                                                                  ON Instructor.CohortId = Cohort.Id; ";


                        SqlDataReader reader = cmd.ExecuteReader();


                        List<Instructor> instructorsAndCohort = new List<Instructor>();


                        while (reader.Read())
                        {

                            int idColumnPosition = reader.GetOrdinal("Id");


                            int idValue = reader.GetInt32(idColumnPosition);

                            int FirstNamePosition = reader.GetOrdinal("FirstName");

                            string FirstNamePositionValue = reader.GetString(FirstNamePosition);

                            int LastNamePosition = reader.GetOrdinal("LastName");
                            string LastNamePositionValue = reader.GetString(LastNamePosition);

                            int CohortNamePosition = reader.GetOrdinal("CohortName");
                            string CohortNamePositionValue = reader.GetString(CohortNamePosition);


                            Instructor instructorAndCohort = new Instructor
                            {
                                Id = idValue,
                                FirstName = FirstNamePositionValue,
                                LastName = LastNamePositionValue,
                                Cohort = new Cohort
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    CohortName = CohortNamePositionValue
                                }

                            };


                            instructorsAndCohort.Add(instructorAndCohort);
                        }


                        reader.Close();


                        return
                            instructorsAndCohort;
                    }
                }
            }



        //Insert a new instructor into the database.Assign the instructor to an existing cohort.--------------------------
        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = $@"INSERT INTO Instructor (FirstName,LastName, CohortId, SlackHandle) 
                                        OUTPUT INSERTED.Id
                                     VALUES(@FirstName,@LastName,@CohortId,@SlackHandle)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle",instructor.SlackHandle));

                  

                                cmd.ExecuteNonQuery();
                       
                }
            }
            
        }

    }
}