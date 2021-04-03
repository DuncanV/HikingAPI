-- CREATE DB
USE [master];
GO

-- COMMENT OUT TO DROP
-- DROP DATABASE [HikingAPI];
--GO

CREATE DATABASE [HikingAPI];
GO

USE [HikingAPI];
GO

/***** Create Duration ******/
CREATE TABLE [dbo].[Duration](
	[durationID] [int] IDENTITY(1,1) NOT NULL,
	[time] [float] DEFAULT 0.0
);
GO

ALTER TABLE [dbo].[Duration] ADD CONSTRAINT [durationID] PRIMARY KEY CLUSTERED ([durationID] ASC);
GO

/***** Create Point Of Interest ******/
CREATE TABLE [dbo].[PointOfInterest](
	[pointOfInterestID] [int] IDENTITY(1,1) NOT NULL,
	[desc] [varchar](255) DEFAULT NULL
);
GO

ALTER TABLE [dbo].[PointOfInterest] ADD CONSTRAINT [pointOfInterestID] PRIMARY KEY CLUSTERED ([pointOfInterestID] ASC);
GO

/***** Create Difficulty ******/
CREATE TABLE [dbo].[Difficulty](
	[difficultyID] [int] IDENTITY(1,1) NOT NULL,
	[desc] [varchar](32) DEFAULT NULL
);
GO

ALTER TABLE [dbo].[Difficulty] ADD CONSTRAINT [difficultyID] PRIMARY KEY CLUSTERED ([difficultyID] ASC);
GO

/***** Create User ******/
CREATE TABLE [dbo].[User](
    [userID] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](32) NOT NULL,
    [surname] [varchar](32) NOT NULL
);
GO

ALTER TABLE [dbo].[User] ADD CONSTRAINT [userID] PRIMARY KEY CLUSTERED ([userID] ASC);
GO

/***** Create Time ******/
CREATE TABLE [dbo].[Time](
	[timeID] [int] IDENTITY(1,1) NOT NULL,
	[desc] [varchar](32) DEFAULT NULL
);
GO

ALTER TABLE [dbo].[Time] ADD CONSTRAINT [timeID] PRIMARY KEY CLUSTERED ([timeID] ASC);
GO

/***** Create Day ******/
CREATE TABLE [dbo].[Day](
	[dayID] [int] IDENTITY(1,1) NOT NULL,
	[desc] [varchar](32) DEFAULT NULL
);
GO

ALTER TABLE [dbo].[Day] ADD CONSTRAINT [dayID] PRIMARY KEY CLUSTERED ([dayID] ASC);
GO

/***** Create Operating Hours ******/
CREATE TABLE [dbo].[OperatingHours](
    [operatingHoursID] [int] IDENTITY(1,1) NOT NULL,
	[time_from] [int] NOT NULL,
	[time_to] [int] NOT NULL,
    [day] [int] NOT NULL,
    FOREIGN KEY ([time_from]) REFERENCES [dbo].[Time](timeID),
    FOREIGN KEY ([time_to]) REFERENCES [dbo].[Time](timeID),
    FOREIGN KEY ([day]) REFERENCES [dbo].[Day](dayID)
);
GO

ALTER TABLE [dbo].[OperatingHours] ADD CONSTRAINT [operatingHoursID] PRIMARY KEY CLUSTERED ([operatingHoursID] ASC);
GO

/***** Create Faciltiy ******/
CREATE TABLE [dbo].[Facility](
    [facilityID] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](32) NOT NULL,
    [latitude] [float] DEFAULT NULL,
    [longitude] [float] DEFAULT NULL,
    [parking] [bit] DEFAULT 1,
    [pets] [bit] DEFAULT 1,
    [bookingRequired] [bit] DEFAULT 1
);
GO

ALTER TABLE [dbo].[Facility] ADD CONSTRAINT [facilityID] PRIMARY KEY CLUSTERED ([facilityID] ASC);
GO

/***** Create Facility Hours Link ******/
CREATE TABLE [dbo].[FacilityHoursLink](
	[facilityID] [int] NOT NULL,
	[operatingHoursID] [int] NOT NULL,
    FOREIGN KEY ([facilityID]) REFERENCES [dbo].[Facility](facilityID),
    FOREIGN KEY ([operatingHoursID]) REFERENCES [dbo].[OperatingHours](operatingHoursID)
);
GO

ALTER TABLE [dbo].[FacilityHoursLink] ADD CONSTRAINT [pkFacilityHoursLink] PRIMARY KEY CLUSTERED ([facilityID] ASC,[operatingHoursID] ASC);
GO

/***** Create Hike ******/
CREATE TABLE [dbo].[Hike](
    [hikeID] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](32) NOT NULL,
    [location] [geography] DEFAULT NULL,
    [enteranceFee] [smallmoney] DEFAULT 0.0,
    [distance] [float] DEFAULT 0.0,
    [difficultyID] [int] NOT NULL,
    [avgDuration] [int] NOT NULL,
    [desc] [varchar](255) DEFAULT NULL,
    [map] [varbinary](max) DEFAULT NULL,
    [maxGroupSize] [smallint] DEFAULT 1,
    [facilityID] [int] DEFAULT NULL,
    FOREIGN KEY ([difficultyID]) REFERENCES [dbo].[Difficulty](difficultyID),
    FOREIGN KEY ([avgDuration]) REFERENCES [dbo].[Duration](durationID),
    FOREIGN KEY ([facilityID]) REFERENCES [dbo].[Facility](facilityID)
);
GO

ALTER TABLE [dbo].[Hike] ADD CONSTRAINT [hikeID] PRIMARY KEY CLUSTERED ([hikeID] ASC);
GO

/***** Create Hike Log ******/
CREATE TABLE [dbo].[HikeLog](
	[hikeID] [int] NOT NULL,
	[userID] [int] NOT NULL,
    FOREIGN KEY ([hikeID]) REFERENCES [dbo].[Hike](hikeID),
    FOREIGN KEY ([userID]) REFERENCES [dbo].[User](userID)
);
GO

ALTER TABLE [dbo].[HikeLog] ADD CONSTRAINT [pkHikeLog] PRIMARY KEY CLUSTERED ([hikeID] ASC,[userID] ASC);
GO

/***** Create Hike Interest Link ******/
CREATE TABLE [dbo].[HikeInterestLink](
	[hikeID] [int] NOT NULL,
	[pointOfInterestID] [int] NOT NULL,
    FOREIGN KEY ([hikeID]) REFERENCES [dbo].[Hike](hikeID),
    FOREIGN KEY ([pointOfInterestID]) REFERENCES [dbo].[PointOfInterest](pointOfInterestID)
);
GO

ALTER TABLE [dbo].[HikeInterestLink] ADD CONSTRAINT [pkHikeInterestLink] PRIMARY KEY CLUSTERED ([hikeID] ASC,[pointOfInterestID] ASC);
GO

---------------------------------------------------------------
--INSERTS
---------------------------------------------------------------

INSERT INTO [dbo].[Day]
        ([desc])
     VALUES
        ('Monday'),
        ('Tuesday'),
        ('Wednesday'),
        ('Thursday'),
        ('Friday'),
        ('Saturday'),
        ('Sunday');

GO

INSERT INTO [dbo].[Difficulty]
           ([desc])
     VALUES
           ('Easy'),
           ('Amateur'),
           ('Intermediate'),
           ('Difficult'),
           ('Professional');
GO

INSERT INTO [dbo].[Time]
           ([desc])
     VALUES
            ('08:00'),
            ('08:30'),
            ('09:00'),
            ('09:30'),
            ('10:00'),
            ('10:30'),
            ('11:00'),
            ('11:30'),
            ('12:00'),
            ('12:30'),
            ('13:00'),
            ('13:30'),
            ('14:00'),
            ('14:30'),
            ('15:00'),
            ('15:30'),
            ('16:00'),
            ('16:30'),
            ('17:00'),
            ('17:30'),
            ('18:00');
GO


INSERT INTO [dbo].[Duration]
           ([time])
     VALUES
           (0.00),
           (0.25),
           (0.50),
           (0.75),
           (1.00),
           (1.25),
           (1.50),
           (1.75),
           (2.00),
           (2.25),
           (2.50),
           (2.75),
           (3.00),
           (3.25),
           (3.50),
           (3.75),
           (4.00),
           (4.25),
           (4.50),
           (4.75),
           (5.00),
           (5.25),
           (5.50),
           (5.75),
           (6);
GO

INSERT INTO [dbo].[User]
           ([name],[surname])
     VALUES
           ('Stuart','Barclay'),
           ('Kiara','Smith'),
           ('Thami','Xabanisa'),
           ('Keamo','Mfoloe'),
           ('Duncan','Vodden');
GO

INSERT INTO [dbo].[OperatingHours]
           ([time_from] ,[time_to] ,[day])
     VALUES
           (1, 11, 1),
           (1, 17, 2),
           (1, 17, 3),
           (1, 17, 4),
           (1, 17, 5),
           (1, 19, 6),
           (1, 19, 7);
GO

INSERT INTO [dbo].[PointOfInterest]
           ([desc])
     VALUES
           ('Non-dangerous wildlife'),
           ('Waterfall'),
           ('River'),
           ('Views of the ocean'),
           ('Views of the city'),
           ('Fossils'),
           ('Historic Creations');
GO

INSERT INTO [dbo].[Facility]
           ([name], [latitude], [longitude], [parking], [pets], [bookingRequired])
     VALUES
           ('Moreleta Kloof Nature Area', -25.8321 , 28.2914, 1, 0, 0),
           ('Chapmans Peak Drive', -34.0883, 18.3594, 1, 1, 0);
GO

INSERT INTO [dbo].[FacilityHoursLink]
           ([facilityID] ,[operatingHoursID])
     VALUES
            (1 ,1),
            (1 ,2),
            (1 ,3),
            (1 ,4),
            (1 ,5),
            (2 ,1),
            (2 ,2),
            (2 ,3),
            (2 ,4),
            (2 ,5),
            (1 ,6),
            (1 ,7);

GO

INSERT INTO [dbo].[Hike]
           ([name], [location], [enteranceFee], [distance], [difficultyID], [avgDuration], [desc], [map], [maxGroupSize], [facilityID])
     VALUES
           ('Chapmans Peak Hiking Trail', NULL, 42.00, 5, 3, 13, 'This is one of the most rewarding hikes in the Cape Peninsula with outstanding views, beautiful fynbos and the sound of the adjacent surf and horizons to hang your dreams onto. ', NULL, 10, 2),
            ('Rademeyer hiking trail', NULL, 0.00, 1.5, 1, 4, 'Walk among springboks, zebras, blesboks, impalas, duikers, and ostriches in a well-maintained nature reserve at the tributary of the perennial Moreleta Spruit', NULL, 5, 1),
            ('Suikerbos hiking trail', NULL, 0.00, 3.32, 4, 12, 'Suikerbos Trail is 3.3 kilometres long. These are quite manageable on your own, but guided walks are always recommended for those that want to reap the full benefit of the knowledge and experience of the guide.', NULL, 4, 1);
GO

INSERT INTO [dbo].[HikeLog]
           ([hikeID],[userID])
     VALUES
           (1,1),
           (2,1),
           (3,1),
           (1,2),
           (1,3),
           (2,4),
           (1,5);
GO

INSERT INTO [dbo].[HikeInterestLink]
           ([hikeID] ,[pointOfInterestID])
     VALUES
            (1 , 4),
            (1 , 5),
            (1 , 6),
            (1 , 7),
            (2 , 1),
            (2 , 2),
            (2 , 3),
            (2 , 5),
            (3 , 1),
            (3 , 5),
            (3 , 6),
            (3 , 7);
GO