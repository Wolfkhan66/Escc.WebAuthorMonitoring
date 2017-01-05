
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.WebAuthorMonitoring.SqlServer
{
    /// <summary>
    /// Use SQL Server to store quality monitoring messages sent to web authors
    /// </summary>
    public class SqlServerRepository : IWebAuthorMonitoringRepository
    {
        /// <summary>
        /// Reads the possible types of problem which could be reported.
        /// </summary>
        /// <returns></returns>
        public IList<ProblemType> ReadProblemTypes()
        {
            CheckForConnectionString();

            var problemTypes = new List<ProblemType>();
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["CmsSupport"].ConnectionString, CommandType.StoredProcedure, "usp_WebAuthorProblemTypes_Select"))
            {
                while (reader.Read())
                {
                    var problemType = new ProblemType()
                        {
                            ProblemTypeId = Int32.Parse(reader["ProblemTypeId"].ToString(), CultureInfo.InvariantCulture),
                            Name = reader["ProblemTypeName"].ToString(),
                            DefaultText = reader["DefaultText"].ToString()
                        };
                    problemTypes.Add(problemType);
                }
            }
            return problemTypes;
        }

        /// <summary>
        /// Saves the problemReport and adds a unique reference number in the <see cref="ProblemReport.ProblemReportId"/> property.
        /// </summary>
        /// <param name="problemReport">The problem report.</param>
        public void SaveProblemReport(ProblemReport problemReport)
        {
            if (problemReport == null) throw new ArgumentNullException("problemReport");

            CheckForConnectionString();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CmsSupport"].ConnectionString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    // Add problem report
                    var sqlParameters = new SqlParameter[6];
                    sqlParameters[0] = new SqlParameter("@problemReportId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    sqlParameters[1] = new SqlParameter("@pageUrl", problemReport.Page.PageUrl.ToString());
                    sqlParameters[2] = new SqlParameter("@pageTitle", problemReport.Page.PageTitle);
                    sqlParameters[3] = new SqlParameter("@reportDate", problemReport.ReportDate);
                    sqlParameters[4] = new SqlParameter("@messageHtml", problemReport.MessageHtml);
                    sqlParameters[5] = new SqlParameter("@webAuthorPermissionsGroupName", problemReport.WebAuthorPermissionsGroupName);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "usp_WebAuthorProblemReport_Insert", sqlParameters);
                    problemReport.ProblemReportId = Int32.Parse(sqlParameters[0].Value.ToString(), CultureInfo.InvariantCulture);

                    // Add problem types to report
                    foreach (ProblemType problemType in problemReport.ProblemTypes)
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "usp_WebAuthorProblemReport_InsertProblemType",
                                                  new SqlParameter("@problemReportId", problemReport.ProblemReportId),
                                                  new SqlParameter("@problemTypeId", problemType.ProblemTypeId));
                    }

                    // Add web authors to report
                    foreach (WebAuthor webAuthor in problemReport.WebAuthors)
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "usp_WebAuthorProblemReport_InsertWebAuthor",
                                                  new SqlParameter("@problemReportId", problemReport.ProblemReportId),
                                                  new SqlParameter("@name", webAuthor.Name),
                                                  new SqlParameter("@username", webAuthor.UserName),
                                                  new SqlParameter("@emailAddress", webAuthor.EmailAddress));
                    }

                    trans.Commit();
                }
                catch (SqlException)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Reads a problem problemReport.
        /// </summary>
        /// <param name="problemReportId">The problem problemReport id.</param>
        /// <returns></returns>
        public ProblemReport ReadProblemReport(int problemReportId)
        {
            CheckForConnectionString();

            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["CmsSupport"].ConnectionString, CommandType.StoredProcedure, "usp_WebAuthorProblemReport_Select", new SqlParameter("@problemReportId", problemReportId)))
            {
                var reports = BuildProblemReports(reader);
                return reports.Count > 0 ? reports[0] : null;
            }
        }

        /// <summary>
        /// Reads problem reports
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webAuthorPermissionsGroupName">Name of the web author permissions group.</param>
        /// <param name="webAuthorName">Name or username of the web author.</param>
        /// <returns></returns>
        public webAuthorProblemReports ReadProblemReports(DateTime? startDate, DateTime? endDate, Uri pageUrl, string webAuthorPermissionsGroupName, string webAuthorName,int pageIndex, int PageSize)
        {
            CheckForConnectionString();

            var sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter("@startDate", DBNull.Value);
            if (startDate.HasValue) sqlParameters[0].Value = startDate.Value;

            sqlParameters[1] = new SqlParameter("@endDate", DBNull.Value);
            if (endDate.HasValue) sqlParameters[1].Value = endDate.Value;

            sqlParameters[2] = new SqlParameter("@pageUrl", DBNull.Value);
            if (pageUrl != null) sqlParameters[2].Value = pageUrl.ToString();

            sqlParameters[3] = new SqlParameter("@webAuthorPermissionsGroupName", DBNull.Value);
            if (!String.IsNullOrEmpty(webAuthorPermissionsGroupName)) sqlParameters[3].Value = webAuthorPermissionsGroupName;

            sqlParameters[4] = new SqlParameter("@webAuthorName", DBNull.Value);
            if (!String.IsNullOrEmpty(webAuthorName)) sqlParameters[4].Value = webAuthorName;

            sqlParameters[5] = new SqlParameter("@PageIndex", SqlDbType.Int);
            sqlParameters[5].Value = pageIndex;

            sqlParameters[6] = new SqlParameter("@PageSize", SqlDbType.Int);
            sqlParameters[6].Value = PageSize;

            webAuthorProblemReports reports = new webAuthorProblemReports();

            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["CmsSupport"].ConnectionString, CommandType.StoredProcedure, "usp_WebAuthorProblemReport_SelectSearch", sqlParameters))
            {
                reports.reports = BuildProblemReports(reader);

                reader.NextResult();
                reader.Read();
                reports.totalResults = Int32.Parse(reader["TotalResults"].ToString());
                return reports;
            }
        }

        private static List<ProblemReport> BuildProblemReports(SqlDataReader reader)
        {
            var webAuthorsDone = new List<int>();
            var problemTypesDone = new List<int>();

            var reports = new Dictionary<int, ProblemReport>();
            while (reader.Read())
            {
                var problemReportId = Int32.Parse(reader["ProblemReportId"].ToString(), CultureInfo.InvariantCulture);
                if (!reports.ContainsKey(problemReportId))
                {
                    CreateProblemReport(reader, webAuthorsDone, problemTypesDone, reports, problemReportId);
                }

                AddProblemType(reader, problemTypesDone, reports, problemReportId);

                AddWebAuthor(reader, webAuthorsDone, reports, problemReportId);
            }

            return new List<ProblemReport>(reports.Values);
        }

        private static void CreateProblemReport(SqlDataReader reader, List<int> webAuthorsDone, List<int> problemTypesDone, Dictionary<int, ProblemReport> reports, int problemReportId)
        {
            webAuthorsDone.Clear();
            problemTypesDone.Clear();

            reports.Add(problemReportId, new ProblemReport()
                {
                    ProblemReportId = problemReportId,
                    Page = new Page()
                        {
                            PageUrl = new Uri(reader["PageUrl"].ToString(), UriKind.RelativeOrAbsolute),
                            PageTitle = reader["PageTitle"].ToString()
                        },
                    ReportDate = DateTime.Parse(reader["ReportDate"].ToString(), CultureInfo.CurrentCulture),
                    MessageHtml = reader["MessageHtml"].ToString(),
                    WebAuthorPermissionsGroupName = reader["WebAuthorPermissionsGroupName"].ToString()
                });
        }

        private static void AddWebAuthor(SqlDataReader reader, List<int> webAuthorsDone, Dictionary<int, ProblemReport> reports, int problemReportId)
        {
            if (reader["WebAuthorId"] != DBNull.Value)
            {
                var webAuthorId = Int32.Parse(reader["WebAuthorId"].ToString(), CultureInfo.InvariantCulture);
                if (!webAuthorsDone.Contains(webAuthorId))
                {
                    reports[problemReportId].WebAuthors.Add(new WebAuthor()
                        {
                            WebAuthorId = webAuthorId,
                            Name = reader["Name"].ToString(),
                            UserName = reader["Username"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString()
                        });
                }
            }
        }

        private static void AddProblemType(SqlDataReader reader, List<int> problemTypesDone, Dictionary<int, ProblemReport> reports, int problemReportId)
        {
            if (reader["ProblemTypeId"] != DBNull.Value)
            {
                var problemTypeId = Int32.Parse(reader["ProblemTypeId"].ToString(), CultureInfo.InvariantCulture);
                if (!problemTypesDone.Contains(problemTypeId))
                {
                    reports[problemReportId].ProblemTypes.Add(new ProblemType()
                        {
                            ProblemTypeId = problemTypeId,
                            Name = reader["ProblemTypeName"].ToString()
                        });
                }
            }
        }

        private static void CheckForConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["CmsSupport"] == null ||
                String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["CmsSupport"].ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "The connection string for the web author monitoring database must be set in web.config or app.config using the key 'CmsSupport'");
            }
        }

    }
}
