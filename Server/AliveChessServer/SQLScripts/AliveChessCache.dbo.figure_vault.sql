IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[figure_vault]')) 
   ALTER TABLE [dbo].[figure_vault] 
   ENABLE  CHANGE_TRACKING
GO
