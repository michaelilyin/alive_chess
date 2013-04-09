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
ALTER TABLE [dbo].[statement] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_statement_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[statement] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_statement_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[statement_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[statement_Tombstone]( 
    [statement_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[statement_Tombstone] ADD CONSTRAINT [PKDEL_statement_Tombstone_statement_id]
   PRIMARY KEY CLUSTERED
    ([statement_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[statement_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[statement_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[statement_DeletionTrigger] 
    ON [dbo].[statement] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[statement_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[statement_id] = [dbo].[statement_Tombstone].[statement_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[statement_Tombstone] 
    ([statement_id], DeletionDate)
    SELECT [statement_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[statement_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[statement_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[statement_UpdateTrigger] 
    ON [dbo].[statement] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[statement] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[statement_id] = [dbo].[statement].[statement_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[statement_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[statement_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[statement_InsertTrigger] 
    ON [dbo].[statement] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[statement] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[statement_id] = [dbo].[statement].[statement_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
