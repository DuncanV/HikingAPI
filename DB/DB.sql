-- phpMyAdmin SQL Dump
-- version 5.1.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 05, 2021 at 12:09 AM
-- Server version: 10.4.18-MariaDB
-- PHP Version: 8.0.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+02:00";

CREATE DATABASE hikingapi;
USE hikingapi;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `hikingapi`
--

-- --------------------------------------------------------

--
-- Table structure for table `day`
--

CREATE TABLE `day` (
  `dayID` int(11) NOT NULL,
  `description` varchar(32) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `day`
--

INSERT INTO `day` (`dayID`, `description`) VALUES
(1, 'Monday'),
(2, 'Tuesday'),
(3, 'Wednesday'),
(4, 'Thursday'),
(5, 'Friday'),
(6, 'Saturday'),
(7, 'Sunday');

-- --------------------------------------------------------

--
-- Table structure for table `difficulty`
--

CREATE TABLE `difficulty` (
  `difficultyID` int(11) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `difficulty`
--

INSERT INTO `difficulty` (`difficultyID`, `description`) VALUES
(1, 'Easy'),
(2, 'Amateur'),
(3, 'Intermediate'),
(4, 'Difficult'),
(5, 'Professional');

-- --------------------------------------------------------

--
-- Table structure for table `duration`
--

CREATE TABLE `duration` (
  `durationID` int(11) NOT NULL,
  `time` float DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `duration`
--

INSERT INTO `duration` (`durationID`, `time`) VALUES
(1, 0),
(2, 0.25),
(3, 0.5),
(4, 0.75),
(5, 1),
(6, 1.25),
(7, 1.5),
(8, 1.75),
(9, 2),
(10, 2.25),
(11, 2.5),
(12, 2.75),
(13, 3),
(14, 3.25),
(15, 3.5),
(16, 3.75),
(17, 4),
(18, 4.25),
(19, 4.5),
(20, 4.75),
(21, 5),
(22, 5.25),
(23, 5.5),
(24, 5.75),
(25, 6);

-- --------------------------------------------------------

--
-- Table structure for table `facility`
--

CREATE TABLE `facility` (
  `facilityID` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `latitude` float DEFAULT NULL,
  `longitude` float DEFAULT NULL,
  `parking` tinyint(1) NOT NULL DEFAULT 1,
  `pets` tinyint(1) NOT NULL DEFAULT 1,
  `bookingRequired` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `facility`
--

INSERT INTO `facility` (`facilityID`, `name`, `latitude`, `longitude`, `parking`, `pets`, `bookingRequired`) VALUES
(1, 'Moreleta Kloof Nature Area', -25.8321, 28.2914, 1, 0, 0),
(2, 'Chapmans Peak Drive', -34.0883, 18.3594, 1, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `facilityhourslink`
--

CREATE TABLE `facilityhourslink` (
  `facilityHoursLinkID` int(11) NOT NULL,
  `facilityID` int(11) NOT NULL,
  `operatingHoursID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `facilityhourslink`
--

INSERT INTO `facilityhourslink` (`facilityHoursLinkID`, `facilityID`, `operatingHoursID`) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 1, 3),
(4, 1, 4),
(5, 1, 5),
(6, 2, 1),
(7, 2, 2),
(8, 2, 3),
(9, 2, 4),
(10, 2, 5),
(11, 1, 6),
(12, 1, 7);

-- --------------------------------------------------------

--
-- Table structure for table `hike`
--

CREATE TABLE `hike` (
  `hikeID` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `coordinates` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`coordinates`)),
  `enteranceFee` float NOT NULL DEFAULT 0,
  `distance` float NOT NULL DEFAULT 0,
  `difficultyID` int(11) NOT NULL,
  `avgDuration` int(11) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `map` longblob DEFAULT NULL,
  `maxGroupSize` smallint(6) NOT NULL DEFAULT 1,
  `facilityID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `hike`
--

INSERT INTO `hike` (`hikeID`, `name`, `coordinates`, `enteranceFee`, `distance`, `difficultyID`, `avgDuration`, `description`, `map`, `maxGroupSize`, `facilityID`) VALUES
(1, 'Chapmans Peak Hiking Trail', NULL, 42, 5, 3, 13, 'This is one of the most rewarding hikes in the Cape Peninsula with outstanding views, beautiful fynbos and the sound of the adjacent surf and horizons to hang your dreams onto. ', NULL, 10, 2),
(2, 'Rademeyer hiking trail', NULL, 0, 1.5, 1, 4, 'Walk among springboks, zebras, blesboks, impalas, duikers, and ostriches in a well-maintained nature reserve at the tributary of the perennial Moreleta Spruit', NULL, 5, 1),
(3, 'Suikerbos hiking trail', NULL, 0, 3.32, 4, 12, 'Suikerbos Trail is 3.3 kilometres long. These are quite manageable on your own, but guided walks are always recommended for those that want to reap the full benefit of the knowledge and experience of the guide.', NULL, 4, 1);

-- --------------------------------------------------------

--
-- Table structure for table `hikeinterestlink`
--

CREATE TABLE `hikeinterestlink` (
  `hikeInterestID` int(11) NOT NULL,
  `hikeID` int(11) NOT NULL,
  `pointOfInterestID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `hikeinterestlink`
--

INSERT INTO `hikeinterestlink` (`hikeInterestID`, `hikeID`, `pointOfInterestID`) VALUES
(1, 1, 4),
(2, 1, 5),
(3, 1, 6),
(4, 1, 7),
(5, 2, 1),
(6, 2, 2),
(7, 2, 3),
(8, 2, 5),
(9, 3, 1),
(10, 3, 5),
(11, 3, 6),
(12, 3, 7);

-- --------------------------------------------------------

--
-- Table structure for table `hikelog`
--

CREATE TABLE `hikelog` (
  `hikeLogID` int(11) NOT NULL,
  `hikeID` int(11) NOT NULL,
  `userID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `hikelog`
--

INSERT INTO `hikelog` (`hikeLogID`, `hikeID`, `userID`) VALUES
(1, 1, 1),
(2, 2, 1),
(3, 3, 1),
(4, 1, 2),
(5, 1, 3),
(6, 2, 4),
(7, 1, 5);

-- --------------------------------------------------------

--
-- Table structure for table `operatinghours`
--

CREATE TABLE `operatinghours` (
  `operatingHoursID` int(11) NOT NULL,
  `time_from` int(11) NOT NULL,
  `time_to` int(11) NOT NULL,
  `day` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `operatinghours`
--

INSERT INTO `operatinghours` (`operatingHoursID`, `time_from`, `time_to`, `day`) VALUES
(1, 1, 11, 1),
(2, 1, 17, 2),
(3, 1, 17, 3),
(4, 1, 17, 4),
(5, 1, 17, 5),
(6, 1, 19, 6),
(7, 1, 19, 7);

-- --------------------------------------------------------

--
-- Table structure for table `pointofinterest`
--

CREATE TABLE `pointofinterest` (
  `pointOfInterestID` int(11) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `pointofinterest`
--

INSERT INTO `pointofinterest` (`pointOfInterestID`, `description`) VALUES
(1, 'Non-dangerous wildlife'),
(2, 'Waterfall'),
(3, 'River'),
(4, 'Views of the ocean'),
(5, 'Views of the city'),
(6, 'Fossils'),
(7, 'Historic Creations');

-- --------------------------------------------------------

--
-- Table structure for table `time`
--

CREATE TABLE `time` (
  `timeID` int(11) NOT NULL,
  `description` varchar(32) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `time`
--

INSERT INTO `time` (`timeID`, `description`) VALUES
(1, '08:00'),
(2, '08:30'),
(3, '09:00'),
(4, '09:30'),
(5, '10:00'),
(6, '10:30'),
(7, '11:00'),
(8, '11:30'),
(9, '12:00'),
(10, '12:30'),
(11, '13:00'),
(12, '13:30'),
(13, '14:00'),
(14, '14:30'),
(15, '15:00'),
(16, '15:30'),
(17, '16:00'),
(18, '16:30'),
(19, '17:00'),
(20, '17:30'),
(21, '18:00');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `userID` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `surname` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`userID`, `name`, `surname`) VALUES
(1, 'Stuart', 'Barclay'),
(2, 'Kiara', 'Smith'),
(3, 'Thami', 'Xabanisa'),
(4, 'Keamo', 'Mfoloe'),
(5, 'Duncan', 'Vodden');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `day`
--
ALTER TABLE `day`
  ADD PRIMARY KEY (`dayID`);

--
-- Indexes for table `difficulty`
--
ALTER TABLE `difficulty`
  ADD PRIMARY KEY (`difficultyID`);

--
-- Indexes for table `duration`
--
ALTER TABLE `duration`
  ADD PRIMARY KEY (`durationID`);

--
-- Indexes for table `facility`
--
ALTER TABLE `facility`
  ADD PRIMARY KEY (`facilityID`);

--
-- Indexes for table `facilityhourslink`
--
ALTER TABLE `facilityhourslink`
  ADD PRIMARY KEY (`facilityHoursLinkID`),
  ADD KEY `facility_hourslink` (`facilityID`),
  ADD KEY `operatingHours_hoursLink` (`operatingHoursID`);

--
-- Indexes for table `hike`
--
ALTER TABLE `hike`
  ADD PRIMARY KEY (`hikeID`),
  ADD KEY `difficulty` (`difficultyID`),
  ADD KEY `avgDuration` (`avgDuration`),
  ADD KEY `facility_hike` (`facilityID`);

--
-- Indexes for table `hikeinterestlink`
--
ALTER TABLE `hikeinterestlink`
  ADD PRIMARY KEY (`hikeInterestID`),
  ADD KEY `poi_hikeinterestlink` (`pointOfInterestID`),
  ADD KEY `hike_hikeinterestlink` (`hikeID`);

--
-- Indexes for table `hikelog`
--
ALTER TABLE `hikelog`
  ADD PRIMARY KEY (`hikeLogID`),
  ADD KEY `hike_hikelog` (`hikeID`),
  ADD KEY `user_hikelog` (`userID`);

--
-- Indexes for table `operatinghours`
--
ALTER TABLE `operatinghours`
  ADD PRIMARY KEY (`operatingHoursID`),
  ADD KEY `time_from` (`time_from`),
  ADD KEY `time_to` (`time_to`),
  ADD KEY `day` (`day`);

--
-- Indexes for table `pointofinterest`
--
ALTER TABLE `pointofinterest`
  ADD PRIMARY KEY (`pointOfInterestID`);

--
-- Indexes for table `time`
--
ALTER TABLE `time`
  ADD PRIMARY KEY (`timeID`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`userID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `day`
--
ALTER TABLE `day`
  MODIFY `dayID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `difficulty`
--
ALTER TABLE `difficulty`
  MODIFY `difficultyID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `duration`
--
ALTER TABLE `duration`
  MODIFY `durationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `facility`
--
ALTER TABLE `facility`
  MODIFY `facilityID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `facilityhourslink`
--
ALTER TABLE `facilityhourslink`
  MODIFY `facilityHoursLinkID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `hike`
--
ALTER TABLE `hike`
  MODIFY `hikeID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `hikeinterestlink`
--
ALTER TABLE `hikeinterestlink`
  MODIFY `hikeInterestID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `hikelog`
--
ALTER TABLE `hikelog`
  MODIFY `hikeLogID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `operatinghours`
--
ALTER TABLE `operatinghours`
  MODIFY `operatingHoursID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `pointofinterest`
--
ALTER TABLE `pointofinterest`
  MODIFY `pointOfInterestID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `time`
--
ALTER TABLE `time`
  MODIFY `timeID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `userID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `facilityhourslink`
--
ALTER TABLE `facilityhourslink`
  ADD CONSTRAINT `facility_hoursLink` FOREIGN KEY (`facilityID`) REFERENCES `facility` (`facilityID`),
  ADD CONSTRAINT `operatingHours_hoursLink` FOREIGN KEY (`operatingHoursID`) REFERENCES `operatinghours` (`operatingHoursID`);

--
-- Constraints for table `hike`
--
ALTER TABLE `hike`
  ADD CONSTRAINT `duration_hike` FOREIGN KEY (`avgDuration`) REFERENCES `duration` (`durationID`),
  ADD CONSTRAINT `difficulty_hike` FOREIGN KEY (`difficultyID`) REFERENCES `difficulty` (`difficultyID`),
  ADD CONSTRAINT `facility_hike` FOREIGN KEY (`facilityID`) REFERENCES `facility` (`facilityID`);

--
-- Constraints for table `hikeinterestlink`
--
ALTER TABLE `hikeinterestlink`
  ADD CONSTRAINT `hike_hikeInterestLink` FOREIGN KEY (`hikeID`) REFERENCES `hike` (`hikeID`),
  ADD CONSTRAINT `poi_hikeInterestLink` FOREIGN KEY (`pointOfInterestID`) REFERENCES `pointofinterest` (`pointOfInterestID`);

--
-- Constraints for table `hikelog`
--
ALTER TABLE `hikelog`
  ADD CONSTRAINT `hike_hikelog` FOREIGN KEY (`hikeID`) REFERENCES `hike` (`hikeID`),
  ADD CONSTRAINT `user_hikelog` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`);

--
-- Constraints for table `operatinghours`
--
ALTER TABLE `operatinghours`
  ADD CONSTRAINT `day_operatingHours` FOREIGN KEY (`day`) REFERENCES `day` (`dayID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `time_from_operatingHours` FOREIGN KEY (`time_from`) REFERENCES `time` (`timeID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `time_to_operatingHours` FOREIGN KEY (`time_to`) REFERENCES `time` (`timeID`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
