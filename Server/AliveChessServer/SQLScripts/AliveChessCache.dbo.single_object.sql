IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[single_object]')) 
   ALTER TABLE [dbo].[single_object] 
   ENABLE  CHANGE_TRACKING
GO
