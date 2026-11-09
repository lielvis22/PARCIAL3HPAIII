/* ============================================================
   Parcial #3 - Ejercicio 3: Login con BCrypt
   Script de creación de la base de datos (SQL Server)
   ============================================================ */

IF DB_ID('LoginBCrypt') IS NULL
    CREATE DATABASE LoginBCrypt;
GO

USE LoginBCrypt;
GO

-- Tabla de usuarios: la contraseña NUNCA se guarda en texto plano,
-- solo el hash generado con BCrypt (60 caracteres, incluye la sal).
IF OBJECT_ID('dbo.Usuarios') IS NULL
CREATE TABLE dbo.Usuarios
(
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario   NVARCHAR(50)  NOT NULL UNIQUE,
    NombreCompleto  NVARCHAR(100) NOT NULL,
    PasswordHash    NVARCHAR(100) NOT NULL,
    FechaCreacion   DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- Tabla de intentos de acceso (fallidos y exitosos) para análisis posterior.
IF OBJECT_ID('dbo.IntentosLogin') IS NULL
CREATE TABLE dbo.IntentosLogin
(
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario   NVARCHAR(50)  NOT NULL,
    Exitoso         BIT           NOT NULL,
    Fecha           DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

/* ------------------------------------------------------------
   Consultas útiles para el análisis de intentos
   ------------------------------------------------------------ */
-- Historial completo
-- SELECT * FROM dbo.IntentosLogin ORDER BY Fecha DESC;

-- Resumen por usuario
-- SELECT NombreUsuario,
--        SUM(CASE WHEN Exitoso = 1 THEN 1 ELSE 0 END) AS Exitosos,
--        SUM(CASE WHEN Exitoso = 0 THEN 1 ELSE 0 END) AS Fallidos
-- FROM dbo.IntentosLogin
-- GROUP BY NombreUsuario;

/* ------------------------------------------------------------
   Backup de la base de datos (para subir al repositorio).
   Cambie la ruta por una carpeta existente en su equipo.
   ------------------------------------------------------------ */
-- BACKUP DATABASE LoginBCrypt
-- TO DISK = 'C:\Backups\LoginBCrypt.bak'
-- WITH FORMAT, INIT, NAME = 'Backup LoginBCrypt - Parcial 3';
