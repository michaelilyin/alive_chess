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
ALTER TABLE [dbo].[border] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_border_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[border] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_border_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[border_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[border_Tombstone]( 
    [border_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[border_Tombstone] ADD CONSTRAINT [PKDEL_border_Tombstone_border_id]
   PRIMARY KEY CLUSTERED
    ([border_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[border_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[border_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[border_DeletionTrigger] 
    ON [dbo].[border] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[border_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[border_id] = [dbo].[border_Tombstone].[border_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[border_Tombstone] 
    ([border_id], DeletionDate)
    SELECT [border_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[border_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[border_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[border_UpdateTrigger] 
    ON [dbo].[border] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[border] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[border_id] = [dbo].[border].[border_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[border_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[border_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[border_InsertTrigger] 
    ON [dbo].[border] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[border] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[border_id] = [dbo].[border].[border_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
