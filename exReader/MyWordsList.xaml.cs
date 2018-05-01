using exReader.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace exReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyWordsList : Page
    {
        private ObservableCollection<Vocabulary> booklists;
        private ObservableCollection<Vocabulary> nobooklists;    
        private ObservableCollection<Vocabulary> yesbooklists;
     
        public MyWordsList()
        {
            this.InitializeComponent(); 
            booklists = new ObservableCollection<Vocabulary>(new WordBook().CET4_Book);
            nobooklists = new ObservableCollection<Vocabulary>(WordBook.GetNoWordBook(booklists));
            yesbooklists = new ObservableCollection<Vocabulary>(WordBook.GetYesWordBook(booklists));
        }


        private async void AllBook_button_Click(object sender, RoutedEventArgs e)
        {
            /*
            Vocabulary v = new Vocabulary();
            v.Word = "new";
            v.Translation = "a. 新的";
            v.Classification = 1;
            v.YesorNo = 1;
            v.StateColor = "#ff0000";
            booklists.Add(v);
            Vocabulary b = new Vocabulary();
            b.Word = "vocabulary";
            b.Translation = "n. 词汇";
            b.Classification = 1;
            b.YesorNo = 0;
            b.StateColor = "#ff0000";
            booklists.Add(b);
            */
            await SwitchWordsBook(1);

        }

        private async void Cet4Button_Click(object sender, RoutedEventArgs e)
        {

            await SwitchWordsBook(1);
            // booklists = new WordsManager.WordBook.GetBooks(2);
           // booklists.Clear();
           // ObservableCollection<Vocabulary> newBooklists = new ObservableCollection<Vocabulary>(WordBook.GetBooks(2));
        }

        private async void Cet6Button_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(3);
        }

        private async void KyButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(1);  
        }

        private async void ToeflButton_Click(object sender, RoutedEventArgs e)
        {        
            await SwitchWordsBook(2);
        }

        private async void IeltsButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(3);
        }

        private async void GreButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(1);
        }

        async private Task SwitchWordsBook(int type)
        {
            booklists.Clear();
            nobooklists.Clear();
            yesbooklists.Clear();
            ObservableCollection<Vocabulary> booklist;
            ObservableCollection<Vocabulary> nbooklist;
            ObservableCollection<Vocabulary> ybooklist;
            switch (type)
            {
                
                case 1:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().CET4_Book);
                    break;
                case 2:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().CET6_Book);
                    break;
                case 3:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().Kaoyan_Book);
                    break;
                case 4:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().TOEFL_Book);
                    break;
                case 5:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().IELTS_Book);
                    break;
                case 6:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().GRE_Book);
                    break;
                default:
                    booklist = new ObservableCollection<Vocabulary>(new WordBook().CET4_Book);                 
                    break;
            }

            foreach (var each in booklist)
            {
                booklists.Add(each);
            }

            nbooklist = WordBook.GetNoWordBook(booklists);
            foreach (var each in nbooklist)
            {
                nobooklists.Add(each);
            }

            ybooklist = WordBook.GetYesWordBook(booklists);
            foreach (var each in ybooklist)
            {
                yesbooklists.Add(each);
            }

        }

        private void Allwords_listview_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void Allwords_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void WordRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;       
            Vocabulary word = button.DataContext as Vocabulary;
            if(word.YesorNo == 0)
            {
                word.YesorNo = 1;
                nobooklists.Remove(word);
                yesbooklists.Add(word);
            }
            else
            {
                word.YesorNo = -1;  //被用户删除的状态
                yesbooklists.Remove(word);
            }
            // Debug.WriteLine("word: " + word.Word + "\n trans: " + word.Translation + "\n y_or_n: " + word.YesorNo + "\n clas: " + word.Classification);
            // ...
        }

        public ObservableCollection<Vocabulary> GetStateWordBook(ObservableCollection<Vocabulary> allWordBook,int type)
        {
            var noWordBook = new ObservableCollection<Vocabulary>();
            var yesWordBook = new ObservableCollection<Vocabulary>();
            foreach (var word in allWordBook)
            {
                if (word.YesorNo == 0)
                {
                    word.StateColor = "#00ff00";
                    noWordBook.Add(word);
                }

                else if (word.YesorNo == 1)
                {
                    word.StateColor = "#ff0000";
                    yesWordBook.Add(word);
                }
            }
            if (type == 0) return noWordBook;
            else return yesWordBook;
        }


        public void Print(ObservableCollection<Vocabulary> v)
        {
            foreach(var i in v)
            {
                Debug.WriteLine(i.Word);
            }
        }

      
    }
}
