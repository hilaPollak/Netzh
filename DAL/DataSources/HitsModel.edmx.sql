
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/28/2019 00:41:21
-- Generated from EDMX file: C:\Users\הילה\Desktop\Hits\DAL\DataSources\HitsModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Hits];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Report__Reporter__2D27B809]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reports] DROP CONSTRAINT [FK__Report__Reporter__2D27B809];
GO
IF OBJECT_ID(N'[dbo].[FK__UpdatedRe__Repor__300424B4]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UpdatedReports] DROP CONSTRAINT [FK__UpdatedRe__Repor__300424B4];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Reports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reports];
GO
IF OBJECT_ID(N'[dbo].[Reporters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reporters];
GO
IF OBJECT_ID(N'[dbo].[UpdatedReports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UpdatedReports];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [ReportID] int  NOT NULL,
    [TimeFalling] datetime  NOT NULL,
    [Address] nvarchar(50)  NOT NULL,
    [ReporterID] int  NOT NULL,
    [AddressCoordinates] nvarchar(50)  NOT NULL,
    [NumberOfBooms] int  NOT NULL,
    [Updated] int  NULL
);
GO

-- Creating table 'Reporters'
CREATE TABLE [dbo].[Reporters] (
    [ReporterName] nvarchar(50)  NOT NULL,
    [ReporterID] int  NOT NULL,
    [ReporterAddress] nvarchar(80)  NOT NULL,
    [LatLongAddress] nvarchar(50)  NOT NULL,
    [ReporterProfilePicture] varbinary(max)  NULL
);
GO

-- Creating table 'UpdatedReports'
CREATE TABLE [dbo].[UpdatedReports] (
    [NewCoordinates] nvarchar(50)  NOT NULL,
    [ReportID] int  NOT NULL,
    [NewTime] datetime  NOT NULL,
    [NumberOfHits] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ReportID] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([ReportID] ASC);
GO

-- Creating primary key on [ReporterID] in table 'Reporters'
ALTER TABLE [dbo].[Reporters]
ADD CONSTRAINT [PK_Reporters]
    PRIMARY KEY CLUSTERED ([ReporterID] ASC);
GO

-- Creating primary key on [ReportID] in table 'UpdatedReports'
ALTER TABLE [dbo].[UpdatedReports]
ADD CONSTRAINT [PK_UpdatedReports]
    PRIMARY KEY CLUSTERED ([ReportID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ReporterID] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [FK__Report__Reporter__2D27B809]
    FOREIGN KEY ([ReporterID])
    REFERENCES [dbo].[Reporters]
        ([ReporterID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Report__Reporter__2D27B809'
CREATE INDEX [IX_FK__Report__Reporter__2D27B809]
ON [dbo].[Reports]
    ([ReporterID]);
GO

-- Creating foreign key on [ReportID] in table 'UpdatedReports'
ALTER TABLE [dbo].[UpdatedReports]
ADD CONSTRAINT [FK__UpdatedRe__Repor__300424B4]
    FOREIGN KEY ([ReportID])
    REFERENCES [dbo].[Reports]
        ([ReportID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------