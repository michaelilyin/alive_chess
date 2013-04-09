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
ALTER TABLE [dbo].[king] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_king_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[king] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_king_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[king_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[king_Tombstone]( 
    [king_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[king_Tombstone] ADD CONSTRAINT [PKDEL_king_Tombstone_king_id]
   PRIMARY KEY CLUSTERED
    ([king_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[king_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[king_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[king_DeletionTrigger] 
    ON [dbo].[king] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[king_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[king_id] = [dbo].[king_Tombstone].[king_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[king_Tombstone] 
    ([king_id], DeletionDate)
    SELECT [king_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[king_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[king_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[king_UpdateTrigger] 
    ON [dbo].[king] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[king] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[king_id] = [dbo].[king].[king_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[king_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[king_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[king_InsertTrigger] 
    ON [dbo].[king] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[king] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[king_id] = [dbo].[king].[king_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
