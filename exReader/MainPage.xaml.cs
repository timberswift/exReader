using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace exReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            DatabaseManager.WordManage.instance = new DatabaseManager.WordManage();
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            readerPageHome.IsSelected = true;
            
        }
        private void NavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (myPassageItem.IsSelected)
            {
                MyFrame.Navigate(typeof(MyPassage));
                MyNavigationView.Header = "我的文章";
            }
            else if (myWordsListItem.IsSelected)
            {
                MyFrame.Navigate(typeof(MyWordsList));
                MyNavigationView.Header = "我的生词本";
            }
            else if (readerPageHome.IsSelected)
            {
                MyFrame.Navigate(typeof(MainReader));
                MyNavigationView.Header = "Header of this passage";
            }
        }
        public void SetSelectedNavigationItem(int index)
        {
            MyNavigationView.SelectedItem = MyNavigationView.MenuItems[index];
        }
    }
}
