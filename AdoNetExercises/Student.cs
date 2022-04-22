namespace AdoNetExercises
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string ClassYear { get; set; }
        public List<SectionRoster> SectionRosters { get; set; }
    }
}