namespace AdoNetExercises
{
    public class Semester
    {
        public int SemesterID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public List<Section> Sections { get; set; }
    }
}