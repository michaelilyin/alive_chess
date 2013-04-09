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
ALTER TABLE [dbo].[inner_building] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_inner_building_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[inner_building] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_inner_building_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[inner_building_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[inner_building_Tombstone]( 
    [inner_building_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[inner_building_Tombstone] ADD CONSTRAINT [PKDEL_inner_building_Tombstone_inner_building_id]
   PRIMARY KEY CLUSTERED
    ([inner_building_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[inner_building_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[inner_building_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[inner_building_DeletionTrigger] 
    ON [dbo].[inner_building] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[inner_building_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[inner_building_id] = [dbo].[inner_building_Tombstone].[inner_building_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[inner_building_Tombstone] 
    ([inner_building_id], DeletionDate)
    SELECT [inner_building_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[inner_building_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[inner_building_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[inner_building_UpdateTrigger] 
    ON [dbo].[inner_building] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[inner_building] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[inner_building_id] = [dbo].[inner_building].[inner_building_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[inner_building_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[inner_building_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[inner_building_InsertTrigger] 
    ON [dbo].[inner_building] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[inner_building] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[inner_building_id] = [dbo].[inner_building].[inner_building_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
