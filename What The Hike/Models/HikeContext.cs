using MySql.Data.EntityFramework;
using System;
using System.Data.Common;
using System.Data.Entity;

namespace What_The_Hike
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class HikeContext : DbContext
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


		public HikeContext() : base() { }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
			base.OnModelCreating(modelBuilder);
		}
	}
}