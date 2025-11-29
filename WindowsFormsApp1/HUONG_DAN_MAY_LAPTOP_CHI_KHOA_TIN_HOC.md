# HƯỚNG DẪN CẤU HÌNH CHO MÁY LAPTOP
## CHỈ KẾT NỐI ĐẾN KHOA TIN HỌC (SV_TH)

---

## I. TỔNG QUAN

### Mục tiêu:
- Máy Laptop **CHỦ YẾU** kết nối đến **Server 1 (SQLEXPRESS04 - Khoa Tin Học)**
- **KHÔNG** kết nối đến Server Phân Phối (SQLEXPRESS03) 
- Chỉ xem và quản lý **sinh viên Khoa Tin Học**
- **CÓ CHỨC NĂNG** chuyển khoa từ Tin Học sang Ngoại Ngữ (kết nối tạm thời đến SQLEXPRESS05)

### Thông tin kết nối:
```
Máy Desktop: DESKTOP-4EOK9DU (hoặc 192.168.1.X)

Server chính (Khoa Tin Học):
  - Server: SQLEXPRESS04
  - Port: 1434
  - Database: qldiemsv
  
Server phụ (Khoa Ngoại Ngữ - chỉ cho chức năng chuyển khoa):
  - Server: SQLEXPRESS05
  - Port: 1435
  - Database: qldiemsv

Username: sa
Password: 123456
```

---

## II. YÊU CẦU TRƯỚC KHI BẮT ĐẦU

### 1. Kiểm tra kết nối mạng
```powershell
# Ping đến máy Desktop
ping 192.168.1.X

# Kiểm tra port 1434 (Server Khoa Tin Học - chính)
telnet 192.168.1.X 1434

# Kiểm tra port 1435 (Server Khoa Ngoại Ngữ - cho chức năng chuyển khoa)
telnet 192.168.1.X 1435
```
*Thay X bằng IP thực của máy Desktop*

### 2. Lấy thông tin từ máy Desktop
- IP Address của máy Desktop (ví dụ: 192.168.1.5)
- Computer Name: DESKTOP-4EOK9DU
- Mật khẩu sa: 123456

### 3. Đảm bảo máy Desktop đã:
- Mở Firewall port **1434** (Khoa Tin Học)
- Mở Firewall port **1435** (Khoa Ngoại Ngữ - cho chức năng chuyển khoa)
- SQL Server (SQLEXPRESS04) đang chạy
- SQL Server (SQLEXPRESS05) đang chạy (cho chức năng chuyển khoa)
- Enable Remote Connection
- Enable SQL Server Authentication (Mixed Mode)

---

## III. CÀI ĐẶT SQL SERVER MANAGEMENT STUDIO (SSMS)

> **Tùy chọn**: Nếu muốn xem và truy vấn trực tiếp database

1. Tải SSMS: https://aka.ms/ssmsfullsetup
2. Cài đặt với cấu hình mặc định
3. Khởi động SSMS

---

## IV. KIỂM TRA KẾT NỐI SQL SERVER

### Bước 1: Mở SQL Server Management Studio

1. Nhấn **Connect** → **Database Engine**
2. Điền thông tin:
   ```
   Server type: Database Engine
   Server name: 192.168.1.X\SQLEXPRESS04,1434
   Authentication: SQL Server Authentication
   Login: sa
   Password: 123456
   ```
3. Nhấn **Connect**

### Bước 2: Kiểm tra Database

1. Mở **Databases** → **qldiemsv**
2. Kiểm tra các tables:
   - `dbo.DMSV` (Danh mục sinh viên)
   - `dbo.KetQua` (Kết quả học tập)
   - `dbo.DMkhoa` (Danh mục khoa)
   - `dbo.DMMH` (Danh mục môn học)

### Bước 3: Xem dữ liệu Khoa Tin Học

```sql
-- Xem tất cả sinh viên Khoa Tin Học
SELECT * FROM DMSV WHERE Makhoa = 'TH';

-- Xem kết quả học tập
SELECT 
    sv.MaSV,
    sv.HoTen,
    sv.Lop,
    kq.MaMH,
    kq.Diem
FROM DMSV sv
LEFT JOIN KetQua kq ON sv.MaSV = kq.MaSV
WHERE sv.Makhoa = 'TH';
```

**Nếu kết nối và query thành công = Hoàn hảo!**

---

## V. CẤU HÌNH ỨNG DỤNG WINDOWS FORMS

### Bước 1: Copy ứng dụng từ máy Desktop

1. Copy thư mục `WindowsFormsApp1` sang máy Laptop
2. Đặt ở vị trí bất kỳ (ví dụ: `C:\Projects\WindowsFormsApp1`)

### Bước 2: Mở Visual Studio

1. Mở Visual Studio 2019/2022
2. File → Open → Project/Solution
3. Chọn file `WindowsFormsApp1.sln`

### Bước 3: Chỉnh sửa DatabaseConnection.cs

**QUAN TRỌNG**: Cập nhật Connection String cho cả 2 servers

```csharp
// File: DatabaseConnection.cs

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DatabaseConnection
    {
        // ============================================
        // KẾT NỐI ĐẾN KHOA TIN HỌC (SQLEXPRESS04) - Server chính
        // Thay 192.168.1.X bằng IP thực của máy Desktop
        // ============================================
        
        // Connection string cho Khoa Tin Học (Server chính)
        public static string ConnectionString_SV_TH = @"Data Source=192.168.1.X\SQLEXPRESS04,1434;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        
        // Connection string cho Khoa Ngoại Ngữ (CHỈ dùng cho chức năng CHUYỂN KHOA)
        public static string ConnectionString_SV_NN = @"Data Source=192.168.1.X\SQLEXPRESS05,1435;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        
        // Connection string chính (Server Tin Học)
        public static string MainConnectionString = ConnectionString_SV_TH;
        
        // XÓA hoặc COMMENT OUT connection string Server Phân Phối
        // public static string ConnectionString_SV_MAIN = ...
        
        /// <summary>
        /// Test kết nối đến Server Khoa Tin Học
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString_SV_TH))
                {
                    conn.Open();
                    MessageBox.Show("Kết nối thành công đến Server Khoa Tin Học (SQLEXPRESS04)!", 
                                    "Thành công", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối:\n{ex.Message}", 
                                "Lỗi", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Lấy connection đến Server Khoa Tin Học
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString_SV_TH);
        }
        
        /// <summary>
        /// Lấy connection đến Server Khoa Ngoại Ngữ (chỉ dùng cho chuyển khoa)
        /// </summary>
        public static SqlConnection GetConnectionNN()
        {
            return new SqlConnection(ConnectionString_SV_NN);
        }
    }
}
```

### Bước 4: Cập nhật FormLogin.cs (Form đăng nhập)

```csharp
// File: FormLogin.cs

private void btnLogin_Click(object sender, EventArgs e)
{
    try
    {
        // Chỉ kết nối đến Server Khoa Tin Học
        string connectionString = $"Data Source={txtServerName.Text};Initial Catalog=qldiemsv;User ID={txtUsername.Text};Password={txtPassword.Text};Connect Timeout=30";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            
            // Kiểm tra xem có phải Server Khoa Tin Học không
            string query = "SELECT DB_NAME(), @@SERVERNAME, @@VERSION";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string dbName = reader.GetString(0);
                        string serverName = reader.GetString(1);
                        
                        // Hiển thị thông tin server
                        MessageBox.Show($"Kết nối thành công!\n\nDatabase: {dbName}\nServer: {serverName}", 
                                        "Đăng nhập thành công", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Information);
                    }
                }
            }
            
            // Lưu connection string vào DatabaseConnection
            DatabaseConnection.ConnectionString_SV_TH = connectionString;
            DatabaseConnection.MainConnectionString = connectionString;
            
            // Mở form chính
            this.Hide();
            FormMain mainForm = new FormMain();
            mainForm.ShowDialog();
            this.Close();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi đăng nhập:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### Bước 5: Cập nhật các Form khác (FormMain, FormSearch...)

**Lưu ý**: Trong tất cả các form khác, đảm bảo chỉ truy vấn đến Server Khoa Tin Học:

```csharp
// Ví dụ trong FormMain.cs

private void LoadData()
{
    try
    {
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
            
            // Chỉ lấy sinh viên Khoa Tin Học
            string query = @"
                SELECT 
                    sv.MaSV,
                    sv.HoTen,
                    sv.Ngaysinh,
                    sv.Lop,
                    sv.Makhoa,
                    k.Tenkhoa
                FROM DMSV sv
                INNER JOIN DMkhoa k ON sv.Makhoa = k.Makhoa
                WHERE sv.Makhoa = 'TH'
                ORDER BY sv.MaSV";
            
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            
            dataGridView1.DataSource = dt;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi tải dữ liệu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## VI. GIỚI HẠN CHỨC NĂNG

### Các chức năng CÓ THỂ sử dụng:

✅ **XEM danh sách sinh viên Khoa Tin Học**
```sql
SELECT * FROM DMSV WHERE Makhoa = 'TH';
```

✅ **THÊM sinh viên mới vào Khoa Tin Học**
```sql
INSERT INTO DMSV (MaSV, HoTen, Ngaysinh, Lop, Makhoa)
VALUES ('A05', 'Nguyen Van E', '2000-05-05', 'CTRR1', 'TH');
```

✅ **SỬA thông tin sinh viên Khoa Tin Học**
```sql
UPDATE DMSV 
SET HoTen = 'Nguyen Van E Updated'
WHERE MaSV = 'A05' AND Makhoa = 'TH';
```

✅ **XÓA sinh viên Khoa Tin Học**
```sql
DELETE FROM DMSV 
WHERE MaSV = 'A05' AND Makhoa = 'TH';
```

✅ **XEM kết quả học tập Khoa Tin Học**
```sql
SELECT * FROM KetQua WHERE MaSV IN (SELECT MaSV FROM DMSV WHERE Makhoa = 'TH');
```

✅ **NHẬP/SỬA/XÓA điểm cho sinh viên Khoa Tin Học**

### Các chức năng KHÔNG THỂ sử dụng:

❌ **Xem danh sách sinh viên Khoa Ngoại Ngữ** (không có quyền xem)
❌ **Thêm/Sửa/Xóa sinh viên Khoa Ngoại Ngữ** (không có quyền)
❌ **Xem báo cáo tổng hợp** (chỉ có dữ liệu 1 khoa)
❌ **Tìm kiếm toàn hệ thống** (chỉ tìm trong Khoa Tin Học)

### Chức năng ĐẶC BIỆT - CHUYỂN KHOA:

✅ **CHUYỂN sinh viên từ Khoa Tin Học sang Khoa Ngoại Ngữ**

Đây là chức năng DUY NHẤT được phép tương tác với Server Khoa Ngoại Ngữ.

**Quy trình chuyển khoa**:
1. Kiểm tra sinh viên có tồn tại trong Khoa Tin Học không
2. Thêm sinh viên vào Server Khoa Ngoại Ngữ (SQLEXPRESS05)
3. Xóa sinh viên khỏi Server Khoa Tin Học (SQLEXPRESS04)
4. Cập nhật mã khoa từ 'TH' → 'NN'

**Lưu ý**: Sau khi chuyển khoa, sinh viên sẽ BIẾN MẤT khỏi danh sách (vì không còn thuộc Khoa TH)

---

## VII. CHỨC NĂNG CHUYỂN KHOA (TIN HỌC → NGOẠI NGỮ)

### Mô tả:
Đây là chức năng ĐẶC BIỆT cho phép chuyển sinh viên từ Khoa Tin Học (SQLEXPRESS04) sang Khoa Ngoại Ngữ (SQLEXPRESS05).

### Điều kiện:
- Sinh viên PHẢI tồn tại trong Khoa Tin Học (Makhoa = 'TH')
- Sinh viên CHƯA tồn tại trong Khoa Ngoại Ngữ (để tránh trùng lặp)
- Cả 2 servers (SQLEXPRESS04 và SQLEXPRESS05) PHẢI online

### Code mẫu - Button "Chuyển Khoa" trong FormMain:

**CÁCH 1: Tự động phát hiện khoa và chuyển (Khuyến nghị - ĐƠN GIẢN NHẤT)**

```csharp
// FormMain.cs

private void btnChuyenKhoa_Click(object sender, EventArgs e)
{
    try
    {
        // Lấy mã sinh viên được chọn
        if (dataGridView1.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn sinh viên cần chuyển khoa!", 
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        string maSV = dataGridView1.SelectedRows[0].Cells["Mã SV"].Value.ToString();
        
        // Xác nhận
        DialogResult result = MessageBox.Show(
            $"Bạn có chắc chắn muốn chuyển khoa cho sinh viên {maSV}?\n\n" +
            "Hệ thống sẽ tự động phát hiện:\n" +
            "- Nếu đang ở Khoa Tin Học → Chuyển sang Khoa Ngoại Ngữ\n" +
            "- Nếu đang ở Khoa Ngoại Ngữ → Chuyển sang Khoa Tin Học\n\n" +
            "Lưu ý: Kết quả học tập sẽ được chuyển theo!",
            "Xác nhận chuyển khoa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.No)
            return;
        
        // Sử dụng DirectTransferHelper - TỰ ĐỘNG phát hiện và chuyển
        string message;
        bool success = DirectTransferHelper.TransferStudentAuto(maSV, out message);
        
        if (success)
        {
            MessageBox.Show(message, "Thành công", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData(); // Refresh lại danh sách
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
```

**CÁCH 2: Chỉ định rõ hướng chuyển (Chỉ chuyển TH → NN)**

```csharp
private void btnChuyenKhoa_Click(object sender, EventArgs e)
{
    try
    {
        // ... code validation ...
        
        string maSV = dataGridView1.SelectedRows[0].Cells["Mã SV"].Value.ToString();
        
        // Xác nhận
        DialogResult result = MessageBox.Show(
            $"Bạn có chắc chắn muốn chuyển sinh viên {maSV} từ Khoa Tin Học sang Khoa Ngoại Ngữ?\n\n" +
            "Lưu ý: Kết quả học tập sẽ được chuyển theo!",
            "Xác nhận chuyển khoa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.No)
            return;
        
        // Chỉ chuyển từ TH → NN
        string message;
        bool success = DirectTransferHelper.TransferFromTHtoNN(maSV, out message);
        
        if (success)
        {
            MessageBox.Show(message, "Thành công", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
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
```

**CÁCH 3: Code chi tiết từng bước (Tham khảo)**

```csharp
// FormMain.cs

private void btnChuyenKhoa_Click(object sender, EventArgs e)
{
    try
    {
        // Lấy mã sinh viên được chọn
        if (dataGridView1.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn sinh viên cần chuyển khoa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        string maSV = dataGridView1.SelectedRows[0].Cells["Mã SV"].Value.ToString();
        
        // Xác nhận
        DialogResult result = MessageBox.Show(
            $"Bạn có chắc chắn muốn chuyển sinh viên {maSV} từ Khoa Tin Học sang Khoa Ngoại Ngữ?\n\n" +
            "Lưu ý: Kết quả học tập sẽ được chuyển theo!",
            "Xác nhận chuyển khoa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.No)
            return;
        
        // Bước 1: Lấy thông tin sinh viên từ Server Tin Học
        DataRow studentData = null;
        DataTable ketQuaData = new DataTable();
        
        using (SqlConnection connTH = DatabaseConnection.GetConnection())
        {
            connTH.Open();
            
            // Lấy thông tin sinh viên
            string queryStudent = "SELECT * FROM DMSV WHERE MaSV = @MaSV AND Makhoa = 'TH'";
            SqlCommand cmdStudent = new SqlCommand(queryStudent, connTH);
            cmdStudent.Parameters.AddWithValue("@MaSV", maSV);
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmdStudent);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên trong Khoa Tin Học!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            studentData = dt.Rows[0];
            
            // Lấy kết quả học tập
            string queryKetQua = "SELECT * FROM KetQua WHERE MaSV = @MaSV";
            SqlCommand cmdKetQua = new SqlCommand(queryKetQua, connTH);
            cmdKetQua.Parameters.AddWithValue("@MaSV", maSV);
            
            SqlDataAdapter adapterKQ = new SqlDataAdapter(cmdKetQua);
            adapterKQ.Fill(ketQuaData);
        }
        
        // Bước 2: Kiểm tra sinh viên đã tồn tại ở Server Ngoại Ngữ chưa
        using (SqlConnection connNN = DatabaseConnection.GetConnectionNN())
        {
            connNN.Open();
            
            string queryCheck = "SELECT COUNT(*) FROM DMSV WHERE MaSV = @MaSV";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, connNN);
            cmdCheck.Parameters.AddWithValue("@MaSV", maSV);
            
            int count = (int)cmdCheck.ExecuteScalar();
            
            if (count > 0)
            {
                MessageBox.Show("Sinh viên đã tồn tại trong Khoa Ngoại Ngữ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Bước 3: Thêm sinh viên vào Server Ngoại Ngữ
            string queryInsertSV = @"
                INSERT INTO DMSV (MaSV, HoTen, Ngaysinh, Lop, Makhoa)
                VALUES (@MaSV, @HoTen, @Ngaysinh, @Lop, 'NN')";
            
            SqlCommand cmdInsertSV = new SqlCommand(queryInsertSV, connNN);
            cmdInsertSV.Parameters.AddWithValue("@MaSV", studentData["MaSV"]);
            cmdInsertSV.Parameters.AddWithValue("@HoTen", studentData["HoTen"]);
            cmdInsertSV.Parameters.AddWithValue("@Ngaysinh", studentData["Ngaysinh"]);
            cmdInsertSV.Parameters.AddWithValue("@Lop", studentData["Lop"]);
            
            cmdInsertSV.ExecuteNonQuery();
            
            // Bước 4: Chuyển kết quả học tập
            foreach (DataRow row in ketQuaData.Rows)
            {
                string queryInsertKQ = @"
                    INSERT INTO KetQua (MaSV, MaMH, Diem)
                    VALUES (@MaSV, @MaMH, @Diem)";
                
                SqlCommand cmdInsertKQ = new SqlCommand(queryInsertKQ, connNN);
                cmdInsertKQ.Parameters.AddWithValue("@MaSV", row["MaSV"]);
                cmdInsertKQ.Parameters.AddWithValue("@MaMH", row["MaMH"]);
                cmdInsertKQ.Parameters.AddWithValue("@Diem", row["Diem"]);
                
                cmdInsertKQ.ExecuteNonQuery();
            }
        }
        
        // Bước 5: Xóa sinh viên khỏi Server Tin Học
        using (SqlConnection connTH = DatabaseConnection.GetConnection())
        {
            connTH.Open();
            
            // Xóa kết quả học tập
            string queryDeleteKQ = "DELETE FROM KetQua WHERE MaSV = @MaSV";
            SqlCommand cmdDeleteKQ = new SqlCommand(queryDeleteKQ, connTH);
            cmdDeleteKQ.Parameters.AddWithValue("@MaSV", maSV);
            cmdDeleteKQ.ExecuteNonQuery();
            
            // Xóa sinh viên
            string queryDeleteSV = "DELETE FROM DMSV WHERE MaSV = @MaSV AND Makhoa = 'TH'";
            SqlCommand cmdDeleteSV = new SqlCommand(queryDeleteSV, connTH);
            cmdDeleteSV.Parameters.AddWithValue("@MaSV", maSV);
            cmdDeleteSV.ExecuteNonQuery();
        }
        
        // Thông báo thành công
        MessageBox.Show(
            $"Chuyển khoa thành công!\n\n" +
            $"Sinh viên {maSV} đã được chuyển từ Khoa Tin Học sang Khoa Ngoại Ngữ.\n" +
            $"Số điểm đã chuyển: {ketQuaData.Rows.Count}",
            "Thành công",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        
        // Refresh lại danh sách
        LoadData();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi chuyển khoa:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### Test chức năng chuyển khoa:

**Bước 1**: Kiểm tra sinh viên trong Khoa Tin Học
```sql
-- Chạy trên SQLEXPRESS04
SELECT * FROM DMSV WHERE MaSV = 'A10' AND Makhoa = 'TH';
```

**Bước 2**: Thực hiện chuyển khoa trong ứng dụng
- Chọn sinh viên A10
- Click nút "CHUYỂN KHOA"
- Xác nhận "Yes"

**Bước 3**: Kiểm tra kết quả
```sql
-- Kiểm tra trên SQLEXPRESS04 (không còn sinh viên A10)
SELECT * FROM DMSV WHERE MaSV = 'A10' AND Makhoa = 'TH';
-- Kết quả: 0 rows

-- Kiểm tra trên SQLEXPRESS05 (đã có sinh viên A10)
SELECT * FROM DMSV WHERE MaSV = 'A10' AND Makhoa = 'NN';
-- Kết quả: 1 row với Makhoa = 'NN'
```

**Lưu ý quan trọng**:
- Sau khi chuyển khoa, sinh viên sẽ BIẾN MẤT khỏi danh sách trên máy laptop (vì chỉ hiển thị Khoa TH)
- Nếu muốn xem lại sinh viên đã chuyển, phải vào máy Desktop và xem trên Server Ngoại Ngữ
- Không thể chuyển ngược (từ NN về TH) trên máy laptop này

---

## VIII. DEMO CODE MẪU

### Ví dụ 1: Form đăng nhập chỉ cho Khoa Tin Học

```csharp
// FormLogin.Designer.cs - Thiết kế form

namespace WindowsFormsApp1
{
    partial class FormLogin
    {
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            
            // Label Title
            this.lblTitle.Text = "ĐĂNG NHẬP - KHOA TIN HỌC";
            this.lblTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            
            // TextBox Server (mặc định là Server Khoa Tin Học)
            this.txtServerName.Text = "192.168.1.X\\SQLEXPRESS04,1434";
            this.txtServerName.ReadOnly = true; // Không cho phép đổi server
            
            // TextBox Username
            this.txtUsername.Text = "sa";
            
            // TextBox Password
            this.txtPassword.PasswordChar = '*';
            
            // Button Login
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += btnLogin_Click;
        }
    }
}
```

### Ví dụ 2: Form chính chỉ hiển thị sinh viên Khoa Tin Học

```csharp
// FormMain.cs

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    
                    // CHỈ lấy sinh viên Khoa Tin Học (Makhoa = 'TH')
                    string query = @"
                        SELECT 
                            sv.MaSV AS 'Mã SV',
                            sv.HoTen AS 'Họ Tên',
                            CONVERT(VARCHAR(10), sv.Ngaysinh, 103) AS 'Ngày Sinh',
                            sv.Lop AS 'Lớp',
                            k.Tenkhoa AS 'Khoa'
                        FROM DMSV sv
                        INNER JOIN DMkhoa k ON sv.Makhoa = k.Makhoa
                        WHERE sv.Makhoa = 'TH'
                        ORDER BY sv.MaSV";
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    
                    // Hiển thị số lượng sinh viên
                    lblTotal.Text = $"Tổng số sinh viên Khoa Tin Học: {dt.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Mở form thêm sinh viên (chỉ cho phép thêm vào Khoa Tin Học)
            FormAddStudent formAdd = new FormAddStudent();
            if (formAdd.ShowDialog() == DialogResult.OK)
            {
                LoadData(); // Refresh lại danh sách
            }
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Tìm kiếm chỉ trong Khoa Tin Học
            FormSearch formSearch = new FormSearch();
            formSearch.ShowDialog();
        }
    }
}
```

### Ví dụ 3: Form tìm kiếm chỉ trong Khoa Tin Học

```csharp
// FormSearch.cs

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtKeyword.Text.Trim();
                
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    
                    // Tìm kiếm CHỈ trong Khoa Tin Học
                    string query = @"
                        SELECT 
                            sv.MaSV AS 'Mã SV',
                            sv.HoTen AS 'Họ Tên',
                            CONVERT(VARCHAR(10), sv.Ngaysinh, 103) AS 'Ngày Sinh',
                            sv.Lop AS 'Lớp',
                            k.Tenkhoa AS 'Khoa'
                        FROM DMSV sv
                        INNER JOIN DMkhoa k ON sv.Makhoa = k.Makhoa
                        WHERE sv.Makhoa = 'TH'
                          AND (sv.MaSV LIKE @keyword 
                               OR sv.HoTen LIKE @keyword 
                               OR sv.Lop LIKE @keyword)
                        ORDER BY sv.MaSV";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    
                    lblResult.Text = $"Tìm thấy {dt.Rows.Count} sinh viên trong Khoa Tin Học";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
```

---

## IX. BUILD VÀ CHẠY ỨNG DỤNG

### Bước 1: Cập nhật IP máy Desktop

1. Mở file `DatabaseConnection.cs`
2. Thay `192.168.1.X` bằng IP thực của máy Desktop
3. Save file

### Bước 2: Clean và Rebuild Solution

1. Nhấn **Ctrl + Shift + B** để Build
2. Kiểm tra Output window không có lỗi
3. Nếu có lỗi: Build → Clean Solution → Build → Rebuild Solution

### Bước 3: Chạy ứng dụng

1. Nhấn **F5** để Run
2. Form đăng nhập sẽ hiện ra
3. Nhập thông tin:
   ```
   Server: 192.168.1.X\SQLEXPRESS04,1434
   Username: sa
   Password: 123456
   ```
4. Nhấn **Đăng nhập**

### Bước 4: Kiểm tra dữ liệu

1. Form chính sẽ hiển thị danh sách sinh viên Khoa Tin Học
2. Thử các chức năng:
   - Thêm sinh viên mới
   - Sửa thông tin
   - Xóa sinh viên
   - Tìm kiếm

---

## X. KIỂM TRA VÀ TROUBLESHOOTING

### Test 1: Kiểm tra kết nối

```sql
-- Chạy trong SSMS hoặc trong ứng dụng
SELECT DB_NAME() AS 'Database', @@SERVERNAME AS 'Server';
```

**Kết quả mong đợi**:
```
Database: qldiemsv
Server: DESKTOP-4EOK9DU\SQLEXPRESS04
```

### Test 2: Kiểm tra dữ liệu Khoa Tin Học

```sql
-- Đếm số sinh viên Khoa Tin Học
SELECT COUNT(*) AS 'Tổng SV Khoa TH' FROM DMSV WHERE Makhoa = 'TH';

-- Xem chi tiết
SELECT * FROM DMSV WHERE Makhoa = 'TH';
```

### Test 3: Kiểm tra không thấy dữ liệu khoa khác

```sql
-- Thử query sinh viên Khoa Ngoại Ngữ (phải trả về 0 dòng vì không có trong DB này)
SELECT COUNT(*) AS 'SV Khoa NN' FROM DMSV WHERE Makhoa = 'NN';
```

**Kết quả mong đợi**: 0 (vì Server này chỉ có dữ liệu Khoa Tin Học)

### Lỗi thường gặp:

#### Lỗi 1: "A network-related or instance-specific error"

**Nguyên nhân**:
- Không ping được máy Desktop
- Port 1434 chưa mở
- SQL Server service chưa chạy

**Giải pháp**:
```powershell
# 1. Test ping
ping 192.168.1.X

# 2. Test port
telnet 192.168.1.X 1434

# 3. Kiểm tra firewall (trên máy Desktop)
# Mở Windows Firewall → Inbound Rules → Kiểm tra port 1434
```

#### Lỗi 2: "Login failed for user 'sa'"

**Nguyên nhân**:
- Sai mật khẩu
- Tài khoản sa chưa enable
- Mixed Mode chưa bật

**Giải pháp**:
1. Vào máy Desktop → SSMS
2. Right-click server → Properties → Security
3. Chọn "SQL Server and Windows Authentication mode"
4. Restart SQL Server service

#### Lỗi 3: "Cannot open database 'qldiemsv'"

**Nguyên nhân**:
- Database chưa được tạo trên SQLEXPRESS04

**Giải pháp**:
1. Vào máy Desktop
2. Chạy script `00_CreateDatabase.sql` trên SQLEXPRESS04
3. Chạy script `02_Insert_Data_SQLEXPRESS04_KhoaTH.sql`

#### Lỗi 4: "Lỗi chuyển khoa" - Không kết nối được đến SQLEXPRESS05

**Nguyên nhân**:
- Port 1435 chưa mở
- SQL Server (SQLEXPRESS05) chưa chạy
- Connection string sai

**Giải pháp**:
```powershell
# 1. Test ping
ping 192.168.1.X

# 2. Test port 1435
telnet 192.168.1.X 1435

# 3. Kiểm tra SQL Server service trên máy Desktop
# Đảm bảo SQL Server (SQLEXPRESS05) đang chạy
```

#### Lỗi 5: "Sinh viên đã tồn tại trong Khoa Ngoại Ngữ"

**Nguyên nhân**:
- Sinh viên đã được chuyển khoa trước đó

**Giải pháp**:
1. Vào máy Desktop
2. Kiểm tra trên SQLEXPRESS05:
```sql
SELECT * FROM DMSV WHERE MaSV = 'A10';
```
3. Nếu cần, xóa sinh viên trên SQLEXPRESS05 và thử lại

---

## XI. SƠ ĐỒ KẾT NỐI VỚI CHỨC NĂNG CHUYỂN KHOA

```
┌─────────────────────────────────────────┐
│        MÁY DESKTOP (MÁY MẸ)            │
│        IP: 192.168.1.X                  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ SQLEXPRESS04 (Port 1434)         │◄─┼─── Kết nối CHÍNH (90% thời gian)
│  │ ================================  │  │    - Xem danh sách
│  │ Database: qldiemsv               │  │    - Thêm/Sửa/Xóa sinh viên
│  │ Khoa: TIN HỌC (TH)               │  │    - Tìm kiếm
│  │ ================================  │  │    - Nhập điểm
│  │ Tables:                          │  │
│  │   - DMSV (A01, A02, A03, A10)    │  │
│  │   - KetQua                       │  │
│  │   - DMkhoa (TH)                  │  │
│  │   - DMMH                         │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ SQLEXPRESS05 (Port 1435)         │◄─┼─── Kết nối PHỤ (chỉ cho CHUYỂN KHOA)
│  │ ================================  │  │    - Thêm sinh viên từ TH → NN
│  │ Database: qldiemsv               │  │    - Chuyển kết quả học tập
│  │ Khoa: NGOẠI NGỮ (NN)             │  │
│  │ ================================  │  │    ⚠️ KHÔNG được:
│  │ Tables:                          │  │    ❌ Xem danh sách
│  │   - DMSV (B01, B02, ...)         │  │    ❌ Thêm/Sửa/Xóa trực tiếp
│  │   - KetQua                       │  │    ❌ Tìm kiếm
│  │   - DMkhoa (NN)                  │  │
│  │   - DMMH                         │  │
│  └──────────────────────────────────┘  │
└─────────────────────────────────────────┘
              ↑
              │ TCP/IP
              │ Port 1434 (chính) + Port 1435 (phụ)
              │
┌─────────────────────────────────────────┐
│        MÁY LAPTOP (MÁY CON)            │
│        IP: 192.168.1.10                 │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ Windows Forms App                │  │
│  │ ================================  │  │
│  │ Kết nối CHÍNH:                   │  │
│  │   → SQLEXPRESS04 (Khoa TH)       │  │
│  │                                  │  │
│  │ Kết nối PHỤ:                     │  │
│  │   → SQLEXPRESS05 (chỉ chuyển khoa)│  │
│  │                                  │  │
│  │ Chức năng:                       │  │
│  │  ✅ Xem SV Khoa TH              │  │
│  │  ✅ Thêm SV Khoa TH             │  │
│  │  ✅ Sửa SV Khoa TH              │  │
│  │  ✅ Xóa SV Khoa TH              │  │
│  │  ✅ Tìm kiếm trong Khoa TH      │  │
│  │  ✅ CHUYỂN KHOA TH → NN         │  │
│  │  ❌ Xem SV Khoa NN               │  │
│  └──────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

### Luồng dữ liệu khi CHUYỂN KHOA:

```
BƯỚC 1: Đọc thông tin sinh viên
┌────────────┐
│ Máy Laptop │ ──── SELECT * FROM DMSV WHERE MaSV='A10' ───► SQLEXPRESS04
└────────────┘                                               (Khoa TH)
                                                             
BƯỚC 2: Thêm vào Server Ngoại Ngữ                            
┌────────────┐
│ Máy Laptop │ ──── INSERT INTO DMSV (MaSV, ..., Makhoa='NN') ───► SQLEXPRESS05
└────────────┘                                                      (Khoa NN)

BƯỚC 3: Xóa khỏi Server Tin Học
┌────────────┐
│ Máy Laptop │ ──── DELETE FROM DMSV WHERE MaSV='A10' ───► SQLEXPRESS04
└────────────┘                                             (Khoa TH)

KẾT QUẢ:
- Sinh viên A10 biến mất khỏi SQLEXPRESS04 ❌
- Sinh viên A10 xuất hiện trong SQLEXPRESS05 ✅ (nhưng máy laptop không thấy)
```

---

## XII. DỮ LIỆU MẪU TRONG SERVER KHOA TIN HỌC
              ↑
              │ TCP/IP - Port 1434
              │ Kết nối TRỰC TIẾP
              │
┌─────────────────────────────────────────┐
│        MÁY LAPTOP (MÁY CON)            │
│        IP: 192.168.1.10                 │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ Windows Forms App                │  │
│  │ ================================  │  │
│  │ CHỈ kết nối đến:                 │  │
│  │ SQLEXPRESS04 (Khoa Tin Học)      │  │
│  │                                  │  │
│  │ Chức năng:                       │  │
│  │  ✅ Xem SV Khoa TH              │  │
│  │  ✅ Thêm SV Khoa TH             │  │
│  │  ✅ Sửa SV Khoa TH              │  │
│  │  ✅ Xóa SV Khoa TH              │  │
│  │  ✅ Tìm kiếm trong Khoa TH      │  │
│  │  ❌ Không thấy khoa khác         │  │
│  └──────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

---

## XI. DỮ LIỆU MẪU TRONG SERVER KHOA TIN HỌC

### Sinh viên Khoa Tin Học (SQLEXPRESS04):

| MaSV | HoTen          | Ngaysinh   | Lop   | Makhoa |
|------|----------------|------------|-------|--------|
| A01  | Nguyen Van A   | 2000-01-01 | CTRR1 | TH     |
| A02  | Tran Thi B     | 2000-02-02 | CTRR1 | TH     |
| A03  | Le Van C       | 2000-03-03 | CTRR2 | TH     |
| A04  | Pham Thi D     | 2000-04-04 | CTRR2 | TH     |

### Kết quả học tập:

| MaSV | MaMH | Diem |
|------|------|------|
| A01  | MH01 | 8.5  |
| A01  | MH02 | 7.0  |
| A02  | MH01 | 9.0  |
| A03  | MH02 | 6.5  |

---

## XIII. CHECKLIST CUỐI CÙNG

### Trên máy Desktop:
- [ ] SQL Server (SQLEXPRESS04) đang chạy
- [ ] SQL Server (SQLEXPRESS05) đang chạy (cho chức năng chuyển khoa)
- [ ] Port 1434 đã mở trong Firewall
- [ ] Port 1435 đã mở trong Firewall (cho chức năng chuyển khoa)
- [ ] Remote connection đã enable
- [ ] Tài khoản sa đã enable với password 123456
- [ ] Database qldiemsv đã tạo trên SQLEXPRESS04
- [ ] Database qldiemsv đã tạo trên SQLEXPRESS05 (cho chức năng chuyển khoa)
- [ ] Dữ liệu Khoa Tin Học đã insert vào SQLEXPRESS04
- [ ] Dữ liệu Khoa Ngoại Ngữ đã insert vào SQLEXPRESS05

### Trên máy Laptop:
- [ ] Ping được IP máy Desktop
- [ ] Telnet được port 1434 (Server chính - Khoa TH)
- [ ] Telnet được port 1435 (Server phụ - Khoa NN, cho chuyển khoa)
- [ ] SSMS kết nối được đến SQLEXPRESS04
- [ ] Query `SELECT * FROM DMSV WHERE Makhoa='TH'` thành công
- [ ] Đã cập nhật IP trong `DatabaseConnection.cs` cho CẢ 2 connection strings
- [ ] Visual Studio build thành công
- [ ] Ứng dụng chạy được và login thành công
- [ ] Hiển thị đúng sinh viên Khoa Tin Học
- [ ] Các chức năng CRUD hoạt động
- [ ] Chức năng CHUYỂN KHOA hoạt động (test với 1 sinh viên mẫu)

**Nếu tất cả đều ✓ → CẤU HÌNH HOÀN THÀNH!**

---

## XIV. GHI CHÚ QUAN TRỌNG
**Nếu tất cả đều ✓ → CẤU HÌNH HOÀN THÀNH!**

---

## XIV. GHI CHÚ QUAN TRỌNG

### 1. Phân quyền dữ liệu:
- Máy Laptop **CHỦ YẾU** làm việc với dữ liệu Khoa Tin Học
- **CÓ THỂ** chuyển sinh viên sang Khoa Ngoại Ngữ (chức năng đặc biệt)
- **KHÔNG** thể xem hoặc chỉnh sửa dữ liệu Khoa Ngoại Ngữ (trừ chuyển khoa)

### 2. Về chức năng CHUYỂN KHOA:
- Đây là chức năng 1 CHIỀU: TH → NN
- Không thể chuyển ngược từ NN → TH trên máy laptop
- Sinh viên sau khi chuyển khoa sẽ BIẾN MẤT khỏi danh sách (không còn thuộc Khoa TH)
- Để xem sinh viên đã chuyển, phải vào máy Desktop xem trên SQLEXPRESS05
- Kết quả học tập sẽ được chuyển theo sinh viên

### 3. Bảo mật:
- Nên tạo tài khoản riêng cho Khoa Tin Học (không dùng sa)
- Giới hạn quyền chỉ SELECT, INSERT, UPDATE, DELETE trên tables Khoa TH
- Cho phép INSERT vào Server Ngoại Ngữ (chỉ cho chuyển khoa)

### 4. Hiệu năng:
- Kết nối đến 2 servers nên đảm bảo mạng LAN ổn định
- Chức năng chuyển khoa cần Transaction để đảm bảo tính toàn vẹn
- Nếu lỗi giữa chừng, cần rollback để tránh mất dữ liệu

### 5. Backup:
- Nên backup database định kỳ trên máy Desktop (cả SQLEXPRESS04 và SQLEXPRESS05)
- Máy Laptop chỉ là client, không cần backup

### 6. Sau khi chuyển khoa:
- Sinh viên sẽ không còn hiển thị trong danh sách máy Laptop
- Nếu cần kiểm tra, vào máy Desktop và query:
  ```sql
  -- Kiểm tra trên SQLEXPRESS05
  SELECT * FROM DMSV WHERE MaSV = 'A10' AND Makhoa = 'NN';
  ```
- Nếu muốn chuyển ngược (NN → TH), phải làm trên máy Desktop hoặc tạo máy Laptop khác cho Khoa NN

---

**CHÚC BẠN CẤU HÌNH THÀNH CÔNG!** 🎉

**LƯU Ý**: Gửi file này cho bạn bạn và hướng dẫn:
1. Đọc kỹ phần I, II, III để hiểu tổng quan
2. Làm theo từng bước trong phần IV, V
3. Đặc biệt chú ý phần VII về chức năng CHUYỂN KHOA
4. Test kỹ chức năng chuyển khoa với 1 sinh viên mẫu trước khi sử dụng thật
