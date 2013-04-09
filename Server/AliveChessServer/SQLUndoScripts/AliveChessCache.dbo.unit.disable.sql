IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[unit]')) 
   ALTER TABLE [dbo].[unit] 
   DISABLE  CHANGE_TRACKING
GO
