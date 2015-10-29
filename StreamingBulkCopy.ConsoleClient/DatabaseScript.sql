USE [StreamingBulkCopy]
GO

/****** Object:  Table [dbo].[MyDomainEntities]    Script Date: 9/12/2014 10:41:33 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Data]') AND type in (N'U'))
DROP TABLE [dbo].[Data]
GO
USE [master]
GO

/****** Object:  Database [StreamingBulkCopy]    Script Date: 9/12/2014 10:41:33 AM ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'StreamingBulkCopy')
DROP DATABASE [StreamingBulkCopy]
GO

/****** Object:  Database [StreamingBulkCopy]    Script Date: 9/12/2014 10:41:33 AM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StreamingBulkCopy')
BEGIN
CREATE DATABASE [StreamingBulkCopy]
END
GO

ALTER DATABASE [StreamingBulkCopy] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StreamingBulkCopy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StreamingBulkCopy] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET ARITHABORT OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [StreamingBulkCopy] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StreamingBulkCopy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StreamingBulkCopy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StreamingBulkCopy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StreamingBulkCopy] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET RECOVERY FULL 
GO
ALTER DATABASE [StreamingBulkCopy] SET  MULTI_USER 
GO
ALTER DATABASE [StreamingBulkCopy] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StreamingBulkCopy] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StreamingBulkCopy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StreamingBulkCopy] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'StreamingBulkCopy', N'ON'
GO
USE [StreamingBulkCopy]
GO
/****** Object:  Table [dbo].[MyDomainEntities]    Script Date: 9/12/2014 10:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Data]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Data](
	[Field1] [nvarchar](max) NULL,
	[Field2] [nvarchar](max) NULL,
	[Field3] [nvarchar](max) NULL
) ON [PRIMARY]
END
GO
USE [master]
GO
ALTER DATABASE [StreamingBulkCopy] SET READ_WRITE 
GO
