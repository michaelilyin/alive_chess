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
ALTER TABLE [dbo].[mine] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_mine_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[mine] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_mine_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mine_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[mine_Tombstone]( 
    [mine_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[mine_Tombstone] ADD CONSTRAINT [PKDEL_mine_Tombstone_mine_id]
   PRIMARY KEY CLUSTERED
    ([mine_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mine_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mine_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[mine_DeletionTrigger] 
    ON [dbo].[mine] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[mine_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[mine_id] = [dbo].[mine_Tombstone].[mine_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[mine_Tombstone] 
    ([mine_id], DeletionDate)
    SELECT [mine_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mine_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mine_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[mine_UpdateTrigger] 
    ON [dbo].[mine] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[mine] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[mine_id] = [dbo].[mine].[mine_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mine_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[mine_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[mine_InsertTrigger] 
    ON [dbo].[mine] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[mine] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[mine_id] = [dbo].[mine].[mine_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
