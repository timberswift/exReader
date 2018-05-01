﻿using System;
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
       

    }
}
