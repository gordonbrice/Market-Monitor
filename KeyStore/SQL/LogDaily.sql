USE [MarketMonitor]
GO

/****** Object:  Table [dbo].[LogDaily]    Script Date: 8/14/2021 2:20:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LogDaily](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Count] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Date] [date] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


