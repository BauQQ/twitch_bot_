-- phpMyAdmin SQL Dump
-- version 4.9.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 06, 2020 at 06:36 PM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `chatbot`
--

-- --------------------------------------------------------

--
-- Table structure for table `chat_bans`
--

CREATE TABLE `chat_bans` (
  `cid` int(11) NOT NULL,
  `id` int(11) NOT NULL,
  `time` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `commands`
--

CREATE TABLE `commands` (
  `id` int(11) NOT NULL,
  `cmd` varchar(255) NOT NULL,
  `name` longtext NOT NULL,
  `enabled` tinyint(1) NOT NULL DEFAULT 1,
  `dcmd` varchar(255) NOT NULL,
  `userlevel` int(2) NOT NULL DEFAULT 0,
  `ucd` int(11) NOT NULL DEFAULT 0,
  `gcd` int(11) NOT NULL DEFAULT 0,
  `cost` int(11) NOT NULL DEFAULT 0,
  `respons` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `commands`
--

INSERT INTO `commands` (`id`, `cmd`, `name`, `enabled`, `dcmd`, `userlevel`, `ucd`, `gcd`, `cost`, `respons`) VALUES
(1, '!mycommands', 'commands', 1, 'commands', 0, 0, 0, 0, ''),
(2, '!berganderf', 'Berganderf', 1, 'berganderf', 0, 0, 0, 0, 'Hello Sir ${user}, welcome back to the channel, i hope you have a nice day.'),
(3, '!purpose', 'purpose', 1, 'purpose', 0, 0, 0, 0, 'I have no purpose, yet..'),
(4, '!rps', 'Rock Paper Siccors', 1, 'rps', 0, 0, 0, 0, '${dcmd} ${query} ${rounds} ${wager}'),
(5, '!rpsaccept', 'Rock Paper Siccors', 1, 'rps', 0, 0, 0, 0, '${dcmd}'),
(6, '!rpsdeny', 'Rock Paper Siccors', 1, 'rps', 0, 0, 0, 0, '${dcmd}'),
(7, '!rpscancel', 'Rock Paper Siccors', 1, 'rps', 0, 0, 0, 0, '${dcmd}'),
(8, '!demo', 'Demo test', 1, 'demo', 0, 20, 5, 0, '${user} this is a demo command!'),
(9, '!math', 'math test', 1, 'math', 0, 0, 0, 0, '${dcmd} ${number} ${number2}'),
(10, '!RRAccept', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(11, '!RRdeny', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(12, '!RRshoot', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(13, '!RRspin', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(14, '!RRcancel', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(15, '!rradmcancel', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd}'),
(16, '!RRoullete', 'Russian Roullete', 1, 'rroullete', 0, 0, 0, 0, '${dcmd} ${target} ${gunsize} ${bullets} ${wager}');

-- --------------------------------------------------------

--
-- Table structure for table `command_alias`
--

CREATE TABLE `command_alias` (
  `i` int(11) NOT NULL,
  `id` int(11) NOT NULL,
  `a_cmd` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `command_alias`
--

INSERT INTO `command_alias` (`i`, `id`, `a_cmd`) VALUES
(1, 2, 'berganderf'),
(2, 2, 'Berganderf'),
(3, 4, 'rps'),
(4, 5, 'rpsaccept'),
(5, 6, 'rpsdeny'),
(6, 6, 'rpscancel'),
(7, 10, 'rraccept'),
(8, 10, 'Rraccept'),
(9, 10, 'rRaccept'),
(10, 11, 'rrdeny'),
(11, 11, 'Rrdeny'),
(12, 11, 'rRdeny'),
(13, 12, 'Rrshoot'),
(14, 12, 'rRshoot'),
(16, 12, 'rrshoot'),
(17, 13, 'Rrspin'),
(18, 13, 'rRspin'),
(19, 13, 'rrspin'),
(20, 14, 'Rrcancel'),
(21, 14, 'rRcancel'),
(22, 14, 'rrcancel'),
(23, 15, 'Rradmcancel'),
(24, 15, 'rRadmcancel'),
(25, 15, 'rradmcancel'),
(26, 16, 'RRoullete'),
(27, 16, 'Rroullete'),
(28, 16, 'rRoullete'),
(29, 16, 'rroullete');

-- --------------------------------------------------------

--
-- Table structure for table `command_locked`
--

CREATE TABLE `command_locked` (
  `i` int(11) NOT NULL,
  `id` int(11) NOT NULL,
  `locked` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `command_locked`
--

INSERT INTO `command_locked` (`i`, `id`, `locked`) VALUES
(1, 2, 2),
(2, 2, 1),
(3, 15, 1);

-- --------------------------------------------------------

--
-- Table structure for table `viewers`
--

CREATE TABLE `viewers` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `level` int(2) NOT NULL,
  `subscriber` tinyint(1) NOT NULL,
  `chatMod` tinyint(1) NOT NULL DEFAULT 0,
  `points` bigint(20) NOT NULL DEFAULT 0,
  `activityStamp` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `viewers`
--

INSERT INTO `viewers` (`id`, `username`, `level`, `subscriber`, `chatMod`, `points`, `activityStamp`) VALUES
(1, 'luffaow', 2, 1, 1, 1000, '2020-01-05 13:47:59'),
(2, 'berganderf', 0, 1, 0, 10000, '2020-01-05 14:05:35'),
(3, 'spectrapulse', 1, 1, 0, 100, '2020-01-05 14:05:52'),
(4, 'commanderroot', 0, 0, 0, 0, '2020-01-05 14:07:10');

-- --------------------------------------------------------

--
-- Table structure for table `viewer_blacklist`
--

CREATE TABLE `viewer_blacklist` (
  `id` int(11) NOT NULL,
  `permanent` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `viewer_blacklist`
--

INSERT INTO `viewer_blacklist` (`id`, `permanent`) VALUES
(4, 1);

-- --------------------------------------------------------

--
-- Table structure for table `viewer_credits_list`
--

CREATE TABLE `viewer_credits_list` (
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `viewer_credits_list`
--

INSERT INTO `viewer_credits_list` (`id`) VALUES
(1);

-- --------------------------------------------------------

--
-- Table structure for table `viewer_twitchauto`
--

CREATE TABLE `viewer_twitchauto` (
  `i` int(11) NOT NULL,
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `TwitchAuto` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `viewer_twitchauto`
--

INSERT INTO `viewer_twitchauto` (`i`, `id`, `username`, `TwitchAuto`) VALUES
(1, 1, 'luffaow', 'berganderfauto'),
(2, 2, 'berganderf', 'berganderfauto'),
(3, 1, 'luffaow', 'demo');

-- --------------------------------------------------------

--
-- Table structure for table `viewer_whitelist`
--

CREATE TABLE `viewer_whitelist` (
  `id` int(11) NOT NULL,
  `permanent` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `viewer_whitelist`
--

INSERT INTO `viewer_whitelist` (`id`, `permanent`) VALUES
(1, 1),
(2, 0),
(3, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `chat_bans`
--
ALTER TABLE `chat_bans`
  ADD PRIMARY KEY (`cid`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `commands`
--
ALTER TABLE `commands`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `command_alias`
--
ALTER TABLE `command_alias`
  ADD PRIMARY KEY (`i`);

--
-- Indexes for table `command_locked`
--
ALTER TABLE `command_locked`
  ADD PRIMARY KEY (`i`);

--
-- Indexes for table `viewers`
--
ALTER TABLE `viewers`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `viewer_blacklist`
--
ALTER TABLE `viewer_blacklist`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `viewer_credits_list`
--
ALTER TABLE `viewer_credits_list`
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `viewer_twitchauto`
--
ALTER TABLE `viewer_twitchauto`
  ADD PRIMARY KEY (`i`);

--
-- Indexes for table `viewer_whitelist`
--
ALTER TABLE `viewer_whitelist`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `chat_bans`
--
ALTER TABLE `chat_bans`
  MODIFY `cid` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `commands`
--
ALTER TABLE `commands`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `command_alias`
--
ALTER TABLE `command_alias`
  MODIFY `i` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT for table `command_locked`
--
ALTER TABLE `command_locked`
  MODIFY `i` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `viewers`
--
ALTER TABLE `viewers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `viewer_credits_list`
--
ALTER TABLE `viewer_credits_list`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `viewer_twitchauto`
--
ALTER TABLE `viewer_twitchauto`
  MODIFY `i` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
