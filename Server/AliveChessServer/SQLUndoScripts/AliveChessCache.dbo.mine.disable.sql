IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[mine]')) 
   ALTER TABLE [dbo].[mine] 
   DISABLE  CHANGE_TRACKING
GO
