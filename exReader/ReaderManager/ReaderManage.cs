using exReader.PassageManager;
using exReader.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [DataMember]
        public ObservableCollection<Vocabulary> readerWordLists = new ObservableCollection<Vocabulary>(WordBook.GetBooks(1));
       // private Tuple<int, int, int, int,int, int> readerChooseMode;
        private int readerChooseMode; //000000  CET4|CET6|Kaoyan|T|I|G    value range: 0~63

        


        //public 

    }

    public class ReaderWords
    {
        private Vocabulary vocabulary;
        //private 
    }
}
