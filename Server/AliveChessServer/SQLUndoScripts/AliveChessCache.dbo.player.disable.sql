IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[player]')) 
   ALTER TABLE [dbo].[player] 
   DISABLE  CHANGE_TRACKING
GO
