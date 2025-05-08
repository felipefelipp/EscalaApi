using System.Data;
using Microsoft.Data.SqlClient;

namespace EscalaApi.Data;

public static class DatabaseContext
{
    private static string _connectionString;
    
    // Método de inicialização (chame no startup)
    public static void Initialize(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Validação básica da connection string
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new ArgumentNullException(nameof(_connectionString), 
                "Connection string não configurada. Verifique appsettings.json");
        }
    }
    
    public static IDbConnection GetConnection()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException(
                "DatabaseContext não foi inicializado. Chame Initialize() primeiro.");
        }
        
        return new SqlConnection(_connectionString);
    }
}