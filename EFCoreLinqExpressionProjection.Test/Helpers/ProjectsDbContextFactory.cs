using System;
using System.Data.Common;
using EFCoreLinqExpressionProjection.Test.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreLinqExpressionProjection.Test.Helpers
{
    internal class ProjectsDbContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<ProjectsDbContext> CreateOptions(bool serverEvaluation)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ProjectsDbContext>()
                .UseSqlite(_connection);

            if (serverEvaluation)
            {
                dbContextOptionsBuilder = dbContextOptionsBuilder.ConfigureWarnings(warnings =>
                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }

            return dbContextOptionsBuilder.Options;
        }

        public ProjectsDbContext CreateContext(bool serverEvaluation = true)
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions(serverEvaluation);
                using (var context = new ProjectsDbContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new ProjectsDbContext(CreateOptions(serverEvaluation));
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
