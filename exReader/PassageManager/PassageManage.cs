using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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

        

    }

    public class PassageManage
    {

        public Passage GetPassage()
        {
            Passage p = new Passage();
            p.Content = "Donald Trump has pulled back from a potential trade war with Europe by postponing a decision on imposing steel and aluminum tariffs until 1 June.The US president imposed a worldwide 25 % tariff on steel imports and a 10 % tariff on aluminum in March but granted temporary exemptions to Canada, Mexico, Brazil, the European Union(EU), Australia and Argentina.These were due to expire at 12.01am on Tuesday.The extension offers temporary reprieve for French president Emmanuel Macron and German chancellor Angela Merkel, who lobbied Trump during visits to the White House last week.It could also be seen by political analysts as the latest issue on which Trump’s bark has proved worse than his bite.The administration “reached agreements in principle with Argentina, Australia, and Brazil with respect to steel and aluminum, the details of which will be finalized shortly”, the White House said on Monday. “The Administration is also extending negotiations with Canada, Mexico, and the European Union for a final 30 days. ";
            p.HeadName = "New Great frfre efref erfre fer American";
            return p;

        }
    }
}
