IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[statement]')) 
   ALTER TABLE [dbo].[statement] 
   ENABLE  CHANGE_TRACKING
GO
