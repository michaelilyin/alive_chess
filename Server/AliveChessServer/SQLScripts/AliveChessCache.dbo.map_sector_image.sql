IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[map_sector_image]')) 
   ALTER TABLE [dbo].[map_sector_image] 
   ENABLE  CHANGE_TRACKING
GO
