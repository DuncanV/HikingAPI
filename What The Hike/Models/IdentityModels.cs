using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.EntityFramework;

namespace What_The_Hike.Models
{
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(HikeContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(HikeContext context)
            : base(context)
        {
        }
    }
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [DataMember]
        [Required]
        public int userID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("userID")]
        public virtual User User { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class HikeContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbSet<Day> Day { get; set; }
        public DbSet<Difficulty> Difficulty { get; set; }
        public DbSet<Duration> Duration { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<FacilityHoursLink> FacilityHoursLink { get; set; }
        public DbSet<Hike> Hike { get; set; }
        public DbSet<HikeInterestLink> HikeInterestLink { get; set; }
        public DbSet<HikeLog> HikeLog { get; set; }
        public DbSet<OperatingHours> OperatingHours { get; set; }
        public DbSet<PointOfInterest> PointOfInterest { get; set; }
        public DbSet<Time> Time { get; set; }
        public DbSet<User> User { get; set; }

        public HikeContext() : base("HikeContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            base.OnModelCreating(modelBuilder);
        }

        public static HikeContext Create()
        {
            return new HikeContext();
        }
    }
}