IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903072644_InitialCreate'
)
BEGIN
    CREATE TABLE [Materials] (
        [MaterialId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Category] nvarchar(50) NOT NULL,
        [Brand] nvarchar(100) NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [UnitOfMeasure] nvarchar(20) NOT NULL,
        [InStockQty] int NOT NULL,
        [ReorderLevel] int NOT NULL,
        [GstPercent] decimal(5,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [AddedOn] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Materials] PRIMARY KEY ([MaterialId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903072644_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250903072644_InitialCreate', N'9.0.8');
END;

COMMIT;
GO

