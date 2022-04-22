﻿namespace AdoNetExercises
{
    public class Section
    {
        public int SectionID { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public int SemesterID { get; set; }
        public Semester Semester { get; set; }
        public int PeriodID { get; set; }
        public Period Period { get; set; }
        public int RoomID { get; set; }
        public Room Room { get; set; }
    }
}