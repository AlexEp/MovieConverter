CREATE TABLE [dbo].[UploadedFiles] (
    [Id]       NVARCHAR (50)  NOT NULL,
    [Filename] NVARCHAR (250) NOT NULL,
    [Hashed]   NVARCHAR (50)  NULL,
    [Created]  DATETIME       NOT NULL,
    CONSTRAINT [PK_SourceFiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

