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
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

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
using Windows.UI.Popups;
using System.Runtime.InteropServices;

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
        private bool toggle_state = false;
        private int i = 1;
        //private RichEditBox
        private ObservableCollection<Vocabulary> readerWordLists;// =  new ObservableCollection<Vocabulary>();   //提词列表ListView绑定的数据
                                                                 //
        //public ObservableCollection<String> fonts = new ObservableCollection<String>();


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
          

        }

        


        public MainReader()
        {

            //editor.PlaceholderText = "This is some sample text";
            //editor.Document.Selection.CharacterFormat.Size = 15;
            //editor.Document.SetText(options: Windows.UI.Text.TextSetOptions.None, value: "This is some sample text");
            //SetLineSpace(editor, 300);
            this.InitializeComponent();


        

            //fonts.Add(new FontFamily("Oblique"));
            //fonts.Add(new FontFamily("Courier New");
            //fonts.Add(new FontFamily("Courier New"));
            //fonts.Add(new FontFamily("Times New Roman"));
            //fonts.Add(new FontFamily("Segoe UI"));
            //fonts.Add(new FontFamily("Comic Sans MS"));


            reader = new ReaderManage();
            fileManage = new FileManage();
            readerWordLists = new ObservableCollection<Vocabulary>();   
            UpdateBindingData(reader.ReaderWordLists, reader.ReaderChooseMode);

            bindHeadName = "Passage header";
            
            initReader();

            //ITextParagraphFormat textFormat = null;
            //ITextParagraphFormat textFormat;
            //textFormat= ITextParagraphFormat.ReferenceEquals;
            //editor.Document.SetDefaultParagraphFormat(textFormat);

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
                editor.Document.Selection.SetText(TextSetOptions.FormatRtf, reader.ReaderPassage.Content);
                int type = CacheReaderManage.CacheReader.ReaderChooseMode;
                if (CacheReaderManage.CacheReader.ReaderChooseMode != 0)
                {
                    SearchWordList(GetStringType(type), type);
                }
            }
    
        }

        //高亮切换
        private  void off_on_highlight_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    toggle_state = true;
                    try
                    {   
                        SetModeHighLight(reader.ReaderChooseMode);
                    }
                    catch { }
                   
                }
                else
                {
                    toggle_state = false;
                    try
                    {
                         SetHighLight(Colors.Transparent);
                    }
                    catch { }
                }
            }
        }


        //设置指定文本高亮
        private void SetHighLight(Windows.UI.Color color)
        {
            int count = 0;
            List<ITextSelection> Selections = new List<ITextSelection>();
            while (count < reader.ReaderPassage.HighLightInfo.Count)
            {
                ITextSelection selection = editor.Document.Selection;
                selection.StartPosition = reader.ReaderPassage.HighLightInfo[count].Item1;
                selection.EndPosition = selection.StartPosition + reader.ReaderPassage.HighLightInfo[count].Item2;
                selection.CharacterFormat.BackgroundColor = color;
                Selections.Add(selection);
                count++;
            }

        }

        //从提词表移除选定单词
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (toggle_state)
            {
                SetHighLight(Colors.Transparent);
            }
            Button button = sender as Button;
            Vocabulary item = button.DataContext as Vocabulary;
 
            int index = reader.ReaderWordLists.IndexOf(reader.ReaderWordLists.Where(x => x.Word == item.Word).FirstOrDefault());
            readerWordLists.Remove(item);
            reader.ReaderWordLists.RemoveAt(index);
            if (readerWordLists.Count == 0) reader_empty.Opacity = 1;  //列表已清除为空

            if (toggle_state)
            {
                SetModeHighLight(reader.ReaderChooseMode);
            }
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
            if(reader.ReaderPassage.HeadName == null)
            {
               string headname = await InputHeaderDialogAsync("输入文章标题:");
               if (headname == "") reader.ReaderPassage.HeadName = "Passage " + i++;
               else reader.ReaderPassage.HeadName = headname;
            }

            GetEditBoxText();  //从文本框获取文章内容

            if (reader.ReaderPassage.Content == null)
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
           // CacheReaderManage.CacheReader = reader;
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
            SetHighLight(Colors.Transparent);     
            GetEditBoxText();  //从文本框获取文章内容
            reader.MatchWords(type, t);
            UpdateBindingData(reader.ReaderWordLists, t);
            SetModeLabel(t);
            reader.ReaderPassage.HighLightInfo = PassageManage.GetHighLightInfo(reader.ReaderPassage.Content, readerWordLists);
            if (toggle_state == true)
            {
                SetModeHighLight(t);
            }
            CacheReaderManage.CacheReader = reader;
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


        //每次打开工程open file，刷新UI
        private void RefreshUI()
        {

            GetEditBoxText();  
            
            SetModeLabel(reader.ReaderChooseMode);    //设置选词模式Label
       
            SetEditBoxText();   //文本框设置文章Content

            //ShowHightLightInfo(Passage reader_passage);   //显示文章高亮信息

            UpdateBindingData(reader.ReaderWordLists, reader.ReaderChooseMode); //刷新提词绑定列表
  
            CacheReaderManage.CacheReader = reader;  //添加到缓存reader

        }

        //从文本框获取文章内容
        private void GetEditBoxText()
        {
            string value = string.Empty;
            editor.Document.GetText(TextGetOptions.AdjustCrlf, out value); 
            reader.ReaderPassage.Content = value;
        }

        //设置文本框的文章内容
        private void SetEditBoxText()
        {
           // editor.Document.Selection.SetText(TextSetOptions.None, null);
            editor.Document.Selection.SetText(TextSetOptions.FormatRtf, reader.ReaderPassage.Content);
        }

        //设置提词模式Label
        private void SetModeLabel(int type)
        {
            switch(type)
            {
                case 0: Mode_Lable.Text = "未选择";break;
                case 1: Mode_Lable.Text = "CET4"; break;
                case 2: Mode_Lable.Text = "CET6"; break;
                case 3: Mode_Lable.Text = "考研"; break;
                case 4: Mode_Lable.Text = "托福TOEFL"; break;
                case 5: Mode_Lable.Text = "雅思IELTS"; break;
                case 6: Mode_Lable.Text = "GRE"; break;
                default:break;
            }
        }
        //设置提词模式Label
        private void SetModeHighLight(int type)
        {
            switch (type)
            {     
                case 1: SetHighLight(Colors.Orange);  break;
                case 2: SetHighLight(Colors.DarkOrange); break;
                case 3: SetHighLight(Colors.SkyBlue); break;
                case 4: SetHighLight(Colors.GreenYellow); break;
                case 5: SetHighLight(Colors.MediumSpringGreen); break;
                case 6: SetHighLight(Colors.DeepSkyBlue); break;
                default: break;
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

        //输入文章标题Dialog
        private async Task<string> InputHeaderDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
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


       

        private void fontStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if(selectedText != null)
            {
                //var family = fontStyle.SelectedItem;
                //string a = "Oblique";
                var family = fontStyle.SelectedItem.ToString();
                //Windows.UI.Text.ITextCharacterFormat charFormatting1 = selectedText.CharacterFormat;
                //if (fontStyle.SelectedItem.ToString() == a)
                //charFormatting1.FontStyle.ToString() = fontStyle.SelectedItem.ToString();
                // charFormatting1.FontStyle = Windows.UI.Text.FontStyle.
                selectedText.CharacterFormat.Name = family;



            }
            
        }

        //字体加粗
        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
            else
            {

            }
        }

        //字体斜体
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

        //字体加下划线
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

        //字体变小
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

        //字体变大
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
