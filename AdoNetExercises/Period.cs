namespace AdoNetExercises
{
    public class Period
    {
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<Section> Sections { get; set; }
    }
}