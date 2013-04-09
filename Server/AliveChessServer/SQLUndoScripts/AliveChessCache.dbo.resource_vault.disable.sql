IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[resource_vault]')) 
   ALTER TABLE [dbo].[resource_vault] 
   DISABLE  CHANGE_TRACKING
GO
