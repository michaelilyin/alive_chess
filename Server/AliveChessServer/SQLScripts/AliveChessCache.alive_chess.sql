IF NOT EXISTS (SELECT * FROM sys.change_tracking_databases WHERE database_id = DB_ID(N'alive_chess')) 
   ALTER DATABASE [alive_chess] 
   SET  CHANGE_TRACKING = ON
GO
