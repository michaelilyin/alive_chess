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
ALTER TABLE [dbo].[animat] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_animat_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[animat] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_animat_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[animat_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[animat_Tombstone]( 
    [animat_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[animat_Tombstone] ADD CONSTRAINT [PKDEL_animat_Tombstone_animat_id]
   PRIMARY KEY CLUSTERED
    ([animat_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[animat_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[animat_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[animat_DeletionTrigger] 
    ON [dbo].[animat] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[animat_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[animat_id] = [dbo].[animat_Tombstone].[animat_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[animat_Tombstone] 
    ([animat_id], DeletionDate)
    SELECT [animat_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[animat_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[animat_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[animat_UpdateTrigger] 
    ON [dbo].[animat] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[animat] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[animat_id] = [dbo].[animat].[animat_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[animat_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[animat_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[animat_InsertTrigger] 
    ON [dbo].[animat] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[animat] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[animat_id] = [dbo].[animat].[animat_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
