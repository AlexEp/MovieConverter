CREATE TABLE [dbo].[ConvertedFiles] (
    [Id]            NVARCHAR (50) NOT NULL,
    [SourceFilesId] NVARCHAR (50) NOT NULL,
    [Created]       DATETIME      NOT NULL,
    [Codec]         NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ConvertedFiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ConvertedFiles_SourceFiles] FOREIGN KEY ([SourceFilesId]) REFERENCES [dbo].[UploadedFiles] ([Id])
);

