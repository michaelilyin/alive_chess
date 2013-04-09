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
ALTER TABLE [dbo].[unit] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_unit_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[unit] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_unit_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[unit_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[unit_Tombstone]( 
    [unit_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[unit_Tombstone] ADD CONSTRAINT [PKDEL_unit_Tombstone_unit_id]
   PRIMARY KEY CLUSTERED
    ([unit_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[unit_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[unit_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[unit_DeletionTrigger] 
    ON [dbo].[unit] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[unit_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[unit_id] = [dbo].[unit_Tombstone].[unit_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[unit_Tombstone] 
    ([unit_id], DeletionDate)
    SELECT [unit_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[unit_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[unit_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[unit_UpdateTrigger] 
    ON [dbo].[unit] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[unit] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[unit_id] = [dbo].[unit].[unit_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[unit_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[unit_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[unit_InsertTrigger] 
    ON [dbo].[unit] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[unit] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[unit_id] = [dbo].[unit].[unit_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
