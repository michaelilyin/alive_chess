IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[animat]')) 
   ALTER TABLE [dbo].[animat] 
   DISABLE  CHANGE_TRACKING
GO
