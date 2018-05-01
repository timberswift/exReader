using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace exReader.WordsManager
{
    //汤浩工作空间
    //本空间下实现单词本界面各功能实现
    //附操作 MyWordsList.xaml.cs

    public enum Type
    {
        CET4 = 1,
        CET6 = 2,
        KY = 3,
        TOEFL = 4,
        IETLTS = 5,
    }

    public class Vocabulary
    {
        //private Tuple<string, string, int, int, string> Word;
        private string word;           //单词
        private string translation;//单词释义
        // public Type Classification { get; set; }     //单词分类
        private int classification;    //单词分类
        private int yesorNo;         //单词掌握情况
        private string stateColor;

        public string Word
        {
            get { return word; }
            set { word = value; }
            
        }
        public string Translation
        {
            get { return translation; }
            set { translation = value; }

        }
        public int Classification
        {
            get { return classification; }
            set { classification = value; }
        }
        public int YesorNo
        {
            get { return yesorNo; }
            set { yesorNo = value; }
        }
        public string StateColor
        {
            get { return stateColor; }
            set { stateColor = value; }
        }

        
    }

    public class WordBook
    {
        private List<Vocabulary> reader_Book;
        public List<Vocabulary> CET4_Book = GetBooks(1);
        // public List<Vocabulary> CET { get { return CET4_Book; } set { value = GetBooks(2); } }
        public List<Vocabulary> CET6_Book = GetBooks(2); // { get; set; }
        public List<Vocabulary> Kaoyan_Book = GetBooks(3); //{ get; set; }
        public List<Vocabulary> TOEFL_Book = GetBooks(1); //{ get; set; }
        public List<Vocabulary> IELTS_Book = GetBooks(2); //{ get; set; }
        public List<Vocabulary> GRE_Book = GetBooks(3); //{ get; set; }

        public List<Vocabulary> Reader_Book
        {
            get { return reader_Book; }
            set { reader_Book = value; }
        }

        public static List<Vocabulary> GetBooks(int type)  //1.提取reader_Book的各类单词  2.累加历史单词本  3.然后去重复
        {
            var books = new List<Vocabulary>();           
            /*
            foreach (var word in Reader_Book)
            {
                if (word.Classification == type) books.Add(word);
                    
            }

            //去重复
            return books;
             */
            switch (type)
            {
                case 1:
                    
                    books.Add(new Vocabulary { Word = "hello", Translation = "n. 你好", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "word", Translation = "n. 话", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "class", Translation = "n. 班级，分类", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "translate", Translation = "v. 翻译", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "book", Translation = "n. 书  v.预订 v.测试长度1 a.测试长度2", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "static", Translation = "a. 静态的", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "head", Translation = "n. 头", Classification = type, YesorNo = 1 });

                    books = MarkColor(books);
                    return books;
                case 2:
                    books.Add(new Vocabulary { Word = "visual", Translation = "a. 视觉的", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "studio", Translation = "n. 工作站", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "reader", Translation = "n. 阅读器", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "constraint", Translation = "n. 限制", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "Chinese", Translation = "n. 中国人 a. 中国人的", Classification = type, YesorNo = 1 });
                   
                    books = MarkColor(books);
                    return books;
                default:
                    books.Add(new Vocabulary { Word = "passage", Translation = "n. 文章", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "manager", Translation = "n. 经理、管理器", Classification = type, YesorNo = 1 });
                    books.Add(new Vocabulary { Word = "ink", Translation = "n. 墨水", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "delicate", Translation = "a. 易碎的", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "enchanted", Translation = "a. 迷人的", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "static", Translation = "a. 静态的", Classification = type, YesorNo = 0 });
                    books.Add(new Vocabulary { Word = "head", Translation = "n. 头", Classification = type, YesorNo = 1 });

                    books = MarkColor(books);
                    return books;
            }
           
        }

        public static ObservableCollection<Vocabulary> GetNoWordBook(ObservableCollection<Vocabulary> allWordBook)
        {
            var noWordBook = new ObservableCollection<Vocabulary>();
            foreach(var word in allWordBook)
            {
                if (word.YesorNo == 0)
                {
                    word.StateColor = "#ff0000";
                    noWordBook.Add(word);
                }
            }
            return noWordBook;
        }

        public static ObservableCollection<Vocabulary> GetYesWordBook(ObservableCollection<Vocabulary> allWordBook)
        {
            var yesWordBook = new ObservableCollection<Vocabulary>();
            foreach (var word in allWordBook)
            {
                if (word.YesorNo == 1)
                {
                    word.StateColor = "#00ff00";
                    yesWordBook.Add(word);
                }
            }
            return yesWordBook;
        }

        public static List<Vocabulary> MarkColor(List<Vocabulary> v)
        {
            foreach(var item in v)
            {
                if(item.YesorNo == 1)
                {
                    item.StateColor = "#00ff00";
                }
                else if(item.YesorNo == 0)
                {
                    item.StateColor = "#ff0000";
                }
            }
            return v;
        }

    }
}
