//Query the database for all the Exercises.
//Find all the exercises in the database where the language is JavaScript.
//Insert a new exercise into the database.
//Find all instructors in the database. Include each instructor's cohort.
//Insert a new instructor into the database.Assign the instructor to an existing cohort.
//Assign an existing exercise to an existing student.
using System;
using System.Collections.Generic;
using System.Linq;
using StudentExcercise4.Data;
using StudentExcercise4.Models;

namespace StudentExcercise4

{
    class Program

    {

        /// <summary>
        /// In Program.cs, complete the PrintDepartmentReport(string title, List<Department> departments).
        ///  The Main method is the starting point for a .net application.
        /// </summary>
        static void Main(string[] args)
        {
            // We must create an instance of the Repository class in order to use it's methods to
            //  interact with the database.
            Repository repository = new Repository();

            List<Excercise> AllExcercises = repository.GetAllExercises();

            foreach (Excercise e in AllExcercises)
            {
                Console.WriteLine(e.ExcerciseName, e.ExcerciseLanguage);
            }
            Pause();

            //Find all the exercises in the database where the language is JavaScript.-----------------------------
            Console.WriteLine("JavaScript Excercises");
            List<Excercise> JavaScriptExcercises = repository.GetAllJavaScriptExcercises();
            foreach (Excercise excercise in JavaScriptExcercises)
            {
                Console.WriteLine(excercise.ExcerciseName, excercise.ExcerciseLanguage);
            }

            //Insert a new exercise into the database.---------------------------------------------------


            Excercise WebAPI = new Excercise { ExcerciseName = "StudentExcercise4", ExcerciseLanguage = "C#" };
            repository.AddExercise(WebAPI);
            Console.WriteLine($"New Excercise Add is {WebAPI.ExcerciseName} in {WebAPI.ExcerciseLanguage}");
            Pause();


            // Find all instructors in the database. Include each instructor's cohort.-------------------------


            List<Instructor> InstructorAndTheirCohort = repository.GetInstructorsAndTheirCohort();

            Console.WriteLine("Instructor and Cohort");
            foreach (Instructor Instructor in InstructorAndTheirCohort)
            {
                Console.WriteLine($"{Instructor.FirstName} {Instructor.LastName} {Instructor.Cohort.CohortName}");
            }
            Pause();

            //Insert a new instructor into the database.Assign the instructor to an existing cohort.------------------------

            Instructor Jisie = new Instructor
            {
                FirstName = "Jisie",
                LastName = "David",
                CohortId = 3,
                SlackHandle = "@jisie"
            };

            repository.AddInstructor(Jisie);
            Console.WriteLine($"{Jisie.FirstName} is FrontEnd instructor of cohort{Jisie.CohortId}");

            List<Instructor> Instructors = repository.GetInstructorsAndTheirCohort();
            foreach (Instructor instructor in Instructors)
            {
                Console.WriteLine($"All Instructors after adding Jisie{ instructor}");
            }
            Pause();
        }




        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
