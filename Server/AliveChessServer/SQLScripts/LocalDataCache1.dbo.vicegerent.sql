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
ALTER TABLE [dbo].[vicegerent] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_vicegerent_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[vicegerent] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_vicegerent_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[vicegerent_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[vicegerent_Tombstone]( 
    [vicegerent_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[vicegerent_Tombstone] ADD CONSTRAINT [PKDEL_vicegerent_Tombstone_vicegerent_id]
   PRIMARY KEY CLUSTERED
    ([vicegerent_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[vicegerent_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[vicegerent_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[vicegerent_DeletionTrigger] 
    ON [dbo].[vicegerent] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[vicegerent_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[vicegerent_id] = [dbo].[vicegerent_Tombstone].[vicegerent_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[vicegerent_Tombstone] 
    ([vicegerent_id], DeletionDate)
    SELECT [vicegerent_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[vicegerent_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[vicegerent_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[vicegerent_UpdateTrigger] 
    ON [dbo].[vicegerent] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[vicegerent] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[vicegerent_id] = [dbo].[vicegerent].[vicegerent_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[vicegerent_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[vicegerent_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[vicegerent_InsertTrigger] 
    ON [dbo].[vicegerent] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[vicegerent] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[vicegerent_id] = [dbo].[vicegerent].[vicegerent_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
