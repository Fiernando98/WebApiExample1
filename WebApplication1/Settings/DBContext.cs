using System.Data.SQLite;
namespace WebApplication1.Settings {
    public class DBContext {
        private const string DBName = "db.sqlite";

        public static SQLiteConnection GetInstance() {
            SQLiteConnection db = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBName));
            db.Open();
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

        private static void CreateTables(SQLiteConnection dbContext) {
            String queryCreate = "CREATE TABLE IF NOT EXISTS Foods (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL, description TEXT NOT NULL, calories NUMERIC NOT NULL)";
            SQLiteCommand command = new SQLiteCommand(queryCreate, dbContext);
            command.ExecuteNonQuery();
        }
    }
}
