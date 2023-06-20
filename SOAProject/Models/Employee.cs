using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOAProject.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }  
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
        public int DailyHours { get; set; }
        public int LeaveDays { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set;}
        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
