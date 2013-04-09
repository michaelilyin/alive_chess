IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[transition_rule]')) 
   ALTER TABLE [dbo].[transition_rule] 
   DISABLE  CHANGE_TRACKING
GO
