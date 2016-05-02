using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
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
    public sealed partial class About : Page
    {

        private IMobileServiceTable<Author> Table2 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items2;
        public About()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            string intro1 = "Streamer is a concept which aims to innovate the way the eBooks are distributed and published. The basic concept is evolved from the fact that in many a cases, especially during studying, student end up needing a lot of books to cover a single subject. Streamer wishes to solve this problem by providing chapter wise pricing of books.";
            string intro2 = "Until now, no product in market has leveraged the power of eBooks over physical copies; the eBooks can be split or merged easily and hence can be sold in pieces. Streamer aims to do just that. Apart from widespread academic application streamer as a concept can also be used in main streamer literature books. New authors can use streamer as a platform to promote their works; by selling first few parts for free they can capture a reader’s attention, who in turn can buy the whole book if they like the same.";
            string intro3 = "Author on his end has to do no extra work. He or she must upload a single pdf file and, along with other details, must mention the end number of each chapter as well as their pricing.Streamer comprises of two applications.One is for author which has tools like dash board, upload forms etc.which are meant for author and publishers.The other app is intended for general public user through which a user can download the eBooks and read them.";
            string intro4 = "The eBooks downloaded are stored in isolated storage of user and that too in byte form. So it cannot be shared by one user to another and hence it also deals with issues of piracy surrounding eBook distribution. Since all the data is cloud synchronized a user can enjoy their purchases across multiple devices using his or her account.";
            string intro5 = "Both the apps are UWP apps and made completely using XAML and C#. The app uses Azure mobile service for maintaining its backend apart from web app written in python. The app also relies on blob storage, schedulers and SQL database provided by azure.";
            string intro6 = "On each transaction 20% of cost will be kept by us as fee. There will be no upfront cost to register.Publishers presently have quite a concerns with eBooks as they offer them no significant advantage over traditional books and also have a longer shelf life over physical books. Our product wishes to give them a new way of monetization and publishing. This will increase the overall acceptance of eBooks in general and hence help in reducing use of paper in printing of books.";
            IntroBox.Text = intro1 + "\n\n" + intro2 + "\n\n" + intro3 + "\n\n" + intro4 + "\n\n" + intro5 + "\n\n" + intro6;

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

        //private async void button_Click(object sender, RoutedEventArgs e)
        //{
        //    StorageFolder folder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
        //    StorageFile sampleFile2 = await folder2.GetFileAsync("sample2.txt");

        //    string testlol2 = await Windows.Storage.FileIO.ReadTextAsync(sampleFile2);
        //    //we have id
        //    items2 = await Table2.Where(Author
        //                    => Author.Id == testlol2).ToCollectionAsync();
        //    Author a = items2[0];
        //    a.books += "F8506193-E3FC-47F7-9A6C-7A3D479475BD" + ",";
        //    await Table2.UpdateAsync(a);
        //}
    }
}
