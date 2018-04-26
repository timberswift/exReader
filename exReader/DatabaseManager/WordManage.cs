using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace exReader.DatabaseManager
{
    class WordManage
    {
        public static WordManage instance;
        SqliteConnection db;
        public WordManage()
        {
            string path = Path.GetFullPath("db/dic.db");
            db = new SqliteConnection("Filename="+path);
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.CommandText = "SELECT sw FROM stardict WHERE sw = 'age'";
            command.Connection = db;
            var reader = command.ExecuteReader();
            reader.Read();
            String a = reader.GetString(0);
        }
    }
}
