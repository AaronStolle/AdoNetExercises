using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetExercises
{
    public class ADOGrade
    {
        public int GradeID { get; set; }
        public int SectionRosterID { get; set; }
        public int StudentID { get; set; }
        // decimal that accepts nulls
        public decimal? Score { get; set; }
    }
}
