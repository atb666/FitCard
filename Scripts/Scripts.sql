USE [master]
GO
/****** Object:  Database [FitCard]    Script Date: 12/03/2017 19:06:04 ******/
CREATE DATABASE [FitCard] ON  PRIMARY 
( NAME = N'FitCard', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\FitCard.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FitCard_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\FitCard_log.LDF' , SIZE = 576KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FitCard] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FitCard].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FitCard] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [FitCard] SET ANSI_NULLS OFF
GO
ALTER DATABASE [FitCard] SET ANSI_PADDING OFF
GO
ALTER DATABASE [FitCard] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [FitCard] SET ARITHABORT OFF
GO
ALTER DATABASE [FitCard] SET AUTO_CLOSE ON
GO
ALTER DATABASE [FitCard] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [FitCard] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [FitCard] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [FitCard] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [FitCard] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [FitCard] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [FitCard] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [FitCard] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [FitCard] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [FitCard] SET  ENABLE_BROKER
GO
ALTER DATABASE [FitCard] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [FitCard] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [FitCard] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [FitCard] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [FitCard] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [FitCard] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [FitCard] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [FitCard] SET  READ_WRITE
GO
ALTER DATABASE [FitCard] SET RECOVERY SIMPLE
GO
ALTER DATABASE [FitCard] SET  MULTI_USER
GO
ALTER DATABASE [FitCard] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [FitCard] SET DB_CHAINING OFF
GO
USE [FitCard]
GO
/****** Object:  Table [dbo].[State]    Script Date: 12/03/2017 19:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[State](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[r_State]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[r_State]
	@Id	INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM State T1 
	WHERE
		(@Id IS NULL OR T1.Id = @Id)
END
GO
/****** Object:  Table [dbo].[City]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StateId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[r_City]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[r_City]
	@Id			INT = NULL,
	@StateId	INT = NULL
AS
	SET NOCOUNT ON;

	SELECT *
	FROM City T1
	WHERE	(@Id IS NULL OR T1.Id = @Id)
		AND (@StateId IS NULL OR T1.StateId = @StateId)
GO
/****** Object:  Table [dbo].[Establishment]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Establishment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](50) NOT NULL,
	[TradingName] [varchar](50) NULL,
	[Cnpj] [varchar](50) NOT NULL,
	[Mail] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[CityId] [int] NULL,
	[Phone] [varchar](50) NULL,
	[RegistrationDate] [datetime] NULL,
	[Category] [smallint] NOT NULL,
	[Status] [smallint] NOT NULL,
 CONSTRAINT [PK_Establishment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[u_Establishment]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[u_Establishment]
	@Id					BIGINT,
	@CompanyName		VARCHAR(50),
	@TradingName		VARCHAR(50) = NULL,
	@Cnpj				VARCHAR(50),
	@Mail				VARCHAR(50) = NULL,
	@Address			VARCHAR(50) = NULL,
	@CityId				INT = NULL,
	@Phone				VARCHAR(50) = NULL,
	@RegistrationDate	DATETIME = NULL,
	@Category			SMALLINT,
	@Status				SMALLINT
AS

	UPDATE Establishment
	SET	CompanyName = @CompanyName,
		TradingName = @TradingName,
		Cnpj = @Cnpj,
		Mail = @Mail,
		Address = @Address,
		CityId = @CityId,
		Phone = @Phone,
		RegistrationDate = @RegistrationDate,
		Category = @Category,
		Status = @Status
	WHERE	Id = @Id
GO
/****** Object:  StoredProcedure [dbo].[r_Establishment]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[r_Establishment]
	@Id	BIGINT = NULL
AS
	SET NOCOUNT ON;

	SELECT *
	FROM Establishment T1
	WHERE	(@Id IS NULL OR T1.Id = @Id)
GO
/****** Object:  StoredProcedure [dbo].[d_Establishment]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[d_Establishment]
	@Id	BIGINT
AS
	DELETE Establishment
	WHERE Id = @Id
GO
/****** Object:  StoredProcedure [dbo].[c_Establishment]    Script Date: 12/03/2017 19:06:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[c_Establishment]
	@Id					BIGINT OUTPUT,
	@CompanyName		VARCHAR(50),
	@TradingName		VARCHAR(50) = NULL,
	@Cnpj				VARCHAR(50),
	@Mail				VARCHAR(50) = NULL,
	@Address			VARCHAR(50) = NULL,
	@CityId				INT = NULL,
	@Phone				VARCHAR(50) = NULL,
	@RegistrationDate	DATETIME = NULL,
	@Category			SMALLINT,
	@Status				SMALLINT
AS

	INSERT INTO Establishment
	(
		CompanyName,
		TradingName,
		Cnpj,
		Mail,
		Address,
		CityId,
		Phone,
		RegistrationDate,
		Category,
		Status
	)
	VALUES
	(
		@CompanyName,
		@TradingName,
		@Cnpj,
		@Mail,
		@Address,
		@CityId,
		@Phone,
		@RegistrationDate,
		@Category,
		@Status
	)

	SET @Id = SCOPE_IDENTITY();
GO
/****** Object:  ForeignKey [FK_City_State]    Script Date: 12/03/2017 19:06:15 ******/
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [FK_City_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_State]
GO
/****** Object:  ForeignKey [FK_Establishment_Establishment]    Script Date: 12/03/2017 19:06:15 ******/
ALTER TABLE [dbo].[Establishment]  WITH CHECK ADD  CONSTRAINT [FK_Establishment_Establishment] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Establishment] CHECK CONSTRAINT [FK_Establishment_Establishment]
GO
INSERT INTO [State] (Name) VALUES ('São Paulo')
INSERT INTO [State] (Name) VALUES ('Minas Gerais')
INSERT INTO City (Name, StateId) VALUES ('Campinas', 1)
INSERT INTO City (Name, StateId) VALUES ('São Paulo', 1)
INSERT INTO City (Name, StateId) VALUES ('Belo Horizonte', 2)
INSERT INTO City (Name, StateId) VALUES ('Poços de Caldas', 2)
GO