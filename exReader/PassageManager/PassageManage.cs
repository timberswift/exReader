using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using exReader.DatabaseManager;
using System.Diagnostics;
using exReader.WordsManager;
using System.Collections.ObjectModel;

namespace exReader.PassageManager
{
    //侯平月工作空间
    //本空间下实现 Passage类的数据定义封装、页面风格实现、高亮信息收集
    //附操作 MainReader.xaml.cs

    [DataContract]
    public class Passage
    {
        private string headName;
        private string content;
        [DataMember]
        public string HeadName { get { return headName; } set { headName = value; } }
        [DataMember]
        public string Content { get { return content; } set { content = value; } }     //存储文本内容
        // [DataMember]
        //public Dictionary<int, Tuple<string, string>> HightLightInfo { get; set; }  //Very Important!! 存储高亮信息，字典键值对为 <位置(int)，<单词(string)，颜色(string)
        //other
        private List<Tuple<int, int>> highLightInfo = new List<Tuple<int, int>>();

        public List<Tuple<int, int>> HighLightInfo
        {
            get { return highLightInfo; }
            set { highLightInfo = value; }
        }





    }

    public class PassageManage
    {
        private static List<Passage> historyPassages = new List<Passage>();
        public static List<Passage> HistoryPassages
        {
            get
            {  return LoadPassages(); }
            set
            {  historyPassages = value; }
        }

        //保存文章到数据库
        public static void SavePassage(Passage passage)
        {
            List<Passage> database_passages = new List<Passage>();
            database_passages =  UserDataDB.instance.LoadPassage();   
            
           foreach(var i in database_passages)
            {
                Debug.WriteLine(i.Content+"\n");
            }
            historyPassages = database_passages;
            int index = historyPassages.IndexOf(historyPassages.Where(x => x.HeadName == passage.HeadName).FirstOrDefault());
            if (index < 0)
            {

                 historyPassages.Add(passage);
                 UserDataDB.instance.SavaPassage(passage); //文章存入数据库
            }
            else
            {
                historyPassages.RemoveAt(index);
                historyPassages.Add(passage);
                UserDataDB.instance.SavaPassage(passage); //文章存入数据库
            }

        }

        //加载历史文章
        public static List<Passage> LoadPassages()
        {
            return UserDataDB.instance.LoadPassage();
        }

        //清空历史文章
        public static void ClearPassages()
        {
            List<Passage> passages = UserDataDB.instance.LoadPassage();
            foreach(var p in passages)
            {
                UserDataDB.instance.DeletePassage(p.HeadName);
            }
        }

        //删除文章
        public static void DeletePassage(Passage p)
        {
            try { UserDataDB.instance.DeletePassage(p.HeadName); }
            catch { }
        }

        //获取初始文章
        public  Passage GetPassage()
        {
            Passage p = new Passage();
            //p.Content = "Donald Trump has pulled back from a potential trade war with Europe by postponing a decision on imposing steel and aluminum tariffs until 1 June.The US president imposed a worldwide 25 % tariff on steel imports and a 10 % tariff on aluminum in March but granted temporary exemptions to Canada, Mexico, Brazil, the European Union(EU), Australia and Argentina.These were due to expire at 12.01am on Tuesday.The extension offers temporary reprieve for French president Emmanuel Macron and German chancellor Angela Merkel, who lobbied Trump during visits to the White House last week.It could also be seen by political analysts as the latest issue on which Trump’s bark has proved worse than his bite.The administration “reached agreements in principle with Argentina, Australia, and Brazil with respect to steel and aluminum, the details of which will be finalized shortly”, the White House said on Monday. “The Administration is also extending negotiations with Canada, Mexico, and the European Union for a final 30 days. ";
            p.Content = "A London-based British Council employee has been arrested during a family visit to Iran, as the authorities in the country step up targeting people with ties to UK institutions.Aras Amiri, a 32 - cases are a worrying development for Nazanin Zaghari-Ratcliffe, the British woman who is serving a five - year jail sentence in Tehran on alleged spying charges and whose family insists she is being punished as a tool of diplomatic pressure.year - old Iranian national, was visiting her home country to see her ailing grandmother before the Persian new year, in March, when she was detained, said her cousin, Mohsen Omrani, on Wednesday.Amiri’s detention is the latest in a string of recent arrests involving British dual nationals or Iranians linked with British institutions. Last week it emerged that Abbas Edalat, a professor at Imperial College London, had been arrested in April by the hardline Revolutionary Guards.";

            p.HeadName = "New Great America";
            return p;
        }


        //获取单词高亮位置
        public static List<Tuple<int, int>> GetHighLightInfo(String content, ObservableCollection<Vocabulary> readerlist)
        {
            List<Tuple<int, int>> WordInfo = new List<Tuple<int, int>>();
            foreach(var a in readerlist)
            {
                int index = content.IndexOf(a.Word);
                Tuple<int, int> tuple = new Tuple<int, int>(index, a.Word.Length);
                WordInfo.Add(tuple);

            }
            return WordInfo;
        }

        
    }


}
