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

    // 王云昊、李京懋工作空间
    // 实现 词库数据库 与 前端 对接，实现提词功能

    /* 单词属性规定
     
    public string Word { get; set; }            // "book"
    public string Translation { get; set; }     // "n. 书；v. 预订"
           //public string pronunciation { get; set; } //   音标待定  "/`boo:k/"
    public Type Classification { get; set; }    // "CET4"
    public int YesorNo { get; set; }            // "1"
 
    */

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
