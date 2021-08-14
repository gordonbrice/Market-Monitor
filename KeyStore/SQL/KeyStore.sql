USE [MarketMonitor]
GO

/****** Object:  Table [dbo].[KeyStore]    Script Date: 8/14/2021 2:18:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KeyStore](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[KeyType] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


