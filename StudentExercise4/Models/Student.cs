using System.Collections.Generic;

namespace StudentExcercise4.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public List<Excercise> Excercises { get; set; } = new List<Excercise>();
    }
}