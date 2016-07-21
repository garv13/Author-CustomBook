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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StreamerAuthor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyBook : Page
    {
        private IMobileServiceTable<Author> Table2 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items2;
        private IMobileServiceTable<Book> Table = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items;
        private IMobileServiceTable<Chapter> Table3 = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items3;
        List<Dashboard> dl = new List<Dashboard>();
        string testlol2;
        public MyBook()
        {
            this.InitializeComponent();

             dl = new List<Dashboard>();
         
            Loaded += MyBook_Loaded;
        }

        private async void MyBook_Loaded(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            Loading.IsIndeterminate = true;
            try
            {
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await folder.GetFileAsync("sample.txt");

                string testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                //we have publisher name
                StorageFolder folder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile2 = await folder.GetFileAsync("sample2.txt");

                testlol2 = await Windows.Storage.FileIO.ReadTextAsync(sampleFile2);
                //we have id
                await loadDashboard(0);
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Sorry can't update now");
                await msgbox.ShowAsync();
                Loading.Visibility = Visibility.Collapsed;
            }
        }

        private async Task loadDashboard(int i)
        {
            items2 = await Table2.Where(Author
                               => Author.Id == testlol2).ToCollectionAsync();
            Author a = items2[0];
            //string[] b = a.books.Split(',');
            items = await Table.Skip(i*15).Take(15).Where(Book
                        => Book.PublisherId == a.Id).ToCollectionAsync();
            Dashboard temp;
            foreach (Book lol in items)
            {
                //if (lol.Id == "A2592E4D-8663-4A2D-869B-CCDE2FB2A039")
                //{ }
                //else
                //{
                temp = new Dashboard();
                int downloads = 0;
                items3 = await Table3.Where(Chapter
                            => Chapter.bookid == lol.Id).ToCollectionAsync();
                foreach (Chapter lol2 in items3)
                {
                    downloads += lol2.downloads;
                }
                temp.downloads = downloads.ToString();
                temp.title = lol.Title;
                temp.Id = lol.Id;
                if (lol.IsReady)
                    temp.status = "In Store";
                else
                    temp.status = "Processing";


                BitmapImage im = new BitmapImage();
                im.DecodePixelHeight = 300;
                im.DecodePixelWidth = 200;
                im.UriSource = new Uri(lol.ImageUri2);
                im.DecodePixelHeight = 300;
                im.DecodePixelWidth = 200;
                temp.image = im;

                dl.Add(temp);
            }
            //}
            if (dl.Count != 0)
            {

                event3.ItemsSource = dl;
                Loading.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageDialog msgbox = new MessageDialog("No Book Published");
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

        private void event3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Book send = new Book();
            Dashboard sel = e.ClickedItem as Dashboard;
            string id = sel.Id;
            foreach (Book lol2 in items)
            {
                if (lol2.Id == id)
                {
                    send = lol2;
                    break;
                }
            }
            Frame.Navigate(typeof(Update), send);
        }
    }
}
