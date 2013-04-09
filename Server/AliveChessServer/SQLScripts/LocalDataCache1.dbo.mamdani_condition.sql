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
ALTER TABLE [dbo].[mamdani_condition] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_mamdani_condition_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[mamdani_condition] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_mamdani_condition_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mamdani_condition_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[mamdani_condition_Tombstone]( 
    [mamdani_cond_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[mamdani_condition_Tombstone] ADD CONSTRAINT [PKDEL_mamdani_condition_Tombstone_mamdani_cond_id]
   PRIMARY KEY CLUSTERED
    ([mamdani_cond_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mamdani_condition_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mamdani_condition_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[mamdani_condition_DeletionTrigger] 
    ON [dbo].[mamdani_condition] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[mamdani_condition_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[mamdani_cond_id] = [dbo].[mamdani_condition_Tombstone].[mamdani_cond_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[mamdani_condition_Tombstone] 
    ([mamdani_cond_id], DeletionDate)
    SELECT [mamdani_cond_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mamdani_condition_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mamdani_condition_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[mamdani_condition_UpdateTrigger] 
    ON [dbo].[mamdani_condition] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[mamdani_condition] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[mamdani_cond_id] = [dbo].[mamdani_condition].[mamdani_cond_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mamdani_condition_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mamdani_condition_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[mamdani_condition_InsertTrigger] 
    ON [dbo].[mamdani_condition] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[mamdani_condition] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[mamdani_cond_id] = [dbo].[mamdani_condition].[mamdani_cond_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
