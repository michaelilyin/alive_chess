IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[map]')) 
   ALTER TABLE [dbo].[map] 
   DISABLE  CHANGE_TRACKING
GO
