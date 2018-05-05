﻿using System;
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
using Windows.UI.Notifications;

namespace exReader.FileManager
{
    //汤浩工作空间
    //本类实现将 #工程文件# 涉及到的 #类数据# 打包与解包，实现序列化、反序列化，实现导入、导出工程文件 （自定义文件名 .xread）
    //附操作 MainReader.xaml.cs

    public class FileManage
    {
    
        //序列化
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

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                ShowToastNotification("exReader提示", "成功导出工程文件!");
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

        //反序列化
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




    }
}
