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
ALTER TABLE [dbo].[transition_rule] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_transition_rule_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[transition_rule] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_transition_rule_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_rule_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[transition_rule_Tombstone]( 
    [transition_rule_id] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[transition_rule_Tombstone] ADD CONSTRAINT [PKDEL_transition_rule_Tombstone_transition_rule_id]
   PRIMARY KEY CLUSTERED
    ([transition_rule_id])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_rule_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_rule_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[transition_rule_DeletionTrigger] 
    ON [dbo].[transition_rule] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[transition_rule_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[transition_rule_id] = [dbo].[transition_rule_Tombstone].[transition_rule_id] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[transition_rule_Tombstone] 
    ([transition_rule_id], DeletionDate)
    SELECT [transition_rule_id], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_rule_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_rule_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[transition_rule_UpdateTrigger] 
    ON [dbo].[transition_rule] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[transition_rule] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[transition_rule_id] = [dbo].[transition_rule].[transition_rule_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_rule_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_rule_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[transition_rule_InsertTrigger] 
    ON [dbo].[transition_rule] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[transition_rule] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[transition_rule_id] = [dbo].[transition_rule].[transition_rule_id] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
