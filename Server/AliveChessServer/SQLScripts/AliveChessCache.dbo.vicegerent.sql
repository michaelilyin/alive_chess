IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[vicegerent]')) 
   ALTER TABLE [dbo].[vicegerent] 
   ENABLE  CHANGE_TRACKING
GO
