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
ALTER TABLE [dbo].[castle] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_castle_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[castle] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_castle_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[castle_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[castle_Tombstone]( 
    [castle_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[castle_Tombstone] ADD CONSTRAINT [PKDEL_castle_Tombstone_castle_id]
   PRIMARY KEY CLUSTERED
    ([castle_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[castle_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[castle_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[castle_DeletionTrigger] 
    ON [dbo].[castle] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[castle_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[castle_id] = [dbo].[castle_Tombstone].[castle_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[castle_Tombstone] 
    ([castle_id], DeletionDate)
    SELECT [castle_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[castle_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[castle_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[castle_UpdateTrigger] 
    ON [dbo].[castle] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[castle] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[castle_id] = [dbo].[castle].[castle_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[castle_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[castle_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[castle_InsertTrigger] 
    ON [dbo].[castle] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[castle] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[castle_id] = [dbo].[castle].[castle_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
