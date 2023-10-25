using BluckImport.Core.Interface;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BluckImport.Infrastructure
{
    public class BulkImportRepository : IBulkImportRepository
    {
            private readonly IConfiguration _configuration;
            private static string con = string.Empty;

            public BulkImportRepository(IConfiguration configuration)
            {
                _configuration = configuration;
                con = _configuration["ConnectionStrings:DataConnect"];
            }
            public static IDbConnection connection
            {
                get
                {
                    return new SqlConnection(con);
                }
            }

        
    }
}
