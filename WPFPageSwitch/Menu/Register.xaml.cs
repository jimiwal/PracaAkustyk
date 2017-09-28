using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserDomain.Infrastructure.Repositories;
using UserDomain.Model.Entities;

namespace WPFPageSwitch
{
	/// <summary>
	/// Interaction logic for Register.xaml
	/// </summary>
	public partial class Register : UserControl
	{
		public Register()
		{
			this.InitializeComponent();
            this.DataContext = this;
        }

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new MainMenu());
		}

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            var users = UserRepositorySingleton.Instance.Get(string.Empty, string.Empty, usernameTextBox_Copy1.Text, string.Empty).ToList();
            if(users.Count == 0)
            {
                try
                {
                    var newUser = new User()
                    {
                        Name = usernameTextBox.Text,
                        LastName = usernameTextBox_Copy.Text,
                        Email = usernameTextBox_Copy1.Text,
                        Phone = usernameTextBox_Copy3.Text,
                        Address = usernameTextBox_Copy4.Text
                    };

                    UserRepositorySingleton.Instance.Save(newUser);
                    UserRepositorySingleton.Instance.Flush();
                }
                catch (Exception ex)
                {
                    statusTb.Text = ex.Message;
                }
                

                statusTb.Text = "User successfully saved !!!";
            }
            else
            {
                statusTb.Text = "User Already Exist !!!! (duplicate email)";
            }
        }
    }
}