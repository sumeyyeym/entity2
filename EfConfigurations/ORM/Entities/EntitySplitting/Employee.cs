namespace EfConfigurations.ORM.Entities.EntitySplitting
{
    using System.ComponentModel.DataAnnotations;
    public class Employee
    {
        public int EmployeeID { get; set; }

        [MaxLength(50)] //bunların çalışması için using System.ComponentModel.DataAnnotations; gerekli
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }
        
        public virtual EmployeeContactDetail EmployeeContactDetail { get; set; }
    }
}
