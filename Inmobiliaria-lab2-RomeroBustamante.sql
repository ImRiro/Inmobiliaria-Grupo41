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
-- Table structure for table `imageninmueble`
--

DROP TABLE IF EXISTS `imageninmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imageninmueble` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `id_inmueble` int NOT NULL,
  `url` varchar(500) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_imagen_inmueble` (`id_inmueble`),
  CONSTRAINT `fk_imagen_inmueble` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imageninmueble`
--

LOCK TABLES `imageninmueble` WRITE;
/*!40000 ALTER TABLE `imageninmueble` DISABLE KEYS */;
INSERT INTO `imageninmueble` VALUES (106,101,'/inmuebles/galeria/a9697ace00834c9596922d3333c100f9.jpg'),(107,101,'/inmuebles/galeria/919562ee18b344eb964a1371a87b4cd4.jpg');
/*!40000 ALTER TABLE `imageninmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmueble` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `idPropietario` int NOT NULL,
  `IdTipoInmueble` int NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `latitud` decimal(10,7) NOT NULL DEFAULT '0.0000000',
  `longitud` decimal(10,7) NOT NULL DEFAULT '0.0000000',
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  `metros_cuadrados` decimal(10,2) NOT NULL DEFAULT '0.00',
  `habitaciones` int NOT NULL DEFAULT '0',
  `ruta_portada` varchar(500) DEFAULT NULL,
  `Disponible` tinyint NOT NULL DEFAULT '1',
  `porcentaje_sena` decimal(5,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_inmueble_propietario` (`idPropietario`),
  KEY `fk_inmueble_tipo` (`IdTipoInmueble`),
  CONSTRAINT `fk_inmueble_propietario` FOREIGN KEY (`idPropietario`) REFERENCES `propietarios` (`Id`),
  CONSTRAINT `fk_inmueble_tipo` FOREIGN KEY (`IdTipoInmueble`) REFERENCES `tipoinmueble` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (101,101,101,'Av. Corrientes 1234, CABA',-34.6000000,-58.3800000,1,45.00,2,NULL,1,10.00),(102,102,102,'Florida 500, CABA',-34.6000000,-58.3800000,1,120.00,0,'/inmuebles/portadas/f6a4b068ef0144cca6884511a108c25c.webp',1,20.00),(103,103,103,'Ruta 40 Km 2000, Bariloche',-41.1300000,-71.3100000,1,85.00,3,NULL,0,15.00),(104,104,104,'Ruta Panamericana Km 30',-34.4841000,-58.6433000,1,500.00,1,NULL,1,30.00),(105,105,105,'Calle Falsa 123, Springfield',-34.6131000,-58.3772000,1,65.00,3,NULL,1,10.00);
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilinos`
--

DROP TABLE IF EXISTS `inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilinos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DNI` varchar(10) NOT NULL,
  `Nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DNI` (`DNI`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (101,'99887766','Florencia','fdiaz@ejemplo.com','Diaz',1),(102,'88776655','Diego','dalvarez@ejemplo.com','Alvarez',1),(103,'77665544','Sofia','sromero@ejemplo.com','Romero',1),(104,'66554433','Tomas','therrera@ejemplo.com','Herrera',1),(105,'55443322','Micaela','msuarez@ejemplo.com','Suarez',1);
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pago` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `idreserva` int NOT NULL,
  `concepto` varchar(200) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `fecha` date NOT NULL,
  `anulado` tinyint(1) NOT NULL DEFAULT '0',
  `fecha_anulacion` date DEFAULT NULL,
  `idusuariocreador` int DEFAULT NULL,
  `idusuarioanulador` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_pago_reserva` (`idreserva`),
  KEY `fk_pago_usuario_creador` (`idusuariocreador`),
  KEY `fk_pago_usuario_anulador` (`idusuarioanulador`),
  CONSTRAINT `fk_pago_reserva` FOREIGN KEY (`idreserva`) REFERENCES `reserva` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `fk_pago_usuario_anulador` FOREIGN KEY (`idusuarioanulador`) REFERENCES `usuario` (`Id`),
  CONSTRAINT `fk_pago_usuario_creador` FOREIGN KEY (`idusuariocreador`) REFERENCES `usuario` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (101,101,'Pago de seña del 10%',150.00,'2026-09-20',0,NULL,101,NULL),(102,102,'Pago total anticipado',3000.00,'2026-10-01',0,NULL,101,NULL),(103,103,'Pago de seña del 15%',300.00,'2026-11-15',0,NULL,102,NULL),(104,104,'Pago de seña (Anulado por cancelación)',75.00,'2026-11-01',1,'2026-12-01',102,104),(105,105,'Primera quincena',1120.00,'2027-02-01',0,NULL,103,NULL),(106,107,'Seña de reserva',480.00,'2026-09-17',0,NULL,106,NULL),(107,108,'Seña de reserva',290.00,'2026-09-17',0,NULL,106,NULL);
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietarios`
--

DROP TABLE IF EXISTS `propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DNI` varchar(10) NOT NULL,
  `Nombre` varchar(150) NOT NULL,
  `Apellido` varchar(150) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DNI` (`DNI`)
) ENGINE=InnoDB AUTO_INCREMENT=117 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (101,'11223344','Carlos','Gomez','carlos.gomez@ejemplo.com',1),(102,'22334455','Laura','Martinez','laura.m@ejemplo.com',1),(103,'33445566','Jorge','Lopez','jlopez@ejemplo.com',1),(104,'44556677','Ana','Silva','ana.silva@ejemplo.com',1),(105,'55667788','Martin','Paz','mpaz88@ejemplo.com',1),(107,'40123456','Juan','Pérez','juan.perez@gmail.com',1),(108,'41234567','María','Gómez','maria.gomez@gmail.com',1),(109,'42345678','Lucas','Fernández','lucas.fernandez@gmail.com',1),(110,'43456789','Sofía','Rodríguez','sofia.rodriguez@gmail.com',1),(111,'44567890','Martín','López','martin.lopez@gmail.com',1),(112,'45678901','Camila','Martínez','camila.martinez@gmail.com',1),(113,'46789012','Nicolás','García','nicolas.garcia@gmail.com',1),(114,'47890123','Valentina','Sánchez','valentina.sanchez@gmail.com',1),(115,'48901234','Diego','Torres','diego.torres@gmail.com',1),(116,'49012345','Carolina','Ramírez','carolina.ramirez@gmail.com',1);
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reserva`
--

DROP TABLE IF EXISTS `reserva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reserva` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `idinmueble` int NOT NULL,
  `idinquilino` int NOT NULL,
  `fecha_desde` date NOT NULL,
  `fecha_hasta` date NOT NULL,
  `fecha_cancelacion` date DEFAULT NULL,
  `monto_diario` decimal(10,2) NOT NULL,
  `costo_total` decimal(10,2) NOT NULL,
  `IdUsuarioCreador` int DEFAULT NULL,
  `IdUsuarioFinalizador` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_reserva_inmueble` (`idinmueble`),
  KEY `fk_reserva_inquilino` (`idinquilino`),
  KEY `FK_Reserva_Usuario_Creador` (`IdUsuarioCreador`),
  KEY `FK_Reserva_Usuario_Anulador` (`IdUsuarioFinalizador`),
  CONSTRAINT `fk_reserva_inmueble` FOREIGN KEY (`idinmueble`) REFERENCES `inmueble` (`Id`),
  CONSTRAINT `fk_reserva_inquilino` FOREIGN KEY (`idinquilino`) REFERENCES `inquilinos` (`Id`),
  CONSTRAINT `FK_Reserva_Usuario_Anulador` FOREIGN KEY (`IdUsuarioFinalizador`) REFERENCES `usuario` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `FK_Reserva_Usuario_Creador` FOREIGN KEY (`IdUsuarioCreador`) REFERENCES `usuario` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=109 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reserva`
--

LOCK TABLES `reserva` WRITE;
/*!40000 ALTER TABLE `reserva` DISABLE KEYS */;
INSERT INTO `reserva` VALUES (101,101,101,'2026-10-01','2026-10-10',NULL,150.00,1500.00,101,NULL),(102,102,102,'2026-11-05','2026-11-15',NULL,300.00,3000.00,101,NULL),(103,103,103,'2026-12-20','2026-12-30',NULL,200.00,2000.00,102,NULL),(104,104,104,'2027-01-05','2027-01-10','2026-12-01',50.00,250.00,102,104),(105,105,105,'2027-02-01','2027-02-28',NULL,80.00,2240.00,103,NULL),(107,104,103,'2026-09-09','2026-09-17',NULL,200.00,1600.00,106,NULL),(108,101,105,'2026-09-01','2026-09-30',NULL,100.00,2900.00,106,NULL);
/*!40000 ALTER TABLE `reserva` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipoinmueble`
--

DROP TABLE IF EXISTS `tipoinmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipoinmueble` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Activo` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipoinmueble`
--

LOCK TABLES `tipoinmueble` WRITE;
/*!40000 ALTER TABLE `tipoinmueble` DISABLE KEYS */;
INSERT INTO `tipoinmueble` VALUES (101,'Oficina Comercial',1),(102,'Local a la Calle',1),(103,'Cabaña',1),(104,'Galpón',1),(105,'PH',1);
/*!40000 ALTER TABLE `tipoinmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(100) NOT NULL,
  `clave` varchar(255) NOT NULL,
  `rol` varchar(20) NOT NULL DEFAULT 'Empleado',
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `avatar_url` varchar(500) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (101,'emp1@inmobiliaria.com','AQAAAAIAAYagAAAAEEolEjemploHash123==','Empleado','Raul','Perez',NULL,1),(102,'emp2@inmobiliaria.com','AQAAAAIAAYagAAAAEEolEjemploHash123==','Empleado','Carla','Gimenez',NULL,1),(103,'emp3@inmobiliaria.com','AQAAAAIAAYagAAAAEEolEjemploHash123==','Empleado','Esteban','Quito',NULL,1),(104,'gerente@inmobiliaria.com','AQAAAAIAAYagAAAAEEolEjemploHash123==','Administrador','Valeria','Rios',NULL,1),(105,'auditor@inmobiliaria.com','AQAAAAIAAYagAAAAEEolEjemploHash123==','Administrador','Hugo','Sosa',NULL,1),(106,'admin@inmobiliaria.com','AQAAAAIAAYagAAAAEDdVfOObMnhwiqCtDpXg1Jmt1cgRqOwx/oDssxu89wHrLoRzltgJdUPzAgmr5z6b0g==','Administrador','Admin','Sistema',NULL,1);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
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

-- Dump completed on 2026-09-17 23:57:39
