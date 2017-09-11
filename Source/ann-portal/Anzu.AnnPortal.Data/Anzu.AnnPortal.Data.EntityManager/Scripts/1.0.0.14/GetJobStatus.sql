USE [AestheticNeuralNetworksPortal]
GO
/****** Object:  StoredProcedure [dbo].[GetJobStatus]    Script Date: 4/11/2017 8:49:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Charith>
-- Create date: <11/04/2017>
-- Description:	<Get SQL Job Status by name>
-- =============================================
CREATE PROCEDURE [dbo].[GetJobStatus]
	-- Add the parameters for the stored procedure here
	@jobName NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON  

	DECLARE @xp_results TABLE  
	(job_id               UNIQUEIDENTIFIER NOT NULL, 
	last_run_date         INT              NOT NULL, 
	last_run_time         INT              NOT NULL, 
	next_run_date         INT              NOT NULL, 
	next_run_time         INT              NOT NULL, 
	next_run_schedule_id  INT              NOT NULL, 
	requested_to_run      INT              NOT NULL, 
	request_source        INT              NOT NULL, 
	request_source_id     sysname          COLLATE database_default NULL, 
	running               INT              NOT NULL, 
	current_step          INT              NOT NULL, 
	current_retry_attempt INT              NOT NULL, 
	job_state             INT              NOT NULL) 

	DECLARE @job_owner sysname   SET @job_owner = SUSER_SNAME() 
	INSERT INTO @xp_results 
	EXECUTE master.dbo.xp_sqlagent_enum_jobs 1, @job_owner 

	SELECT running, current_step, last_run_date, last_run_time, job_state
	FROM @xp_results x 
		INNER JOIN msdb.dbo.sysjobs sj ON sj.job_id = x.job_id 
	WHERE sj.name = @jobName
	
END
