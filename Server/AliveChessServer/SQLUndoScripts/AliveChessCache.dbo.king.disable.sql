IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[king]')) 
   ALTER TABLE [dbo].[king] 
   DISABLE  CHANGE_TRACKING
GO
