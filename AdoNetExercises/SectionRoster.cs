namespace AdoNetExercises
{
    public class SectionRoster
    {
        public int SectionRosterID { get; set; }
        public decimal CurrentGrade { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public int SectionID { get; set; }
        public Section Section { get; set; }
    }
}