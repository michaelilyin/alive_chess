IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[map_point_image]')) 
   ALTER TABLE [dbo].[map_point_image] 
   DISABLE  CHANGE_TRACKING
GO
