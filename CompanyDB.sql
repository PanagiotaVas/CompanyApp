USE [master]
GO
/****** Object:  Database [CompanyDB]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE DATABASE [CompanyDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CompanyDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CompanyDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CompanyDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CompanyDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CompanyDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CompanyDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CompanyDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CompanyDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CompanyDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CompanyDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CompanyDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CompanyDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CompanyDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CompanyDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CompanyDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CompanyDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CompanyDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CompanyDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CompanyDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CompanyDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CompanyDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CompanyDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CompanyDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CompanyDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CompanyDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CompanyDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CompanyDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CompanyDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CompanyDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CompanyDB] SET  MULTI_USER 
GO
ALTER DATABASE [CompanyDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CompanyDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CompanyDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CompanyDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CompanyDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CompanyDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CompanyDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CompanyDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CompanyDB]
GO
/****** Object:  User [usercomp]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE USER [usercomp] FOR LOGIN [usercomp] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [usercomp]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/6/2024 1:08:36 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPLOYEES]    Script Date: 19/6/2024 1:08:36 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLOYEES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FIRSTNAME] [nvarchar](100) NULL,
	[LASTNAME] [nvarchar](100) NULL,
	[EMAIL] [nvarchar](200) NOT NULL,
	[PHONE_NUMBER] [nvarchar](10) NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_EMPLOYEES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPLOYEES_X_TASKS]    Script Date: 19/6/2024 1:08:36 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLOYEES_X_TASKS](
	[EMPLOYEE_ID] [int] NOT NULL,
	[TASK_ID] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_EMPLOYEES_X_TASKS] PRIMARY KEY CLUSTERED 
(
	[TASK_ID] ASC,
	[EMPLOYEE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASKS]    Script Date: 19/6/2024 1:08:36 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASKS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TITLE] [nvarchar](100) NOT NULL,
	[DESCRIPTION] [nvarchar](300) NULL,
	[DEADLINE] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TASKS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 19/6/2024 1:08:36 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[USERNAME] [nvarchar](100) NOT NULL,
	[PASSWORD] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_USERS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_EMPLOYEES_UserId]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE NONCLUSTERED INDEX [IX_EMPLOYEES_UserId] ON [dbo].[EMPLOYEES]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_EMAIL]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_EMAIL] ON [dbo].[EMPLOYEES]
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EMPLOYEES_X_TASKS_EMPLOYEE_ID]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE NONCLUSTERED INDEX [IX_EMPLOYEES_X_TASKS_EMPLOYEE_ID] ON [dbo].[EMPLOYEES_X_TASKS]
(
	[EMPLOYEE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EMPLOYEES_X_TASKS_UserId]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE NONCLUSTERED INDEX [IX_EMPLOYEES_X_TASKS_UserId] ON [dbo].[EMPLOYEES_X_TASKS]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TASKS_UserId]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE NONCLUSTERED INDEX [IX_TASKS_UserId] ON [dbo].[TASKS]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_USERNAME]    Script Date: 19/6/2024 1:08:36 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_USERNAME] ON [dbo].[USERS]
(
	[USERNAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EMPLOYEES]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEES_USERS] FOREIGN KEY([UserId])
REFERENCES [dbo].[USERS] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[EMPLOYEES] CHECK CONSTRAINT [FK_EMPLOYEES_USERS]
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS]  WITH CHECK ADD  CONSTRAINT [FK__EMPLOYEES__EMPLO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEES] ([ID])
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS] CHECK CONSTRAINT [FK__EMPLOYEES__EMPLO]
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS]  WITH CHECK ADD  CONSTRAINT [FK__EMPLOYEES__PROJ] FOREIGN KEY([TASK_ID])
REFERENCES [dbo].[TASKS] ([ID])
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS] CHECK CONSTRAINT [FK__EMPLOYEES__PROJ]
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEES_X_TASKS_USERS_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[USERS] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EMPLOYEES_X_TASKS] CHECK CONSTRAINT [FK_EMPLOYEES_X_TASKS_USERS_UserId]
GO
ALTER TABLE [dbo].[TASKS]  WITH CHECK ADD  CONSTRAINT [FK_TASKS_USERS_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[USERS] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TASKS] CHECK CONSTRAINT [FK_TASKS_USERS_UserId]
GO
USE [master]
GO
ALTER DATABASE [CompanyDB] SET  READ_WRITE 
GO
