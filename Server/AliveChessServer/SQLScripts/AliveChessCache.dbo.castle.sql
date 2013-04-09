IF NOT EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[castle]')) 
   ALTER TABLE [dbo].[castle] 
   ENABLE  CHANGE_TRACKING
GO
