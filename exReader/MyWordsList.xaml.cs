using exReader.DatabaseManager;
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
using Windows.UI;
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
        private ObservableCollection<Vocabulary> booklists = new ObservableCollection<Vocabulary>();
        private ObservableCollection<Vocabulary> nobooklists = new ObservableCollection<Vocabulary>();
        private ObservableCollection<Vocabulary> yesbooklists = new ObservableCollection<Vocabulary>();

        public MyWordsList()
        {
            this.InitializeComponent();
            all_empty.Opacity = 0;
            yes_empty.Opacity = 0;
            no_empty.Opacity = 0;
          
        }

        private async void AllBook_button_Click(object sender, RoutedEventArgs e)
        {
           
            await SwitchWordsBook(0);

        }

        private async void Cet4Button_Click(object sender, RoutedEventArgs e)
        {
            //SectionChange(0);
            await SwitchWordsBook(1);
        }

        private async void Cet6Button_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(2);
        }

        private async void KyButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(3);  
        }

        private async void ToeflButton_Click(object sender, RoutedEventArgs e)
        {        
            await SwitchWordsBook(4);
        }

        private async void IeltsButton_Click(object sender, RoutedEventArgs e)
        {
           
            await SwitchWordsBook(5);
        }

        private async void GreButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchWordsBook(6);
        }

        // 匹配选择单词本，从数据库取出相应单词本，放入绑定数据列表
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
                case 0:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.All_Book);
                    break;
                case 1:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("cet4"));
                    break;
                case 2:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("cet6"));
                    break;
                case 3:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("ky"));
                    break;
                case 4:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("toefl"));
                    break;
                case 5:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("ielts"));
                    break;
                case 6:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("gre"));
                    break;
                default:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.FetchWordBook("cet4"));                 
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

            ShowEmptyLabel(booklist, 1);
            ShowEmptyLabel(nbooklist, 2);
            ShowEmptyLabel(ybooklist, 3);

        }

        // # 移动单词掌握状态
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
            ShowEmptyLabel(yesbooklists, 3);

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


        //显示空列表标签
        private void ShowEmptyLabel(ObservableCollection<Vocabulary> lists,int type)
        {           
            if(lists.Count == 0)
            {
                switch (type)
                {
                    case 1: all_empty.Opacity = 1; break;
                    case 2: no_empty.Opacity = 1; break;
                    case 3: yes_empty.Opacity = 1; break;
                    default:break;
                }
            }
            else
            {
                switch (type)
                {
                    case 1: all_empty.Opacity = 0; break;
                    case 2: no_empty.Opacity = 0; break;
                    case 3: yes_empty.Opacity = 0; break;
                    default: break;
                }
            }

        }

        // # 显示当前选择单词本模式背景
        private void SectionChange(int type)
        {
            switch(type)
            {
                case 0:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100,207, 103, 63)) ;
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));     
                    break;
                case 1:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case 2:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case 3:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case 4:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case 5:
                    all_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    cet4_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    cet6_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    kaoyan_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    toefl_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ielts_section.Background = new SolidColorBrush(Color.FromArgb(100, 207, 103, 63));
                    gre_section.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
            }
        }

        public void Print(ObservableCollection<Vocabulary> v)
        {
            foreach (var i in v)
            {
                Debug.WriteLine(i.Word);
            }
        }

    }
}
