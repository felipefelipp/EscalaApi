using System.Data.SQLite;

namespace EscalaApi.Data;

public static class DatabaseContext
{
    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection("Data Source=./database");
    }
}