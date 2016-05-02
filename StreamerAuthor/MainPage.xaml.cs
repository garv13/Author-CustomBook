using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StreamerAuthor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

   
    public sealed partial class MainPage : Page
    {
        private IMobileServiceTable<Author> Table2 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items2;
        ObservableCollection<LessonList> ls;
        Dictionary<string, string> li;
        Dictionary<string, string> li2;
        List<string> li3;
        Dictionary<string, string> li4;

        StorageFile media =null;
        StorageFile media2 = null;
        public MainPage()
        {
            this.InitializeComponent();
          ls = new ObservableCollection<LessonList>();
            li = new Dictionary<string, string>();
            li2 = new Dictionary<string, string>();
            li3 = new List<string>();
            li4 = new Dictionary<string, string>();
        }

        async Task<bool> checkFun()
        {
            int i = 1;

            if (!Regex.Match(BookName.Text, @"^[a-zA-Z0-9\s]+$").Success)
                i = 0;

            if (!(Price.Text.All(char.IsDigit) && Price.Text.Length != 0))
            {
                i = 0;
            }

            if (!Regex.Match(Desciption.Text, @"^[a-zA-Z0-9\s\.]+$").Success)
                i = 0;

            if (!Regex.Match(Genre.Text, @"^[a-zA-Z\s\.]+$").Success)
                i = 0;

            if (!Regex.Match(AutName.Text, @"^[a-zA-Z\s\.]+$").Success)
                i = 0;

            if (i == 0)
            {
                MessageDialog msgbox = new MessageDialog("Information incorrect");
                await msgbox.ShowAsync();
                Loading.Visibility = Visibility.Collapsed;
                return true;
            }
            if (media == null || media2 == null)
            {
                MessageDialog msgbox = new MessageDialog("Select Image And Pdf");
                await msgbox.ShowAsync();
                Loading.Visibility = Visibility.Collapsed;
                return true;
            }
            else
                return false;
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Loading.Visibility = Visibility.Visible;
                Loading.IsIndeterminate = true;
                submit1.Visibility = Visibility.Collapsed;
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await folder.GetFileAsync("sample.txt");

                string testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                //we have publisher name
                StorageFolder folder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile2 = await folder.GetFileAsync("sample2.txt");

                string testlol2 = await Windows.Storage.FileIO.ReadTextAsync(sampleFile2);
                //we have id
                if (await checkFun())
                {
                    submit1.Visibility = Visibility.Visible;
                    return;
                }
                items2 = await Table2.Where(Author
                                => Author.Id == testlol2).ToCollectionAsync();
                Author a = items2[0];
                Book item = new Book();
                item.Title = BookName.Text;
                item.Description = Desciption.Text;
                item.Price = int.Parse(Price.Text);
                item.Genre = Genre.Text;
                item.Author = AutName.Text;
                item.Publisher = testlol;
                item.Chapters = int.Parse(NoOfLessons.Text);
                item.IsReady = false;


                item.ResourceName = item.Title + Guid.NewGuid().ToString();
                item.ResourceName2 = item.Title + Guid.NewGuid().ToString();
                item.PageNo = "";
                item.ChapterCost = "";
                item.ChapterName = "";

                foreach (string val in li3)
                {
                    item.PageNo += li[val];
                    item.ChapterCost += li2[val];
                    item.ChapterName += li4[val];
                    item.PageNo += ",";
                    item.ChapterCost += ",";
                    item.ChapterName += ",";

                }
                item.ChapterCost = item.ChapterCost.Substring(0, (item.ChapterCost.Length - 1));
                item.PageNo = item.PageNo.Substring(0, (item.PageNo.Length - 1));
                item.ChapterName = item.ChapterName.Substring(0, (item.ChapterName.Length - 1));



                var credentials = new StorageCredentials("ebookstreamer2", "XJ+r94AJ31aNTKfuImglzEbelP20fSESTxH1Z3SSiMLetYsRWtVrkDsDBuEgPQAiWgTdkkpfu0m4eSHbm7w/KA==");
                var client = new CloudBlobClient(new Uri("https://ebookstreamer2.blob.core.windows.net/"), credentials);
                var container = client.GetContainerReference("coverpage");
                await container.CreateIfNotExistsAsync();

                var perm = new BlobContainerPermissions();
                perm.PublicAccess = BlobContainerPublicAccessType.Blob;
                await container.SetPermissionsAsync(perm);
                var blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
                using (var fileStream = await media.OpenSequentialReadAsync())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }


                var container2 = client.GetContainerReference("books");
                await container2.CreateIfNotExistsAsync();
                var perm2 = new BlobContainerPermissions();
                perm2.PublicAccess = BlobContainerPublicAccessType.Blob;
                await container2.SetPermissionsAsync(perm2);
                var blockBlob2 = container2.GetBlockBlobReference(Guid.NewGuid().ToString() + ".pdf");
                using (var fileStream = await media2.OpenStreamForReadAsync())
                {
                    await blockBlob2.UploadFromStreamAsync(fileStream.AsInputStream());
                }

                item.ImageUri = blockBlob2.StorageUri.PrimaryUri.ToString();

                item.ImageUri2 = blockBlob.StorageUri.PrimaryUri.ToString();
                await App.MobileService.GetTable<Book>().InsertAsync(item);
                a.books += item.Id + ",";
                await Table2.UpdateAsync(a);
                Loading.Visibility = Visibility.Collapsed;
                MessageDialog mess = new MessageDialog("Your Book Has been uploaded successfully!");
                await mess.ShowAsync();
                Frame.Navigate(typeof(MyBook));
            }
            catch(Exception)
            {
                Loading.Visibility = Visibility.Collapsed;
                MessageDialog mess = new MessageDialog("Your Book Has not been uploaded Now :(:(");
                await mess.ShowAsync();
            }
        }

        public List<Control> AllChildren(DependencyObject test2)
        {
            var list = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(test2); i++)
            {
                var child = VisualTreeHelper.GetChild(test2, i);
                if (child is Control)
                {
                    list.Add(child as Control);
                    list.AddRange(AllChildren(child));

                }

            }
            return list;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");

                media = await openPicker.PickSingleFileAsync();
                //img.Content = media.Name;
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Image not Selected");
                await msgbox.ShowAsync();
                return;
            }
    
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

        private void No_OfLessons_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            LessonList ob = new LessonList();
            ls = new ObservableCollection<LessonList>();
            li = new Dictionary<string, string>();
            li2 = new Dictionary<string, string>();
            li4 = new Dictionary<string, string>();
            li3 = new List<string>();
            try
            {

                int v = int.Parse(NoOfLessons.Text);
                for (int i = 0; i < v; i++)
                {
                    ob = new LessonList();
                    ob.lesson = "Chapter No.:" + (i + 1).ToString();
                    li.Add(ob.lesson, "");
                    li2.Add(ob.lesson, "");
                    li4.Add(ob.lesson, "");
                    li3.Insert(i, ob.lesson);                           
                    ls.Add(ob);
                }
                event1.ItemsSource = ls;
                event1.Visibility = Visibility.Visible;
                

            }
            catch(Exception)
            {
                event1.Visibility = Visibility.Collapsed;
            }
        }

        private void PageNo_LostFocus(object sender, RoutedEventArgs e)
        {
            var hell = sender as TextBox;
            var hell2 = hell.Parent as Grid;
            var hell3 = hell2.Children;
            var hell4 = (TextBlock)hell3[0];
            string name = hell4.Text;
            li[name] = hell.Text;
        }

        private void Price_LostFocus(object sender, RoutedEventArgs e)
        {
            var hell = sender as TextBox;
            var hell2 = hell.Parent as Grid;
            var hell3 = hell2.Children;
            var hell4 = (TextBlock)hell3[0];
            string name = hell4.Text;
            li2[name] = hell.Text;
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

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            checkBox.Visibility = Visibility.Collapsed;
            BoxPrice.Visibility = Visibility.Visible;

        }

        private void BoxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            LessonList ob = new LessonList();

            try
            {
                foreach (LessonList l in ls)
                {
                    l.price = BoxPrice.Text;
                }
                event1.ItemsSource = null;
                event1.Visibility = Visibility.Collapsed;
                event1.ItemsSource = ls;
                event1.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Wallet));
        }

        private void ChapterName_LostFocus(object sender, RoutedEventArgs e)
        {
            var hell = sender as TextBox;
            var hell2 = hell.Parent as Grid;
            var hell3 = hell2.Children;
            var hell4 = (TextBlock)hell3[0];
            string name = hell4.Text;
            li4[name] = hell.Text;
        }
    }
}
