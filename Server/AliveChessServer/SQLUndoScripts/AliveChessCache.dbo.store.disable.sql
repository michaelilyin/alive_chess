IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[store]')) 
   ALTER TABLE [dbo].[store] 
   DISABLE  CHANGE_TRACKING
GO
