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
ALTER TABLE [dbo].[store] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_store_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[store] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_store_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[store_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[store_Tombstone]( 
    [store_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[store_Tombstone] ADD CONSTRAINT [PKDEL_store_Tombstone_store_id]
   PRIMARY KEY CLUSTERED
    ([store_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[store_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[store_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[store_DeletionTrigger] 
    ON [dbo].[store] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[store_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[store_id] = [dbo].[store_Tombstone].[store_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[store_Tombstone] 
    ([store_id], DeletionDate)
    SELECT [store_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[store_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[store_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[store_UpdateTrigger] 
    ON [dbo].[store] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[store] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[store_id] = [dbo].[store].[store_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[store_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[store_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[store_InsertTrigger] 
    ON [dbo].[store] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[store] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[store_id] = [dbo].[store].[store_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
