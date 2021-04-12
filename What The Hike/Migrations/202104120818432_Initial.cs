namespace What_The_Hike.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Day",
                c => new
                    {
                        dayID = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.dayID);
            
            CreateTable(
                "dbo.Difficulty",
                c => new
                    {
                        difficultyID = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.difficultyID);
            
            CreateTable(
                "dbo.Duration",
                c => new
                    {
                        durationID = c.Int(nullable: false, identity: true),
                        time = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.durationID);
            
            CreateTable(
                "dbo.Facility",
                c => new
                    {
                        facilityID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        latitude = c.Single(nullable: false),
                        longitude = c.Single(nullable: false),
                        parking = c.Boolean(nullable: false),
                        pets = c.Boolean(nullable: false),
                        bookingRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.facilityID);
            
            CreateTable(
                "dbo.FacilityHoursLink",
                c => new
                    {
                        facilityHoursLinkID = c.Int(nullable: false, identity: true),
                        facilityID = c.Int(nullable: false),
                        operatingHoursID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.facilityHoursLinkID)
                .ForeignKey("dbo.Facility", t => t.facilityID)
                .ForeignKey("dbo.OperatingHours", t => t.operatingHoursID)
                .Index(t => t.facilityID)
                .Index(t => t.operatingHoursID);
            
            CreateTable(
                "dbo.OperatingHours",
                c => new
                    {
                        operatingHoursID = c.Int(nullable: false, identity: true),
                        time_from = c.Int(nullable: false),
                        time_to = c.Int(nullable: false),
                        day = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.operatingHoursID)
                .ForeignKey("dbo.Day", t => t.day)
                .ForeignKey("dbo.Time", t => t.time_from)
                .ForeignKey("dbo.Time", t => t.time_to)
                .Index(t => t.time_from)
                .Index(t => t.time_to)
                .Index(t => t.day);
            
            CreateTable(
                "dbo.Time",
                c => new
                    {
                        timeID = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.timeID);
            
            CreateTable(
                "dbo.Hike",
                c => new
                    {
                        hikeID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        map = c.Binary(nullable: false),
                        description = c.String(maxLength: 255, storeType: "nvarchar"),
                        coordinates = c.String(unicode: false),
                        distance = c.Single(nullable: false),
                        enteranceFee = c.Single(nullable: false),
                        maxGroupSize = c.Short(nullable: false),
                        difficultyID = c.Int(nullable: false),
                        avgDuration = c.Int(nullable: false),
                        facilityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.hikeID)
                .ForeignKey("dbo.Difficulty", t => t.difficultyID)
                .ForeignKey("dbo.Duration", t => t.avgDuration)
                .ForeignKey("dbo.Facility", t => t.facilityID)
                .Index(t => t.difficultyID)
                .Index(t => t.avgDuration)
                .Index(t => t.facilityID);
            
            CreateTable(
                "dbo.HikeInterestLink",
                c => new
                    {
                        hikeInterestID = c.Int(nullable: false, identity: true),
                        hikeID = c.Int(nullable: false),
                        pointOfInterestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.hikeInterestID)
                .ForeignKey("dbo.Hike", t => t.hikeID)
                .ForeignKey("dbo.PointOfInterest", t => t.pointOfInterestID)
                .Index(t => t.hikeID)
                .Index(t => t.pointOfInterestID);
            
            CreateTable(
                "dbo.PointOfInterest",
                c => new
                    {
                        pointOfInterestID = c.Int(nullable: false, identity: true),
                        description = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.pointOfInterestID);
            
            CreateTable(
                "dbo.HikeLog",
                c => new
                    {
                        hikeLogID = c.Int(nullable: false, identity: true),
                        hikeID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.hikeLogID)
                .ForeignKey("dbo.Hike", t => t.hikeID)
                .ForeignKey("dbo.User", t => t.userID)
                .Index(t => t.hikeID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 32, storeType: "nvarchar"),
                        surname = c.String(maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.userID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HikeLog", "userID", "dbo.User");
            DropForeignKey("dbo.HikeLog", "hikeID", "dbo.Hike");
            DropForeignKey("dbo.HikeInterestLink", "pointOfInterestID", "dbo.PointOfInterest");
            DropForeignKey("dbo.HikeInterestLink", "hikeID", "dbo.Hike");
            DropForeignKey("dbo.Hike", "facilityID", "dbo.Facility");
            DropForeignKey("dbo.Hike", "avgDuration", "dbo.Duration");
            DropForeignKey("dbo.Hike", "difficultyID", "dbo.Difficulty");
            DropForeignKey("dbo.FacilityHoursLink", "operatingHoursID", "dbo.OperatingHours");
            DropForeignKey("dbo.OperatingHours", "time_to", "dbo.Time");
            DropForeignKey("dbo.OperatingHours", "time_from", "dbo.Time");
            DropForeignKey("dbo.OperatingHours", "day", "dbo.Day");
            DropForeignKey("dbo.FacilityHoursLink", "facilityID", "dbo.Facility");
            DropIndex("dbo.HikeLog", new[] { "userID" });
            DropIndex("dbo.HikeLog", new[] { "hikeID" });
            DropIndex("dbo.HikeInterestLink", new[] { "pointOfInterestID" });
            DropIndex("dbo.HikeInterestLink", new[] { "hikeID" });
            DropIndex("dbo.Hike", new[] { "facilityID" });
            DropIndex("dbo.Hike", new[] { "avgDuration" });
            DropIndex("dbo.Hike", new[] { "difficultyID" });
            DropIndex("dbo.OperatingHours", new[] { "day" });
            DropIndex("dbo.OperatingHours", new[] { "time_to" });
            DropIndex("dbo.OperatingHours", new[] { "time_from" });
            DropIndex("dbo.FacilityHoursLink", new[] { "operatingHoursID" });
            DropIndex("dbo.FacilityHoursLink", new[] { "facilityID" });
            DropTable("dbo.User");
            DropTable("dbo.HikeLog");
            DropTable("dbo.PointOfInterest");
            DropTable("dbo.HikeInterestLink");
            DropTable("dbo.Hike");
            DropTable("dbo.Time");
            DropTable("dbo.OperatingHours");
            DropTable("dbo.FacilityHoursLink");
            DropTable("dbo.Facility");
            DropTable("dbo.Duration");
            DropTable("dbo.Difficulty");
            DropTable("dbo.Day");
        }
    }
}
