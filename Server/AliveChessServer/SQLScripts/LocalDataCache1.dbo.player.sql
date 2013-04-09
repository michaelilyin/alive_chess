/****
Этот сценарий SQL был создан с помощью диалогового окна "Настройка 
синхронизации данных". Сценарий содержит операторы, которые 
создают столбцы отслеживания изменений, таблицу удаленных 
элементов и триггеры базы данных сервера. Эти объекты базы данных 
необходимы службам синхронизации для успешной синхронизации 
данных между базами данных на клиенте и на сервере. Дополнительные 
сведения см. в справке "Настройка сервера базы данных для синхронизации".
****/


IF @@TRANCOUNT > 0
set ANSI_NULLS ON 
set QUOTED_IDENTIFIER ON 

GO
BEGIN TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[player] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_player_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[player] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_player_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[player_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[player_Tombstone]( 
    [player_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[player_Tombstone] ADD CONSTRAINT [PKDEL_player_Tombstone_player_id]
   PRIMARY KEY CLUSTERED
    ([player_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[player_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[player_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[player_DeletionTrigger] 
    ON [dbo].[player] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[player_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[player_id] = [dbo].[player_Tombstone].[player_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[player_Tombstone] 
    ([player_id], DeletionDate)
    SELECT [player_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[player_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[player_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[player_UpdateTrigger] 
    ON [dbo].[player] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[player] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[player_id] = [dbo].[player].[player_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[player_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[player_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[player_InsertTrigger] 
    ON [dbo].[player] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[player] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[player_id] = [dbo].[player].[player_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
