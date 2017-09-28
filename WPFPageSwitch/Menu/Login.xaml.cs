using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserDomain.Infrastructure.Repositories;
using UserDomain.Model.Entities;
using UserDomain.Services.DomainLayer;

namespace WPFPageSwitch
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : UserControl, ISwitchable
	{
		public Login()
		{
			this.InitializeComponent();
            Users = new ObservableCollection<User>();

            this.Loaded += Login_Loaded;
            this.DataContext = this;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            var users = UserRepositorySingleton.Instance.FindAll().ToList();
            users.ForEach(x => Users.Add(x));
        }

        public System.Collections.ObjectModel.ObservableCollection<User> Users { get; set; }        

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new MainMenu());
		}

        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if(listView.SelectedItem != null)
            {
                var userToRemove = listView.SelectedItem as User;
                if(userToRemove != null)
                {
                    UserRepositorySingleton.Instance.Remove(userToRemove);
                    UserRepositorySingleton.Instance.Flush();

                    Users.Remove(userToRemove);

                    listView.SelectedIndex = listView.Items.Count - 1;
                }
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var user = ((ListViewItem)sender).Content as User; //Casting back to the binded Track

            UserService.SelectedUser = user;

            Switcher.Switch(new MainMenu());
        }
    }
}