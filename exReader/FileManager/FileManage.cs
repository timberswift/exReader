using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using exReader.WordsManager;
using exReader.PassageManager;
using Windows.Storage;
using System.IO;
using System.Diagnostics;
using Windows.Storage.Pickers;

namespace exReader.FileManager
{
    //侯平月、汤浩、ljm、wyh工作空间
    //本类实现将 #工程文件# 涉及到的 #类数据# 打包与解包，实现序列化、反序列化，实现导入、导出工程文件 （自定义文件名 .xread）
    //附操作 MainReader.xaml.cs

    [DataContract]
    public class FileManage
    {
        [DataMember]
        public Passage passage { get; set; }
        public WordBook wordBook { get; set; }

      // DataContractSerializer serializer = new DataContractSerializer(typeof(Passage));


        public async void SerializeFile(string fileName,Passage passage)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(FileManage));
            StorageFolder storageFolder = KnownFolders.MusicLibrary;
            StorageFile file1 = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            var stream = await file1.OpenStreamForWriteAsync();
            Debug.WriteLine("write stream: " + stream.ToString());
            serializer.WriteObject(stream, passage);

        }

        public async void DeSerializeFile()
        {
            DataContractSerializer deserializer = new DataContractSerializer(typeof(FileManage));
            Passage ps;

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".xread");

            StorageFile storageFile = await picker.PickSingleFileAsync();
            var stream = await storageFile.OpenStreamForReadAsync();
            ps = deserializer.ReadObject(stream) as Passage;

            this.passage = ps;
        }
       
      

    }
}
