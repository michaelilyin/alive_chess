IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[resource]')) 
   ALTER TABLE [dbo].[resource] 
   ENABLE  CHANGE_TRACKING
GO