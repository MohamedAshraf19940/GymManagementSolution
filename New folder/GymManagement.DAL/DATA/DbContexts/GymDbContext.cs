using GymManagement.DAL.DATA.Configurations;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.DATA.DbContexts
{
    public class GymDbContext :IdentityDbContext<ApplicationUser>
    {

   
        public GymDbContext(DbContextOptions<GymDbContext> options):base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-4M2VPDA\\MSSQLSERVER02;Database=GymManagementMinic;Trusted_Connection=true;TrustServerCertificate=true");
        
        
        
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);//For Identity Configuration

            //
            //
            //
            //
            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.Property(u => u.FirstName).HasColumnType("varchar").HasMaxLength(50);
                eb.Property(u => u.LastName).HasColumnType("varchar").HasMaxLength(50);
            
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);


        }




        //public DbSet<ApplicationUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
        //public DbSet<IdentityUserRole<string>> UserRoles { get; set; }

        ////////////////////////////////////////////////////////////
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords     { get; set; }
        public DbSet<MemberShip> MemberShips     { get; set; }
        public DbSet<Booking> Bookings     { get; set; }


    }
}
