using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace exReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainReader : Page
    {
        ObservableCollection<FontFamily> fonts = new ObservableCollection<FontFamily>();
        public MainReader()
        {
            this.InitializeComponent();
            fonts.Add(new FontFamily("Arial"));
            fonts.Add(new FontFamily("Courier New"));
            fonts.Add(new FontFamily("Times New Roman"));
            this.InitializeComponent();
            this.InitializeComponent();
         
        }

        private void words_view_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (ListViewItem eachItem in words_view.SelectedItems)
            {
                words_view.Items.Remove(eachItem);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem eachItem in words_view.SelectedItems)
            {
                words_view.Items.Remove(eachItem);
            }
        }
    }
} 
