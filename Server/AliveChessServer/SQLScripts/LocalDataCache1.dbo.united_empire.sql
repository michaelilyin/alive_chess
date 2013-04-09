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
ALTER TABLE [dbo].[united_empire] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_united_empire_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[united_empire] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_united_empire_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[united_empire_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[united_empire_Tombstone]( 
    [united_empire_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[united_empire_Tombstone] ADD CONSTRAINT [PKDEL_united_empire_Tombstone_united_empire_id]
   PRIMARY KEY CLUSTERED
    ([united_empire_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[united_empire_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[united_empire_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[united_empire_DeletionTrigger] 
    ON [dbo].[united_empire] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[united_empire_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[united_empire_id] = [dbo].[united_empire_Tombstone].[united_empire_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[united_empire_Tombstone] 
    ([united_empire_id], DeletionDate)
    SELECT [united_empire_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[united_empire_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[united_empire_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[united_empire_UpdateTrigger] 
    ON [dbo].[united_empire] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[united_empire] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[united_empire_id] = [dbo].[united_empire].[united_empire_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[united_empire_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[united_empire_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[united_empire_InsertTrigger] 
    ON [dbo].[united_empire] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[united_empire] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[united_empire_id] = [dbo].[united_empire].[united_empire_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
