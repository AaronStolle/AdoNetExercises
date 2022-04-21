namespace AdoNetExercises
{
    public class SectionRosterView
    {
        public int SectionID { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TeacherName { get; set; }
        public List<RosterItem> rosterItems { get; set; }
    }
}