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
ALTER TABLE [dbo].[chess_level] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_chess_level_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[chess_level] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_chess_level_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[chess_level_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[chess_level_Tombstone]( 
    [level_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[chess_level_Tombstone] ADD CONSTRAINT [PKDEL_chess_level_Tombstone_level_id]
   PRIMARY KEY CLUSTERED
    ([level_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[chess_level_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[chess_level_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[chess_level_DeletionTrigger] 
    ON [dbo].[chess_level] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[chess_level_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[level_id] = [dbo].[chess_level_Tombstone].[level_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[chess_level_Tombstone] 
    ([level_id], DeletionDate)
    SELECT [level_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[chess_level_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[chess_level_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[chess_level_UpdateTrigger] 
    ON [dbo].[chess_level] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[chess_level] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[level_id] = [dbo].[chess_level].[level_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[chess_level_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[chess_level_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[chess_level_InsertTrigger] 
    ON [dbo].[chess_level] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[chess_level] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[level_id] = [dbo].[chess_level].[level_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
