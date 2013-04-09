IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[multy_object]')) 
   ALTER TABLE [dbo].[multy_object] 
   DISABLE  CHANGE_TRACKING
GO
