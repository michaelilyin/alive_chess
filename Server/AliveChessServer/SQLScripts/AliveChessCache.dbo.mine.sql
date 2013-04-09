IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[mine]')) 
   ALTER TABLE [dbo].[mine] 
   ENABLE  CHANGE_TRACKING
GO
