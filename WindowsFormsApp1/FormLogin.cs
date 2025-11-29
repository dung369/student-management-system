using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string connectionString = $"Data Source={txtServer.Text};Initial Catalog=qldiemsv;User ID={txtUsername.Text};Password={txtPassword.Text}";
            
            string message;
            bool isConnected = DatabaseConnection.TestConnection(connectionString, out message);
            
            if (isConnected)
            {
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "✓ " + message;
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "✗ " + message;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Vui lòng nhập tên server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServer.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập username!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập password!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Cập nhật connection string
            string connectionString = $"Data Source={txtServer.Text};Initial Catalog=qldiemsv;User ID={txtUsername.Text};Password={txtPassword.Text}";
            
            string message;
            bool isConnected = DatabaseConnection.TestConnection(connectionString, out message);
            
            if (isConnected)
            {
                // Lưu connection string vào DatabaseConnection
                DatabaseConnection.MainConnectionString = connectionString;
                
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Mở form chính
                FormMain formMain = new FormMain();
                this.Hide();
                formMain.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show(message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
