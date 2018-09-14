/*
Navicat MySQL Data Transfer

Source Server         : 192.16.10.111_3306
Source Server Version : 50554
Source Host           : 192.16.10.111:3306
Source Database       : firewallrules

Target Server Type    : MYSQL
Target Server Version : 50554
File Encoding         : 65001

Date: 2018-07-11 22:09:24
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for APC
-- ----------------------------
DROP TABLE IF EXISTS `APC`;
CREATE TABLE `APC` (
  `protocol` varchar(255) DEFAULT NULL,
  `rules` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of APC
-- ----------------------------
INSERT INTO `APC` VALUES ('http', 'iptables -A INPUT -p tcp --dport 80 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 80 -j ACCEPT && iptables -A FORWARD -p tcp --sport 80 -j ACCEPT && iptables -A FORWARD -p tcp --dport 80 -j ACCEPT');
INSERT INTO `APC` VALUES ('telnet', 'iptables -A INPUT -p tcp --dport 23 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 23 -j ACCEPT && iptables -A FORWARD -p tcp --sport 23  -j ACCEPT && iptables -A FORWARD -p tcp --dport 23  -j ACCEPT');
INSERT INTO `APC` VALUES ('pop3', 'iptables -A INPUT -p tcp --dport 110 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 110 -j ACCEPT && iptables -A FORWARD -p tcp --sport 110  -j ACCEPT && iptables -A FORWARD -p tcp --dport 110  -j ACCEPT');
INSERT INTO `APC` VALUES ('ftp', 'iptables -A INPUT -p tcp --dport 21 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 21 -j ACCEPT && iptables -A FORWARD -p tcp --sport 21 -j ACCEPT && iptables -A FORWARD -p tcp --dport 21 -j ACCEPT');
INSERT INTO `APC` VALUES ('smtp', 'iptables -A INPUT -p tcp --dport 25 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 25 -j ACCEPT && iptables -A FORWARD -p tcp --sport 25  -j ACCEPT && iptables -A FORWARD -p tcp --dport 25  -j ACCEPT');
INSERT INTO `APC` VALUES ('ssh', 'iptables -A INPUT -p tcp --dport 22 -j ACCEPT && iptables -A OUTPUT -p tcp --dport 22 -j ACCEPT && iptables -A FORWARD -p tcp --sport 22 -j ACCEPT && iptables -A FORWARD -p tcp --dport 22 -j ACCEPT');

-- ----------------------------
-- Table structure for CNC
-- ----------------------------
DROP TABLE IF EXISTS `CNC`;
CREATE TABLE `CNC` (
  `rules` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of CNC
-- ----------------------------

-- ----------------------------
-- Table structure for DPI
-- ----------------------------
DROP TABLE IF EXISTS `DPI`;
CREATE TABLE `DPI` (
  `rules` varchar(1000) CHARACTER SET utf8 NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of DPI
-- ----------------------------

-- ----------------------------
-- Table structure for NAT
-- ----------------------------
DROP TABLE IF EXISTS `NAT`;
CREATE TABLE `NAT` (
  `rules` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of NAT
-- ----------------------------

-- ----------------------------
-- Table structure for POW
-- ----------------------------
DROP TABLE IF EXISTS `POW`;
CREATE TABLE `POW` (
  `rules` varchar(255) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of POW
-- ----------------------------

-- ----------------------------
-- Table structure for PRT
-- ----------------------------
DROP TABLE IF EXISTS `PRT`;
CREATE TABLE `PRT` (
  `rules` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of PRT
-- ----------------------------

-- ----------------------------
-- Table structure for STD
-- ----------------------------
DROP TABLE IF EXISTS `STD`;
CREATE TABLE `STD` (
  `rules` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of STD
-- ----------------------------

-- ----------------------------
-- Table structure for WHL
-- ----------------------------
DROP TABLE IF EXISTS `WHL`;
CREATE TABLE `WHL` (
  `rules` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of WHL
-- ----------------------------
