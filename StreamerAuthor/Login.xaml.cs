using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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
    public sealed partial class Login : Page
    {
        private IMobileServiceTable<Author> Table = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items;
        public Login()
        {
            this.InitializeComponent();
        }

        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await lol();
        }

        private async Task lol()
        {
            try
            {
                Loading.Visibility = Visibility.Visible;
                Loading.IsIndeterminate = true;
                items = await Table.Where(Author
                               => Author.username == UserName.Text).ToCollectionAsync();
                if (items.Count != 0)
                {
                    if (Password.Password == items[0].password)
                    {
                        MessageDialog msgbox = new MessageDialog("Welcome " + UserName.Text);
                        await msgbox.ShowAsync();
                        StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                        StorageFile sampleFile =
                            await folder.CreateFileAsync("sample.txt", CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(sampleFile, items[0].publishername);
                        StorageFolder folder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
                        StorageFile sampleFile2 =
                            await folder.CreateFileAsync("sample2.txt", CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(sampleFile2, items[0].Id);
                        Loading.Visibility = Visibility.Collapsed;
                        Frame.Navigate(typeof(MyBook));
                    }
                }
                else
                {
                    MessageDialog msgbox = new MessageDialog("Password or username incorrect");
                    Loading.Visibility = Visibility.Collapsed;
                    await msgbox.ShowAsync();
                }
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not correct please try again later :(:(");
                Loading.Visibility = Visibility.Collapsed;
                await msgbox.ShowAsync();
            }
        }

        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }

        private async void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
              //  await lol();
            }
        }
    }
}
