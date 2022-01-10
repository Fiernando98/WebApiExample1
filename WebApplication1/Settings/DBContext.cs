using System.Data.SQLite;
namespace WebApplication1.Settings {
    public class DBContext {
        private const string DBName = "db.sqlite";

        public static SQLiteConnection GetInstance() {
            SQLiteConnection db = new SQLiteConnection(string.Format("Data Source={0};Version=3;",DBName));
            db.Open();
            TurnOnFK(db);
            return db;
        }

        public static void ConectDB() {
            if (!File.Exists(Path.GetFullPath(DBName))) {
                SQLiteConnection.CreateFile(DBName);
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
            list.Add("CREATE TABLE IF NOT EXISTS Restaurants (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL)");
            list.Add("CREATE TABLE IF NOT EXISTS Foods (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, id_restaurant INTEGER, name TEXT NOT NULL, description TEXT NOT NULL, calories NUMERIC NOT NULL, FOREIGN KEY(id_restaurant) REFERENCES Restaurants(id) ON DELETE CASCADE ON UPDATE CASCADE)");
            list.Add("CREATE TABLE IF NOT EXISTS AuthTokens (id_restaurant INTEGER, token TEXT NOT NULL, FOREIGN KEY(id_restaurant) REFERENCES Restaurants(id) ON DELETE CASCADE ON UPDATE CASCADE)");
            return list;
        }
    }
}
