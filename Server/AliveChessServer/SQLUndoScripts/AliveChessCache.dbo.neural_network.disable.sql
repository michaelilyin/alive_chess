IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[neural_network]')) 
   ALTER TABLE [dbo].[neural_network] 
   DISABLE  CHANGE_TRACKING
GO