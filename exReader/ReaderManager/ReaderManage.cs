using exReader.PassageManager;
using exReader.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace exReader.ReaderManager
{
    //李京懋工作空间
    //本空间下实现 阅读器主页各功能、主要的提词算法
    //附操作 MainReader.xaml.cs


    [DataContract]
    public class ReaderManage
    {
        private Passage readerPassage;
        private WordBook readerWordBook;   
        private ObservableCollection<Vocabulary> readerWordLists;
        private int readerChooseMode;

        [DataMember]
        public Passage ReaderPassage
        {
            get
            {
                return (new PassageManage().GetPassage());
            }
            set
            {
                readerPassage = value;
            }
        }

        public WordBook ReaderWordBook
        {
            get
            {
                return readerWordBook;
            }
            set
            {
                readerWordBook = value;
            }
        }

        [DataMember]
        public int ReaderChooseMode
        {
            get
            {
                return readerChooseMode;
            }
            set
            {
                readerChooseMode = value;
            }
        }

        [DataMember]
        public ObservableCollection<Vocabulary> ReaderWordLists
        {
            get
            {
                return readerWordLists;
            }
            set
            {
                readerWordLists = value;
            }
        }



        //在数据库匹配单词
        public void MatchWords(string type, int t)
        {
            List<Vocabulary> lists = DatabaseManager.WordManage.instance.QueryWord(this.ReaderPassage.Content, type);
            List<Vocabulary> newlist = lists.GroupBy(x => x.Word).Select(x => x.First()).ToList<Vocabulary>();  //去重复      
            ObservableCollection<Vocabulary> vocabularies = new ObservableCollection<Vocabulary>(newlist);
            this.readerChooseMode = t;
            this.ReaderWordLists = vocabularies;
        }

    }

    public class ReaderWords
    {

        //private 
    }
}
