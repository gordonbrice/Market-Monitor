
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Gordon Brice
-- Create date: 2021-08-14
-- Description:	
-- =============================================
ALTER PROCEDURE LogDailyRollup 
	-- Add the parameters for the stored procedure here
	@Date date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select Name, Message
into #tmp
from Log 
where DATENAME(DAYOFYEAR, TimeStamp) < DATENAME(DAYOFYEAR, dateadd(DAY, 1, @Date)) or DATENAME(YEAR, TimeStamp) < DATENAME(YEAR, dateadd(DAY, 1, @Date))

insert LogDaily(Count, Name, Message, Date)
select count(*) as Count, Name, Message, @Date as Date 
from #tmp
group by Name, Message
having Message <> 'Working'

drop table #tmp
delete from Log where TimeStamp < dateadd(DAY, 1, @Date)
END
GO