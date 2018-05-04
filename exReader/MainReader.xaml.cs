using exReader.DatabaseManager;
using exReader.FileManager;
using exReader.PassageManager;
using exReader.ReaderManager;
using exReader.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Text;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Text;
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
        private ReaderManage reader;
        private FileManage fileManage;// = new FileManage();
        private string bindHeadName;
        //private RichEditBox
        private ObservableCollection<Vocabulary> readerWordLists;// =  new ObservableCollection<Vocabulary>();   //提词列表ListView绑定的数据
        ObservableCollection<FontFamily> fonts = new ObservableCollection<FontFamily>();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        public MainReader()
        {
            //editor.PlaceholderText = "This is some sample text";
            //editor.Document.Selection.CharacterFormat.Size = 15;
            //editor.Document.SetText(options: Windows.UI.Text.TextSetOptions.None, value: "This is some sample text");
            this.InitializeComponent();

            fonts.Add(new FontFamily("Arial"));
            fonts.Add(new FontFamily("Courier New"));
            fonts.Add(new FontFamily("Times New Roman"));

            reader = new ReaderManage();
            fileManage = new FileManage();
            readerWordLists = new ObservableCollection<Vocabulary>();   
            UpdateBindingData(reader.ReaderWordLists, reader.ReaderChooseMode);

            bindHeadName = "Passage header";
            

            initReader();
            

        }

        

        private void docunment_operat()
        {
            string str;
            editor.Document.Selection.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out str);

            byte[] bytes = Encoding.Unicode.GetBytes(str);
            string str2 = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            editor.Document.Selection.SetText(TextSetOptions.FormatRtf, str2);
        }

        private void words_view_ItemClick(object sender, ItemClickEventArgs e)
        {

        }


        private void initReader()
        {

            //暂时文章从

            if (CacheReaderManage.CacheReader == null)
            {
                reader.ReaderPassage = new PassageManage().GetPassage();
                reader.ReaderChooseMode = 0;

            }
            else
            {
                reader = CacheReaderManage.CacheReader;
                int type = CacheReaderManage.CacheReader.ReaderChooseMode;
                if (CacheReaderManage.CacheReader.ReaderChooseMode != 0)
                {
                    SearchWordList(GetStringType(type), type);
                }
            }

            editor.Document.Selection.SetText(TextSetOptions.FormatRtf, reader.ReaderPassage.Content);

        }

        //高亮切换
        private void off_on_highlight_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    reader_empty.Opacity = 1;
                }
                else
                {
                    reader_empty.Opacity = 0;
                }
            }
        }

        //设置指定文本高亮
        private void SetHighLight (string text, Windows.UI.Color color)
        {
            editor.Document.GetText(TextGetOptions.None, out text);
            var editorLengh = text.Length;
            editor.Document.Selection.SetRange(0, editorLengh);
            int i = 1;
            while(i >0)
            {
                i = editor.Document.Selection.FindText(text, editorLengh, FindOptions.None);
                ITextSelection seoectedText = editor.Document.Selection;
                if(seoectedText !=null)
                {
                    seoectedText.CharacterFormat.BackgroundColor = color;
                }
            }
        }


        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Vocabulary item = button.DataContext as Vocabulary;
 
            int index = reader.ReaderWordLists.IndexOf(reader.ReaderWordLists.Where(x => x.Word == item.Word).FirstOrDefault());

            //   result.YesorNo = 1;
            readerWordLists.Remove(item);
            reader.ReaderWordLists.RemoveAt(index);
            if (readerWordLists.Count == 0) reader_empty.Opacity = 1;  //列表已清除为空

        }

        private void export_file_Click(object sender, RoutedEventArgs e)
        {
            FileManage fileManage = new FileManage();

            fileManage.SerializeFile(reader);
            ShowToastNotification("exReader提示", "成功导出工程文件!");
        }

        private async void save_file_Click(object sender, RoutedEventArgs e)
        {
            //保存当前editor文本到readerPassage的Content
           // reader.ReaderPassage.Content = editor.Document.Selection.GetText(TextGetOptions.FormatRtf,*);

            if (reader.ReaderPassage == null)
            {
                var dialog = new ContentDialog()
                {
                    Title = "exReader提示",
                    Content = "没有可保存的文章！",
                    PrimaryButtonText = "确定",
                    // SecondaryButtonText = "取消",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            else
            {
                PassageManage.SavePassage(reader.ReaderPassage);
                var dialog = new ContentDialog()
                {
                    Title = "exReader提示",
                    Content = "成功保存到阅读历史记录！",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
          
        }

        private async void open_file_Click(object sender, RoutedEventArgs e)
        {

            reader = await fileManage.DeSerializeFile();
            if (reader != null)
            {
                RefreshUI();
                ShowToastNotification("exReader提示", "已打开文章");
            }
            
        }

     
       
        private void cet4_button_Click(object sender, RoutedEventArgs e)
        {
            SearchWordList("cet4",1);
        }

        private void cet6_button_Click(object sender, RoutedEventArgs e)
        {
          
            SearchWordList("cet6",2);
        }

        private void kaoyan_button_Click(object sender, RoutedEventArgs e)
        {
          
            SearchWordList("ky",3);
        }

        private void toefl_button_Click(object sender, RoutedEventArgs e)
        {
           
            SearchWordList("toefl",4);
        }

        private void ielts_button_Click(object sender, RoutedEventArgs e)
        {
           
            SearchWordList("ielts", 5);
        }

        private void gre_button_Click(object sender, RoutedEventArgs e)
        {
            
            SearchWordList("gre",6);
        }

        //根据按钮选择生成提词列表
        private void SearchWordList(string type,int t)
        {
            reader.MatchWords(type, t);
            UpdateBindingData(reader.ReaderWordLists, t);
            SetModeLabel(t);
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
            if(readerWordLists.Count != 0)
            {
                //提词表存入单词本数据库
                WordBook.StorageWordBook(WordBook.SetBooks(readerWordLists, type_mark));
                ShowToastNotification("exReader提示", "成功导入到我的生词本!");
            }
            else
            {
                ShowToastNotification("exReader提示", "导入失败！提词列表为空");
            }
            
        }

        //获得从 #文章页面# 传来的 Passage 对象
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Passage)
            {
                Passage choose_passage = (Passage)e.Parameter;
                reader.ReaderPassage = choose_passage;
                editor.Document.Selection.SetText(TextSetOptions.FormatRtf, choose_passage.Content);
               // editor.Header = choose_passage.HeadName;
            }
        }

        //离开Reader页面之前，缓存当前reader信息
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CacheReaderManage.CacheReader = reader;
        }

        //每次打开工程open file，刷新UI
        private void RefreshUI()
        {
            // reader.ReaderPassage.Content = editor.Document.GetText(TextGetOptions.None, out text);

            //设置选词模式Label
            SetModeLabel(reader.ReaderChooseMode);

            //文本框设置文章Content
            editor.Document.Selection.SetText(TextSetOptions.FormatRtf, reader.ReaderPassage.Content);

            //显示文章高亮信息
           //ShowHightLightInfo(Passage reader_passage);   

            //刷新提词绑定列表
            UpdateBindingData(reader.ReaderWordLists, reader.ReaderChooseMode);

            //添加到缓存reader
            CacheReaderManage.CacheReader = reader;

        }

        //设置提词模式Label
        private void SetModeLabel(int type)
        {
            switch(type)
            {
                case 0: Mode_Lable.Text = "未选择"; break;
                case 1: Mode_Lable.Text = "CET4"; break;
                case 2: Mode_Lable.Text = "CET6"; break;
                case 3: Mode_Lable.Text = "考研"; break;
                case 4: Mode_Lable.Text = "托福TOEFL"; break;
                case 5: Mode_Lable.Text = "雅思IELTS"; break;
                case 6: Mode_Lable.Text = "GRE"; break;
                default:break;
            }
        }

        private string GetStringType(int type)
        {
            switch (type)
            {                
                case 1: return "cet4";
                case 2: return "cet6";
                case 3: return "ky";
                case 4: return "toefl";
                case 5: return "ielts";
                case 6: return "gre";
                default: return null;
            }
        }

        //显示Toast通知
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


        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void text_smaller_button_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                try
                {
                   
                    if (charFormatting.Size >= 7)
                    {
                        //selectedText.CharacterFormat.BackgroundColor = Colors.Blue;
                        charFormatting.Size--;
                        selectedText.CharacterFormat = charFormatting;
                    }
                }
                catch (Exception)
                {
                    selectedText.CharacterFormat.BackgroundColor = Colors.Red;
                }
            }else
            {
                if (editor.FontSize >= 7)
                    editor.FontSize--;
            }
        }

        private void text_bigger_button_Click(object sender, RoutedEventArgs e)
        {
            //ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                try
                {
                    //charFormatting.BackgroundColor = colors[0];
                    //var color = colors[1];
                    //selectedText.CharacterFormat.BackgroundColor = color;
                    //ITextSelection selectedText = reb.Document.Selection;
                    if (charFormatting.Size <= 30)
                    {
                        //selectedText.CharacterFormat.BackgroundColor = Colors.Blue;
                        charFormatting.Size++;
                        selectedText.CharacterFormat = charFormatting;
                    }
                }
                catch (Exception)
                {
                    selectedText.CharacterFormat.BackgroundColor = Colors.Red;
                }
                
            }else
            {
                try
                {
                    if (editor.FontSize <= 30)
                    {
                        //selectedText.CharacterFormat.BackgroundColor = Colors.Blue;
                        editor.FontSize++;
                    }
                }
                catch (Exception)
                {
                    //editor.Background = Colors.Red;
                }
            }
        }


       
    }
} 
