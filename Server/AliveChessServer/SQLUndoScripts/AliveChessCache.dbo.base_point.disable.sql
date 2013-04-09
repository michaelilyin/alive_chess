IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[base_point]')) 
   ALTER TABLE [dbo].[base_point] 
   DISABLE  CHANGE_TRACKING
GO
