USE [MarketMonitor]
GO

/****** Object:  Table [dbo].[DisplayProperties]    Script Date: 8/14/2021 2:17:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DisplayProperties](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KeyId] [bigint] NOT NULL,
	[DisplayName] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[FastQueryInterval] [int] NOT NULL,
	[SlowQueryInterval] [int] NULL,
	[QueryIntervalMultiplier] [int] NOT NULL
) ON [PRIMARY]
GO


