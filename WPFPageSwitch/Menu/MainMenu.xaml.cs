using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UserDomain.Services.DomainLayer;
using WPFPageSwitch.Menu;

namespace WPFPageSwitch
{
	public partial class MainMenu : UserControl, ISwitchable
	{

        public static readonly DependencyProperty UserLabelProperty =
      DependencyProperty.Register("UserLabel", typeof(string),
        typeof(MainMenu), new PropertyMetadata(""));

        public MainMenu()
		{
            
			InitializeComponent();
            this.DataContext = this;
            this.Loaded += MainMenu_Loaded;

		}

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            UserLabel = UserService.SelectedUser != null ? UserService.SelectedUser.Name + " - " + UserService.SelectedUser.Email : "No User has been selected";

            if(UserService.SelectedUser == null)
            {
                newProcessBtn.IsEnabled = false;
                myProcessesBtn.IsEnabled = false;
            }
            else
            {
                newProcessBtn.IsEnabled = true;
                myProcessesBtn.IsEnabled = true;
            }
        }

        public String UserLabel
        {
            get { return (String)GetValue(UserLabelProperty); }
            set { SetValue(UserLabelProperty, value); }
        }

        private void newGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new Process_Step1());
		}

		private void loadGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new MyProcesses());
		}

		private void optionButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new BaseSoundsDefinition());
		}

        //private void ShowMessageBox(string title, string message, MessageBoxIcon icon)
        //{
            //MessageBoxChildWindow messageWindow =
            //    new MessageBoxChildWindow(title, message, MessageBoxButtons.Ok, icon);

            //messageWindow.Show();
        //}

        #region Event For Child Window
        private void loginWindowForm_SubmitClicked(object sender, EventArgs e)
        {
            //ShowMessageBox("Login Successful", "Welcome, " + loginForm.NameText, MessageBoxIcon.Information);

        }

        private void registerForm_SubmitClicked(object sender, EventArgs e)
        {
        }


        #endregion

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void loginTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	Switcher.Switch(new Login());
        }

        private void registerTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	Switcher.Switch(new Register());
        }
        #endregion

        private void sequenceButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Option());
        }
    }
}