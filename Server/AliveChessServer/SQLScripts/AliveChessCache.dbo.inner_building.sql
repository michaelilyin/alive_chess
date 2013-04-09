IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[inner_building]')) 
   ALTER TABLE [dbo].[inner_building] 
   ENABLE  CHANGE_TRACKING
GO
