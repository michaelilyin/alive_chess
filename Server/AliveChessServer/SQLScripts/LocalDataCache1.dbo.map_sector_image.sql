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
ALTER TABLE [dbo].[map_sector_image] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_map_sector_image_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[map_sector_image] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_map_sector_image_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[map_sector_image_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[map_sector_image_Tombstone]( 
    [map_sector_image_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[map_sector_image_Tombstone] ADD CONSTRAINT [PKDEL_map_sector_image_Tombstone_map_sector_image_id]
   PRIMARY KEY CLUSTERED
    ([map_sector_image_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[map_sector_image_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[map_sector_image_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[map_sector_image_DeletionTrigger] 
    ON [dbo].[map_sector_image] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[map_sector_image_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[map_sector_image_id] = [dbo].[map_sector_image_Tombstone].[map_sector_image_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[map_sector_image_Tombstone] 
    ([map_sector_image_id], DeletionDate)
    SELECT [map_sector_image_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[map_sector_image_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[map_sector_image_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[map_sector_image_UpdateTrigger] 
    ON [dbo].[map_sector_image] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[map_sector_image] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[map_sector_image_id] = [dbo].[map_sector_image].[map_sector_image_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[map_sector_image_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[map_sector_image_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[map_sector_image_InsertTrigger] 
    ON [dbo].[map_sector_image] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[map_sector_image] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[map_sector_image_id] = [dbo].[map_sector_image].[map_sector_image_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
