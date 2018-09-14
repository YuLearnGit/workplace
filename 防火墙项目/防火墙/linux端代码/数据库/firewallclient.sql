/*
Navicat MySQL Data Transfer

Source Server         : 192.16.10.8_3306
Source Server Version : 50554
Source Host           : 192.16.10.8:3306
Source Database       : firewallclient

Target Server Type    : MYSQL
Target Server Version : 50554
File Encoding         : 65001

Date: 2018-06-06 19:45:48
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for apc
-- ----------------------------
DROP TABLE IF EXISTS `apc`;
CREATE TABLE `apc` (
  `protocol` varchar(255) NOT NULL,
  `status` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of apc
-- ----------------------------
INSERT INTO `apc` VALUES ('http', 'allow');
INSERT INTO `apc` VALUES ('ftp', 'allow');
INSERT INTO `apc` VALUES ('smtp', 'allow');
INSERT INTO `apc` VALUES ('pop3', 'allow');
INSERT INTO `apc` VALUES ('telnet', 'allow');
INSERT INTO `apc` VALUES ('ssh', 'allow');

-- ----------------------------
-- Table structure for arp
-- ----------------------------
DROP TABLE IF EXISTS `arp`;
CREATE TABLE `arp` (
  `fwMAC` varchar(255) DEFAULT NULL,
  `gateIP` varchar(255) DEFAULT NULL,
  `gateMAC` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of arp
-- ----------------------------
INSERT INTO `arp` VALUES ('00:0b:ab:c6:5f:c5', '172.16.10.254', '08:57:00:e5:87:a7');

-- ----------------------------
-- Table structure for cnc
-- ----------------------------
DROP TABLE IF EXISTS `cnc`;
CREATE TABLE `cnc` (
  `devIP` varchar(255) NOT NULL,
  `connlimit` int(11) NOT NULL,
  `srcIP` varchar(255) DEFAULT NULL,
  `dstIP` varchar(255) DEFAULT NULL,
  `sport` varchar(255) DEFAULT NULL,
  `dport` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of cnc
-- ----------------------------

-- ----------------------------
-- Table structure for dnat
-- ----------------------------
DROP TABLE IF EXISTS `dnat`;
CREATE TABLE `dnat` (
  `fwMAC` varchar(255) DEFAULT NULL,
  `origin_dstIP` varchar(255) DEFAULT NULL,
  `origin_dport` varchar(255) DEFAULT NULL,
  `map_IP` varchar(255) DEFAULT NULL,
  `map_port` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of dnat
-- ----------------------------

-- ----------------------------
-- Table structure for dnp3
-- ----------------------------
DROP TABLE IF EXISTS `dnp3`;
CREATE TABLE `dnp3` (
  `dev_IP` varchar(255) DEFAULT NULL,
  `dst_IP` varchar(255) DEFAULT NULL,
  `src_IP` varchar(255) DEFAULT NULL,
  `log_flag` varchar(255) DEFAULT NULL,
  `method` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of dnp3
-- ----------------------------

-- ----------------------------
-- Table structure for firewallip
-- ----------------------------
DROP TABLE IF EXISTS `firewallip`;
CREATE TABLE `firewallip` (
  `fw_ip` varchar(255) DEFAULT NULL,
  `fw_mac` varchar(255) DEFAULT NULL,
  `dev_ip` varchar(255) DEFAULT NULL,
  `dev_mac` varchar(255) DEFAULT NULL,
  `dev_type` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of firewallip
-- ----------------------------

-- ----------------------------
-- Table structure for fwproperty
-- ----------------------------
DROP TABLE IF EXISTS `fwproperty`;
CREATE TABLE `fwproperty` (
  `名称` varchar(255) DEFAULT NULL,
  `防火墙ID` varchar(255) DEFAULT NULL,
  `防火墙IP` varchar(255) DEFAULT NULL,
  `防火墙描述` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of fwproperty
-- ----------------------------

-- ----------------------------
-- Table structure for macs
-- ----------------------------
DROP TABLE IF EXISTS `macs`;
CREATE TABLE `macs` (
  `Macs` varchar(255) DEFAULT NULL,
  `Manufacturers` varchar(255) DEFAULT NULL,
  UNIQUE KEY `Mac` (`Macs`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of macs
-- ----------------------------
INSERT INTO `macs` VALUES ('C0-98-79', 'acer');
INSERT INTO `macs` VALUES ('00-00-E2', 'acer');
INSERT INTO `macs` VALUES ('00-60-67', 'acer');
INSERT INTO `macs` VALUES ('DC-66-3A', 'acer');
INSERT INTO `macs` VALUES ('00-A0-60', 'acer');
INSERT INTO `macs` VALUES ('EC-F4-BB', 'Dell');
INSERT INTO `macs` VALUES ('D0-67-E5', 'Dell');
INSERT INTO `macs` VALUES ('18-A9-9B', 'Dell');
INSERT INTO `macs` VALUES ('F8-DB-88', 'Dell');
INSERT INTO `macs` VALUES ('18-FB-7B', 'Dell');
INSERT INTO `macs` VALUES ('74-E6-E2', 'Dell');
INSERT INTO `macs` VALUES ('10-98-36', 'Dell');
INSERT INTO `macs` VALUES ('44-A8-42', 'Dell');
INSERT INTO `macs` VALUES ('34-E6-D7', 'Dell');
INSERT INTO `macs` VALUES ('00-0B-DB', 'Dell');
INSERT INTO `macs` VALUES ('00-11-43', 'Dell');
INSERT INTO `macs` VALUES ('00-18-8B', 'Dell');
INSERT INTO `macs` VALUES ('D4-BE-D9', 'Dell');
INSERT INTO `macs` VALUES ('00-1A-A0', 'Dell');
INSERT INTO `macs` VALUES ('00-21-70', 'Dell');
INSERT INTO `macs` VALUES ('00-26-B9', 'Dell');
INSERT INTO `macs` VALUES ('A4-BA-DB', 'Dell');
INSERT INTO `macs` VALUES ('00-1E-4F', 'Dell');
INSERT INTO `macs` VALUES ('5C-F9-DD', 'Dell');
INSERT INTO `macs` VALUES ('00-06-5B', 'Dell');
INSERT INTO `macs` VALUES ('80-18-44', 'Dell');
INSERT INTO `macs` VALUES ('48-4D-7E', 'Dell');
INSERT INTO `macs` VALUES ('14-B3-1F', 'Dell');
INSERT INTO `macs` VALUES ('28-C8-25', 'Dell');
INSERT INTO `macs` VALUES ('00-16-F0', 'Dell');
INSERT INTO `macs` VALUES ('18-03-73', 'Dell');
INSERT INTO `macs` VALUES ('F8-B1-56', 'Dell');
INSERT INTO `macs` VALUES ('1C-40-24', 'Dell');
INSERT INTO `macs` VALUES ('F8-BC-12', 'Dell');
INSERT INTO `macs` VALUES ('D0-43-1E', 'Dell');
INSERT INTO `macs` VALUES ('24-6E-96', 'Dell');
INSERT INTO `macs` VALUES ('20-47-47', 'Dell');
INSERT INTO `macs` VALUES ('4C-76-25', 'Dell');
INSERT INTO `macs` VALUES ('B8-AC-6F', 'Dell');
INSERT INTO `macs` VALUES ('00-1E-C9', 'Dell');
INSERT INTO `macs` VALUES ('78-45-C4', 'Dell');
INSERT INTO `macs` VALUES ('14-9E-CF', 'Dell');
INSERT INTO `macs` VALUES ('D4-81-D7', 'Dell');
INSERT INTO `macs` VALUES ('84-2B-BC', 'Dell');
INSERT INTO `macs` VALUES ('98-90-96', 'Dell');
INSERT INTO `macs` VALUES ('B8-2A-72', 'Dell');
INSERT INTO `macs` VALUES ('00-0F-1F', 'Dell');
INSERT INTO `macs` VALUES ('14-FE-B5', 'Dell');
INSERT INTO `macs` VALUES ('00-15-C5', 'Dell');
INSERT INTO `macs` VALUES ('D4-AE-52', 'Dell');
INSERT INTO `macs` VALUES ('54-9F-35', 'Dell');
INSERT INTO `macs` VALUES ('64-00-6A', 'Dell');
INSERT INTO `macs` VALUES ('B4-E1-0F', 'Dell');
INSERT INTO `macs` VALUES ('00-23-AE', 'Dell');
INSERT INTO `macs` VALUES ('18-66-DA', 'Dell');
INSERT INTO `macs` VALUES ('28-F1-0E', 'Dell');
INSERT INTO `macs` VALUES ('98-40-BB', 'Dell');
INSERT INTO `macs` VALUES ('18-DB-F2', 'Dell');
INSERT INTO `macs` VALUES ('84-8F-69', 'Dell');
INSERT INTO `macs` VALUES ('90-B1-1C', 'Dell');
INSERT INTO `macs` VALUES ('F8-CA-B8', 'Dell');
INSERT INTO `macs` VALUES ('24-B6-FD', 'Dell');
INSERT INTO `macs` VALUES ('00-0D-56', 'Dell');
INSERT INTO `macs` VALUES ('00-12-3F', 'Dell');
INSERT INTO `macs` VALUES ('00-13-72', 'Dell');
INSERT INTO `macs` VALUES ('74-86-7A', 'Dell');
INSERT INTO `macs` VALUES ('34-17-EB', 'Dell');
INSERT INTO `macs` VALUES ('00-19-B9', 'Dell');
INSERT INTO `macs` VALUES ('00-22-19', 'Dell');
INSERT INTO `macs` VALUES ('00-B0-D0', 'Dell');
INSERT INTO `macs` VALUES ('5C-26-0A', 'Dell');
INSERT INTO `macs` VALUES ('B0-83-FE', 'Dell');
INSERT INTO `macs` VALUES ('14-18-77', 'Dell');
INSERT INTO `macs` VALUES ('00-24-E8', 'Dell');
INSERT INTO `macs` VALUES ('F4-8E-38', 'Dell');
INSERT INTO `macs` VALUES ('84-7B-EB', 'Dell');
INSERT INTO `macs` VALUES ('10-7D-1A', 'Dell');
INSERT INTO `macs` VALUES ('78-2B-CB', 'Dell');
INSERT INTO `macs` VALUES ('B8-CA-3A', 'Dell');
INSERT INTO `macs` VALUES ('F0-1F-AF', 'Dell');
INSERT INTO `macs` VALUES ('C8-1F-66', 'Dell');
INSERT INTO `macs` VALUES ('00-14-22', 'Dell');
INSERT INTO `macs` VALUES ('00-1C-23', 'Dell');
INSERT INTO `macs` VALUES ('00-21-9B', 'Dell');
INSERT INTO `macs` VALUES ('00-08-74', 'Dell');
INSERT INTO `macs` VALUES ('00-25-64', 'Dell');
INSERT INTO `macs` VALUES ('84-2B-2B', 'Dell');
INSERT INTO `macs` VALUES ('E0-DB-55', 'Dell');
INSERT INTO `macs` VALUES ('A4-1F-72', 'Dell');
INSERT INTO `macs` VALUES ('00-C0-4F', 'Dell');
INSERT INTO `macs` VALUES ('F0-4D-A2', 'Dell');
INSERT INTO `macs` VALUES ('BC-30-5B', 'Dell');
INSERT INTO `macs` VALUES ('00-1D-09', 'Dell');
INSERT INTO `macs` VALUES ('44-86-C1', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-18-D1', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-0B-A3', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-0B-23', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-50-07', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-A0-03', 'SIEMENS');
INSERT INTO `macs` VALUES ('68-1F-D8', 'SIEMENS');
INSERT INTO `macs` VALUES ('B8-7A-C9', 'SIEMENS');
INSERT INTO `macs` VALUES ('88-4B-39', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-1F-F8', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-0D-41', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-05-19', 'SIEMENS');
INSERT INTO `macs` VALUES ('08-00-06', 'SIEMENS');
INSERT INTO `macs` VALUES ('B4-B1-5A', 'SIEMENS');
INSERT INTO `macs` VALUES ('20-87-56', 'SIEMENS');
INSERT INTO `macs` VALUES ('18-AE-BB', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-19-28', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-13-A3', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-0E-8C', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-C0-E4', 'SIEMENS');
INSERT INTO `macs` VALUES ('20-E7-91', 'SIEMENS');
INSERT INTO `macs` VALUES ('78-9F-87', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-1B-1B', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-11-33', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-90-40', 'SIEMENS');
INSERT INTO `macs` VALUES ('20-A8-B9', 'SIEMENS');
INSERT INTO `macs` VALUES ('28-63-36', 'SIEMENS');
INSERT INTO `macs` VALUES ('DC-05-75', 'SIEMENS');
INSERT INTO `macs` VALUES ('40-EC-F8', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-1C-06', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-18-65', 'SIEMENS');
INSERT INTO `macs` VALUES ('2000/11/6', 'SIEMENS');
INSERT INTO `macs` VALUES ('00-01-E3', 'SIEMENS');
INSERT INTO `macs` VALUES ('E0-DC-A0', 'SIEMENS');
INSERT INTO `macs` VALUES ('2C-30-33', 'GE');
INSERT INTO `macs` VALUES ('18-62-2C', 'GE');
INSERT INTO `macs` VALUES ('7C-03-D8', 'GE');
INSERT INTO `macs` VALUES ('E8-F1-B0', 'GE');
INSERT INTO `macs` VALUES ('34-8A-AE', 'GE');
INSERT INTO `macs` VALUES ('1C-49-7B', 'GE');
INSERT INTO `macs` VALUES ('4C-B8-2C', 'GE');
INSERT INTO `macs` VALUES ('60-48-26', 'GE');
INSERT INTO `macs` VALUES ('E0-36-E3', 'GE');
INSERT INTO `macs` VALUES ('90-83-7A', 'GE');
INSERT INTO `macs` VALUES ('F0-61-30', 'GE');
INSERT INTO `macs` VALUES ('80-05-DF', 'GE');
INSERT INTO `macs` VALUES ('E4-38-F2', 'GE');
INSERT INTO `macs` VALUES ('54-1F-D5', 'GE');
INSERT INTO `macs` VALUES ('6C-E4-CE', 'GE');
INSERT INTO `macs` VALUES ('28-A1-92', 'GE');
INSERT INTO `macs` VALUES ('A0-8C-15', 'GE');
INSERT INTO `macs` VALUES ('44-23-AA', 'GE');
INSERT INTO `macs` VALUES ('20-10-7A', 'GE');
INSERT INTO `macs` VALUES ('88-E7-A6', 'GE');
INSERT INTO `macs` VALUES ('AC-8A-CD', 'GE');
INSERT INTO `macs` VALUES ('24-C9-DE', 'GE');
INSERT INTO `macs` VALUES ('4C-73-67', 'GE');
INSERT INTO `macs` VALUES ('C8-20-8E', 'GE');
INSERT INTO `macs` VALUES ('04-74-A1', 'GE');
INSERT INTO `macs` VALUES ('CC-34-D7', 'GE');
INSERT INTO `macs` VALUES ('98-73-C4', 'GE');
INSERT INTO `macs` VALUES ('5C-17-D3', 'GE');
INSERT INTO `macs` VALUES ('D4-8F-AA', 'GE');
INSERT INTO `macs` VALUES ('24-AF-54', 'GE');
INSERT INTO `macs` VALUES ('AC-EA-6A', 'GE');
INSERT INTO `macs` VALUES ('D8-28-C9', 'GE');
INSERT INTO `macs` VALUES ('00-27-1A', 'GE');
INSERT INTO `macs` VALUES ('00-27-02', 'GE');
INSERT INTO `macs` VALUES ('00-26-26', 'GE');
INSERT INTO `macs` VALUES ('00-26-13', 'GE');
INSERT INTO `macs` VALUES ('00-26-89', 'GE');
INSERT INTO `macs` VALUES ('00-25-60', 'GE');
INSERT INTO `macs` VALUES ('00-24-FA', 'GE');
INSERT INTO `macs` VALUES ('00-22-E6', 'GE');
INSERT INTO `macs` VALUES ('00-23-83', 'GE');
INSERT INTO `macs` VALUES ('00-21-F4', 'GE');
INSERT INTO `macs` VALUES ('00-21-ED', 'GE');
INSERT INTO `macs` VALUES ('00-22-3D', 'GE');
INSERT INTO `macs` VALUES ('00-1F-86', 'GE');
INSERT INTO `macs` VALUES ('00-1F-2B', 'GE');
INSERT INTO `macs` VALUES ('00-1F-2C', 'GE');
INSERT INTO `macs` VALUES ('00-1F-53', 'GE');
INSERT INTO `macs` VALUES ('00-1C-4B', 'GE');
INSERT INTO `macs` VALUES ('00-1A-03', 'GE');
INSERT INTO `macs` VALUES ('00-19-F4', 'GE');
INSERT INTO `macs` VALUES ('00-15-19', 'GE');
INSERT INTO `macs` VALUES ('00-15-64', 'GE');
INSERT INTO `macs` VALUES ('00-13-E2', 'GE');
INSERT INTO `macs` VALUES ('00-13-D5', 'GE');
INSERT INTO `macs` VALUES ('00-13-C6', 'GE');
INSERT INTO `macs` VALUES ('00-13-9B', 'GE');
INSERT INTO `macs` VALUES ('00-12-70', 'GE');
INSERT INTO `macs` VALUES ('00-0C-38', 'GE');
INSERT INTO `macs` VALUES ('00-0C-3F', 'GE');
INSERT INTO `macs` VALUES ('00-0B-77', 'GE');
INSERT INTO `macs` VALUES ('00-0A-1A', 'GE');
INSERT INTO `macs` VALUES ('00-0A-DF', 'GE');
INSERT INTO `macs` VALUES ('00-09-51', 'GE');
INSERT INTO `macs` VALUES ('2000/6/24', 'GE');
INSERT INTO `macs` VALUES ('00-04-7B', 'GE');
INSERT INTO `macs` VALUES ('00-06-42', 'GE');
INSERT INTO `macs` VALUES ('00-05-54', 'GE');
INSERT INTO `macs` VALUES ('00-30-E6', 'GE');
INSERT INTO `macs` VALUES ('00-50-BF', 'GE');
INSERT INTO `macs` VALUES ('00-50-D9', 'GE');
INSERT INTO `macs` VALUES ('00-D0-21', 'GE');
INSERT INTO `macs` VALUES ('00-50-2B', 'GE');
INSERT INTO `macs` VALUES ('00-60-A9', 'GE');
INSERT INTO `macs` VALUES ('00-60-AA', 'GE');
INSERT INTO `macs` VALUES ('00-E0-77', 'GE');
INSERT INTO `macs` VALUES ('00-20-44', 'GE');
INSERT INTO `macs` VALUES ('00-C0-F7', 'GE');
INSERT INTO `macs` VALUES ('08-00-68', 'GE');
INSERT INTO `macs` VALUES ('08-00-62', 'GE');
INSERT INTO `macs` VALUES ('00-00-C1', 'GE');
INSERT INTO `macs` VALUES ('08-00-19', 'GE');
INSERT INTO `macs` VALUES ('00-DD-06', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0B', 'GE');
INSERT INTO `macs` VALUES ('00-DD-00', 'GE');
INSERT INTO `macs` VALUES ('00-DD-03', 'GE');
INSERT INTO `macs` VALUES ('08-00-65', 'GE');
INSERT INTO `macs` VALUES ('98-8B-5D', 'GE');
INSERT INTO `macs` VALUES ('94-FE-F4', 'GE');
INSERT INTO `macs` VALUES ('C8-CD-72', 'GE');
INSERT INTO `macs` VALUES ('E8-BE-81', 'GE');
INSERT INTO `macs` VALUES ('00-23-48', 'GE');
INSERT INTO `macs` VALUES ('2C-B0-5D', 'GE');
INSERT INTO `macs` VALUES ('00-14-6C', 'GE');
INSERT INTO `macs` VALUES ('A4-2B-8C', 'GE');
INSERT INTO `macs` VALUES ('04-A1-51', 'GE');
INSERT INTO `macs` VALUES ('28-C6-8E', 'GE');
INSERT INTO `macs` VALUES ('50-4A-6E', 'GE');
INSERT INTO `macs` VALUES ('A4-C7-DE', 'GE');
INSERT INTO `macs` VALUES ('00-09-E1', 'GE');
INSERT INTO `macs` VALUES ('60-3E-CA', 'GE');
INSERT INTO `macs` VALUES ('88-A6-C6', 'GE');
INSERT INTO `macs` VALUES ('4C-FA-CA', 'GE');
INSERT INTO `macs` VALUES ('A0-04-60', 'GE');
INSERT INTO `macs` VALUES ('9C-3D-CF', 'GE');
INSERT INTO `macs` VALUES ('A4-08-F5', 'GE');
INSERT INTO `macs` VALUES ('9C-50-EE', 'GE');
INSERT INTO `macs` VALUES ('BC-45-2E', 'GE');
INSERT INTO `macs` VALUES ('C0-AC-54', 'GE');
INSERT INTO `macs` VALUES ('40-F2-01', 'GE');
INSERT INTO `macs` VALUES ('C8-91-F9', 'GE');
INSERT INTO `macs` VALUES ('C4-BB-EA', 'GE');
INSERT INTO `macs` VALUES ('CC-37-AB', 'GE');
INSERT INTO `macs` VALUES ('DC-15-DB', 'GE');
INSERT INTO `macs` VALUES ('E0-35-60', 'GE');
INSERT INTO `macs` VALUES ('80-0E-24', 'GE');
INSERT INTO `macs` VALUES ('90-20-83', 'GE');
INSERT INTO `macs` VALUES ('00-7D-FA', 'GE');
INSERT INTO `macs` VALUES ('28-44-30', 'GE');
INSERT INTO `macs` VALUES ('1C-41-58', 'GE');
INSERT INTO `macs` VALUES ('9C-E1-D6', 'GE');
INSERT INTO `macs` VALUES ('D0-DF-B2', 'GE');
INSERT INTO `macs` VALUES ('00-BF-15', 'GE');
INSERT INTO `macs` VALUES ('7C-16-0D', 'GE');
INSERT INTO `macs` VALUES ('14-35-8B', 'GE');
INSERT INTO `macs` VALUES ('F0-5F-5A', 'GE');
INSERT INTO `macs` VALUES ('A4-7C-14', 'GE');
INSERT INTO `macs` VALUES ('00-D6-32', 'GE');
INSERT INTO `macs` VALUES ('78-EF-4C', 'GE');
INSERT INTO `macs` VALUES ('94-8B-03', 'GE');
INSERT INTO `macs` VALUES ('80-2D-E1', 'GE');
INSERT INTO `macs` VALUES ('AC-47-23', 'GE');
INSERT INTO `macs` VALUES ('E0-CF-2D', 'GE');
INSERT INTO `macs` VALUES ('70-72-CF', 'GE');
INSERT INTO `macs` VALUES ('28-FB-D3', 'GE');
INSERT INTO `macs` VALUES ('00-25-26', 'GE');
INSERT INTO `macs` VALUES ('00-24-63', 'GE');
INSERT INTO `macs` VALUES ('00-23-B2', 'GE');
INSERT INTO `macs` VALUES ('00-23-A5', 'GE');
INSERT INTO `macs` VALUES ('00-22-C7', 'GE');
INSERT INTO `macs` VALUES ('00-21-79', 'GE');
INSERT INTO `macs` VALUES ('00-1F-4B', 'GE');
INSERT INTO `macs` VALUES ('00-1C-91', 'GE');
INSERT INTO `macs` VALUES ('00-1C-71', 'GE');
INSERT INTO `macs` VALUES ('00-1D-F4', 'GE');
INSERT INTO `macs` VALUES ('00-1B-41', 'GE');
INSERT INTO `macs` VALUES ('00-1B-0B', 'GE');
INSERT INTO `macs` VALUES ('00-19-EA', 'GE');
INSERT INTO `macs` VALUES ('00-17-DE', 'GE');
INSERT INTO `macs` VALUES ('00-18-4C', 'GE');
INSERT INTO `macs` VALUES ('00-17-20', 'GE');
INSERT INTO `macs` VALUES ('00-16-A3', 'GE');
INSERT INTO `macs` VALUES ('00-16-0F', 'GE');
INSERT INTO `macs` VALUES ('00-15-0B', 'GE');
INSERT INTO `macs` VALUES ('00-14-DD', 'GE');
INSERT INTO `macs` VALUES ('00-14-56', 'GE');
INSERT INTO `macs` VALUES ('00-14-4C', 'GE');
INSERT INTO `macs` VALUES ('00-13-D2', 'GE');
INSERT INTO `macs` VALUES ('00-13-A5', 'GE');
INSERT INTO `macs` VALUES ('00-13-7E', 'GE');
INSERT INTO `macs` VALUES ('00-12-93', 'GE');
INSERT INTO `macs` VALUES ('00-11-D3', 'GE');
INSERT INTO `macs` VALUES ('00-12-3D', 'GE');
INSERT INTO `macs` VALUES ('00-0E-76', 'GE');
INSERT INTO `macs` VALUES ('00-0F-94', 'GE');
INSERT INTO `macs` VALUES ('00-0E-34', 'GE');
INSERT INTO `macs` VALUES ('00-0B-D9', 'GE');
INSERT INTO `macs` VALUES ('00-0B-D5', 'GE');
INSERT INTO `macs` VALUES ('00-0B-B6', 'GE');
INSERT INTO `macs` VALUES ('00-0A-1C', 'GE');
INSERT INTO `macs` VALUES ('00-09-91', 'GE');
INSERT INTO `macs` VALUES ('00-09-C5', 'GE');
INSERT INTO `macs` VALUES ('00-0A-BC', 'GE');
INSERT INTO `macs` VALUES ('00-0A-65', 'GE');
INSERT INTO `macs` VALUES ('00-07-F5', 'GE');
INSERT INTO `macs` VALUES ('00-07-E8', 'GE');
INSERT INTO `macs` VALUES ('2000/9/30', 'GE');
INSERT INTO `macs` VALUES ('00-08-DB', 'GE');
INSERT INTO `macs` VALUES ('00-08-60', 'GE');
INSERT INTO `macs` VALUES ('00-05-E6', 'GE');
INSERT INTO `macs` VALUES ('00-06-7F', 'GE');
INSERT INTO `macs` VALUES ('00-06-F0', 'GE');
INSERT INTO `macs` VALUES ('00-04-0A', 'GE');
INSERT INTO `macs` VALUES ('00-04-87', 'GE');
INSERT INTO `macs` VALUES ('00-03-DB', 'GE');
INSERT INTO `macs` VALUES ('00-02-81', 'GE');
INSERT INTO `macs` VALUES ('00-02-6E', 'GE');
INSERT INTO `macs` VALUES ('00-02-31', 'GE');
INSERT INTO `macs` VALUES ('00-02-2D', 'GE');
INSERT INTO `macs` VALUES ('00-01-7B', 'GE');
INSERT INTO `macs` VALUES ('00-01-A3', 'GE');
INSERT INTO `macs` VALUES ('00-02-9A', 'GE');
INSERT INTO `macs` VALUES ('00-02-0A', 'GE');
INSERT INTO `macs` VALUES ('00-01-F7', 'GE');
INSERT INTO `macs` VALUES ('00-D0-F5', 'GE');
INSERT INTO `macs` VALUES ('00-D0-10', 'GE');
INSERT INTO `macs` VALUES ('00-D0-8C', 'GE');
INSERT INTO `macs` VALUES ('00-50-2F', 'GE');
INSERT INTO `macs` VALUES ('00-50-27', 'GE');
INSERT INTO `macs` VALUES ('00-10-5D', 'GE');
INSERT INTO `macs` VALUES ('00-E0-AF', 'GE');
INSERT INTO `macs` VALUES ('00-60-30', 'GE');
INSERT INTO `macs` VALUES ('00-60-A4', 'GE');
INSERT INTO `macs` VALUES ('00-A0-32', 'GE');
INSERT INTO `macs` VALUES ('00-C0-BA', 'GE');
INSERT INTO `macs` VALUES ('00-40-99', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0E', 'GE');
INSERT INTO `macs` VALUES ('00-DD-07', 'GE');
INSERT INTO `macs` VALUES ('00-00-59', 'GE');
INSERT INTO `macs` VALUES ('08-00-02', 'GE');
INSERT INTO `macs` VALUES ('40-65-A3', 'GE');
INSERT INTO `macs` VALUES ('00-78-9E', 'GE');
INSERT INTO `macs` VALUES ('44-E9-DD', 'GE');
INSERT INTO `macs` VALUES ('C4-3D-C7', 'GE');
INSERT INTO `macs` VALUES ('00-0F-B5', 'GE');
INSERT INTO `macs` VALUES ('00-09-5B', 'GE');
INSERT INTO `macs` VALUES ('F8-73-94', 'GE');
INSERT INTO `macs` VALUES ('C0-FF-D4', 'GE');
INSERT INTO `macs` VALUES ('40-5D-82', 'GE');
INSERT INTO `macs` VALUES ('80-37-73', 'GE');
INSERT INTO `macs` VALUES ('4C-F2-BF', 'GE');
INSERT INTO `macs` VALUES ('70-D9-31', 'GE');
INSERT INTO `macs` VALUES ('E0-1D-3B', 'GE');
INSERT INTO `macs` VALUES ('0C-BF-15', 'GE');
INSERT INTO `macs` VALUES ('00-21-00', 'GE');
INSERT INTO `macs` VALUES ('90-72-82', 'GE');
INSERT INTO `macs` VALUES ('10-DA-43', 'GE');
INSERT INTO `macs` VALUES ('00-A0-F4', 'GE');
INSERT INTO `macs` VALUES ('48-83-C7', 'GE');
INSERT INTO `macs` VALUES ('DC-37-52', 'GE');
INSERT INTO `macs` VALUES ('8C-EA-1B', 'GE');
INSERT INTO `macs` VALUES ('A0-B8-F8', 'GE');
INSERT INTO `macs` VALUES ('24-7F-20', 'GE');
INSERT INTO `macs` VALUES ('24-20-C7', 'GE');
INSERT INTO `macs` VALUES ('90-17-11', 'GE');
INSERT INTO `macs` VALUES ('F0-82-61', 'GE');
INSERT INTO `macs` VALUES ('D0-84-B0', 'GE');
INSERT INTO `macs` VALUES ('8C-10-D4', 'GE');
INSERT INTO `macs` VALUES ('BC-6A-2F', 'GE');
INSERT INTO `macs` VALUES ('D8-5D-EF', 'GE');
INSERT INTO `macs` VALUES ('D8-93-41', 'GE');
INSERT INTO `macs` VALUES ('EC-B9-07', 'GE');
INSERT INTO `macs` VALUES ('8C-F8-13', 'GE');
INSERT INTO `macs` VALUES ('74-8F-1B', 'GE');
INSERT INTO `macs` VALUES ('18-20-A6', 'GE');
INSERT INTO `macs` VALUES ('BC-F6-1C', 'GE');
INSERT INTO `macs` VALUES ('90-7A-0A', 'GE');
INSERT INTO `macs` VALUES ('F8-35-DD', 'GE');
INSERT INTO `macs` VALUES ('E0-AA-B0', 'GE');
INSERT INTO `macs` VALUES ('54-39-68', 'GE');
INSERT INTO `macs` VALUES ('20-FA-BB', 'GE');
INSERT INTO `macs` VALUES ('08-0D-84', 'GE');
INSERT INTO `macs` VALUES ('28-38-CF', 'GE');
INSERT INTO `macs` VALUES ('D0-31-10', 'GE');
INSERT INTO `macs` VALUES ('D8-C0-68', 'GE');
INSERT INTO `macs` VALUES ('D0-A3-11', 'GE');
INSERT INTO `macs` VALUES ('88-AC-C1', 'GE');
INSERT INTO `macs` VALUES ('88-8B-5D', 'GE');
INSERT INTO `macs` VALUES ('8C-54-1D', 'GE');
INSERT INTO `macs` VALUES ('94-F6-92', 'GE');
INSERT INTO `macs` VALUES ('00-26-A1', 'GE');
INSERT INTO `macs` VALUES ('00-25-A7', 'GE');
INSERT INTO `macs` VALUES ('00-26-4F', 'GE');
INSERT INTO `macs` VALUES ('00-22-1D', 'GE');
INSERT INTO `macs` VALUES ('00-21-52', 'GE');
INSERT INTO `macs` VALUES ('00-21-9A', 'GE');
INSERT INTO `macs` VALUES ('00-1E-E9', 'GE');
INSERT INTO `macs` VALUES ('00-1D-26', 'GE');
INSERT INTO `macs` VALUES ('00-1C-82', 'GE');
INSERT INTO `macs` VALUES ('00-1B-D1', 'GE');
INSERT INTO `macs` VALUES ('00-19-26', 'GE');
INSERT INTO `macs` VALUES ('00-19-02', 'GE');
INSERT INTO `macs` VALUES ('00-16-2F', 'GE');
INSERT INTO `macs` VALUES ('00-12-2B', 'GE');
INSERT INTO `macs` VALUES ('00-0E-71', 'GE');
INSERT INTO `macs` VALUES ('00-0D-FE', 'GE');
INSERT INTO `macs` VALUES ('00-0E-E0', 'GE');
INSERT INTO `macs` VALUES ('00-0B-5A', 'GE');
INSERT INTO `macs` VALUES ('00-0C-37', 'GE');
INSERT INTO `macs` VALUES ('00-0B-F6', 'GE');
INSERT INTO `macs` VALUES ('00-09-CE', 'GE');
INSERT INTO `macs` VALUES ('00-07-8B', 'GE');
INSERT INTO `macs` VALUES ('00-07-E6', 'GE');
INSERT INTO `macs` VALUES ('00-CB-BD', 'GE');
INSERT INTO `macs` VALUES ('00-05-F5', 'GE');
INSERT INTO `macs` VALUES ('2000/6/23', 'GE');
INSERT INTO `macs` VALUES ('00-05-96', 'GE');
INSERT INTO `macs` VALUES ('00-05-83', 'GE');
INSERT INTO `macs` VALUES ('00-03-AF', 'GE');
INSERT INTO `macs` VALUES ('2000/3/30', 'GE');
INSERT INTO `macs` VALUES ('00-02-50', 'GE');
INSERT INTO `macs` VALUES ('00-B0-17', 'GE');
INSERT INTO `macs` VALUES ('00-02-1B', 'GE');
INSERT INTO `macs` VALUES ('00-01-B1', 'GE');
INSERT INTO `macs` VALUES ('00-30-45', 'GE');
INSERT INTO `macs` VALUES ('00-30-1A', 'GE');
INSERT INTO `macs` VALUES ('00-10-D4', 'GE');
INSERT INTO `macs` VALUES ('00-C0-64', 'GE');
INSERT INTO `macs` VALUES ('00-80-6C', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0F', 'GE');
INSERT INTO `macs` VALUES ('00-DD-08', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0C', 'GE');
INSERT INTO `macs` VALUES ('00-40-6B', 'GE');
INSERT INTO `macs` VALUES ('00-19-4B', 'GE');
INSERT INTO `macs` VALUES ('00-1F-95', 'GE');
INSERT INTO `macs` VALUES ('00-0E-59', 'GE');
INSERT INTO `macs` VALUES ('A0-1B-29', 'GE');
INSERT INTO `macs` VALUES ('90-01-3B', 'GE');
INSERT INTO `macs` VALUES ('A8-58-40', 'GE');
INSERT INTO `macs` VALUES ('84-1B-5E', 'GE');
INSERT INTO `macs` VALUES ('20-4E-7F', 'GE');
INSERT INTO `macs` VALUES ('A0-21-B7', 'GE');
INSERT INTO `macs` VALUES ('00-24-B2', 'GE');
INSERT INTO `macs` VALUES ('C0-3F-0E', 'GE');
INSERT INTO `macs` VALUES ('00-1F-33', 'GE');
INSERT INTO `macs` VALUES ('50-6A-03', 'GE');
INSERT INTO `macs` VALUES ('6C-B0-CE', 'GE');
INSERT INTO `macs` VALUES ('10-0D-7F', 'GE');
INSERT INTO `macs` VALUES ('00-26-82', 'GE');
INSERT INTO `macs` VALUES ('00-1A-73', 'GE');
INSERT INTO `macs` VALUES ('00-90-4B', 'GE');
INSERT INTO `macs` VALUES ('B0-7F-B9', 'GE');
INSERT INTO `macs` VALUES ('A4-E5-97', 'GE');
INSERT INTO `macs` VALUES ('54-7F-54', 'GE');
INSERT INTO `macs` VALUES ('00-14-B4', 'GE');
INSERT INTO `macs` VALUES ('9C-DD-1F', 'GE');
INSERT INTO `macs` VALUES ('40-C7-29', 'GE');
INSERT INTO `macs` VALUES ('00-25-D4', 'GE');
INSERT INTO `macs` VALUES ('7C-EB-AE', 'GE');
INSERT INTO `macs` VALUES ('00-0D-2C', 'GE');
INSERT INTO `macs` VALUES ('08-3E-5D', 'GE');
INSERT INTO `macs` VALUES ('7C-26-64', 'GE');
INSERT INTO `macs` VALUES ('F8-AB-05', 'GE');
INSERT INTO `macs` VALUES ('08-02-8E', 'GE');
INSERT INTO `macs` VALUES ('D8-6C-E9', 'GE');
INSERT INTO `macs` VALUES ('3C-81-D8', 'GE');
INSERT INTO `macs` VALUES ('2C-E4-12', 'GE');
INSERT INTO `macs` VALUES ('18-1E-78', 'GE');
INSERT INTO `macs` VALUES ('00-37-B7', 'GE');
INSERT INTO `macs` VALUES ('84-A4-23', 'GE');
INSERT INTO `macs` VALUES ('54-36-9B', 'GE');
INSERT INTO `macs` VALUES ('E4-85-01', 'GE');
INSERT INTO `macs` VALUES ('94-E2-FD', 'GE');
INSERT INTO `macs` VALUES ('DC-DA-4F', 'GE');
INSERT INTO `macs` VALUES ('F4-C4-47', 'GE');
INSERT INTO `macs` VALUES ('70-93-83', 'GE');
INSERT INTO `macs` VALUES ('88-A7-3C', 'GE');
INSERT INTO `macs` VALUES ('B0-24-F3', 'GE');
INSERT INTO `macs` VALUES ('F8-A2-B4', 'GE');
INSERT INTO `macs` VALUES ('74-3E-CB', 'GE');
INSERT INTO `macs` VALUES ('94-B8-C5', 'GE');
INSERT INTO `macs` VALUES ('E0-55-97', 'GE');
INSERT INTO `macs` VALUES ('28-91-D0', 'GE');
INSERT INTO `macs` VALUES ('80-2A-FA', 'GE');
INSERT INTO `macs` VALUES ('28-B3-AB', 'GE');
INSERT INTO `macs` VALUES ('2C-BE-97', 'GE');
INSERT INTO `macs` VALUES ('0C-AF-5A', 'GE');
INSERT INTO `macs` VALUES ('F8-2F-5B', 'GE');
INSERT INTO `macs` VALUES ('F8-35-53', 'GE');
INSERT INTO `macs` VALUES ('FC-8F-C4', 'GE');
INSERT INTO `macs` VALUES ('F4-94-61', 'GE');
INSERT INTO `macs` VALUES ('E8-94-4C', 'GE');
INSERT INTO `macs` VALUES ('D4-28-B2', 'GE');
INSERT INTO `macs` VALUES ('A0-F2-17', 'GE');
INSERT INTO `macs` VALUES ('94-85-7A', 'GE');
INSERT INTO `macs` VALUES ('64-99-5D', 'GE');
INSERT INTO `macs` VALUES ('24-45-97', 'GE');
INSERT INTO `macs` VALUES ('AC-E3-48', 'GE');
INSERT INTO `macs` VALUES ('2C-3F-3E', 'GE');
INSERT INTO `macs` VALUES ('88-94-F9', 'GE');
INSERT INTO `macs` VALUES ('E4-75-1E', 'GE');
INSERT INTO `macs` VALUES ('5C-87-78', 'GE');
INSERT INTO `macs` VALUES ('00-26-BC', 'GE');
INSERT INTO `macs` VALUES ('00-25-88', 'GE');
INSERT INTO `macs` VALUES ('00-25-2C', 'GE');
INSERT INTO `macs` VALUES ('00-24-96', 'GE');
INSERT INTO `macs` VALUES ('00-24-64', 'GE');
INSERT INTO `macs` VALUES ('00-23-61', 'GE');
INSERT INTO `macs` VALUES ('00-22-7B', 'GE');
INSERT INTO `macs` VALUES ('00-21-0B', 'GE');
INSERT INTO `macs` VALUES ('00-1F-37', 'GE');
INSERT INTO `macs` VALUES ('00-1D-1C', 'GE');
INSERT INTO `macs` VALUES ('00-1B-0A', 'GE');
INSERT INTO `macs` VALUES ('00-1A-E1', 'GE');
INSERT INTO `macs` VALUES ('00-1B-D9', 'GE');
INSERT INTO `macs` VALUES ('00-1A-15', 'GE');
INSERT INTO `macs` VALUES ('00-18-49', 'GE');
INSERT INTO `macs` VALUES ('00-16-3B', 'GE');
INSERT INTO `macs` VALUES ('00-16-B3', 'GE');
INSERT INTO `macs` VALUES ('00-17-11', 'GE');
INSERT INTO `macs` VALUES ('00-15-52', 'GE');
INSERT INTO `macs` VALUES ('00-14-F9', 'GE');
INSERT INTO `macs` VALUES ('00-14-DE', 'GE');
INSERT INTO `macs` VALUES ('00-13-0F', 'GE');
INSERT INTO `macs` VALUES ('00-13-FF', 'GE');
INSERT INTO `macs` VALUES ('00-12-A5', 'GE');
INSERT INTO `macs` VALUES ('00-12-45', 'GE');
INSERT INTO `macs` VALUES ('00-0F-7F', 'GE');
INSERT INTO `macs` VALUES ('00-0C-C7', 'GE');
INSERT INTO `macs` VALUES ('00-0B-F9', 'GE');
INSERT INTO `macs` VALUES ('00-0C-C0', 'GE');
INSERT INTO `macs` VALUES ('00-0A-B1', 'GE');
INSERT INTO `macs` VALUES ('00-09-AF', 'GE');
INSERT INTO `macs` VALUES ('00-09-E3', 'GE');
INSERT INTO `macs` VALUES ('2000/8/17', 'GE');
INSERT INTO `macs` VALUES ('00-07-78', 'GE');
INSERT INTO `macs` VALUES ('00-04-3A', 'GE');
INSERT INTO `macs` VALUES ('00-04-70', 'GE');
INSERT INTO `macs` VALUES ('00-03-AB', 'GE');
INSERT INTO `macs` VALUES ('00-01-CA', 'GE');
INSERT INTO `macs` VALUES ('00-02-5B', 'GE');
INSERT INTO `macs` VALUES ('00-30-B0', 'GE');
INSERT INTO `macs` VALUES ('00-50-C1', 'GE');
INSERT INTO `macs` VALUES ('00-30-C3', 'GE');
INSERT INTO `macs` VALUES ('00-10-DA', 'GE');
INSERT INTO `macs` VALUES ('00-E0-61', 'GE');
INSERT INTO `macs` VALUES ('00-A0-FC', 'GE');
INSERT INTO `macs` VALUES ('00-60-85', 'GE');
INSERT INTO `macs` VALUES ('00-E0-BE', 'GE');
INSERT INTO `macs` VALUES ('00-A0-21', 'GE');
INSERT INTO `macs` VALUES ('00-20-66', 'GE');
INSERT INTO `macs` VALUES ('00-40-D4', 'GE');
INSERT INTO `macs` VALUES ('00-40-2B', 'GE');
INSERT INTO `macs` VALUES ('00-C0-2B', 'GE');
INSERT INTO `macs` VALUES ('00-C0-D5', 'GE');
INSERT INTO `macs` VALUES ('00-00-92', 'GE');
INSERT INTO `macs` VALUES ('00-40-0C', 'GE');
INSERT INTO `macs` VALUES ('08-00-3C', 'GE');
INSERT INTO `macs` VALUES ('00-DD-09', 'GE');
INSERT INTO `macs` VALUES ('00-DD-04', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0D', 'GE');
INSERT INTO `macs` VALUES ('00-00-6F', 'GE');
INSERT INTO `macs` VALUES ('F4-EB-38', 'GE');
INSERT INTO `macs` VALUES ('00-1E-74', 'GE');
INSERT INTO `macs` VALUES ('00-60-4C', 'GE');
INSERT INTO `macs` VALUES ('00-26-91', 'GE');
INSERT INTO `macs` VALUES ('C0-D0-44', 'GE');
INSERT INTO `macs` VALUES ('6C-2E-85', 'GE');
INSERT INTO `macs` VALUES ('CC-33-BB', 'GE');
INSERT INTO `macs` VALUES ('68-15-90', 'GE');
INSERT INTO `macs` VALUES ('54-64-D9', 'GE');
INSERT INTO `macs` VALUES ('74-44-01', 'GE');
INSERT INTO `macs` VALUES ('E0-91-F5', 'GE');
INSERT INTO `macs` VALUES ('00-1B-2F', 'GE');
INSERT INTO `macs` VALUES ('00-22-3F', 'GE');
INSERT INTO `macs` VALUES ('E0-46-9A', 'GE');
INSERT INTO `macs` VALUES ('00-8E-F2', 'GE');
INSERT INTO `macs` VALUES ('44-94-FC', 'GE');
INSERT INTO `macs` VALUES ('20-E5-2A', 'GE');
INSERT INTO `macs` VALUES ('9C-D3-6D', 'GE');
INSERT INTO `macs` VALUES ('C4-04-15', 'GE');
INSERT INTO `macs` VALUES ('08-BD-43', 'GE');
INSERT INTO `macs` VALUES ('D0-54-2D', 'GE');
INSERT INTO `macs` VALUES ('28-0C-28', 'GE');
INSERT INTO `macs` VALUES ('90-4D-4A', 'GE');
INSERT INTO `macs` VALUES ('7C-79-E8', 'GE');
INSERT INTO `macs` VALUES ('B0-B2-8F', 'GE');
INSERT INTO `macs` VALUES ('B0-B9-8A', 'GE');
INSERT INTO `macs` VALUES ('38-42-A6', 'GE');
INSERT INTO `macs` VALUES ('2C-39-96', 'GE');
INSERT INTO `macs` VALUES ('54-E1-40', 'GE');
INSERT INTO `macs` VALUES ('18-5D-9A', 'GE');
INSERT INTO `macs` VALUES ('5C-3B-35', 'GE');
INSERT INTO `macs` VALUES ('E4-F4-C6', 'GE');
INSERT INTO `macs` VALUES ('78-9F-4C', 'GE');
INSERT INTO `macs` VALUES ('EC-D0-40', 'GE');
INSERT INTO `macs` VALUES ('68-FB-95', 'GE');
INSERT INTO `macs` VALUES ('C4-AD-21', 'GE');
INSERT INTO `macs` VALUES ('2C-62-89', 'GE');
INSERT INTO `macs` VALUES ('8C-6A-E4', 'GE');
INSERT INTO `macs` VALUES ('64-F2-42', 'GE');
INSERT INTO `macs` VALUES ('E8-3E-FB', 'GE');
INSERT INTO `macs` VALUES ('3C-C1-F6', 'GE');
INSERT INTO `macs` VALUES ('AC-51-EE', 'GE');
INSERT INTO `macs` VALUES ('B8-74-47', 'GE');
INSERT INTO `macs` VALUES ('AC-81-12', 'GE');
INSERT INTO `macs` VALUES ('B0-89-91', 'GE');
INSERT INTO `macs` VALUES ('C4-B5-12', 'GE');
INSERT INTO `macs` VALUES ('F0-E5-C3', 'GE');
INSERT INTO `macs` VALUES ('08-F6-F8', 'GE');
INSERT INTO `macs` VALUES ('90-A7-C1', 'GE');
INSERT INTO `macs` VALUES ('88-B6-27', 'GE');
INSERT INTO `macs` VALUES ('F8-47-2D', 'GE');
INSERT INTO `macs` VALUES ('C8-C1-3C', 'GE');
INSERT INTO `macs` VALUES ('AC-BE-B6', 'GE');
INSERT INTO `macs` VALUES ('00-24-C6', 'GE');
INSERT INTO `macs` VALUES ('00-22-71', 'GE');
INSERT INTO `macs` VALUES ('00-22-54', 'GE');
INSERT INTO `macs` VALUES ('00-1F-E0', 'GE');
INSERT INTO `macs` VALUES ('00-1F-74', 'GE');
INSERT INTO `macs` VALUES ('00-1F-0C', 'GE');
INSERT INTO `macs` VALUES ('00-1F-8D', 'GE');
INSERT INTO `macs` VALUES ('00-1E-D0', 'GE');
INSERT INTO `macs` VALUES ('00-1F-44', 'GE');
INSERT INTO `macs` VALUES ('00-1D-1B', 'GE');
INSERT INTO `macs` VALUES ('00-1C-81', 'GE');
INSERT INTO `macs` VALUES ('00-1B-96', 'GE');
INSERT INTO `macs` VALUES ('00-1A-1A', 'GE');
INSERT INTO `macs` VALUES ('00-17-55', 'GE');
INSERT INTO `macs` VALUES ('00-17-13', 'GE');
INSERT INTO `macs` VALUES ('00-14-4D', 'GE');
INSERT INTO `macs` VALUES ('00-13-B1', 'GE');
INSERT INTO `macs` VALUES ('00-0F-5D', 'GE');
INSERT INTO `macs` VALUES ('00-0E-7A', 'GE');
INSERT INTO `macs` VALUES ('00-0D-FD', 'GE');
INSERT INTO `macs` VALUES ('00-09-E5', 'GE');
INSERT INTO `macs` VALUES ('00-0A-AA', 'GE');
INSERT INTO `macs` VALUES ('00-0A-5D', 'GE');
INSERT INTO `macs` VALUES ('00-0A-DC', 'GE');
INSERT INTO `macs` VALUES ('00-08-4E', 'GE');
INSERT INTO `macs` VALUES ('00-09-61', 'GE');
INSERT INTO `macs` VALUES ('00-09-53', 'GE');
INSERT INTO `macs` VALUES ('2000/7/15', 'GE');
INSERT INTO `macs` VALUES ('2000/7/8', 'GE');
INSERT INTO `macs` VALUES ('00-05-3D', 'GE');
INSERT INTO `macs` VALUES ('00-03-5B', 'GE');
INSERT INTO `macs` VALUES ('2000/4/30', 'GE');
INSERT INTO `macs` VALUES ('00-04-1B', 'GE');
INSERT INTO `macs` VALUES ('00-03-81', 'GE');
INSERT INTO `macs` VALUES ('2000/3/10', 'GE');
INSERT INTO `macs` VALUES ('00-30-5A', 'GE');
INSERT INTO `macs` VALUES ('00-30-2F', 'GE');
INSERT INTO `macs` VALUES ('00-30-23', 'GE');
INSERT INTO `macs` VALUES ('00-D0-E9', 'GE');
INSERT INTO `macs` VALUES ('00-E0-9B', 'GE');
INSERT INTO `macs` VALUES ('00-E0-55', 'GE');
INSERT INTO `macs` VALUES ('00-E0-8A', 'GE');
INSERT INTO `macs` VALUES ('00-A0-06', 'GE');
INSERT INTO `macs` VALUES ('00-60-76', 'GE');
INSERT INTO `macs` VALUES ('00-A0-6C', 'GE');
INSERT INTO `macs` VALUES ('00-C0-E5', 'GE');
INSERT INTO `macs` VALUES ('00-C0-B1', 'GE');
INSERT INTO `macs` VALUES ('00-00-FA', 'GE');
INSERT INTO `macs` VALUES ('00-DD-05', 'GE');
INSERT INTO `macs` VALUES ('00-BB-F0', 'GE');
INSERT INTO `macs` VALUES ('00-80-E9', 'GE');
INSERT INTO `macs` VALUES ('00-DD-02', 'GE');
INSERT INTO `macs` VALUES ('00-DD-01', 'GE');
INSERT INTO `macs` VALUES ('00-15-56', 'GE');
INSERT INTO `macs` VALUES ('00-25-69', 'GE');
INSERT INTO `macs` VALUES ('00-1B-BF', 'GE');
INSERT INTO `macs` VALUES ('4C-17-EB', 'GE');
INSERT INTO `macs` VALUES ('7C-03-4C', 'GE');
INSERT INTO `macs` VALUES ('30-46-9A', 'GE');
INSERT INTO `macs` VALUES ('00-26-F2', 'GE');
INSERT INTO `macs` VALUES ('00-18-4D', 'GE');
INSERT INTO `macs` VALUES ('00-1E-2A', 'GE');
INSERT INTO `macs` VALUES ('E8-FC-AF', 'GE');
INSERT INTO `macs` VALUES ('4C-60-DE', 'GE');
INSERT INTO `macs` VALUES ('A0-63-91', 'GE');
INSERT INTO `macs` VALUES ('DC-EF-09', 'GE');
INSERT INTO `macs` VALUES ('20-0C-C8', 'GE');
INSERT INTO `macs` VALUES ('FC-B6-98', 'GE');
INSERT INTO `macs` VALUES ('00-19-C7', 'GE');
INSERT INTO `macs` VALUES ('00-14-A5', 'GE');
INSERT INTO `macs` VALUES ('A0-9D-91', 'GE');
INSERT INTO `macs` VALUES ('AC-84-C9', 'GE');
INSERT INTO `macs` VALUES ('00-14-8C', 'GE');
INSERT INTO `macs` VALUES ('00-DD-0A', 'GE');
INSERT INTO `macs` VALUES ('C0-85-4C', 'GE');
INSERT INTO `macs` VALUES ('F4-54-33', 'Rockwell');
INSERT INTO `macs` VALUES ('E4-90-69', 'Rockwell');
INSERT INTO `macs` VALUES ('00-00-BC', 'Rockwell');
INSERT INTO `macs` VALUES ('00-1D-9C', 'Rockwell');
INSERT INTO `macs` VALUES ('34-C0-F9', 'Rockwell');
INSERT INTO `macs` VALUES ('38-E0-8E', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-1E-D9', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-00-BD', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('08-00-70', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-26-92', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-10-C9', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('EC-5C-69', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-0A-66', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-90-DF', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('10-4B-46', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('00-60-6A', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('58-52-8A', 'MITSUBISHI');
INSERT INTO `macs` VALUES ('20-44-3A', 'Schneider');
INSERT INTO `macs` VALUES ('00-11-00', 'Schneider');
INSERT INTO `macs` VALUES ('00-00-54', 'Schneider');
INSERT INTO `macs` VALUES ('00-13-24', 'Schneider');
INSERT INTO `macs` VALUES ('00-0C-81', 'Schneider');
INSERT INTO `macs` VALUES ('94-2E-17', 'Schneider');
INSERT INTO `macs` VALUES ('50-31-AD', 'ABB');
INSERT INTO `macs` VALUES ('00-1B-45', 'ABB');
INSERT INTO `macs` VALUES ('00-1C-01', 'ABB');
INSERT INTO `macs` VALUES ('00-13-86', 'ABB');
INSERT INTO `macs` VALUES ('00-0C-62', 'ABB');
INSERT INTO `macs` VALUES ('00-0C-DE', 'ABB');
INSERT INTO `macs` VALUES ('00-90-4F', 'ABB');
INSERT INTO `macs` VALUES ('54-F8-76', 'ABB');
INSERT INTO `macs` VALUES ('AC-D3-64', 'ABB');
INSERT INTO `macs` VALUES ('00-24-59', 'ABB');
INSERT INTO `macs` VALUES ('B8-9B-E4', 'ABB');
INSERT INTO `macs` VALUES ('C0-A2-6D', 'ABB');
INSERT INTO `macs` VALUES ('60-39-1F', 'ABB');
INSERT INTO `macs` VALUES ('00-0C-02', 'ABB');
INSERT INTO `macs` VALUES ('00-02-2C', 'ABB');
INSERT INTO `macs` VALUES ('00-02-A3', 'ABB');
INSERT INTO `macs` VALUES ('80-3B-2A', 'ABB');
INSERT INTO `macs` VALUES ('78-AB-60', 'ABB');
INSERT INTO `macs` VALUES ('80-7A-7F', 'ABB');
INSERT INTO `macs` VALUES ('98-03-A0', 'ABB');
INSERT INTO `macs` VALUES ('00-21-C1', 'ABB');
INSERT INTO `macs` VALUES ('00-1C-BC', 'ABB');
INSERT INTO `macs` VALUES ('00-13-DD', 'ABB');
INSERT INTO `macs` VALUES ('00-00-23', 'ABB');
INSERT INTO `macs` VALUES ('00-12-E9', 'ABB');
INSERT INTO `macs` VALUES ('00-0D-97', 'ABB');
INSERT INTO `macs` VALUES ('00-03-2C', 'ABB');
INSERT INTO `macs` VALUES ('98-FF-D0', 'Lenovo');
INSERT INTO `macs` VALUES ('50-3C-C4', 'Lenovo');
INSERT INTO `macs` VALUES ('74-04-2B', 'Lenovo');
INSERT INTO `macs` VALUES ('E0-2C-B2', 'Lenovo');
INSERT INTO `macs` VALUES ('70-72-0D', 'Lenovo');
INSERT INTO `macs` VALUES ('D4-22-3F', 'Lenovo');
INSERT INTO `macs` VALUES ('14-36-C6', 'Lenovo');
INSERT INTO `macs` VALUES ('A4-8C-DB', 'Lenovo');
INSERT INTO `macs` VALUES ('6C-5F-1C', 'Lenovo');
INSERT INTO `macs` VALUES ('14-9F-E8', 'Lenovo');
INSERT INTO `macs` VALUES ('A0-32-99', 'Lenovo');
INSERT INTO `macs` VALUES ('AC-38-70', 'Lenovo');
INSERT INTO `macs` VALUES ('CC-07-E4', 'Lenovo');
INSERT INTO `macs` VALUES ('C8-DD-C9', 'Lenovo');
INSERT INTO `macs` VALUES ('80-CF-41', 'Lenovo');
INSERT INTO `macs` VALUES ('EC-89-F5', 'Lenovo');
INSERT INTO `macs` VALUES ('00-12-FE', 'Lenovo');
INSERT INTO `macs` VALUES ('60-D9-A0', 'Lenovo');
INSERT INTO `macs` VALUES ('20-76-93', 'Lenovo');
INSERT INTO `macs` VALUES ('88-70-8C', 'Lenovo');
INSERT INTO `macs` VALUES ('00-59-07', 'Lenovo');
INSERT INTO `macs` VALUES ('D8-71-57', 'Lenovo');
INSERT INTO `macs` VALUES ('BC-6E-64', 'Sony');
INSERT INTO `macs` VALUES ('A0-E4-53', 'Sony');
INSERT INTO `macs` VALUES ('1C-7B-21', 'Sony');
INSERT INTO `macs` VALUES ('24-21-AB', 'Sony');
INSERT INTO `macs` VALUES ('6C-23-B9', 'Sony');
INSERT INTO `macs` VALUES ('58-17-0C', 'Sony');
INSERT INTO `macs` VALUES ('B8-F9-34', 'Sony');
INSERT INTO `macs` VALUES ('20-54-76', 'Sony');
INSERT INTO `macs` VALUES ('30-39-26', 'Sony');
INSERT INTO `macs` VALUES ('00-EB-2D', 'Sony');
INSERT INTO `macs` VALUES ('00-13-A9', 'Sony');
INSERT INTO `macs` VALUES ('00-21-9E', 'Sony');
INSERT INTO `macs` VALUES ('00-1E-45', 'Sony');
INSERT INTO `macs` VALUES ('00-18-13', 'Sony');
INSERT INTO `macs` VALUES ('AC-9B-0A', 'Sony');
INSERT INTO `macs` VALUES ('00-13-15', 'Sony');
INSERT INTO `macs` VALUES ('00-1F-A7', 'Sony');
INSERT INTO `macs` VALUES ('A8-E3-EE', 'Sony');
INSERT INTO `macs` VALUES ('70-9E-29', 'Sony');
INSERT INTO `macs` VALUES ('FC-0F-E6', 'Sony');
INSERT INTO `macs` VALUES ('04-5D-4B', 'Sony');
INSERT INTO `macs` VALUES ('28-3F-69', 'Sony');
INSERT INTO `macs` VALUES ('40-40-A7', 'Sony');
INSERT INTO `macs` VALUES ('40-B8-37', 'Sony');
INSERT INTO `macs` VALUES ('C4-3A-BE', 'Sony');
INSERT INTO `macs` VALUES ('30-75-12', 'Sony');
INSERT INTO `macs` VALUES ('4C-21-D0', 'Sony');
INSERT INTO `macs` VALUES ('54-53-ED', 'Sony');
INSERT INTO `macs` VALUES ('F0-BF-97', 'Sony');
INSERT INTO `macs` VALUES ('40-2B-A1', 'Sony');
INSERT INTO `macs` VALUES ('00-25-E7', 'Sony');
INSERT INTO `macs` VALUES ('D0-51-62', 'Sony');
INSERT INTO `macs` VALUES ('94-CE-2C', 'Sony');
INSERT INTO `macs` VALUES ('00-1A-80', 'Sony');
INSERT INTO `macs` VALUES ('00-24-BE', 'Sony');
INSERT INTO `macs` VALUES ('00-16-20', 'Sony');
INSERT INTO `macs` VALUES ('00-12-EE', 'Sony');
INSERT INTO `macs` VALUES ('84-C7-EA', 'Sony');
INSERT INTO `macs` VALUES ('F8-46-1C', 'Sony');
INSERT INTO `macs` VALUES ('10-4F-A8', 'Sony');
INSERT INTO `macs` VALUES ('BC-60-A7', 'Sony');
INSERT INTO `macs` VALUES ('44-74-6C', 'Sony');
INSERT INTO `macs` VALUES ('30-A8-DB', 'Sony');
INSERT INTO `macs` VALUES ('54-42-49', 'Sony');
INSERT INTO `macs` VALUES ('00-1D-BA', 'Sony');
INSERT INTO `macs` VALUES ('00-0A-D9', 'Sony');
INSERT INTO `macs` VALUES ('00-0F-DE', 'Sony');
INSERT INTO `macs` VALUES ('00-1E-DC', 'Sony');
INSERT INTO `macs` VALUES ('00-19-63', 'Sony');
INSERT INTO `macs` VALUES ('00-1B-59', 'Sony');
INSERT INTO `macs` VALUES ('78-84-3C', 'Sony');
INSERT INTO `macs` VALUES ('00-23-F1', 'Sony');
INSERT INTO `macs` VALUES ('30-17-C8', 'Sony');
INSERT INTO `macs` VALUES ('18-00-2D', 'Sony');
INSERT INTO `macs` VALUES ('08-00-46', 'Sony');
INSERT INTO `macs` VALUES ('3C-07-71', 'Sony');
INSERT INTO `macs` VALUES ('D8-D4-3C', 'Sony');
INSERT INTO `macs` VALUES ('0C-FE-45', 'Sony');
INSERT INTO `macs` VALUES ('F8-D0-AC', 'Sony');
INSERT INTO `macs` VALUES ('00-D9-D1', 'Sony');
INSERT INTO `macs` VALUES ('00-04-1F', 'Sony');
INSERT INTO `macs` VALUES ('00-1D-0D', 'Sony');
INSERT INTO `macs` VALUES ('58-48-22', 'Sony');
INSERT INTO `macs` VALUES ('68-76-4F', 'Sony');
INSERT INTO `macs` VALUES ('30-F9-ED', 'Sony');
INSERT INTO `macs` VALUES ('00-22-A6', 'Sony');
INSERT INTO `macs` VALUES ('00-24-EF', 'Sony');
INSERT INTO `macs` VALUES ('6C-0E-0D', 'Sony');
INSERT INTO `macs` VALUES ('B4-52-7D', 'Sony');
INSERT INTO `macs` VALUES ('E0-63-E5', 'Sony');
INSERT INTO `macs` VALUES ('00-0E-07', 'Sony');
INSERT INTO `macs` VALUES ('00-1A-75', 'Sony');
INSERT INTO `macs` VALUES ('00-16-B8', 'Sony');
INSERT INTO `macs` VALUES ('00-1D-28', 'Sony');
INSERT INTO `macs` VALUES ('00-1F-E4', 'Sony');
INSERT INTO `macs` VALUES ('00-22-98', 'Sony');
INSERT INTO `macs` VALUES ('28-0D-FC', 'Sony');
INSERT INTO `macs` VALUES ('00-15-C1', 'Sony');
INSERT INTO `macs` VALUES ('00-19-C5', 'Sony');
INSERT INTO `macs` VALUES ('84-8E-DF', 'Sony');
INSERT INTO `macs` VALUES ('44-D4-E0', 'Sony');
INSERT INTO `macs` VALUES ('B4-52-7E', 'Sony');
INSERT INTO `macs` VALUES ('00-00-95', 'Sony');
INSERT INTO `macs` VALUES ('00-01-4A', 'Sony');
INSERT INTO `macs` VALUES ('00-1C-A4', 'Sony');
INSERT INTO `macs` VALUES ('00-23-45', 'Sony');
INSERT INTO `macs` VALUES ('8C-64-22', 'Sony');
INSERT INTO `macs` VALUES ('90-C1-15', 'Sony');
INSERT INTO `macs` VALUES ('84-00-D2', 'Sony');
INSERT INTO `macs` VALUES ('5C-B5-24', 'Sony');
INSERT INTO `macs` VALUES ('9C-5C-F9', 'Sony');
INSERT INTO `macs` VALUES ('00-24-8D', 'Sony');
INSERT INTO `macs` VALUES ('FC-F1-52', 'Sony');

-- ----------------------------
-- Table structure for modbustcp
-- ----------------------------
DROP TABLE IF EXISTS `modbustcp`;
CREATE TABLE `modbustcp` (
  `fw_mac` varchar(255) DEFAULT NULL,
  `protocol` varchar(255) DEFAULT NULL,
  `source` varchar(255) DEFAULT NULL,
  `destination` varchar(255) DEFAULT NULL,
  `coiladdressstart` varchar(255) DEFAULT NULL,
  `coiladdressend` varchar(255) DEFAULT NULL,
  `minspeed` varchar(255) DEFAULT NULL,
  `maxspeed` varchar(255) DEFAULT NULL,
  `functioncode` varchar(255) DEFAULT NULL,
  `method` varchar(255) DEFAULT NULL,
  `log_flag` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of modbustcp
-- ----------------------------
INSERT INTO `modbustcp` VALUES ('00:0b:ab:c6:60:12', 'modbusTcp', 'any', 'any', '12292', '12292', '10', '700', '6', 'true', 'DROP');

-- ----------------------------
-- Table structure for opc
-- ----------------------------
DROP TABLE IF EXISTS `opc`;
CREATE TABLE `opc` (
  `dev_IP` varchar(255) DEFAULT NULL,
  `dst_IP` varchar(255) DEFAULT NULL,
  `src_IP` varchar(255) DEFAULT NULL,
  `log_flag` varchar(255) DEFAULT NULL,
  `method` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of opc
-- ----------------------------

-- ----------------------------
-- Table structure for prt
-- ----------------------------
DROP TABLE IF EXISTS `prt`;
CREATE TABLE `prt` (
  `devIP` varchar(255) NOT NULL,
  `route_type` varchar(255) NOT NULL,
  `host` varchar(255) DEFAULT NULL,
  `net` varchar(255) DEFAULT NULL,
  `netmask` varchar(255) DEFAULT NULL,
  `Iface` varchar(255) DEFAULT NULL,
  `gateway` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of prt
-- ----------------------------

-- ----------------------------
-- Table structure for snat
-- ----------------------------
DROP TABLE IF EXISTS `snat`;
CREATE TABLE `snat` (
  `fwMAC` varchar(50) NOT NULL,
  `origin_devIP` varchar(50) NOT NULL,
  `EthName` varchar(10) NOT NULL,
  `EthIP` varchar(50) NOT NULL,
  `netmask` varchar(255) DEFAULT NULL,
  `NATIP` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of snat
-- ----------------------------
INSERT INTO `snat` VALUES ('MAC', '192.16.10.1', 'eth3', '172.16.10.1', '255.255.255.0', '172.16.10.19');

-- ----------------------------
-- Table structure for std
-- ----------------------------
DROP TABLE IF EXISTS `std`;
CREATE TABLE `std` (
  `devIP` varchar(255) NOT NULL,
  `protocol` varchar(255) NOT NULL,
  `srcIP` varchar(255) DEFAULT NULL,
  `dstIP` varchar(255) DEFAULT NULL,
  `sport` varchar(255) DEFAULT NULL,
  `dport` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of std
-- ----------------------------

-- ----------------------------
-- Table structure for useraccount
-- ----------------------------
DROP TABLE IF EXISTS `useraccount`;
CREATE TABLE `useraccount` (
  `UserID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `UserName` char(16) NOT NULL,
  `Password` char(8) DEFAULT NULL,
  `Purview` char(32) DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `useName` (`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of useraccount
-- ----------------------------
INSERT INTO `useraccount` VALUES ('1', 'admin', 'admin', 'SUPER');
INSERT INTO `useraccount` VALUES ('2', 'Engineer', '33333', 'OPER');
INSERT INTO `useraccount` VALUES ('3', 'Guest', '111', 'GUEST');

-- ----------------------------
-- Table structure for whl
-- ----------------------------
DROP TABLE IF EXISTS `whl`;
CREATE TABLE `whl` (
  `devIP` varchar(255) DEFAULT NULL,
  `dst_IP` varchar(255) DEFAULT NULL,
  `src_IP` varchar(255) DEFAULT NULL,
  `dst_port` varchar(255) DEFAULT NULL,
  `src_port` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of whl
-- ----------------------------
