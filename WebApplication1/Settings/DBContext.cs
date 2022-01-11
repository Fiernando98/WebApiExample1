using System.Data.SQLite;
using WebApplication1.Models;
namespace WebApplication1.Settings {
    public class DBContext {
        private const string DBName = "db.sqlite";

        public static SQLiteConnection GetInstance() {
            SQLiteConnection db = new SQLiteConnection(string.Format("Data Source={0};Version=3;",DBName));
            db.Open();
            TurnOnFK(db);
            return db;
        }

        public static void ConectDB(string pathServer) {
            Directory.CreateDirectory(pathServer);
            string fullPath = Path.Combine(pathServer,DBName);
            if (!File.Exists(Path.GetFullPath(fullPath))) {
                SQLiteConnection.CreateFile(fullPath);
            }
            using (SQLiteConnection dbContext = GetInstance()) {
                CreateTables(dbContext);
            }
        }

        private static void TurnOnFK(SQLiteConnection dbContext) {
            foreach (string table in TablesDB()) {
                try {
                    SQLiteCommand command = new SQLiteCommand("PRAGMA foreign_keys = 1;",dbContext);
                    command.ExecuteNonQuery();
                } catch {
                    throw;
                }
            }
        }

        private static void CreateTables(SQLiteConnection dbContext) {
            foreach (string table in TablesDB()) {
                try {
                    SQLiteCommand command = new SQLiteCommand(table,dbContext);
                    command.ExecuteNonQuery();
                } catch {
                    throw;
                }
            }
        }

        private static List<string> TablesDB() {
            List<string> list = new List<string>();
            list.Add(UserSQLTable.toCreateQuery);
            list.Add(AuthTokenSQLTable.toCreateQuery);
            list.Add(RestaurantSQLTable.toCreateQuery);
            list.Add(FoodSQLTable.toCreateQuery);
            list.Add(FilesSQLTable.toCreateQuery);
            return list;
        }
    }
}
