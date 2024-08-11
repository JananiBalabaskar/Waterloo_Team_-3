using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digident_Group3.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AppointmentType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string PatientAllergy { get; set; }


    }
}
