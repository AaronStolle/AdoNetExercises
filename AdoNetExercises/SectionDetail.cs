using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetExercises
{
    public class SectionDetail
    {
        public int SectionID { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string RoomInformation { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
