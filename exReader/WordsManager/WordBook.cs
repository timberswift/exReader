using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exReader.WordsManager
{
    public class Vocabulary
    {
        public string Word { get; set; }            //单词
        public string Translation { get; set; }     //单词释义
        public int Classification { get; set; }     //单词分类

    }

    public class WordBook
    {
        private List<Vocabulary> CET4_Book { get; set; }
        private List<Vocabulary> CET6_Book { get; set; }
        private List<Vocabulary> Kaoyan_Book { get; set; }
        private List<Vocabulary> TOEFL_Book { get; set; }
        private List<Vocabulary> IELTS_Book { get; set; }
        private List<Vocabulary> GRE_Book { get; set; }

        public static List<Vocabulary> GetBooks()
        {
            var books = new List<Vocabulary>();
            books.Add(new Vocabulary { Word = "hello", Translation ="n. 你好" , Classification = 1 });
            books.Add(new Vocabulary { Word = "word", Translation = "n. 话", Classification = 1 });
            books.Add(new Vocabulary { Word = "class", Translation = "n. 班级，分类", Classification = 1 });
            books.Add(new Vocabulary { Word = "translate", Translation = "v. 翻译", Classification = 1 });
            books.Add(new Vocabulary { Word = "book", Translation = "n. 书", Classification = 1 });
            books.Add(new Vocabulary { Word = "static", Translation = "a. 静态的", Classification = 1 });
            books.Add(new Vocabulary { Word = "head", Translation = "n. 头", Classification = 1 });     
            return books;
        }

    }
}
