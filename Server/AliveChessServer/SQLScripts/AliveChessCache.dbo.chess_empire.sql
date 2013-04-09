IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[chess_empire]')) 
   ALTER TABLE [dbo].[chess_empire] 
   ENABLE  CHANGE_TRACKING
GO
