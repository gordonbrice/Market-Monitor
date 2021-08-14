USE [MarketMonitor]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 8/14/2021 2:19:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Log](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[TimeStamp] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


