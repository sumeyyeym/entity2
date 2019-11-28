using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EfConfigurations.ORM.Entities.EntitySplitting;
using EfConfigurations.ORM.Entities.ManyToMany;

namespace EfConfigurations.ORM.Context
{
    class MyContext : DbContext //bunun çalışması için using System.Data.Entity; ekle
    {
        public MyContext()
        {
            Database.Connection.ConnectionString = "Server=.; database=EFDb; uid = sa; pwd = 123";
        }

        public virtual DbSet<Employee> Employees { get; set; } //<Employee> çalışması için using EfConfigurations.ORM.Entities.EntitySplitting; eklenmesi gerek. eklenmezse hata verir. ctrl + . yaparsan otomatik olarak ekleyebilirsin

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //table splitting

            modelBuilder.Entity<Employee>()
                .HasKey(p => p.EmployeeID) //primary key belirler
                .ToTable("Employees");

            modelBuilder.Entity<EmployeeContactDetail>()
                .HasKey(p => p.EmployeeID)
                .ToTable("Employees"); //iki classın da aynı taploda yer almasını sağladık

            modelBuilder.Entity<Employee>()
                .HasRequired(p => p.EmployeeContactDetail) //bir personelin detayı olmak zorundadır
                .WithRequiredPrincipal(p => p.Employee); //bir detay bir personele bağlı olmak zorundadır

            base.OnModelCreating(modelBuilder);


            //many to many

            modelBuilder.Entity<Teacher>()
                .HasMany(x => x.Students)
                .WithMany(x => x.Teachers)
                .Map(map =>
                {
                    map.ToTable("TeacherStudents");
                    map.MapLeftKey("TeacherId");
                    map.MapRightKey("StudentId");
                });
            base.OnModelCreating(modelBuilder);
        }


    }
}
