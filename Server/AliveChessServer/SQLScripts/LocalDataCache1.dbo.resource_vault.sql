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
ALTER TABLE [dbo].[resource_vault] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_resource_vault_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[resource_vault] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_resource_vault_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[resource_vault_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[resource_vault_Tombstone]( 
    [resource_vault_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[resource_vault_Tombstone] ADD CONSTRAINT [PKDEL_resource_vault_Tombstone_resource_vault_id]
   PRIMARY KEY CLUSTERED
    ([resource_vault_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[resource_vault_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[resource_vault_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[resource_vault_DeletionTrigger] 
    ON [dbo].[resource_vault] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[resource_vault_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[resource_vault_id] = [dbo].[resource_vault_Tombstone].[resource_vault_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[resource_vault_Tombstone] 
    ([resource_vault_id], DeletionDate)
    SELECT [resource_vault_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[resource_vault_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[resource_vault_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[resource_vault_UpdateTrigger] 
    ON [dbo].[resource_vault] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[resource_vault] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[resource_vault_id] = [dbo].[resource_vault].[resource_vault_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[resource_vault_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[resource_vault_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[resource_vault_InsertTrigger] 
    ON [dbo].[resource_vault] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[resource_vault] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[resource_vault_id] = [dbo].[resource_vault].[resource_vault_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
