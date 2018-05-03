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

        private void refreshPassage_button_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainReader));
            for (int i =0;i<10;i++)
            {
                passagelists.Add(new Passage
                {
                    Content = "Donald Trump has pulled back from a potential trade war with Europe by postponing a decision on imposing steel and aluminum tariffs until 1 June.The US president imposed a worldwide 25 % tariff on steel imports and a 10 % tariff on aluminum in March but granted temporary exemptions to Canada, Mexico, Brazil, the European Union(EU), Australia and Argentina.These were due to expire at 12.01am on Tuesday.The extension offers temporary reprieve for French president Emmanuel Macron and German chancellor Angela Merkel, who lobbied Trump during visits to the White House last week.It could also be seen by political analysts as the latest issue on which Trump’s bark has proved worse than his bite.The administration “reached agreements in principle with Argentina, Australia, and Brazil with respect to steel and aluminum, the details of which will be finalized shortly”, the White House said on Monday. “The Administration is also extending negotiations with Canada, Mexico, and the European Union for a final 30 days. "
                    ,
                    HeadName = "Bad now Ameriacan " + i
                });
            }
        }

        //选择文章，跳转至reader界面
        private void Passage_list_ItemClick(object sender, ItemClickEventArgs e)
        {
            Passage choose = new Passage();
            choose = (Passage)e.ClickedItem;
           // Debug.WriteLine(choose.Content);
            Frame.Navigate(typeof(MainReader),choose);
            Frame appFrame = Window.Current.Content as Frame;
           MainPage mainPage = appFrame.Content as MainPage;
            mainPage.SetSelectedNavigationItem(0);

           
        }

        //加载历史文章
        private void LoadPassages()
        {
            if (PassageManage.HistoryPassages != null)
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
