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
using exReader.ReaderManager;

namespace exReader.FileManager
{
    //侯平月、汤浩、ljm、wyh工作空间
    //本类实现将 #工程文件# 涉及到的 #类数据# 打包与解包，实现序列化、反序列化，实现导入、导出工程文件 （自定义文件名 .xread）
    //附操作 MainReader.xaml.cs

    public class FileManage
    {
    
        //public Passage passage { get; set; }
        //public WordBook wordBook { get; set; }

      // DataContractSerializer serializer = new DataContractSerializer(typeof(Passage));


        public async void SerializeFile(ReaderManage reader)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ReaderManage));
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("exReader文件", new List<string>() { ".xread" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "xPassage";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                var stream = await file.OpenStreamForWriteAsync();
                Debug.WriteLine("write stream: " + stream.ToString());
                serializer.WriteObject(stream, reader);

               // await FileIO.WriteTextAsync(file, file.Name);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //.textBlock.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    //this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }      

        }

        public async Task<ReaderManage> DeSerializeFile()
        {
            DataContractSerializer deserializer = new DataContractSerializer(typeof(ReaderManage));
            ReaderManage reader;

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".xread");

            StorageFile storageFile = await picker.PickSingleFileAsync();
            if(storageFile != null)
            {
                var stream = await storageFile.OpenStreamForReadAsync();
                reader = deserializer.ReadObject(stream) as ReaderManage;

                return reader;
            }
            else
            {
                
                return null;
            }
   
        }
       
      

    }
}
