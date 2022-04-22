namespace AdoNetExercises
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Section> Sections { get; set; }
    }
}