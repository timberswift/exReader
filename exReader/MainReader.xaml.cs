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
        private FileManage fileManage = new FileManage();
        private ObservableCollection<Vocabulary> readerWordLists;   //提词列表ListView绑定的数据

        ObservableCollection<FontFamily> fonts = new ObservableCollection<FontFamily>();
        public MainReader()
        {
            this.InitializeComponent();
            fonts.Add(new FontFamily("Arial"));
            fonts.Add(new FontFamily("Courier New"));
            fonts.Add(new FontFamily("Times New Roman"));
            readerWordLists = new ObservableCollection<Vocabulary>(reader.readerWordLists);
        }
        private void words_view_ItemClick(object sender, ItemClickEventArgs e)
        {

        }


        private void getReaderList()
        {
            readerWordLists = new ObservableCollection<Vocabulary>(reader.readerWordLists);
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void export_file_Click(object sender, RoutedEventArgs e)
        {
            fileManage.SerializeFile(reader);
            
        }

        private  void save_file_Click(object sender, RoutedEventArgs e)
        {
            //reader = await fileManage.DeSerializeFile();
        }

        private async void open_file_Click(object sender, RoutedEventArgs e)
        {
            
            reader = await fileManage.DeSerializeFile();
            if(reader!=null) UpdateBindingData(reader.readerWordLists);
            //MainPage.MyNavigationView.Header = "Header of this passage";
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

        public void UpdateBindingData(ObservableCollection<Vocabulary> newlists)
        {
            readerWordLists.Clear();
            foreach (var item in newlists)
            {
                readerWordLists.Add(item);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Passage choose_passage = (Passage)e.Parameter;


           // Debug.WriteLine(choose_passage.HeadName);
            //editor.Document.Add(new Paragraph(new Run(parameters.Content)));


            // parameters.Name
            // parameters.Text
            // ...
        }
   
    }
} 
