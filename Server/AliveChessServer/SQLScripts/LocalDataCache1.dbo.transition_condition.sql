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
ALTER TABLE [dbo].[transition_condition] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_transition_condition_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[transition_condition] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_transition_condition_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_condition_Tombstone]')) 
BEGIN 
CREATE TABLE [dbo].[transition_condition_Tombstone]( 
    [transition_cond] Int NOT NULL,
    [DeletionDate] DateTime NULL
) ON [PRIMARY] 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[transition_condition_Tombstone] ADD CONSTRAINT [PKDEL_transition_condition_Tombstone_transition_cond]
   PRIMARY KEY CLUSTERED
    ([transition_cond])
    ON [PRIMARY]
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_condition_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_condition_DeletionTrigger] 

GO
CREATE TRIGGER [dbo].[transition_condition_DeletionTrigger] 
    ON [dbo].[transition_condition] 
    AFTER DELETE 
AS 
SET NOCOUNT ON 
UPDATE [dbo].[transition_condition_Tombstone] 
    SET [DeletionDate] = GETUTCDATE() 
    FROM deleted 
    WHERE deleted.[transition_cond] = [dbo].[transition_condition_Tombstone].[transition_cond] 
IF @@ROWCOUNT = 0 
BEGIN 
    INSERT INTO [dbo].[transition_condition_Tombstone] 
    ([transition_cond], DeletionDate)
    SELECT [transition_cond], GETUTCDATE()
    FROM deleted 
END 

GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_condition_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_condition_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[transition_condition_UpdateTrigger] 
    ON [dbo].[transition_condition] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[transition_condition] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[transition_cond] = [dbo].[transition_condition].[transition_cond] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[transition_condition_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[transition_condition_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[transition_condition_InsertTrigger] 
    ON [dbo].[transition_condition] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[transition_condition] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[transition_cond] = [dbo].[transition_condition].[transition_cond] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
