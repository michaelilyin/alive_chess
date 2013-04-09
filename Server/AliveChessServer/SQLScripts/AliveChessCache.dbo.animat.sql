IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[animat]')) 
   ALTER TABLE [dbo].[animat] 
   ENABLE  CHANGE_TRACKING
GO
