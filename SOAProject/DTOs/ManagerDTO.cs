using SOAProject.Models;

namespace SOAProject.DTOs
{
    public class ManagerDTO
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
        public int DailyHours { get; set; }
        public int LeaveDays { get; set; }
        public int DepartmentId { get; set; }
    }
}
