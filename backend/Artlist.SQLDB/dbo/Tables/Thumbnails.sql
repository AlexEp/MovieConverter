CREATE TABLE [dbo].[Thumbnails] (
    [Id]           NVARCHAR (50)  NOT NULL,
    [Filename]     NVARCHAR (250) NOT NULL,
    [SourceFileId] NVARCHAR (50)  NOT NULL,
    [FrameNum]     INT            NOT NULL,
    [Created]      DATETIME       NOT NULL,
    CONSTRAINT [PK_Thumbnails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Thumbnails_UploadedFiles] FOREIGN KEY ([SourceFileId]) REFERENCES [dbo].[UploadedFiles] ([Id])
);

