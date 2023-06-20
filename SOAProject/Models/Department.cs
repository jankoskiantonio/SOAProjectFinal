using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOAProject.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public Manager? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
