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
ALTER TABLE [dbo].[conversation_context] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_conversation_context_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[conversation_context] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_conversation_context_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[conversation_context_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[conversation_context_Tombstone]( 
    [context_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[conversation_context_Tombstone] ADD CONSTRAINT [PKDEL_conversation_context_Tombstone_context_id]
   PRIMARY KEY CLUSTERED
    ([context_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[conversation_context_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[conversation_context_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[conversation_context_DeletionTrigger] 
    ON [dbo].[conversation_context] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[conversation_context_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[context_id] = [dbo].[conversation_context_Tombstone].[context_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[conversation_context_Tombstone] 
    ([context_id], DeletionDate)
    SELECT [context_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[conversation_context_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[conversation_context_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[conversation_context_UpdateTrigger] 
    ON [dbo].[conversation_context] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[conversation_context] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[context_id] = [dbo].[conversation_context].[context_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[conversation_context_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[conversation_context_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[conversation_context_InsertTrigger] 
    ON [dbo].[conversation_context] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[conversation_context] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[context_id] = [dbo].[conversation_context].[context_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
