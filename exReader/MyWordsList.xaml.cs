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
            SwitchWordsBook(0);
          
        }

        private  void AllBook_button_Click(object sender, RoutedEventArgs e)
        {
           
           SwitchWordsBook(0);
           allwords_label.Text = "全部词汇";
        }

        private  void Cet4Button_Click(object sender, RoutedEventArgs e)
        {
            //SectionChange(0);
            SwitchWordsBook(1);
            allwords_label.Text = "CET4词汇";
        }

        private void Cet6Button_Click(object sender, RoutedEventArgs e)
        {
             SwitchWordsBook(2);
            allwords_label.Text = "CET6词汇";
        }

        private  void KyButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchWordsBook(3);
            allwords_label.Text = "考研词汇";
        }

        private  void ToeflButton_Click(object sender, RoutedEventArgs e)
        {        
             SwitchWordsBook(4);
            allwords_label.Text = "TOEFL词汇";
        }

        private void IeltsButton_Click(object sender, RoutedEventArgs e)
        {
           
            SwitchWordsBook(5);
            allwords_label.Text = "IELTS词汇";
        }

        private  void GreButton_Click(object sender, RoutedEventArgs e)
        {
             SwitchWordsBook(6);
             allwords_label.Text = "GRE词汇";
        }

        // 匹配选择单词本，从数据库取出相应单词本，放入绑定数据列表
        private void SwitchWordsBook(int type)
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
                    booklist = new ObservableCollection<Vocabulary>(WordBook.CET4_Book);
                    break;
                case 2:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.CET6_Book);
                    break;
                case 3:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.Kaoyan_Book);
                    break;
                case 4:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.TOEFL_Book);
                    break;
                case 5:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.IELTS_Book);
                    break;
                case 6:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.GRE_Book);
                    break;
                default:
                    booklist = new ObservableCollection<Vocabulary>(WordBook.All_Book);                 
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
                booklists.Remove(word);  //总词库也移除
                yesbooklists.Remove(word);
            }
            UpdateStateColor();
            ShowEmptyLabel(nobooklists, 2);
            ShowEmptyLabel(yesbooklists, 3);

        }

  
        private void UpdateStateColor()
        {
            
            ObservableCollection<Vocabulary> newlist = new ObservableCollection<Vocabulary>(booklists);
            booklists.Clear();
            foreach(var item in newlist)
            {
                if(item.YesorNo == 1)
                {
                    item.StateColor = "#00ff00";
                    booklists.Add(item);
                }
                else if(item.YesorNo == 0)
                {

                    item.StateColor = "#ff0000";
                    booklists.Add(item);
                }
            }
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
                    all_section.Background =  new SolidColorBrush(Color.FromArgb(100,207, 103, 63)) ;
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
