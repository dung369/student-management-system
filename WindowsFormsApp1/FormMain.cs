using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormMain : Form
    {
        public FormMain()  
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
            LoadStudents();
        }

        // Load danh sách khoa vào ComboBox
        private void LoadKhoa()
        {
            DataTable dt = DatabaseConnection.GetAllKhoa();

            // Tạo dòng "Tất cả"
            DataRow dr = dt.NewRow();
            dr["MaKhoa"] = "ALL";
            dr["TenKhoa"] = "-- Tất cả khoa --";

            // Chèn vào đầu bảng
            dt.Rows.InsertAt(dr, 0);

            cboKhoa.DataSource = dt;
            cboKhoa.DisplayMember = "TenKhoa";
            cboKhoa.ValueMember = "MaKhoa";
        }

        // Load danh sách sinh viên
        private void LoadStudents()
        {
            DataTable dt = DatabaseConnection.GetAllStudents();
            dgvStudents.DataSource = dt;
            
            // Đổi tên cột cho dễ hiểu
            if (dgvStudents.Columns.Count > 0)
            {
                dgvStudents.Columns["Masv"].HeaderText = "Mã SV";
                dgvStudents.Columns["HoSv"].HeaderText = "Họ";
                dgvStudents.Columns["tensv"].HeaderText = "Tên";
                dgvStudents.Columns["phai"].HeaderText = "Giới tính";
                dgvStudents.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgvStudents.Columns["NoiSinh"].HeaderText = "Nơi sinh";
                dgvStudents.Columns["Makh"].HeaderText = "Mã khoa";
                dgvStudents.Columns["HocBong"].HeaderText = "Học bổng";
                dgvStudents.Columns["ServerName"].HeaderText = "Server";
            }
            
            lblServerInfo.Text = $"Tổng số sinh viên: {dt.Rows.Count} ";
        }

        // Clear form
        private void ClearForm()
        {
            txtMaSV.Clear();
            txtHoSV.Clear();
            txtTenSV.Clear();
            txtNoiSinh.Clear();
            txtHocBong.Text = "0";
            chkPhai.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            if (cboKhoa.Items.Count > 0)
                cboKhoa.SelectedIndex = 0;
            
            txtMaSV.Enabled = true;
            txtMaSV.Focus();
        }

        // Validate dữ liệu nhập
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return false;
            }

            if (txtMaSV.Text.Length != 3)
            {
                MessageBox.Show("Mã sinh viên phải có đúng 3 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoSV.Text))
            {
                MessageBox.Show("Vui lòng nhập họ sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNoiSinh.Text))
            {
                MessageBox.Show("Vui lòng nhập nơi sinh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNoiSinh.Focus();
                return false;
            }

            float hocBong;
            if (!float.TryParse(txtHocBong.Text, out hocBong) || hocBong < 0)
            {
                MessageBox.Show("Học bổng phải là số không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHocBong.Focus();
                return false;
            }

            return true;
        }

        // Nút THÊM
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string maSV = txtMaSV.Text.ToUpper().Trim();
            
            // Kiểm tra sinh viên đã tồn tại chưa
            string serverName;
            if (DatabaseConnection.CheckStudentExists(maSV, out serverName))
            {
                MessageBox.Show($"Mã sinh viên {maSV} đã tồn tại trên {serverName}!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSV.Focus();
                return;
            }

            string message;
            bool result = DatabaseConnection.InsertStudent(
                maSV,
                txtHoSV.Text.Trim(),
                txtTenSV.Text.Trim(),
                chkPhai.Checked,
                dtpNgaySinh.Value,
                txtNoiSinh.Text.Trim(),
                cboKhoa.SelectedValue.ToString(),
                float.Parse(txtHocBong.Text),
                out message
            );

            if (result)
            {
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
                ClearForm();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút SỬA
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string maSV = txtMaSV.Text.ToUpper().Trim();
            
            // Kiểm tra sinh viên có tồn tại không
            string serverName;
            if (!DatabaseConnection.CheckStudentExists(maSV, out serverName))
            {
                MessageBox.Show($"Không tìm thấy sinh viên có mã {maSV}!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maKhoa = cboKhoa.SelectedValue.ToString();
            
            // Xác định server dựa vào khoa
            string targetServer = (maKhoa == "TH") ? "SV_TH" : "SV_NN";
            
            // Nếu sinh viên thuộc server khác với khoa hiện tại, cần chuyển
            if (serverName != targetServer)
            {
                DialogResult confirmTransfer = MessageBox.Show(
                    $"Sinh viên đang ở {serverName} nhưng khoa mới thuộc {targetServer}.\n" +
                    "Bạn có muốn chuyển sinh viên sang server mới không?",
                    "Xác nhận chuyển server",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmTransfer == DialogResult.Yes)
                {
                    string transferMsg;
                    if (DatabaseConnection.TransferStudent(maSV, maKhoa, out transferMsg))
                    {
                        MessageBox.Show(transferMsg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadStudents();
                        ClearForm();
                        return;
                    }
                    else
                    {
                        MessageBox.Show(transferMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            // Update trên cùng server
            string updateQuery = $@"UPDATE {serverName}.qldiemsv.dbo.DMSV 
                                   SET HoSv = @HoSv, tensv = @TenSv, phai = @Phai, 
                                       NgaySinh = @NgaySinh, NoiSinh = @NoiSinh, 
                                       Makh = @MaKh, HocBong = @HocBong
                                   WHERE Masv = @MaSV";

            var parameters = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@MaSV", maSV),
                new System.Data.SqlClient.SqlParameter("@HoSv", txtHoSV.Text.Trim()),
                new System.Data.SqlClient.SqlParameter("@TenSv", txtTenSV.Text.Trim()),
                new System.Data.SqlClient.SqlParameter("@Phai", chkPhai.Checked),
                new System.Data.SqlClient.SqlParameter("@NgaySinh", dtpNgaySinh.Value),
                new System.Data.SqlClient.SqlParameter("@NoiSinh", txtNoiSinh.Text.Trim()),
                new System.Data.SqlClient.SqlParameter("@MaKh", maKhoa),
                new System.Data.SqlClient.SqlParameter("@HocBong", float.Parse(txtHocBong.Text))
            };

            int result = DatabaseConnection.ExecuteNonQuery(updateQuery, parameters);
            
            if (result > 0)
            {
                MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
                ClearForm();
            }
        }

        // Nút XÓA
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = txtMaSV.Text.ToUpper().Trim();
            
            // Kiểm tra sinh viên có tồn tại không
            string serverName;
            if (!DatabaseConnection.CheckStudentExists(maSV, out serverName))
            {
                MessageBox.Show($"Không tìm thấy sinh viên có mã {maSV}!", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sinh viên {maSV} trên {serverName}?\n" +
                "Lưu ý: Kết quả học tập của sinh viên cũng sẽ bị xóa!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                // Xóa kết quả trước
                string deleteResultQuery = $"DELETE FROM {serverName}.qldiemsv.dbo.KetQua WHERE MaSV = @MaSV";
                var param1 = new System.Data.SqlClient.SqlParameter[] 
                { 
                    new System.Data.SqlClient.SqlParameter("@MaSV", maSV) 
                };
                DatabaseConnection.ExecuteNonQuery(deleteResultQuery, param1);

                // Xóa sinh viên
                string deleteStudentQuery = $"DELETE FROM {serverName}.qldiemsv.dbo.DMSV WHERE Masv = @MaSV";
                var param2 = new System.Data.SqlClient.SqlParameter[] 
                { 
                    new System.Data.SqlClient.SqlParameter("@MaSV", maSV) 
                };
                int result = DatabaseConnection.ExecuteNonQuery(deleteStudentQuery, param2);

                if (result > 0)
                {
                    MessageBox.Show($"Xóa sinh viên {maSV} thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                    ClearForm();
                }
            }
        }

        // Nút TÌM KIẾM
        private void btnSearch_Click(object sender, EventArgs e)
        {
            FormSearch searchForm = new FormSearch();
            searchForm.ShowDialog();
        }

        // Tìm kiếm nhanh
        private void btnSearchExecute_Click(object sender, EventArgs e)
        {
            SearchStudents();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchStudents();
                e.Handled = true;
            }
        }

        private void SearchStudents()
        {
            string keyword = txtSearch.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadStudents();
                return;
            }

            DataTable dt = DatabaseConnection.SearchStudents(keyword);
            dgvStudents.DataSource = dt;
            
            lblServerInfo.Text = $"Tìm thấy: {dt.Rows.Count} sinh viên | Từ khóa: {keyword}";
        }

        // Nút CHUYỂN KHOA - TỰ ĐỘNG PHÁT HIỆN KHOA
        private void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sinh viên chưa
                if (string.IsNullOrWhiteSpace(txtMaSV.Text))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần chuyển khoa!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã sinh viên
                string maSV = txtMaSV.Text.ToUpper().Trim();

                // Xác nhận với user
                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn chuyển khoa cho sinh viên {maSV}?\n\n" +
                    "Hệ thống sẽ tự động phát hiện:\n" +
                    "- Nếu đang ở Khoa Tin Học → Chuyển sang Khoa Ngoại Ngữ\n" +
                    "- Nếu đang ở Khoa Ngoại Ngữ → Chuyển sang Khoa Tin Học\n\n" +
                    "Lưu ý: Kết quả học tập sẽ được chuyển theo!",
                    "Xác nhận chuyển khoa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.No)
                    return;

                // GỌI FUNCTION TỰ ĐỘNG - DirectTransferHelper.TransferStudentAuto()
                string message;
                bool result = DirectTransferHelper.TransferStudentAuto(maSV, out message);

                // Hiển thị kết quả
                if (result)
                {
                    MessageBox.Show(message, "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi chuyển khoa:\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút LÀM MỚI
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
            ClearForm();
        }

        // Click vào dòng trong DataGridView
        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                
                txtMaSV.Text = row.Cells["Masv"].Value.ToString();
                txtHoSV.Text = row.Cells["HoSv"].Value.ToString();
                txtTenSV.Text = row.Cells["tensv"].Value.ToString();
                chkPhai.Checked = Convert.ToBoolean(row.Cells["phai"].Value);
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtNoiSinh.Text = row.Cells["NoiSinh"].Value.ToString();
                cboKhoa.SelectedValue = row.Cells["Makh"].Value.ToString();
                //txtHocBong.Text = row.Cells["HocBong"].Value.ToString();
                
                txtMaSV.Enabled = false; // Không cho sửa mã sinh viên
            }
        }

        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedValue == null)
                return;

            string maKhoa = cboKhoa.SelectedValue.ToString();

            // Nếu chọn "Tất cả" → Load toàn bộ
            if (maKhoa == "ALL")
            {
                LoadStudents();
            }
            else
            {
                // Lọc theo mã khoa
                dgvStudents.DataSource = DatabaseConnection.GetStudentsByKhoa(maKhoa);

                lblServerInfo.Text = $"Đang xem sinh viên thuộc khoa: {cboKhoa.Text}";
            }
        }

        private void lblNoiSinh_Click(object sender, EventArgs e)
        {

        }
    }
}
