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
ALTER TABLE [dbo].[neural_network] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_neural_network_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[neural_network] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_neural_network_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[neural_network_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[neural_network_Tombstone]( 
    [network_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[neural_network_Tombstone] ADD CONSTRAINT [PKDEL_neural_network_Tombstone_network_id]
   PRIMARY KEY CLUSTERED
    ([network_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[neural_network_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[neural_network_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[neural_network_DeletionTrigger] 
    ON [dbo].[neural_network] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[neural_network_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[network_id] = [dbo].[neural_network_Tombstone].[network_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[neural_network_Tombstone] 
    ([network_id], DeletionDate)
    SELECT [network_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[neural_network_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[neural_network_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[neural_network_UpdateTrigger] 
    ON [dbo].[neural_network] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[neural_network] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[network_id] = [dbo].[neural_network].[network_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[neural_network_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[neural_network_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[neural_network_InsertTrigger] 
    ON [dbo].[neural_network] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[neural_network] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[network_id] = [dbo].[neural_network].[network_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
