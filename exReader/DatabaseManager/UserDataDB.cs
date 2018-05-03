using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exReader.PassageManager;
using exReader.WordsManager;
using Microsoft.Data.Sqlite;

namespace exReader.DatabaseManager
{
    class UserDataDB
    {
        public static UserDataDB instance;
        SqliteConnection db;
        public UserDataDB()
        {
            db = new SqliteConnection("filename=userdata.db");
            db.Open();
            var command = new SqliteCommand();
            command.Connection = db;
            try
            {
                command.CommandText = "CREATE TABLE newword(word varchar(64))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE UNIQUE INDEX newword_word newword(word)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE articles(title text, content text)";
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
        }
        public List<String> FetchWord()
        {
            List<String> words = new List<string>();

            var command = new SqliteCommand();
            command.Connection = db;

            command.CommandText = "SELECT word FROM newword";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                words.Add(reader.GetString(0));
            }
            reader.Close();
            return words;
        }
        public void SaveWordBook(List<Vocabulary> book)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            foreach (Vocabulary v in book)
            {
                command.CommandText = "INSERT OR IGNORE INTO newword VALUES('" + v.Word + "')";
                command.ExecuteNonQuery();
            }
        }
        public void SavaPassage(Passage passage)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO articles (title,content) VALUES ("+passage.HeadName+","+passage.Content+")";
            command.ExecuteNonQuery();
        }
        public List<Passage> LoadPassage()
        {
            List<Passage> myPassages = new List<Passage>();
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT titlt FROM articles";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Passage passage = new Passage();
                passage.HeadName= reader.GetString(0);
                myPassages.Add(passage);
            }
            reader.Close();
            return myPassages;

        }
    }
}
