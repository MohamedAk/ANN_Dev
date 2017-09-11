USE [AestheticNeuralNetworksPortal]
GO
/****** Object:  StoredProcedure [dbo].[GetServicesFromStaging]    Script Date: 4/12/2017 7:21:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:           <Author,,Name>
-- Create date: <Create Date,,>
-- Description:      <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetServicesFromStaging]  
       -- Add the parameters for the stored procedure here
       @emrId VARCHAR(10)
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    SELECT s.EMRId,s.EMRServiceId,s.ServiceName,p.Id,p.PracticeId,p.EmrProcedure,p.ProcedureId,p.CompanyId,p.ProductTypeId,p.IsProductSale, p.IsDiscarded 
       from [AestheticNeuralNetworksStaging].[dbo].[m_Services] as s
       LEFT JOIN annPortal.PracticeProcedures p ON p.EmrProcedure = s.EMRServiceId
       where s.EMRId = @emrId and s.ServiceName is not null
       ORDER BY  p.IsDiscarded, s.ServiceName 

END
