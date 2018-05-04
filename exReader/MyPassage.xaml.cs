using exReader.PassageManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace exReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyPassage : Page
    {
        
        private ObservableCollection<Passage> passagelists;
        public MyPassage()
        {
            this.InitializeComponent();
            passagelists = new ObservableCollection<Passage>();
            LoadPassages();
          
        }

        //清空历史文章列表
        private void clearPassage_button_Click(object sender, RoutedEventArgs e)
        {       
            if (passagelists.Count != 0)
            {
                passagelists.Clear();
                PassageManage.ClearPassages();
                Empty_PassageLabel.Opacity = 1;
            }
            else Empty_PassageLabel.Opacity = 0;
        }

        //选择文章，跳转至reader界面
        private void Passage_list_ItemClick(object sender, ItemClickEventArgs e)
        {
            Passage choose = new Passage();
            choose = (Passage)e.ClickedItem;
            Frame.Navigate(typeof(MainReader),choose);
            Frame appFrame = Window.Current.Content as Frame;
            MainPage mainPage = appFrame.Content as MainPage;
            mainPage.SetSelectedNavigationItem(0);
           
        }

        //加载历史文章
        private void LoadPassages()
        {
            if (PassageManage.HistoryPassages.Count !=0 )
            {
                Empty_PassageLabel.Opacity = 0;
                ObservableCollection<Passage> passages = new ObservableCollection<Passage>(PassageManage.LoadPassages());
                foreach (var p in passages)
                {
                    passagelists.Add(p);
                }
            }
            else Empty_PassageLabel.Opacity = 1;
           
        }
    }
}
