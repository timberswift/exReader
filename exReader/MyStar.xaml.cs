using System;
using System.Collections.Generic;
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
    public sealed partial class MyStar : Page
    {
        public MyStar()
        {
            this.InitializeComponent();
        }

        private async void guardianButton_Click(object sender, RoutedEventArgs e)
        {
           await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.theguardian.com"));
        }

        private async void meduimButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://medium.com"));

        }

        private async void timeButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://nytimes.com"));

        }

        private async void ecoButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.economistasia.com"));

        }

        private async void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.google.com"));

        }

        private async void search_button_Click(object sender, RoutedEventArgs e)
        {
            string search_string = search_field.Text;
            Uri uriResult;
            bool result = Uri.TryCreate(search_string, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            //如果是合法的链接
            if (result)
            {
                string str = search_string.Substring(0, 4);
                if (!string.Equals(str, "http"))
                {
                    search_string = "http://" + search_string;
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(search_string));
                }
                else
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(search_string));
                }
            }
            //否则输入的是关键字
            else
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.baidu.com/s?wd="+search_field.Text));
            }
           

        }
    }
}
