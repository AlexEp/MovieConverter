USE [Artlist]
GO
/****** Object:  Table [dbo].[ConvertedFiles]    Script Date: 5/26/2020 7:06:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConvertedFiles](
	[Id] [nvarchar](50) NOT NULL,
	[SourceFilesId] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Codec] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ConvertedFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thumbnails]    Script Date: 5/26/2020 7:06:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thumbnails](
	[Id] [nvarchar](50) NOT NULL,
	[Filename] [nvarchar](250) NOT NULL,
	[SourceFileId] [nvarchar](50) NOT NULL,
	[FrameNum] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Thumbnails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UploadedFiles]    Script Date: 5/26/2020 7:06:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UploadedFiles](
	[Id] [nvarchar](50) NOT NULL,
	[Filename] [nvarchar](250) NOT NULL,
	[Hashed] [nvarchar](50) NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_SourceFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConvertedFiles]  WITH CHECK ADD  CONSTRAINT [FK_ConvertedFiles_SourceFiles] FOREIGN KEY([SourceFilesId])
REFERENCES [dbo].[UploadedFiles] ([Id])
GO
ALTER TABLE [dbo].[ConvertedFiles] CHECK CONSTRAINT [FK_ConvertedFiles_SourceFiles]
GO
ALTER TABLE [dbo].[Thumbnails]  WITH CHECK ADD  CONSTRAINT [FK_Thumbnails_UploadedFiles] FOREIGN KEY([SourceFileId])
REFERENCES [dbo].[UploadedFiles] ([Id])
GO
ALTER TABLE [dbo].[Thumbnails] CHECK CONSTRAINT [FK_Thumbnails_UploadedFiles]
GO
