using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please Enter Employee Name")]
        [StringLength(200)]
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public double Salary { get; set; }
        public string? Designation { get; set; }
        public bool Is_Active { get; set; }
        public string? Created_By { get; set; }
        public DateTime? Created_Date { get; set; } = DateTime.Now;
        public string? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; } = DateTime.Now;

    }
}
