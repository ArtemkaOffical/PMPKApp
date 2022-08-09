using Microsoft.EntityFrameworkCore;
using PMPK.Models;

namespace PMPK.DAL
{

    public class PMPKContext : DbContext
    {
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Children> Childrens { get; set; }
        public DbSet<Specialists> Specialists { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<FormStudies> FormStudies { get; set; }
        public DbSet<EdOrg> EdOrgs { get; set; }
        public DbSet<PlaceStudy> PlaceStudies { get; set; }
        public DbSet<PlaceOfPMPK> PlaceOfPMPKs { get; set; }
        public DbSet<AdaptiveProgram> AdaptivePrograms { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Models.PMPK> PMPKs { get; set; }

        public PMPKContext()
        {
            Database.EnsureCreated();
        }

        //Переобпределение поведения базового метода
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.OVZ).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.NonRuss).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.GIA9).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.GIA11).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.Control).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.Direction).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.Programm).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.Invalid).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.MSE).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().Property(pmpk => pmpk.FirstPriem).HasDefaultValue(0);
            modelBuilder.Entity<Models.PMPK>().HasIndex(pmpk => pmpk.NumberOfProtocol).IsUnique();
            modelBuilder.Entity<Children>().Property(child => child.MalIm).HasDefaultValue(0);
            modelBuilder.Entity<Children>().HasIndex(child => child.FullName).IsUnique();
            modelBuilder.Entity<Children>().Property(child => child.Mnogodet).HasDefaultValue(0);
            modelBuilder.Entity<Children>().Property(child => child.Sirota).HasDefaultValue(0);

        }
       
        //Переобпределение поведения базового метода
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        
            optionsBuilder.UseSqlServer(DBUtils.GetConnectionString());
        }
    }
}
