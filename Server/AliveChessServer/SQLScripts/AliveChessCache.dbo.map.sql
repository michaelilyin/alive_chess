IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[map]')) 
   ALTER TABLE [dbo].[map] 
   ENABLE  CHANGE_TRACKING
GO
