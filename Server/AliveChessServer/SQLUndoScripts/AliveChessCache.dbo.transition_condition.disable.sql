IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[transition_condition]')) 
   ALTER TABLE [dbo].[transition_condition] 
   DISABLE  CHANGE_TRACKING
GO
