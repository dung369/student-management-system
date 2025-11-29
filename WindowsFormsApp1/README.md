# HỆ THỐNG QUẢN LÝ SINH VIÊN - PHÂN MẢNH CƠ SỞ DỮ LIỆU

## 📋 TỔNG QUAN

Hệ thống quản lý điểm sinh viên với kiến trúc phân mảnh cơ sở dữ liệu SQL Server. Dữ liệu được phân tán trên nhiều server instances theo khoa, cho phép nhiều người dùng trên các máy khác nhau cùng làm việc với dữ liệu được đồng bộ tự động.

**Cấu hình Server:**
- **Máy chủ**: `DESKTOP-4EOK9DU\SQLEXPRESS03` (Port 1433)
- **Máy con 1**: `DESKTOP-4EOK9DU\SQLEXPRESS04` (Port 1434) - Khoa Tin Học
- **Máy con 2**: `DESKTOP-4EOK9DU\SQLEXPRESS05` (Port 1435) - Khoa Ngoại Ngữ

---

## 🏗️ KIẾN TRÚC HỆ THỐNG

### Phân tán dữ liệu:
```
┌─────────────────────────────────────────────────────────────┐
│                    MÁY DESKTOP (Server Mẹ)                  │
│                    Computer: DESKTOP-4EOK9DU                │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌────────────────────────────────────────────────────┐    │
│  │ SQLEXPRESS03 (Port 1433) - Server Phân Phối        │    │
│  │ Data Source: DESKTOP-4EOK9DU\SQLEXPRESS03          │    │
│  │ - Quản lý Linked Servers (SV_MAIN, SV_TH, SV_NN)  │    │
│  │ - Lưu trữ dữ liệu gốc QLDiem                       │    │
│  │ - Views tổng hợp từ tất cả servers                 │    │
│  │ - Stored Procedures xử lý phân mảnh                │    │
│  └────────────────────────────────────────────────────┘    │
│           │                    │                            │
│           │  Linked Servers    │                            │
│           ↓                    ↓                            │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │ SQLEXPRESS04     │  │ SQLEXPRESS05     │               │
│  │ (Port 1434)      │  │ (Port 1435)      │               │
│  │ DESKTOP-4EOK9DU\ │  │ DESKTOP-4EOK9DU\ │               │
│  │ SQLEXPRESS04     │  │ SQLEXPRESS05     │               │
│  │ SV_TH            │  │ SV_NN            │               │
│  │ Khoa Tin Học(TH) │  │ Khoa Ngoại Ngữ   │               │
│  │                  │  │ (NN)             │               │
│  └──────────────────┘  └──────────────────┘               │
└─────────────────────────────────────────────────────────────┘
                           ↑
                           │ Remote Connection
                           │ TCP/IP (không dùng port trong connection string)
                           ↓
┌─────────────────────────────────────────────────────────────┐
│                    MÁY LAPTOP (Client)                      │
│                    IP: 192.168.1.10                         │
├─────────────────────────────────────────────────────────────┤
│  - Kết nối đến Server Mẹ trên Desktop                      │
│  - CRUD operations tự động phân mảnh                        │
│  - Đồng bộ real-time với Desktop                           │
└─────────────────────────────────────────────────────────────┘
```

### Phân mảnh theo Khoa:
- **Khoa Tin Học (TH)** → `DESKTOP-4EOK9DU\SQLEXPRESS04` (Port 1434) - Server SV_TH
- **Khoa Ngoại Ngữ (NN)** → `DESKTOP-4EOK9DU\SQLEXPRESS05` (Port 1435) - Server SV_NN
- **Server Phân Phối** → `DESKTOP-4EOK9DU\SQLEXPRESS03` (Port 1433) - Server SV_MAIN

---

## 🚀 TÍNH NĂNG

### 1. Quản lý Sinh viên
- ✅ **THÊM**: Tự động kiểm tra trùng mã trên tất cả servers
- ✅ **SỬA**: Cập nhật thông tin, tự động chuyển server nếu đổi khoa
- ✅ **XÓA**: Xóa sinh viên và kết quả học tập
- ✅ **TÌM KIẾM**: Tìm theo mã, tên, nơi sinh, khoa
- ✅ **CHUYỂN KHOA**: Chuyển sinh viên giữa các khoa (giữa các servers)

### 2. Xem Kết quả Học tập
- Xem điểm thi của sinh viên
- Hiển thị môn học, lần thi, điểm số
- Dữ liệu từ server tương ứng

### 3. Phân mảnh Tự động
- Thêm sinh viên Khoa TH → Tự động lưu vào SQLEXPRESS04 (SV_TH)
- Thêm sinh viên Khoa NN → Tự động lưu vào SQLEXPRESS05 (SV_NN)
- Chuyển khoa → Tự động di chuyển dữ liệu giữa servers

### 4. Đồng bộ Dữ liệu
- Máy Desktop thêm/sửa → Máy Laptop thấy ngay (sau khi refresh)
- Máy Laptop thêm/sửa → Máy Desktop thấy ngay (sau khi refresh)
- Tất cả thao tác qua Server Mẹ (SQLEXPRESS03)

---

## 📂 CẤU TRÚC PROJECT

```
WindowsFormsApp1/
├── SQL_Scripts/                          # SQL Scripts
│   ├── 00_CreateDatabase.sql            # Tạo database và tables
│   ├── 01_Setup_LinkedServers_SQLEXPRESS03.sql  # Cấu hình Linked Servers
│   ├── 02_Insert_Data_SQLEXPRESS04_KhoaTH.sql   # Dữ liệu Khoa Tin Học
│   ├── 03_Insert_Data_SQLEXPRESS05_KhoaNN.sql   # Dữ liệu Khoa Ngoại Ngữ
│   └── 04_Create_Views_And_StoredProcedures.sql # Views & SPs
│
├── DatabaseConnection.cs                 # Quản lý kết nối database
├── FormLogin.cs / FormLogin.Designer.cs  # Form đăng nhập
├── FormMain.cs / FormMain.Designer.cs    # Form chính - CRUD
├── FormSearch.cs / FormSearch.Designer.cs # Form tìm kiếm
├── FormResults.cs                        # Form xem kết quả học tập
│
├── HUONG_DAN_MAY_DESKTOP.md             # Hướng dẫn cho máy Desktop
├── HUONG_DAN_MAY_LAPTOP.md              # Hướng dẫn cho máy Laptop
└── README.md                             # File này
```

---

## 🛠️ CÔNG NGHỆ SỬ DỤNG

- **Language**: C# (.NET Framework)
- **UI**: Windows Forms
- **Database**: Microsoft SQL Server Express
- **Architecture**: Distributed Database (Phân mảnh ngang - Horizontal Fragmentation)
- **Connection**: ADO.NET, Linked Servers

---

## 📦 YÊU CẦU HỆ THỐNG

### Máy Desktop (Server Mẹ):
- Windows 10/11
- SQL Server Express (3 instances)
- .NET Framework 4.7.2 trở lên
- Visual Studio 2019/2022 (để build)
- RAM: 4GB+ (khuyến nghị 8GB)
- Firewall: Mở ports 1433, 1434, 1435

### Máy Laptop (Client):
- Windows 10/11
- .NET Framework 4.7.2 trở lên
- SQL Server Management Studio (tùy chọn)
- Kết nối mạng LAN với máy Desktop
- Firewall: Mở ports 1433, 1434, 1435

---

## 🔧 HƯỚNG DẪN CÀI ĐẶT

### Cho Máy Desktop (Server Mẹ):
📖 **Xem chi tiết**: [HUONG_DAN_MAY_DESKTOP.md](HUONG_DAN_MAY_DESKTOP.md)

**Các bước chính**:
1. Cài đặt 3 SQL Server instances (SQLEXPRESS03, 04, 05)
2. Cấu hình ports (1433, 1434, 1435)
3. Enable remote connections
4. Kích hoạt tài khoản sa
5. Mở firewall cho 3 ports
6. Chạy SQL scripts để tạo database
7. Cấu hình Linked Servers
8. Build và chạy ứng dụng

### Cho Máy Laptop (Client):
📖 **Xem chi tiết**: [HUONG_DAN_MAY_LAPTOP.md](HUONG_DAN_MAY_LAPTOP.md)

**Các bước chính**:
1. Kiểm tra kết nối mạng với máy Desktop
2. Mở firewall cho 3 ports
3. Copy source code từ máy Desktop
4. Cập nhật Connection String với IP của máy Desktop
5. Build và chạy ứng dụng

---

## 💻 HƯỚNG DẪN SỬ DỤNG

### 1. Đăng nhập
```
Server Name: DESKTOP-4EOK9DU\SQLEXPRESS03
            (KHÔNG dùng port trong connection string)
            (hoặc 192.168.1.X\SQLEXPRESS03 từ máy Laptop - thay X bằng IP thực)
Username: sa
Password: 123456
```

**Lưu ý**: 
- Named Instance của SQL Server không cần chỉ định port trong connection string
- SQL Browser Service sẽ tự động tìm đúng port (1433, 1434, 1435)
- Connection string đúng: `Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03;Initial Catalog=qldiemsv;User ID=sa;Password=123456`

### 2. Thêm Sinh viên
1. Nhập đầy đủ thông tin sinh viên
2. Chọn khoa (TH hoặc NN)
3. Click **THÊM**
4. Hệ thống tự động:
   - Kiểm tra mã sinh viên trùng trên tất cả servers
   - Lưu vào server tương ứng với khoa

### 3. Sửa Thông tin
1. Click vào sinh viên trong danh sách
2. Thông tin hiện lên form
3. Chỉnh sửa thông tin cần thiết
4. Click **SỬA**
5. Nếu đổi khoa, hệ thống tự động chuyển sang server khác

### 4. Xóa Sinh viên
1. Click vào sinh viên cần xóa
2. Click **XÓA**
3. Xác nhận xóa
4. Hệ thống xóa cả kết quả học tập

### 5. Tìm kiếm
- **Tìm nhanh**: Nhập từ khóa ở góc phải → Enter
- **Tìm chi tiết**: Click **TÌM KIẾM** → Chọn tiêu chí

### 6. Chuyển Khoa
1. Click vào sinh viên cần chuyển
2. Chọn khoa mới
3. Click **CHUYỂN KHOA**
4. Xác nhận
5. Hệ thống tự động:
   - Di chuyển thông tin sinh viên
   - Di chuyển kết quả học tập
   - Xóa dữ liệu cũ

---

## 🗄️ CẤU TRÚC DATABASE

### Bảng DMSV (Sinh viên)
```sql
CREATE TABLE DMSV (
    Masv char(3) PRIMARY KEY,
    HoSv nvarchar(30),
    tensv nvarchar(10),
    phai bit,
    NgaySinh datetime,
    NoiSinh nvarchar(25),
    Makh char(2),
    HocBong float
)
```

### Bảng DMkhoa (Khoa)
```sql
CREATE TABLE DMkhoa (
    MaKhoa char(2) PRIMARY KEY,
    TenKhoa nvarchar(20)
)
```

### Bảng DMMH (Môn học)
```sql
CREATE TABLE DMMH (
    MaMh char(2) PRIMARY KEY,
    tenMh nvarchar(30),
    sotiet tinyint
)
```

### Bảng KetQua (Kết quả học tập)
```sql
CREATE TABLE KetQua (
    MaSV char(3),
    MaMH char(2),
    LanThi tinyint,
    Diem decimal(4,2),
    PRIMARY KEY(MaSv, MaMh, LanThi)
)
```

---

## 🔐 BẢO MẬT

### Khuyến nghị:
1. **Đổi mật khẩu sa**: Không dùng mật khẩu mặc định (123456) trong môi trường thực
2. **Firewall**: Chỉ mở ports cần thiết, giới hạn IP được phép kết nối
3. **Encryption**: Cân nhắc mã hóa kết nối (SSL/TLS) cho production
4. **Backup**: Thường xuyên backup database
5. **User Roles**: Tạo các user với quyền hạn phù hợp thay vì dùng sa

---

## 🐛 TROUBLESHOOTING

### Không kết nối được đến server
**Kiểm tra**:
- [ ] SQL Server service đang chạy
- [ ] Port đã cấu hình đúng
- [ ] Firewall đã mở port
- [ ] Ping được máy đích
- [ ] Telnet được port

### Lỗi Login failed
**Kiểm tra**:
- [ ] Mixed Mode đã bật
- [ ] Tài khoản sa đã enable
- [ ] Mật khẩu đúng

### Linked Server lỗi
**Giải pháp**:
- Chạy lại script `01_Setup_LinkedServers_SQLEXPRESS03.sql`
- Kiểm tra credential
- Test kết nối: `SELECT * FROM sys.servers WHERE name IN ('SV_MAIN', 'SV_TH', 'SV_NN')`
- Kiểm tra data_source phải đúng: `DESKTOP-4EOK9DU\SQLEXPRESS04` và `DESKTOP-4EOK9DU\SQLEXPRESS05`

### Lỗi "A network-related or instance-specific error"
**Nguyên nhân**: Connection string sai hoặc SQL Browser Service không chạy

**Giải pháp**:
1. **Bỏ port khỏi connection string**: Dùng `DESKTOP-4EOK9DU\SQLEXPRESS03` thay vì `DESKTOP-4EOK9DU\SQLEXPRESS03,1433`
2. **Kiểm tra SQL Browser Service**:
   ```
   services.msc → SQL Server Browser → Status = Running
   ```
3. **Connection string đúng**:
   ```csharp
   Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30
   ```

### Dữ liệu không đồng bộ
**Giải pháp**:
- Click nút **LÀM MỚI** để tải lại dữ liệu
- Kiểm tra kết nối mạng
- Kiểm tra Linked Servers hoạt động

---

## 📊 DEMO DATA

### Sinh viên Khoa Tin Học (TH) - trên SQLEXPRESS04 (SV_TH):
- A01: Nguyễn Thị Hải
- A03: Lê Thu Bạch Yến

### Sinh viên Khoa Ngoại Ngữ (NN) - trên SQLEXPRESS05 (SV_NN):
- A02: Trần Văn Chính
- A04: Trần Anh Tuấn
- B01: Trần Thanh Mai
- B02: Trần Thị Thu Thủy

---

## 📝 GHI CHÚ QUAN TRỌNG

1. **Connection String Configuration**:
   - ⚠️ **KHÔNG dùng port** trong connection string cho Named Instance
   - ✅ Đúng: `Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03;...`
   - ❌ Sai: `Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03,1433;...`
   - SQL Browser Service sẽ tự động tìm đúng port

2. **Port Configuration**:
   - Port được cấu hình trong SQL Server Configuration Manager
   - Port chỉ dùng cho kết nối trực tiếp qua IP (không dùng Named Instance)
   - Linked Server sử dụng Named Instance, không cần port

3. **Linked Servers**:
   - Chỉ cấu hình trên Server Mẹ (SQLEXPRESS03)
   - Server Con không cần Linked Servers
   - Provider: `SQLOLEDB` hoặc `SQLNCLI`
   - Data Source: Dùng tên instance đầy đủ (VD: `DESKTOP-4EOK9DU\SQLEXPRESS04`)

4. **Performance**:
   - Views có thể chậm với dữ liệu lớn
   - Cân nhắc indexing cho production
   - Cân nhắc caching cho ứng dụng

5. **Scalability**:
   - Có thể thêm nhiều server instances
   - Có thể phân mảnh theo tiêu chí khác (năm học, địa điểm...)
   - Có thể triển khai trên nhiều máy vật lý

---

## 🤝 ĐÓNG GÓP

Dự án này là bài tập học tập về phân mảnh cơ sở dữ liệu.

---

## 📞 HỖ TRỢ

Nếu gặp vấn đề khi cài đặt hoặc sử dụng:
1. Đọc kỹ file hướng dẫn tương ứng
2. Kiểm tra phần Troubleshooting
3. Kiểm tra các checklist trong hướng dẫn
**🎉 Chúc bạn triển khai thành công hệ thống phân mảnh!**

---

## ⚙️ THÔNG SỐ KỸ THUẬT

| Thông số | Giá trị |
|----------|---------|
| **Server Name** | DESKTOP-4EOK9DU |
| **SQLEXPRESS03** | Port 1433 - Server Phân Phối (SV_MAIN) |
| **SQLEXPRESS04** | Port 1434 - Khoa Tin Học (SV_TH) |
| **SQLEXPRESS05** | Port 1435 - Khoa Ngoại Ngữ (SV_NN) |
| **Database Name** | qldiemsv |
| **Authentication** | SQL Server Authentication (sa/123456) |
| **Provider** | SQLOLEDB |
| **Framework** | .NET Framework 4.7.2 |

---

*Last updated: November 29, 2025*
## 📜 LICENSE

Dự án học tập - Sử dụng tự do

---

**🎉 Chúc bạn triển khai thành công hệ thống phân mảnh!**

*Last updated: November 7, 2025*
