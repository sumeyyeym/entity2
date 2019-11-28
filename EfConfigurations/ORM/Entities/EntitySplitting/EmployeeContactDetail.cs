namespace EfConfigurations.ORM.Entities.EntitySplitting
{
    using System.ComponentModel.DataAnnotations;
    public class EmployeeContactDetail
    {
        public int EmployeeID { get; set; }
        [MaxLength(150)]
        public string EMail { get; set; }
        [MaxLength(24)]
        public string Phone { get; set; }
        public virtual Employee Employee { get; set; } // iki tabloyu bir birine navige etmek için kullanıyoruz.
    }
}
