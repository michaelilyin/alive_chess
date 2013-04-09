IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[mamdani_rule]')) 
   ALTER TABLE [dbo].[mamdani_rule] 
   DISABLE  CHANGE_TRACKING
GO
