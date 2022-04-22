using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetExercises
{
    public class Building
    {
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }

        public List<Room> Rooms { get; set; }
    }
}
