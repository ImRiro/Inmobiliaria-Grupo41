-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: inmobiliaria-lab2
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Inquilinos`
--

CREATE DATABASE IF NOT EXISTS `inmobiliaria-lab2` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `inmobiliaria-lab2`;

DROP TABLE IF EXISTS `Inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Inquilinos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DNI` varchar(10) NOT NULL,
  `Nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DNI` (`DNI`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Inquilinos`
--

LOCK TABLES `Inquilinos` WRITE;
/*!40000 ALTER TABLE `Inquilinos` DISABLE KEYS */;
INSERT INTO `Inquilinos` VALUES (1,'12123123','Elin','ReservadorSerial@gmail.com','Quilino');
/*!40000 ALTER TABLE `Inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Propietarios`
--

DROP TABLE IF EXISTS `Propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Propietarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DNI` varchar(10) NOT NULL,
  `Nombre` varchar(150) NOT NULL,
  `Apellido` varchar(150) NOT NULL,
  `Email` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DNI` (`DNI`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Propietarios`
--

LOCK TABLES `Propietarios` WRITE;
/*!40000 ALTER TABLE `Propietarios` DISABLE KEYS */;
INSERT INTO `Propietarios` VALUES (7,'50100100','Roman','Riquelme','JRR10@gmail.com'),(8,'40100100','Lucas','Rodriguez','LuisLuisRodriguez@gmail.com');
/*!40000 ALTER TABLE `Propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'inmobiliaria-lab2'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-20 20:58:05
