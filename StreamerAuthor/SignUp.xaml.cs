using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class SignUp : Page
    {
       // public event EventHandler<BackRequestedEventArgs> BackRequested;
        public SignUp()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private IMobileServiceTable<Author> Table = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items;
        string error = "";
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                    rootFrame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(Login));
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            Loading.IsIndeterminate = true;
            Author a = new Author();
            int i = 1;

            if (!Regex.Match(Name.Text, @"^[a-zA-Z\s\.]+$").Success)
            {
                i = 0;
                error = error + "*Enter a valid name  ";
            }
            if (!(Mobile.Text.All(char.IsDigit) && Mobile.Text.Length == 10))
            {
                error = error + "*Enter valid Mobile Number  ";
                i = 0;
            }

            if (!Regex.Match(Email.Text, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$").Success)
            {
                error = error + "*Enter valid Email Id  ";
                i = 0;
            }

            if (!(UserName.Text.All(char.IsLetterOrDigit) && UserName.Text.Length != 0))
            {
               
                error = error + "*Username can have only alphanumeric values  ";
                i = 0;
            }
            else
            {
                //username exist check
                items = await Table.Where(Author
                              => Author.username == UserName.Text).ToCollectionAsync();
                if (items.Count != 0)
                {
                    error = error + "*Username already exist  ";
                    i = 0;
                }
            }

            if ((Password.Password.Length < 8))
            {
                i = 0;
                error = error + "*Password should be minimum of length 8  ";
            }
            else
            {
                if(Password.Password != PasswordConfirm.Password)
                {
                    i = 0;
                    error = error + "*Password didn't matched  ";
                }
            }
         

            if (i == 0)
            {
                MistakeBox.Text = error;
                MistakeBox.Visibility = Visibility.Visible;
                Loading.Visibility = Visibility.Collapsed;
            }
            
            else
            {
                try
                {
                    a.publishername = Name.Text;
                    a.phone = Mobile.Text;
                    a.email = Email.Text;
                    a.password = Password.Password;
                    a.username = UserName.Text;
                    a.wallet = 0;
                    a.books = "";
                    await App.MobileService.GetTable<Author>().InsertAsync(a);
                    Loading.Visibility = Visibility.Collapsed;
                    Windows.UI.Popups.MessageDialog mess = new MessageDialog("Author has been created!");
                    Frame.Navigate(typeof(Login));
                    //add object a in cloud
                }
                catch(Exception)
                {
                    MessageDialog mess = new MessageDialog("There has been a problem connecting :(:(");
                    Loading.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
 }
