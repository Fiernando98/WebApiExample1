using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class FilesServices {
        public static async Task<Files> Create(IFormFile newItem,string pathServer) {
            try {
                if (newItem.Length <= 0) throw new Exception("Archivo inválido");
                Directory.CreateDirectory(pathServer);
                string fileName = newItem.FileName.Split("/").LastOrDefault()!;
                string filePath = Path.Combine(pathServer,fileName);
                using (Stream fileStream = new FileStream(filePath,FileMode.Create)) {
                    await newItem.CopyToAsync(fileStream);
                }
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {FilesSQLTable.tableName} ({FilesSQLTable.path}) VALUES (?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{FilesSQLTable.path}",filePath));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        long newID = dbContext.LastInsertRowId;
                        return new Files {
                            ID = newID,
                            Path = new Uri(filePath)
                        };
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
