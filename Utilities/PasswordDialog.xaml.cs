using System;
using System.Windows;

namespace Utilities
{
    public partial class PasswordDialog : Window
    {
        public string Password1
        {
            get
            {
                return PasswordPwBox1.Password;
            }
        }
        public string Password2
        {
            get
            {
                return PasswordPwBox2.Password;
            }
        }

        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            PasswordPwBox1.Focus();
        }
    }
}
