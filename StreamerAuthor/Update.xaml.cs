using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class Update : Page
    {
        Book rec;
        private List<ChapterView> list;
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;

        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        private int ind;
        StorageFile media2 = null;
        public Update()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadingBar.IsIndeterminate = true;
            LoadingBar.Visibility = Visibility.Visible;
            rec = new Book();
            rec = e.Parameter as Book;
            Title.Text = rec.Title;
            BitmapImage bit = new BitmapImage(new Uri(rec.ImageUri2));
            DescBlock.Text = rec.Description;
            Cover.Source = bit;
            FullCost.Text = rec.Price.ToString();
            Author.Text = rec.Author;
            FullCost.Text = "Full Book Price: " + rec.Price;
            try
            {
                

                list = new List<ChapterView>();
                ChapterView temp;
                try
                {
                    items = await Table.Where(Chapter
                                => Chapter.bookid == rec.Id).ToCollectionAsync();

                    foreach (Chapter lol in items)
                    {
                        temp = new ChapterView();
                        temp.Id = lol.Id;
                        temp.Title = lol.Name;
                        temp.Price = "Price: " + lol.price.ToString();
                        ind = lol.sno + 1;
                        list.Add(temp);
                    }
                    ind++;
                    LoadingBar.Visibility = Visibility.Collapsed;
                   // LessonNo.Text = "Chapter No: " + ind.ToString();
                    ind--;
                    StoreListView.ItemsSource = list;
                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog mess = new Windows.UI.Popups.MessageDialog("Sorry Can't load the chapters now :(:(");
                    await mess.ShowAsync();
                }
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right try later");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewCost.Text == "")
            { }
            else
            {
                try
                {
                   
                    int temp = int.Parse(NewCost.Text);
                    rec.Price = temp;
                   await Table2.UpdateAsync(rec);
                    Frame.Navigate(typeof(Update), rec);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            var test = sender as Button;
            var test2 = test.Parent as Grid;
            var test3 = test2.Children[4] as TextBlock;
            var test4 = test2.Children[2] as TextBox;
            Chapter temp = new Chapter();
            foreach (Chapter lol in items)
            {
                if (lol.Id == test3.Text)
                {
                    temp = lol;
                    break;
                }   
            }
            temp.price = int.Parse(test4.Text);
            await Table.UpdateAsync(temp);
            Frame.Navigate(typeof(Update), rec);
        }

        private void PageNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Price_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private async void pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".pdf");
                media2 = await openPicker.PickSingleFileAsync();
                // about2 = await Windows.Storage.FileIO.ReadTextAsync(media);
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Pdf not Selected");
                await msgbox.ShowAsync();
                return;
            }
        }

         async void submit_Click(object sender, RoutedEventArgs e)
        {
            Chapter temp = new Chapter();
            var credentials = new StorageCredentials("ebookstreamer2", "XJ+r94AJ31aNTKfuImglzEbelP20fSESTxH1Z3SSiMLetYsRWtVrkDsDBuEgPQAiWgTdkkpfu0m4eSHbm7w/KA==");
            var client = new CloudBlobClient(new Uri("https://ebookstreamer2.blob.core.windows.net/"), credentials);
            var container = client.GetContainerReference("chapters");
            await container.CreateIfNotExistsAsync();

            var perm = new BlobContainerPermissions();
            perm.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(perm);
            var blockBlob = container.GetBlockBlobReference(rec.Id+ind.ToString() + ".pdf");
            using (var fileStream = await media2.OpenSequentialReadAsync())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            temp.uri = blockBlob.StorageUri.PrimaryUri.ToString();
            temp.sno = ind;
            temp.price = int.Parse(Price.Text);
            temp.downloads = 0;
            temp.bookid = rec.Id;
            temp.Name = LessonNo.Text;
            await App.MobileService.GetTable<Chapter>().InsertAsync(temp);
            Frame.Navigate(typeof(Update), rec);

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rec.Description = DescBlock.Text;
            await Table2.UpdateAsync(rec);
            Frame.Navigate(typeof(Update), rec);
        }
    }
}
