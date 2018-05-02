using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using exReader.WordsManager;

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
    public class WordManage
    {
        public static WordManage instance;
        SQLiteConnection dbfile;
        SQLiteConnection db;
        public WordManage()
        {
            //in memory
            string str = "Data Source=:memory:;Version=3;New=True;";
            db = new SQLiteConnection(str);
            db.Open();
            //dictionary file sqlite
            string path = Path.GetFullPath("db/dic.db");
            dbfile = new SQLiteConnection("Data Source=" + path + ";");
            dbfile.Open();
            //数据库读入内存
            dbfile.BackupDatabase(db, "main", "main", -1, null, 0);
            //建立查询缓存单词表
            var command = new SQLiteCommand();
            command.Connection = db;
            command.CommandText = "CREATE TABLE zk AS SELECT word FROM stardict WHERE tag LIKE \'%zk%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE gk AS SELECT word FROM stardict WHERE tag LIKE \'%gk%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE cet4 AS SELECT word FROM stardict WHERE tag LIKE \'%cet4%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE cet6 AS SELECT word FROM stardict WHERE tag LIKE \'%cet6%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE toefl AS SELECT word FROM stardict WHERE tag LIKE \'%toefl%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE gre AS SELECT word FROM stardict WHERE tag LIKE \'%gre%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE ielts AS SELECT word FROM stardict WHERE tag LIKE \'%ielts%\'";
            command.Connection = db;
            command.CommandText = "CREATE TABLE ky AS SELECT word FROM stardict WHERE tag LIKE \'%ky%\'";
            command.ExecuteNonQuery();
            //建立查询文章表
            command.CommandText = "CREATE TABLE wordset AS SELECT word FROM stardict WHERE \'1\' = \'2\'";
            command.ExecuteNonQuery();
            //ttt
            //QueryWord("have if base go usage able technology","cet4");
        }

        public List<Vocabulary> QueryWord(String text, String Type)
        {
            //Type can be   zk gk cet4 cet6 toefl gre ielts ky
            //如果查询的类别不是这些考试类别之一则报错
            if (Type != "zk" && Type != "gk" && Type != "cet4" && Type != "cet6" && Type != "toefl" && Type != "gre" && Type != "ielts" && Type != "ky")
            {
                throw new Exception("Test type " + Type + " not supported.");
            }
            String[] splitedtext = text.Split(new char[] { ' ', ',', '.', '?', '!', '\'', '\"', '=' });
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            //开始添词
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = db;
            foreach (String aword in splitedtext)
            {
                command.CommandText = "INSERT INTO wordset VALUES (\'" + aword + "\')";
                command.ExecuteNonQuery();
            }
            //词语连接
            command.CommandText =
                "SELECT wordset.word,stardict.translation FROM wordset,stardict WHERE wordset.word = stardict.word AND stardict.tag LIKE \'%" + Type + "%\'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Vocabulary v = new Vocabulary();
                v.Word = reader.GetString(0);
                v.Translation = reader.GetString(1);
                v.Classification = 1;
                v.YesorNo = 0;
                vocabularies.Add(v);
            }
            reader.Close();
            //最后删词
            command.CommandText = "DELETE FROM wordset WHERE \'1\'=\'1\'";
            command.ExecuteNonQuery();
            return vocabularies;
        }
    }

}
