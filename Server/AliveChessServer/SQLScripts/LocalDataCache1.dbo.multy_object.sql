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
ALTER TABLE [dbo].[multy_object] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_multy_object_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[multy_object] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_multy_object_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[multy_object_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[multy_object_Tombstone]( 
    [multy_object_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[multy_object_Tombstone] ADD CONSTRAINT [PKDEL_multy_object_Tombstone_multy_object_id]
   PRIMARY KEY CLUSTERED
    ([multy_object_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[multy_object_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[multy_object_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[multy_object_DeletionTrigger] 
    ON [dbo].[multy_object] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[multy_object_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[multy_object_id] = [dbo].[multy_object_Tombstone].[multy_object_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[multy_object_Tombstone] 
    ([multy_object_id], DeletionDate)
    SELECT [multy_object_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[multy_object_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[multy_object_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[multy_object_UpdateTrigger] 
    ON [dbo].[multy_object] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[multy_object] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[multy_object_id] = [dbo].[multy_object].[multy_object_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[multy_object_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[multy_object_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[multy_object_InsertTrigger] 
    ON [dbo].[multy_object] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[multy_object] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[multy_object_id] = [dbo].[multy_object].[multy_object_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
