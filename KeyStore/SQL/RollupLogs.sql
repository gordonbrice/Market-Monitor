
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Gordon Brice
-- Create date: 2021-08-14
-- Description:	
-- =============================================
ALTER PROCEDURE RollupLogs 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Date date
	select top(1) @Date = TimeStamp 
	from Log 
	where DATENAME(DAYOFYEAR, TimeStamp) < DATENAME(DAYOFYEAR, getdate()) or DATENAME(YEAR, TimeStamp) < DATENAME(YEAR, GETDATE())
	order by TimeStamp

	if @Date is not null exec LogDailyRollup @Date
	select @Date as RollupDate
END
GO
