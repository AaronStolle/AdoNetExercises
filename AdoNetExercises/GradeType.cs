using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetExercises
{
    public class GradeType
    {
        public int GradeTypeID { get; set; }
        public string GradeTypeName { get; set; }

        public List<GradeItem> GradeItems { get; set; }
    }
}
