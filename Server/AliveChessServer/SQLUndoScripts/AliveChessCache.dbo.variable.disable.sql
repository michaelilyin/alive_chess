IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[variable]')) 
   ALTER TABLE [dbo].[variable] 
   DISABLE  CHANGE_TRACKING
GO
