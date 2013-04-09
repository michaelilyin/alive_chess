IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[successor]')) 
   ALTER TABLE [dbo].[successor] 
   ENABLE  CHANGE_TRACKING
GO
