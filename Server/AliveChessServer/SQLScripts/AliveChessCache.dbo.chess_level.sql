IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[chess_level]')) 
   ALTER TABLE [dbo].[chess_level] 
   ENABLE  CHANGE_TRACKING
GO
