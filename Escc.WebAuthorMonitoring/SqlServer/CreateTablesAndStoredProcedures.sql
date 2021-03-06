USE [CmsSupport]
GO
/****** Object:  Table [dbo].[WebAuthorProblemReport]    Script Date: 07/29/2013 17:43:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WebAuthorProblemReport](
	[ProblemReportId] [int] IDENTITY(1,1) NOT NULL,
	[PageUrl] [varchar](250) NOT NULL,
	[PageTitle] [varchar](250) NOT NULL,
	[ReportDate] [datetime] NOT NULL,
	[MessageHtml] [text] NOT NULL,
	[WebAuthorPermissionsGroupName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_WebAuthorProblemReport] PRIMARY KEY CLUSTERED 
(
	[ProblemReportId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebAuthorProblemType]    Script Date: 07/29/2013 17:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WebAuthorProblemType](
	[ProblemTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ProblemTypeName] [varchar](100) NOT NULL,
	[DefaultText] [text] NOT NULL,
	[Severity] [int] NOT NULL,
 CONSTRAINT [PK_WebAuthorProblemType] PRIMARY KEY CLUSTERED 
(
	[ProblemTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebAuthorProblemReport_WebAuthorProblemType]    Script Date: 07/29/2013 17:43:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebAuthorProblemReport_WebAuthorProblemType](
	[ProblemReportId] [int] NOT NULL,
	[ProblemTypeId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebAuthorForProblemReport]    Script Date: 07/29/2013 17:43:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WebAuthorForProblemReport](
	[WebAuthorId] [int] IDENTITY(1,1) NOT NULL,
	[ProblemReportId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[EmailAddress] [varchar](255) NOT NULL,
 CONSTRAINT [PK_WebAuthorForProblemReport] PRIMARY KEY CLUSTERED 
(
	[WebAuthorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemReport_Insert]    Script Date: 07/29/2013 17:43:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Inserts a new web author problem report
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemReport_Insert]
	@problemReportId int OUTPUT,
	@pageUrl varchar(250),
	@pageTitle varchar(250),
	@reportDate datetime,
	@messageHtml text,
	@webAuthorPermissionsGroupName varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO WebAuthorProblemReport 
		(PageUrl, PageTitle, ReportDate, MessageHtml, WebAuthorPermissionsGroupName)
	VALUES
		(@pageUrl, @pageTitle, @reportDate, @messageHtml, @webAuthorPermissionsGroupName)

	SELECT @problemReportId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemReport_Select]    Script Date: 07/29/2013 17:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Selects a single web author problem report
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemReport_Select]
	@problemReportId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT WebAuthorProblemReport.ProblemReportId, PageUrl, PageTitle, ReportDate, MessageHtml, WebAuthorPermissionsGroupName,
			WebAuthorProblemType.ProblemTypeId, ProblemTypeName,
			WebAuthorId, Name, Username, EmailAddress
	FROM WebAuthorProblemReport 
	INNER JOIN WebAuthorProblemReport_WebAuthorProblemType ON WebAuthorProblemReport.ProblemReportId = WebAuthorProblemReport_WebAuthorProblemType.ProblemReportId
	INNER JOIN WebAuthorProblemType ON WebAuthorProblemReport_WebAuthorProblemType.ProblemTypeId = WebAuthorProblemType.ProblemTypeId
	INNER JOIN WebAuthorForProblemReport ON WebAuthorProblemReport.ProblemReportId = WebAuthorForProblemReport.ProblemReportId
	WHERE WebAuthorProblemReport.ProblemReportId = @problemReportId
	ORDER BY ReportDate DESC
END
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemReport_SelectSearch]    Script Date: 07/29/2013 17:43:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Selects web author problem reports based on search terms
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemReport_SelectSearch]
@startDate datetime = NULL,
	@endDate datetime = NULL,
	@pageUrl varchar(250) = NULL,
	@webAuthorPermissionsGroupName varchar(100) = NULL,
	@webAuthorName varchar(100) = NULL,
	@PageIndex int,
	@PageSize int
AS
DECLARE
	@firstRecord int,
	@lastRecord int
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

CREATE TABLE #tempReport
(
	tempId bigint IDENTITY PRIMARY KEY,
	[ProblemReportId] [int] Null,
	[ReportDate] [datetime] Null
)

INSERT INTO #tempReport
(
	[ProblemReportId],
	[ReportDate]
)
	SELECT Distinct WebAuthorProblemReport.ProblemReportId , ReportDate
	FROM WebAuthorProblemReport 
	INNER JOIN WebAuthorForProblemReport ON WebAuthorProblemReport.ProblemReportId = WebAuthorForProblemReport.ProblemReportId
	WHERE (WebAuthorProblemReport.ReportDate >= @startDate OR @startDate IS NULL)
	AND (WebAuthorProblemReport.ReportDate <= @endDate OR @endDate IS NULL)
	AND (WebAuthorProblemReport.PageUrl LIKE (@pageUrl+'%') OR @pageUrl IS NULL)
	AND (WebAuthorProblemReport.WebAuthorPermissionsGroupName = @webAuthorPermissionsGroupName OR @webAuthorPermissionsGroupName IS NULL)
	AND (WebAuthorForProblemReport.Name = @webAuthorName OR WebAuthorForProblemReport.Username = @webAuthorName OR @webAuthorName IS NULL)
	ORDER BY ReportDate DESC

	SELECT @firstRecord =  (@PageIndex - 1) * @PageSize
	SELECT @lastRecord =  (@PageIndex * @PageSize + 1)

	SELECT WebAuthorProblemReport.ProblemReportId , ReportDate, PageUrl, PageTitle, MessageHtml, WebAuthorPermissionsGroupName,
			WebAuthorProblemType.ProblemTypeId, ProblemTypeName,
			WebAuthorId, Name, Username, EmailAddress
	FROM WebAuthorProblemReport 
	INNER JOIN WebAuthorProblemReport_WebAuthorProblemType ON WebAuthorProblemReport.ProblemReportId = WebAuthorProblemReport_WebAuthorProblemType.ProblemReportId
	INNER JOIN WebAuthorProblemType ON WebAuthorProblemReport_WebAuthorProblemType.ProblemTypeId = WebAuthorProblemType.ProblemTypeId
	INNER JOIN WebAuthorForProblemReport ON WebAuthorProblemReport.ProblemReportId = WebAuthorForProblemReport.ProblemReportId
	WHERE WebAuthorProblemReport.ProblemReportId IN (SELECT ProblemReportId FROM #tempReport 
		WHERE #tempReport.tempId > @firstRecord AND  #tempReport.tempId  < @lastRecord )
	ORDER BY ReportDate DESC

	SELECT distinct Count(*) as TotalResults FROM WebAuthorProblemReport

DROP TABLE #tempReport
END
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemReport_InsertProblemType]    Script Date: 07/29/2013 17:43:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Adds a problem type to a web author problem report
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemReport_InsertProblemType]
	@problemReportId int,
	@problemTypeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO WebAuthorProblemReport_WebAuthorProblemType 
		(ProblemReportId, ProblemTypeId)
	VALUES
		(@problemReportId, @problemTypeId)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemReport_InsertWebAuthor]    Script Date: 07/29/2013 17:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Adds a web author to a web author problem report
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemReport_InsertWebAuthor]
	@problemReportId int,
	@name varchar(100),
	@username varchar(50),
	@emailAddress varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO WebAuthorForProblemReport
		(ProblemReportId, Name, Username, EmailAddress)
	VALUES
		(@problemReportId, @name, @username, @emailAddress)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_WebAuthorProblemTypes_Select]    Script Date: 07/29/2013 17:43:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rick Mason, Digital Services
-- Create date: 5 July 2013
-- Description:	Selects all web author problem types
-- =============================================
CREATE PROCEDURE [dbo].[usp_WebAuthorProblemTypes_Select]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ProblemTypeId, ProblemTypeName, DefaultText FROM WebAuthorProblemType ORDER BY Severity ASC, ProblemTypeName ASC
END
GO
/****** Object:  ForeignKey [FK_WebAuthorForProblemReport_WebAuthorProblemReport]    Script Date: 07/29/2013 17:43:55 ******/
ALTER TABLE [dbo].[WebAuthorForProblemReport]  WITH CHECK ADD  CONSTRAINT [FK_WebAuthorForProblemReport_WebAuthorProblemReport] FOREIGN KEY([ProblemReportId])
REFERENCES [dbo].[WebAuthorProblemReport] ([ProblemReportId])
GO
ALTER TABLE [dbo].[WebAuthorForProblemReport] CHECK CONSTRAINT [FK_WebAuthorForProblemReport_WebAuthorProblemReport]
GO
/****** Object:  ForeignKey [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemReport]    Script Date: 07/29/2013 17:43:58 ******/
ALTER TABLE [dbo].[WebAuthorProblemReport_WebAuthorProblemType]  WITH CHECK ADD  CONSTRAINT [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemReport] FOREIGN KEY([ProblemReportId])
REFERENCES [dbo].[WebAuthorProblemReport] ([ProblemReportId])
GO
ALTER TABLE [dbo].[WebAuthorProblemReport_WebAuthorProblemType] CHECK CONSTRAINT [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemReport]
GO
/****** Object:  ForeignKey [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemType]    Script Date: 07/29/2013 17:43:59 ******/
ALTER TABLE [dbo].[WebAuthorProblemReport_WebAuthorProblemType]  WITH CHECK ADD  CONSTRAINT [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemType] FOREIGN KEY([ProblemTypeId])
REFERENCES [dbo].[WebAuthorProblemType] ([ProblemTypeId])
GO
ALTER TABLE [dbo].[WebAuthorProblemReport_WebAuthorProblemType] CHECK CONSTRAINT [FK_WebAuthorProblemReport_WebAuthorProblemType_WebAuthorProblemType]
GO
