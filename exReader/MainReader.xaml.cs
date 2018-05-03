using exReader.DatabaseManager;
using exReader.FileManager;
using exReader.PassageManager;
using exReader.ReaderManager;
using exReader.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace exReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class MainReader : Page
    {
        private ReaderManage reader = new ReaderManage();
        private FileManage fileManage;// = new FileManage();
        private ObservableCollection<Vocabulary> readerWordLists;// =  new ObservableCollection<Vocabulary>();   //提词列表ListView绑定的数据
        

        public MainReader()
        {
            //editor.Document.SetText(Windows.UI.Text.TextSetOptions.None, "This is some sample text");
            this.InitializeComponent();
            fileManage = new FileManage();
            readerWordLists = new ObservableCollection<Vocabulary>();
            
            UpdateBindingData(reader.ReaderWordLists, reader.ReaderChooseMode);
            WordBook.InitWordsBook();  //  每次切换回提词界面，单词本清空。
        }
        private void words_view_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void initReaderList()
        {
           // reader
            // readerWordLists = new ObservableCollection<Vocabulary>();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Vocabulary item = button.DataContext as Vocabulary;
           
            Vocabulary result = (Vocabulary)reader.ReaderWordLists.Where(x => x.Word == item.Word);
            result.YesorNo = 1;
            readerWordLists.Remove(item);  
          //  ShowEmptyLabel(yesbooklists, 3);

        }

        private void export_file_Click(object sender, RoutedEventArgs e)
        {
            // FileManage fileManage = new FileManage();
            ReaderManage temp_reader = reader;
            fileManage.SerializeFile(temp_reader);

        }

        private void save_file_Click(object sender, RoutedEventArgs e)
        {
            //reader = await fileManage.DeSerializeFile();
        }

        private async void open_file_Click(object sender, RoutedEventArgs e)
        {

            reader = await fileManage.DeSerializeFile();
            if (reader != null) UpdateBindingData(reader.ReaderWordLists,reader.ReaderChooseMode);
            //MainPage.MyNavigationView.Header = "Header of this passage";
        }

     
       
        private void cet4_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "CET4";
            SearchWordList("cet4",1);
        }

        private void cet6_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "CET6";
            SearchWordList("cet6",2);
        }

        private void kaoyan_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "考研";
            SearchWordList("ky",3);
        }

        private void toefl_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "托福TOEFL";
            SearchWordList("toefl",4);
        }

        private void ielts_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "雅思IELTS";
            SearchWordList("ielts", 5);
        }

        private void gre_button_Click(object sender, RoutedEventArgs e)
        {
            Mode_Lable.Text = "GRE";
            SearchWordList("gre",6);
        }

        //根据按钮选择生成提词列表
        private void SearchWordList(string type,int t)
        {
            reader.MatchWords(type, t);
            UpdateBindingData(reader.ReaderWordLists, t);
        }


        //更新提词列表数据
        public void UpdateBindingData(ObservableCollection<Vocabulary> newlists, int type)
        {
            if (readerWordLists != null && newlists != null)
            {
                readerWordLists.Clear();
                foreach (var item in newlists)
                {
                    if (type != 0) item.Classification = type;
                    readerWordLists.Add(item);
                }
                reader_empty.Opacity = 0;
            }
        }

        //将提词表导出到单词本
        private void export_word_button_Click(object sender, RoutedEventArgs e)
        {
            int type_mark = reader.ReaderChooseMode;

            try
            {
                //提词表存入单词本数据库
                WordBook.StorageWordBook(WordBook.SetBooks(readerWordLists, type_mark));
                ShowToastNotification("exReader提示", "成功导入到我的生词本!");
            }
            catch
            {
                ShowToastNotification("exReader提示", "导入失败！提词列表为空");
            }
            /*
            if (readerWordLists.Count != 0 || readerWordLists == null) ShowToastNotification("exReader提示", "成功导入到我的生词本!");
            else ShowToastNotification("exReader提示", "导入失败！提词列表为空");
            */
            
        }

        //获得从 #文章页面# 传来的 Passage 对象
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Passage)
            {
                Passage choose_passage = (Passage)e.Parameter;

                Debug.WriteLine(choose_passage.HeadName);

            }
        }

        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }

        private void PrintList(ObservableCollection<Vocabulary> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    Debug.WriteLine(item.Word);
                }
            }
            else
            {
                Debug.WriteLine("list is empty!");
            }
        }
    }
} 
