IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[united_empire]')) 
   ALTER TABLE [dbo].[united_empire] 
   ENABLE  CHANGE_TRACKING
GO
