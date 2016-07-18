using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIP2UWP.Models
{
    public class Repair
    {
        public int RefNumber { get; set; }
        public int DealerID { get; set; }
        public string Manufacturer { get; set; }
        public string FuturetelLocation { get; set; }
        public bool Warranty { get; set; }
        public string Status { get; set; }
        public string SVP { get; set; }
        public string LastTechnician { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string DateIn { get; set; }
        public DateTime? DateFinish { get; set; }
        public int AGING { get; set; }
        public int TotalOutput { get; set; }
        //public int Age0B { get; set; }
        //public int Age1B { get; set; }
        //public int Age2B { get; set; }
        //public int Age3B { get; set; }
        //public int Age4B { get; set; }
        //public int Age5B { get; set; }
        //public int Age6to29B { get; set; }
        //public int Age30MoreB { get; set; }
        //public int Age0E { get; set; }
        //public int Age1E { get; set; }
        //public int Age2E { get; set; }
        //public int Age3E { get; set; }
        //public int Age4E { get; set; }
        //public int Age5E { get; set; }
        //public int Age6to29E { get; set; }
        //public int Age30MoreE { get; set; }
        //public int Age0RI { get; set; }
        //public int Age1RI { get; set; }
        //public int Age2RI { get; set; }
        //public int Age3RI { get; set; }
        //public int Age4RI { get; set; }
        //public int Age5RI { get; set; }
        //public int Age6to29RI { get; set; }
        //public int Age30MoreRI { get; set; }
        //public int Age0RIJA { get; set; }
        //public int Age1RIJA { get; set; }
        //public int Age2RIJA { get; set; }
        //public int Age3RIJA { get; set; }
        //public int Age4RIJA { get; set; }
        //public int Age5RIJA { get; set; }
        //public int Age6to29RIJA { get; set; }
        //public int Age30MoreRIJA { get; set; }
    }
}
