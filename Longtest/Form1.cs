using Reposiotry;

namespace Longtest
{
    public partial class Form1 : Form
    {
        public UserServices _user { get; set; }
        public Form1()
        {
            _user = new UserServices();
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;

            var user = _user.GetAll().Where(p => p.UserName.Equals(username)
                            && p.Password.Equals(password) && p.UserRole == 1)
                        .FirstOrDefault();
            if (user!=null)
            {
                //chuyn trang management
                this.Hide();
                var manage = new Management(user);
                manage.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not alone", "Error", MessageBoxButtons.OK);
            }


        }
    }
}