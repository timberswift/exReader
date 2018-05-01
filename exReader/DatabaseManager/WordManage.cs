using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using exReader.WordsManager;
//using Microsoft.Data.Sqlite;
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
        SQLiteConnection dbfile;
        SQLiteConnection db;
        public WordManage()
        {
            //dictionary file sqlite
            string path = Path.GetFullPath("db/dic.db");
            db = new SQLiteConnection("Filename="+path);
            db.Open();
        }
        public List<Vocabulary> QuiryWord(String text, String Type)
        {
            //Type can be   zk gk cet4 cet6 toefl gre ielts ky
            //如果查询的类别不是这些考试类别之一则报错
            if(Type!="zk"&& Type !="gk" && Type !="cet4" && Type !="cet6" && Type !="toefl" && Type !="gre" && Type !="ielts" && Type != "ky")
            {
                throw new Exception("Test type "+Type+" not supported.");
            }
            String[] splitedtext = text.Split(new char[] {' ', ',', '.', '?', '!', '\'', '\"', '='});
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            /*
            foreach(String aword in splitedtext)
            {
                SqliteCommand command = new SqliteCommand();
                command.CommandText = "SELECT * FROM stardict WHERE word = '"+aword+"'";
                command.Connection = db;
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Vocabulary aunit = new Vocabulary();
                    aunit.Word = aword;
                    aunit.Translation = ;
                    aunit.Classification = ;
                    aunit.YesorNo = ;
                    String a = reader;
                }
            }
            */
            return vocabularies;
        }
    }

}
