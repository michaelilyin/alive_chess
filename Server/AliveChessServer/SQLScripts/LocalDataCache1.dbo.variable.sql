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
ALTER TABLE [dbo].[variable] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_variable_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[variable] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_variable_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[variable_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[variable_Tombstone]( 
    [variable_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[variable_Tombstone] ADD CONSTRAINT [PKDEL_variable_Tombstone_variable_id]
   PRIMARY KEY CLUSTERED
    ([variable_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[variable_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[variable_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[variable_DeletionTrigger] 
    ON [dbo].[variable] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[variable_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[variable_id] = [dbo].[variable_Tombstone].[variable_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[variable_Tombstone] 
    ([variable_id], DeletionDate)
    SELECT [variable_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[variable_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[variable_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[variable_UpdateTrigger] 
    ON [dbo].[variable] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[variable] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[variable_id] = [dbo].[variable].[variable_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[variable_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[variable_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[variable_InsertTrigger] 
    ON [dbo].[variable] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[variable] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[variable_id] = [dbo].[variable].[variable_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
