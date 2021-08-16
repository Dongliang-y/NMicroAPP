/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.1.218
 Source Server Type    : MySQL
 Source Server Version : 50621
 Source Host           : 192.168.1.218:3306
 Source Schema         : MicroPlatformSafe

 Target Server Type    : MySQL
 Target Server Version : 50621
 File Encoding         : 65001

 Date: 23/07/2020 11:36:50
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Sys_Loger
-- ----------------------------
DROP TABLE IF EXISTS `Sys_Loger`;
CREATE TABLE `Sys_Loger`  (
  `Id` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LogDate` datetime(0) DEFAULT NULL COMMENT '记录时间',
  `LogThread` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '线程',
  `LogLevel` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '日志级别',
  `LogLogger` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '记录人',
  `LogOperator` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '操作人',
  `LogMessage` varchar(1500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '内容',
  `LogIP` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT 'IP',
  `LogMachineName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '机器名',
  `LogBrowser` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '浏览器',
  `LogLocation` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '请求地址',
  `LogException` varchar(1500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '异常信息',
  `ModuleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '模块名',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '日志表' ROW_FORMAT = Compact;

SET FOREIGN_KEY_CHECKS = 1;
