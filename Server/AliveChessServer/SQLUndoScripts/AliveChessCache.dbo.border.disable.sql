IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[border]')) 
   ALTER TABLE [dbo].[border] 
   DISABLE  CHANGE_TRACKING
GO
