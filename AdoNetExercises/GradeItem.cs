namespace AdoNetExercises
{
    public class GradeItem
    {
        public int GradeItemID { get; set; }
        public int PointsPossible { get; set; }
        public string Descripction { get; set; }
        public int GradeTypeID { get; set; }
        public GradeType GradeType { get; set; }
        public List<Grade> Grades { get; set; }
    }
}