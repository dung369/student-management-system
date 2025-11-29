# HƯỚNG DẪN CÀI ĐẶT VÀ CẤU HÌNH HỆ THỐNG PHÂN MẢNH SQL SERVER
## CHO MÁY DESKTOP (MÁY MẸ)

---

## I. THÔNG TIN HỆ THỐNG

### Cấu hình:
- **Máy Desktop (Máy Mẹ)**: 
  - Computer Name: DESKTOP-4EOK9DU
  - 3 SQL Server Instances:
    - **SQLEXPRESS03 (Port 1433)** - Server máy chủ phân phối (Server Mẹ)
      - Lưu trữ dữ liệu gốc QLDiem
      - Cấu hình phân phối đến Server 1 và Server 2
      - Thiết lập Linked Server
    - **SQLEXPRESS04 (Port 1434)** - Server 1
      - Lưu dữ liệu "SINHVIEN_TH" 
      - Nhận dữ liệu Khoa Tin Học được phân mảnh
    - **SQLEXPRESS05 (Port 1435)** - Server 2
      - Lưu dữ liệu "SINHVIEN_NN"
      - Nhận dữ liệu Khoa Ngoại Ngữ được phân mảnh

- **Máy Laptop (Máy Con)**: 
  - IP: 192.168.1.10
  - 1 SQL Server Instance (chỉ kết nối đến các server đã phân mảnh)

---

## II. CÀI ĐẶT SQL SERVER (TRÊN MÁY DESKTOP)

### Bước 1: Tải SQL Server Express
1. Truy cập: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Tải SQL Server Express Edition (miễn phí)
   - **Basic**: Cài đặt nhanh, mặc định 1 instance tên SQLEXPRESS
   - **Custom**: Tùy chỉnh tên instance, cấu hình chi tiết

### Bước 2: Cài đặt Instance thứ nhất (SQLEXPRESS03)

#### 🔵 CÁCH 1: NẾU ĐÃ TẢI BASIC (Đơn giản hơn)

1. **Lần 1 - Cài SQLEXPRESS (sẽ đổi tên sau)**:
   - Chạy file **Basic** đã tải
   - Click **Accept** → **Install**
   - Đợi cài đặt xong (tự động tạo instance tên SQLEXPRESS)
   - Click **Close**

2. **Cài thêm 2 instances nữa**:
   - Chạy lại file **Basic** 
   - Lần này chọn **Customize**
   - Hoặc tải thêm file **Custom/Advanced** từ link trên
   - Trong màn hình cài đặt, chọn **New SQL Server stand-alone installation**
   - Tại **Instance Configuration**:
     - Chọn **Named instance**
     - Nhập tên: **SQLEXPRESS03**
     - Instance ID: SQLEXPRESS03
   - Tại **Server Configuration**: Automatic
   - Tại **Database Engine Configuration**:
     - Authentication Mode: **Mixed Mode**
     - Password: **123456**
     - Thêm Windows user
   - Click **Install**

3. **Đổi tên instance mặc định** (Optional):
   - Instance đầu tiên tên SQLEXPRESS có thể giữ nguyên
   - HOẶC đổi tên thành SQLEXPRESS05 (dùng làm dự phòng)
   - Cách đổi: Xem phần "Đổi tên Instance" ở cuối mục này

#### 🟢 CÁCH 2: NẾU TẢI CUSTOM (Chi tiết hơn)

1. Chạy file cài đặt
2. Chọn **Custom Installation**
3. Chọn đường dẫn cài đặt → **Install**
4. Đợi tải files → Click **Next**
5. Tại màn hình **Installation Type**, chọn **New SQL Server stand-alone installation**
6. Tại **Instance Configuration**:
   - Chọn **Named instance**
   - Nhập tên: **SQLEXPRESS03**
   - Instance ID: SQLEXPRESS03
7. Tại **Server Configuration**:
   - SQL Server Database Engine: **Automatic**
8. Tại **Database Engine Configuration**:
   - Authentication Mode: **Mixed Mode**
   - Nhập mật khẩu cho sa: **123456** (hoặc mật khẩu của bạn)
   - Thêm Windows user hiện tại
9. Click **Install**

#### 📝 Đổi tên Instance (Nếu cần)

Nếu đã cài instance tên **SQLEXPRESS** và muốn đổi tên:

**CÁCH ĐƠN GIẢN NHẤT**: Giữ nguyên tên, chỉ cần:
- SQLEXPRESS (Port 1433) → Server máy chủ phân phối (lưu dữ liệu gốc + phân mảnh)
- SQLEXPRESS03 (Port 1434) → Server 1 (SINHVIEN_TH - Khoa Tin Học)
- SQLEXPRESS04 (Port 1435) → Server 2 (SINHVIEN_NN - Khoa Ngoại Ngữ)

**Lưu ý**: Nếu dùng SQLEXPRESS mặc định, nhớ cập nhật tên trong các script SQL và Connection String!

### Bước 3: Cài đặt Instance thứ hai

**Nếu đã có SQLEXPRESS từ Basic**:
- Cài thêm 1 instance nữa, đặt tên: **SQLEXPRESS03**
- Làm theo hướng dẫn Cách 1 hoặc Cách 2 ở Bước 2

**Nếu chưa có SQLEXPRESS**:
- Cài instance tên: **SQLEXPRESS04**
- Làm theo hướng dẫn Cách 2 ở Bước 2

### Bước 4: Cài đặt Instance thứ ba (nếu cần)

**Tùy chọn**: Bạn có thể dùng 2 instances cũng được:
- Instance 1 (SQLEXPRESS hoặc SQLEXPRESS03): Server máy chủ phân phối (dữ liệu gốc)
- Instance 2 (SQLEXPRESS04): Server 1 - SINHVIEN_TH (Khoa Tin Học)
- Instance 3 (SQLEXPRESS05): Server 2 - SINHVIEN_NN (Khoa Ngoại Ngữ)

**Khuyến nghị**: Nên cài đủ 3 instances để phân mảnh rõ ràng

**Nếu muốn đủ 3 instances**:
- Lặp lại Bước 2, đặt tên instance: **SQLEXPRESS04** hoặc **SQLEXPRESS05**

### Bước 5: Cài đặt SQL Server Management Studio (SSMS)
1. Tải SSMS: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
2. Cài đặt SSMS

---

## III. CẤU HÌNH PORT CHO CÁC INSTANCES

### Bước 1: Mở SQL Server Configuration Manager
1. Nhấn **Windows + R**
2. Gõ: `SQLServerManager15.msc` (hoặc SQLServerManager16.msc tùy version)
3. Nhấn Enter

### Bước 2: Cấu hình Port cho SQLEXPRESS03 (Port 1433)

1. Trong SQL Server Configuration Manager:
   - Mở **SQL Server Network Configuration**
   - Click **Protocols for SQLEXPRESS03**

2. **Bật TCP/IP**:
   - Right-click **TCP/IP** → chọn **Enable**

3. **Cấu hình Port**:
   - Right-click **TCP/IP** → chọn **Properties**
   - Chọn tab **IP Addresses**
   - Cuộn xuống **IPALL**:
     - TCP Dynamic Ports: **XÓA TRỐNG**
     - TCP Port: **1433**
   - Click **OK**

4. **Restart Service**:
   - Vào **SQL Server Services**
   - Right-click **SQL Server (SQLEXPRESS03)** → chọn **Restart**

### Bước 3: Cấu hình Port cho SQLEXPRESS04 (Port 1434)

1. Lặp lại như Bước 2 nhưng:
   - Chọn **Protocols for SQLEXPRESS04**
   - Đặt TCP Port: **1434**
   - Restart **SQL Server (SQLEXPRESS04)**

### Bước 4: Cấu hình Port cho SQLEXPRESS05 (Port 1435)

1. Lặp lại như Bước 2 nhưng:
   - Chọn **Protocols for SQLEXPRESS05**
   - Đặt TCP Port: **1435**
   - Restart **SQL Server (SQLEXPRESS05)**

---

## IV. KÍCH HOẠT REMOTE CONNECTION

### Bước 1: Cấu hình cho SQLEXPRESS03

1. Mở **SQL Server Management Studio (SSMS)**
2. **Kết nối đến server**:
   - Server type: **Database Engine**
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS03**
   - Authentication: **Windows Authentication** (chọn từ dropdown)
   - Click **Connect**

3. **Bật Remote Connection**:
   - Right-click server name → chọn **Properties**
   - Chọn **Connections**
   - Tích ✓ **Allow remote connections to this server**
   - Click **OK**

4. **Bật Mixed Mode Authentication**:
   - Right-click server name → chọn **Properties**
   - Chọn **Security**
   - Chọn **SQL Server and Windows Authentication mode**
   - Click **OK**

5. **Kích hoạt tài khoản sa**:
   - Mở **Security** → **Logins**
   - Right-click **sa** → chọn **Properties**
   - Tab **General**: Đặt Password: **123456**
   - Tab **Status**:
     - Login: **Enabled**
     - Permission to connect: **Grant**
   - Click **OK**

6. **Restart SQL Server Service**

### Bước 2: Cấu hình cho SQLEXPRESS04
1. **Kết nối đến server**:
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS04**
   - Authentication: **Windows Authentication**
   - Click **Connect**
2. Lặp lại các bước cấu hình như Bước 1 (Bật Remote Connection, Mixed Mode, Enable sa)

### Bước 3: Cấu hình cho SQLEXPRESS05
1. **Kết nối đến server**:
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS05**
   - Authentication: **Windows Authentication**
   - Click **Connect**
2. Lặp lại các bước cấu hình như Bước 1

---

## V. MỞ PORT TRÊN WINDOWS FIREWALL

### Bước 1: Mở Windows Firewall
1. Nhấn **Windows + R**
2. Gõ: `wf.msc`
3. Nhấn Enter

### Bước 2: Tạo Inbound Rule cho Port 1433

1. Click **Inbound Rules** → **New Rule...**
2. Rule Type: **Port** → Next
3. Protocol and Ports:
   - TCP
   - Specific local ports: **1433**
   - Next
4. Action: **Allow the connection** → Next
5. Profile: Tích cả 3 (Domain, Private, Public) → Next
6. Name: **SQL Server Port 1433** → Finish

### Bước 3: Tạo Inbound Rule cho Port 1434

- Lặp lại Bước 2 với Port: **1434**
- Name: **SQL Server Port 1434**

### Bước 4: Tạo Inbound Rule cho Port 1435

- Lặp lại Bước 2 với Port: **1435**
- Name: **SQL Server Port 1435**

### Bước 5: Tạo Outbound Rules (tương tự)

- Lặp lại các bước trên cho **Outbound Rules** với cả 3 ports

---

## VI. TẠO DATABASE VÀ PHÂN MẢNH DỮ LIỆU

### Bước 1: Tạo Database trên tất cả Instances

**⚠️ LƯU Ý QUAN TRỌNG**: Nếu đã chạy script này rồi và muốn chạy lại, bạn cần:
1. **Đóng tất cả query windows** đang mở database `qldiemsv`
2. **Ngắt kết nối**: Right-click vào database `qldiemsv` → chọn **Delete** (hoặc chạy `DROP DATABASE qldiemsv`)
3. Hoặc **bỏ qua lỗi** và chạy tiếp các bước sau (database vẫn hoạt động bình thường)

**CÁCH 1: Chạy từng instance riêng lẻ**
1. Kết nối đến **SQLEXPRESS03** bằng SSMS
2. Mở file: `00_CreateDatabase.sql`
3. Click **Execute** (F5)
4. Lặp lại cho **SQLEXPRESS04** và **SQLEXPRESS05**

**CÁCH 2: Chạy 1 lần cho cả 3 instances (Nhanh hơn)** ⭐ Khuyến nghị
1. Trong SSMS, kết nối đến cả 3 instances:
   - File → Connect Object Explorer → Kết nối **SQLEXPRESS03**
   - File → Connect Object Explorer → Kết nối **SQLEXPRESS04**
   - File → Connect Object Explorer → Kết nối **SQLEXPRESS05**
   
2. Mở file: `00_CreateDatabase.sql`

3. **Cách chạy đơn giản**:
   - Bên **Object Explorer** (bên trái), click chọn instance nào (SQLEXPRESS03, 04, hoặc 05)
   - Quay lại cửa sổ SQL script → Click **Execute (F5)**
   - Script sẽ chạy trên instance bạn vừa chọn!
   
   **Hoặc dùng dropdown** (trong toolbar):
   - Tìm dropdown hiển thị tên server (ví dụ: DESKTOP-4EOK9DU\SQLEXPRESS03)
   - Click dropdown → Chọn instance muốn chạy
   - Click **Execute (F5)**

4. **Lặp lại cho 3 instances**:
   - Chọn **SQLEXPRESS03** → Execute (F5)
   - Chọn **SQLEXPRESS04** → Execute (F5)
   - Chọn **SQLEXPRESS05** → Execute (F5)

✅ **Lợi ích của Cách 2**: Không cần đóng/mở file nhiều lần, chỉ cần **click vào instance bên Object Explorer** rồi Execute!

### Bước 2: Cấu hình Linked Servers (CHỈ trên SQLEXPRESS03)

1. Kết nối đến **SQLEXPRESS03**
2. Mở file: `01_Setup_LinkedServers_SQLEXPRESS03.sql`
3. **QUAN TRỌNG**: Sửa mật khẩu sa trong script nếu khác 123456
4. Click **Execute** (F5)

### Bước 3: Insert dữ liệu Khoa Tin Học

1. Kết nối đến **SQLEXPRESS04** (Server 1)
2. Mở file: `02_Insert_Data_SQLEXPRESS04_KhoaTH.sql`
3. Click **Execute** (F5)

### Bước 4: Insert dữ liệu Khoa Ngoại Ngữ

1. **QUAN TRỌNG**: Đảm bảo đã chạy `00_CreateDatabase.sql` trên SQLEXPRESS05 ở Bước 1
2. Kết nối đến **SQLEXPRESS05** (Server 2)
3. Mở file: `03_Insert_Data_SQLEXPRESS05_KhoaNN.sql`
4. Click **Execute** (F5)

⚠️ **Nếu gặp lỗi "Database 'qldiemsv' does not exist"**:
   - Quay lại **Bước 1** và chạy `00_CreateDatabase.sql` trên SQLEXPRESS05
   - Sau đó quay lại chạy file này

### Bước 5: Tạo Views và Stored Procedures

1. Kết nối đến **SQLEXPRESS03**
2. Mở file: `04_Create_Views_And_StoredProcedures.sql`
3. Click **Execute** (F5)

---

## VII. KIỂM TRA KẾT NỐI

### Kiểm tra Local Connection

1. Mở SSMS
2. Trong cửa sổ **Connect to Server**, điền thông tin:

   **Instance 1 - SQLEXPRESS03:**
   - Server type: **Database Engine** (để mặc định)
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS03,1433** (hoặc **DESKTOP-4EOK9DU\SQLEXPRESS03**)
   - Authentication: Click dropdown, chọn **SQL Server Authentication**
   - Login: **sa**
   - Password: **123456** (hoặc mật khẩu bạn đã đặt)
   - **Remember password**: Tích ✓ (để lần sau không phải nhập lại)
   - Encryption: **Optional** (mặc định)
   - Click nút **Connect**
   
   **Instance 2 - SQLEXPRESS04:**
   - Server type: **Database Engine**
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS04,1434** (hoặc **DESKTOP-4EOK9DU\SQLEXPRESS04**)
   - Authentication: **SQL Server Authentication** (chọn từ dropdown)
   - Login: **sa**
   - Password: **123456**
   - **Remember password**: Tích ✓
   - Encryption: **Optional**
   - Click **Connect**
   
   **Instance 3 - SQLEXPRESS05:**
   - Server type: **Database Engine**
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS05,1435** (hoặc **DESKTOP-4EOK9DU\SQLEXPRESS05**)
   - Authentication: **SQL Server Authentication** (chọn từ dropdown)
   - Login: **sa**
   - Password: **123456**
   - **Remember password**: Tích ✓
   - Encryption: **Optional**
   - Click **Connect**

**Lưu ý:** 
- Nếu kết nối thành công với cả 3 instances bằng tài khoản **sa**, có nghĩa là bạn đã cấu hình đúng!

---

### ❌ XỬ LÝ LỖI: "Login failed for user 'sa'" (Error: 18456)

Nếu gặp lỗi này, làm theo các bước sau:

#### **Bước 1: Kết nối bằng Windows Authentication**
1. Trong cửa sổ **Connect to Server**:
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS03**
   - Authentication: Chọn **Windows Authentication**
   - Click **Connect**

#### **Bước 2: Bật Mixed Mode Authentication**
1. Sau khi kết nối thành công bằng Windows Authentication
2. Trong **Object Explorer**, right-click vào tên server **DESKTOP-4EOK9DU\SQLEXPRESS03**
3. Chọn **Properties**
4. Chọn **Security** (bên trái)
5. Trong phần **Server authentication**, chọn:
   - ☑ **SQL Server and Windows Authentication mode**
6. Click **OK**

#### **Bước 3: Enable tài khoản sa và đặt mật khẩu**
1. Trong **Object Explorer**, mở:
   - **Security** → **Logins**
2. Tìm tài khoản **sa**, right-click → chọn **Properties**
3. **Tab General**:
   - **QUAN TRỌNG**: Bỏ tích ✓ các checkbox sau (nếu có):
     - ☐ **Enforce password policy** (BỎ TÍCH)
     - ☐ **Enforce password expiration** (BỎ TÍCH)
     - ☐ **User must change password at next login** (BỎ TÍCH)
   - Password: **123456**
   - Confirm password: **123456**
   - **Lưu ý**: Nếu mật khẩu 123456 không được chấp nhận, dùng mật khẩu mạnh hơn như: **Admin@123**
4. **Tab Status** (bên trái):
   - Permission to connect to database engine: Chọn **Grant**
   - Login: Chọn **Enabled**
5. Click **OK**

⚠️ **Nếu nút OK không bấm được hoặc không lưu**:
   - Kiểm tra lại **Enforce password policy** đã BỎ TÍCH chưa
   - Thử đổi mật khẩu thành: **Admin@123** hoặc **P@ssw0rd**
   - Restart SSMS và thử lại

#### **Bước 4: Restart SQL Server Service**
1. Đóng SSMS
2. Mở **SQL Server Configuration Manager**:
   - Nhấn **Windows + R**
   - Gõ: `SQLServerManager15.msc`
   - Nhấn Enter
3. Vào **SQL Server Services**
4. Right-click **SQL Server (SQLEXPRESS03)** → chọn **Restart**
5. Đợi service restart xong

#### **Bước 5: Thử kết nối lại bằng sa**
1. Mở lại SSMS
2. Kết nối với:
   - Server name: **DESKTOP-4EOK9DU\SQLEXPRESS03**
   - Authentication: **SQL Server Authentication**
   - Login: **sa**
   - Password: **123456**
   - Click **Connect**

✅ **Nếu vẫn lỗi**, kiểm tra:
- SQL Server Browser Service có đang chạy không (trong SQL Server Configuration Manager)
- TCP/IP Protocol có được Enable chưa (Protocols for SQLEXPRESS03)
- Port 1433 có bị Firewall chặn không

🔄 **Lặp lại các bước trên cho SQLEXPRESS04 và SQLEXPRESS05**

### Kiểm tra Linked Servers

1. Kết nối đến **SQLEXPRESS03**
2. Chạy lệnh:
```sql
-- Kiểm tra kết nối SV_TH
EXEC sp_testlinkedserver 'SV_TH'
SELECT * FROM SV_TH.qldiemsv.dbo.DMSV

-- Kiểm tra kết nối SV_NN
EXEC sp_testlinkedserver 'SV_NN'
SELECT * FROM SV_NN.qldiemsv.dbo.DMSV

-- Xem tất cả sinh viên
SELECT * FROM V_AllStudents
```

### Kiểm tra IP của máy Desktop

1. Mở **Command Prompt**
2. Gõ: `ipconfig`
3. Ghi nhớ **IPv4 Address** (ví dụ: 192.168.1.5)
4. Cung cấp IP này cho máy Laptop để kết nối

---

## VIII. CHẠY ỨNG DỤNG WINDOWS FORMS

### Bước 1: Mở Visual Studio
1. Mở solution **WindowsFormsApp1.sln**

### Bước 2: Cập nhật Connection String (nếu cần)

1. Mở file: `DatabaseConnection.cs`
2. Kiểm tra và cập nhật:
```csharp
public static string ConnectionString_SV_MAIN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03,1433;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
public static string ConnectionString_SV_TH = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS04,1434;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
public static string ConnectionString_SV_NN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS05,1435;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
```

### Bước 3: Build và Run
1. Nhấn **F5** hoặc **Start**
2. Ứng dụng sẽ mở form đăng nhập

### Bước 4: Đăng nhập
- Server Name: `DESKTOP-4EOK9DU\SQLEXPRESS03,1433`
- Username: `sa`
- Password: `123456`

---

## IX. SỬ DỤNG ỨNG DỤNG

### Các chức năng chính:

1. **THÊM sinh viên**: 
   - Tự động kiểm tra trùng mã trên tất cả servers
   - Phân mảnh theo khoa: TH → Server 1 (SQLEXPRESS04), NN → Server 2 (SQLEXPRESS05)

2. **SỬA thông tin**: 
   - Cập nhật thông tin sinh viên
   - Tự động chuyển server nếu đổi khoa (TH ↔ NN)

3. **XÓA sinh viên**: 
   - Xóa cả kết quả học tập

4. **TÌM KIẾM**: 
   - Tìm theo mã, tên, nơi sinh
   - Tìm theo khoa (TH hoặc NN)

5. **CHUYỂN KHOA**: 
   - Chuyển sinh viên giữa Khoa Tin Học và Khoa Ngoại Ngữ
   - Tự động chuyển dữ liệu giữa Server 1 và Server 2
   - Tự động chuyển kết quả học tập

---

## X. LƯU Ý QUAN TRỌNG

### ⚠️ Nếu dùng bản Basic với instance tên SQLEXPRESS:

**Cập nhật tên instance trong các files**:

1. **File SQL Scripts**:
   - `01_Setup_LinkedServers_SQLEXPRESS03.sql`: Đổi tên SQLEXPRESS03 thành SQLEXPRESS
   
2. **File C# Code** (`DatabaseConnection.cs`):
   ```csharp
   // Đổi từ:
   public static string ConnectionString_SV_MAIN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03,1433;...";
   
   // Thành:
   public static string ConnectionString_SV_MAIN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS,1433;...";
   ```

3. **Hoặc dễ hơn**: Cài thêm 2 instances với tên SQLEXPRESS03 và SQLEXPRESS04, giữ nguyên code

### Các lưu ý khác:

1. **Firewall**: Phải mở ports 1433, 1434, 1435
2. **SQL Browser**: Không cần bật vì đã cố định ports
3. **Password**: Đổi mật khẩu sa trong thực tế
4. **Backup**: Thường xuyên backup database
5. **IP thay đổi**: Nếu IP máy Desktop thay đổi, cập nhật Connection String

### Tóm tắt các phương án:

**Phương án 1** (Khuyến nghị - Đầy đủ 3 instances):
- **SQLEXPRESS03 (Port 1433)** - Server máy chủ phân phối (dữ liệu gốc QLDiem)
- **SQLEXPRESS04 (Port 1434)** - Server 1: SINHVIEN_TH (Khoa Tin Học)
- **SQLEXPRESS05 (Port 1435)** - Server 2: SINHVIEN_NN (Khoa Ngoại Ngữ)

**Phương án 2** (Nếu dùng bản Basic):
- **SQLEXPRESS (Port 1433)** - Server máy chủ phân phối (dữ liệu gốc)
- **SQLEXPRESS03 (Port 1434)** - Server 1: SINHVIEN_TH (Khoa Tin Học)
- **SQLEXPRESS04 (Port 1435)** - Server 2: SINHVIEN_NN (Khoa Ngoại Ngữ)

**Phương án 3** (Tối giản - Chỉ 2 instances):
- **SQLEXPRESS (Port 1433)** - Server máy chủ + SINHVIEN_TH (Khoa Tin Học)
- **SQLEXPRESS03 (Port 1434)** - Server 1: SINHVIEN_NN (Khoa Ngoại Ngữ)

---

## XI. TROUBLESHOOTING

### Lỗi: Cannot connect to server
- Kiểm tra SQL Server Service đã chạy chưa
- Kiểm tra Port đã cấu hình đúng chưa
- Kiểm tra Firewall đã mở port chưa

### Lỗi: Login failed for user 'sa'
- Kiểm tra Mixed Mode đã bật chưa
- Kiểm tra tài khoản sa đã enable chưa
- Kiểm tra mật khẩu có đúng không

### Lỗi: Linked Server không kết nối được
- Kiểm tra Linked Server đã tạo đúng chưa
- Chạy lại script `01_Setup_LinkedServers_SQLEXPRESS03.sql`
- Kiểm tra credential (username/password)

---

**Hoàn thành cấu hình cho Máy Desktop!**
**Tiếp theo: Xem file `HUONG_DAN_MAY_LAPTOP.md` để cấu hình máy Laptop**
