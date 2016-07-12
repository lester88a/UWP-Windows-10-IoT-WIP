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
    }
}
