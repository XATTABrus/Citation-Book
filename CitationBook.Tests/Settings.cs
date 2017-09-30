using System.IO;
using Microsoft.Extensions.Configuration;

namespace CitationBook.Tests
{
    public static class Settings
    {
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            return configuration["connectionString"];
        }
    }
}