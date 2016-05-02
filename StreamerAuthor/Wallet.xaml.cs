using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StreamerAuthor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Wallet : Page
    {
        private IMobileServiceTable<Author> Table2 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items2;
        public Wallet()
        {
            this.InitializeComponent();
            Loaded += Wallet_Loaded;
        }

        private async void Wallet_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Loading.Visibility = Visibility.Visible;
                Loading.IsIndeterminate = true;
                StorageFolder folder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile2 = await folder2.GetFileAsync("sample2.txt");

                string testlol2 = await Windows.Storage.FileIO.ReadTextAsync(sampleFile2);
                //we have id
                items2 = items2 = await Table2.Where(Author
                               => Author.Id == testlol2).ToCollectionAsync();
                balance.Text = items2[0].wallet.ToString();
                Loading.Visibility = Visibility.Collapsed;
            }
            catch(Exception)
            {

                MessageDialog msgbox = new MessageDialog("Sorry can't update now");
                await msgbox.ShowAsync();
                Loading.Visibility = Visibility.Collapsed;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyBook));
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }

        private void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Wallet));

        }
    }
}
