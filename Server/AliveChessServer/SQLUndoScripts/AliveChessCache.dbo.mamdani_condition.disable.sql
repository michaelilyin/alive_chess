IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[mamdani_condition]')) 
   ALTER TABLE [dbo].[mamdani_condition] 
   DISABLE  CHANGE_TRACKING
GO
