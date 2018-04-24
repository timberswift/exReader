using System;
using System.Collections.Generic;
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
        private List<WordsManager.Vocabulary> booklists;
        

        public MyWordsList()
        {
            this.InitializeComponent();
            booklists = WordsManager.WordBook.GetBooks();
        }


        private async void allBook_button_Click(object sender, RoutedEventArgs e)
        {
            //await GetWordsBook(1);
            WordsManager.Vocabulary v = new WordsManager.Vocabulary();
            v.Word = "lala";
            v.Translation = "拉拉";
            v.Classification = 1;
            booklists.Add(v);

        }

        private async void cet4Button_Click(object sender, RoutedEventArgs e)
        {
            //await GetWordsBook(2);
            booklists = WordsManager.WordBook.GetBooks();
        }

        private  async void cet6Button_Click(object sender, RoutedEventArgs e)
        {
            //await GetWordsBook(3);
        }

        private void kyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void toeflButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ieltsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void greButton_Click(object sender, RoutedEventArgs e)
        {

        }

        async private Task GetWordsBook(int type)
        {
            switch (type)
            {
                case 0:  booklists = WordsManager.WordBook.GetBooks(); break;
                case 1: booklists = WordsManager.WordBook.GetBooks(); break;
                case 2: booklists = WordsManager.WordBook.GetBooks(); break;
                case 3: booklists = WordsManager.WordBook.GetBooks(); break;
                default: break;
            }
        }

        private void allwords_listview_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void allwords_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
