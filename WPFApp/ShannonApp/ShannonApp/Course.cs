using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonApp
{
    public class Course
    {
        public int ID { get; set; }
        public DateTime Approval_Date { get; set; }
        public string Course_Area { get; set; }
        public string Course_Number { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Instruction_Mode { get; set; }
        public string Instructor { get; set; }
        public int Course_Id { get; set; }
        public string Academic_Org { get; set; }
        public string Student_Verification_Method { get; set; }
        public string Notes { get; set; }
        public bool Approved { get; set; }
        public bool Grandfather { get; set; }
        public string Grandfather_Color_Code { get; set; }
    }
}
