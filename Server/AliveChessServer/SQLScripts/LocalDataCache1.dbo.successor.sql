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
ALTER TABLE [dbo].[successor] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_successor_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[successor] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_successor_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[successor_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[successor_Tombstone]( 
    [successor_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[successor_Tombstone] ADD CONSTRAINT [PKDEL_successor_Tombstone_successor_id]
   PRIMARY KEY CLUSTERED
    ([successor_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[successor_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[successor_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[successor_DeletionTrigger] 
    ON [dbo].[successor] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[successor_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[successor_id] = [dbo].[successor_Tombstone].[successor_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[successor_Tombstone] 
    ([successor_id], DeletionDate)
    SELECT [successor_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[successor_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[successor_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[successor_UpdateTrigger] 
    ON [dbo].[successor] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[successor] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[successor_id] = [dbo].[successor].[successor_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[successor_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[successor_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[successor_InsertTrigger] 
    ON [dbo].[successor] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[successor] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[successor_id] = [dbo].[successor].[successor_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
