using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
        }

        private void FormSearch_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            LoadAllStudents();
        }

        private void LoadKhoa()
        {
            DataTable dt = DatabaseConnection.GetAllKhoa();
            cboKhoa.DataSource = dt;
            cboKhoa.DisplayMember = "TenKhoa";
            cboKhoa.ValueMember = "MaKhoa";
        }

        private void LoadAllStudents()
        {
            DataTable dt = DatabaseConnection.GetAllStudents();
            dgvResults.DataSource = dt;
            UpdateStatus(dt.Rows.Count, "Tất cả sinh viên");
            FormatDataGridView();
        }

        private void FormatDataGridView()
        {
            if (dgvResults.Columns.Count > 0)
            {
                dgvResults.Columns["Masv"].HeaderText = "Mã SV";
                dgvResults.Columns["HoSv"].HeaderText = "Họ";
                dgvResults.Columns["tensv"].HeaderText = "Tên";
                dgvResults.Columns["phai"].HeaderText = "Giới tính";
                dgvResults.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvResults.Columns["NoiSinh"].HeaderText = "Nơi sinh";
                dgvResults.Columns["Makh"].HeaderText = "Mã khoa";
                dgvResults.Columns["HocBong"].HeaderText = "Học bổng";
                dgvResults.Columns["ServerName"].HeaderText = "Server";
            }
        }

        private void UpdateStatus(int count, string criteria)
        {
            lblStatus.Text = $"Kết quả tìm kiếm: {count} sinh viên | Tiêu chí: {criteria}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchByKeyword();
        }

        private void txtKeyword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchByKeyword();
                e.Handled = true;
            }
        }

        private void SearchByKeyword()
        {
            string keyword = txtKeyword.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKeyword.Focus();
                return;
            }

            DataTable dt = DatabaseConnection.SearchStudents(keyword);
            dgvResults.DataSource = dt;
            UpdateStatus(dt.Rows.Count, $"Từ khóa: '{keyword}'");
            FormatDataGridView();
        }

        private void btnSearchByKhoa_Click(object sender, EventArgs e)
        {
            string maKhoa = cboKhoa.SelectedValue.ToString();
            string tenKhoa = cboKhoa.Text;
            
            DataTable dt = DatabaseConnection.GetStudentsByKhoa(maKhoa);
            dgvResults.DataSource = dt;
            UpdateStatus(dt.Rows.Count, $"Khoa: {tenKhoa}");
            FormatDataGridView();
        }

        private void btnViewResults_Click(object sender, EventArgs e)
        {
            if (dgvResults.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xem kết quả!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = dgvResults.CurrentRow.Cells["Masv"].Value.ToString();
            string hoTen = dgvResults.CurrentRow.Cells["HoSv"].Value.ToString() + " " + 
                          dgvResults.CurrentRow.Cells["tensv"].Value.ToString();
            
            DataTable dt = DatabaseConnection.GetStudentResults(maSV);
            
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show($"Sinh viên {hoTen} (Mã: {maSV}) chưa có kết quả học tập!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormResults resultsForm = new FormResults(maSV, hoTen, dt);
            resultsForm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
