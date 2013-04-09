IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[chess_union]')) 
   ALTER TABLE [dbo].[chess_union] 
   ENABLE  CHANGE_TRACKING
GO
