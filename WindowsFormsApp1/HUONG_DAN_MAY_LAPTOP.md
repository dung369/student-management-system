# HƯỚNG DẪN CÀI ĐẶT VÀ CẤU HÌNH HỆ THỐNG
## CHO MÁY LAPTOP (MÁY CON)

---

## I. THÔNG TIN HỆ THỐNG

### Cấu hình:
- **Máy Laptop (Máy Con)**: 
  - IP: 192.168.1.10
  - 1 SQL Server Instance (chỉ để kết nối, không lưu dữ liệu phân mảnh)
  - Vai trò: Kết nối đến các server đã phân mảnh trên Máy Desktop

- **Máy Desktop (Máy Mẹ)**: 
  - IP: 192.168.1.X (hỏi người cấu hình máy Desktop)
  - Computer Name: DESKTOP-4EOK9DU
  - 3 SQL Server Instances đã cấu hình sẵn

---

## II. YÊU CẦU TRƯỚC KHI BẮT ĐẦU

1. **Kiểm tra kết nối mạng**:
   - Máy Laptop và Desktop phải cùng mạng LAN
   - Ping thử từ Laptop đến Desktop:
     ```
     ping 192.168.1.X
     ```
     (Thay X bằng IP của máy Desktop)

2. **Lấy thông tin từ máy Desktop**:
   - IP Address của máy Desktop
   - Computer Name: DESKTOP-4EOK9DU
   - Mật khẩu sa: 123456 (hoặc mật khẩu đã đặt)

---

## III. CÀI ĐẶT SQL SERVER EXPRESS (TÙY CHỌN)

> **LƯU Ý**: Máy Laptop CÓ THỂ không cần cài SQL Server. Nhưng nếu muốn test local hoặc làm việc offline thì cài.

### Nếu muốn cài SQL Server:

1. Tải SQL Server Express: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Cài đặt với 1 instance bất kỳ (ví dụ: SQLEXPRESS)
3. Cài đặt SQL Server Management Studio (SSMS)

---

## IV. MỞ PORT TRÊN WINDOWS FIREWALL (CHO PHÉP KẾT NỐI ĐẾN)

> **Quan trọng**: Mở port này để máy Desktop có thể kết nối ngược lại máy Laptop nếu cần

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

### Bước 3: Tạo thêm Rules cho Port 1434 và 1435
- Lặp lại Bước 2 cho Port **1434**
- Lặp lại Bước 2 cho Port **1435**

---

## V. KIỂM TRA KẾT NỐI ĐẾN MÁY DESKTOP

### Bước 1: Kiểm tra Ping
```cmd
ping 192.168.1.X
```
(Thay X bằng IP của máy Desktop)

### Bước 2: Kiểm tra Port bằng Telnet

1. Bật Telnet Client:
   - Mở **Control Panel** → **Programs** → **Turn Windows features on or off**
   - Tích ✓ **Telnet Client**
   - Click OK

2. Kiểm tra các ports:
```cmd
telnet 192.168.1.X 1433
telnet 192.168.1.X 1434
telnet 192.168.1.X 1435
```

Nếu màn hình đen (blank) = thành công
Nếu báo lỗi = port chưa mở hoặc firewall chặn

### Bước 3: Kiểm tra kết nối SQL Server

1. Mở **SQL Server Management Studio** (nếu đã cài)
2. Thử kết nối:
   - Server name: `192.168.1.X\SQLEXPRESS03,1433`
   - Authentication: **SQL Server Authentication**
   - Login: `sa`
   - Password: `123456`

3. Nếu kết nối thành công → Hoàn hảo!

---

## VI. CÀI ĐẶT ỨNG DỤNG WINDOWS FORMS

### Bước 1: Copy ứng dụng từ máy Desktop

1. Từ máy Desktop, copy thư mục:
   ```
   d:\New folder (10)\WindowsFormsApp1
   ```
   
2. Paste vào máy Laptop (ví dụ: `C:\Projects\WindowsFormsApp1`)

### Bước 2: Cài đặt Visual Studio (nếu chưa có)

1. Tải Visual Studio Community: https://visualstudio.microsoft.com/
2. Cài đặt với workload: **.NET desktop development**

### Bước 3: Mở Solution

1. Mở Visual Studio
2. File → Open → Project/Solution
3. Chọn file: `WindowsFormsApp1.sln`

### Bước 4: Cập nhật Connection String

1. Mở file: `DatabaseConnection.cs`

2. **QUAN TRỌNG**: Thay đổi `DESKTOP-4EOK9DU` thành **IP của máy Desktop**:

```csharp
// TRƯỚC (kết nối local trên máy Desktop):
public static string ConnectionString_SV_MAIN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03,1433;Initial Catalog=qldiemsv;User ID=sa;Password=123456";

// SAU (kết nối remote từ Laptop đến Desktop):
public static string ConnectionString_SV_MAIN = @"Data Source=192.168.1.X\SQLEXPRESS03,1433;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
public static string ConnectionString_SV_TH = @"Data Source=192.168.1.X\SQLEXPRESS04,1434;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
public static string ConnectionString_SV_NN = @"Data Source=192.168.1.X\SQLEXPRESS05,1435;Initial Catalog=qldiemsv;User ID=sa;Password=123456";
```

**Thay 192.168.1.X bằng IP thực của máy Desktop!**

### Bước 5: Build và Run

1. Nhấn **Ctrl + Shift + B** để Build
2. Nhấn **F5** để Run
3. Ứng dụng sẽ mở form đăng nhập

### Bước 6: Đăng nhập

- Server Name: `192.168.1.X\SQLEXPRESS03,1433` (IP máy Desktop)
- Username: `sa`
- Password: `123456`

---

## VII. SỬ DỤNG ỨNG DỤNG

### Các chức năng giống máy Desktop:

1. **THÊM sinh viên**: Thêm vào database phân mảnh trên máy Desktop
2. **SỬA thông tin**: Cập nhật dữ liệu trên máy Desktop
3. **XÓA sinh viên**: Xóa dữ liệu trên máy Desktop
4. **TÌM KIẾM**: Tìm kiếm trên tất cả servers
5. **CHUYỂN KHOA**: Chuyển sinh viên giữa các khoa (giữa các servers)

### Đồng bộ dữ liệu:

- **Máy Laptop thêm/sửa/xóa** → Tự động cập nhật lên **máy Desktop**
- **Máy Desktop thêm/sửa/xóa** → Máy Laptop sẽ thấy khi **Làm mới (Refresh)**

---

## VIII. KIỂM TRA ĐỒNG BỘ

### Test đồng bộ giữa 2 máy:

1. **Trên máy Desktop**:
   - Mở ứng dụng
   - Thêm 1 sinh viên mới (ví dụ: A05, Khoa TH)

2. **Trên máy Laptop**:
   - Mở ứng dụng
   - Click **LÀM MỚI**
   - Kiểm tra sinh viên A05 đã xuất hiện chưa

3. **Trên máy Laptop**:
   - Thêm sinh viên mới (ví dụ: B05, Khoa NN)

4. **Trên máy Desktop**:
   - Click **LÀM MỚI**
   - Kiểm tra sinh viên B05 đã xuất hiện chưa

**Nếu thấy dữ liệu đồng bộ = Thành công!**

---

## IX. LƯU Ý QUAN TRỌNG

### 1. Kết nối mạng:
- Máy Laptop PHẢI kết nối cùng mạng LAN với máy Desktop
- Nếu dùng WiFi, kiểm tra xem có cùng network không

### 2. IP thay đổi:
- Nếu IP máy Desktop thay đổi (do DHCP), cần:
  - Cập nhật lại Connection String trong `DatabaseConnection.cs`
  - Build lại ứng dụng

### 3. Firewall:
- Cả 2 máy đều phải mở ports 1433, 1434, 1435
- Nếu không kết nối được, thử tạm thời tắt firewall để test

### 4. VPN/Proxy:
- Nếu dùng VPN, có thể ảnh hưởng kết nối
- Thử tắt VPN khi test

### 5. SQL Server Service:
- Máy Desktop phải BẬT các SQL Server services:
  - SQL Server (SQLEXPRESS03)
  - SQL Server (SQLEXPRESS04)
  - SQL Server (SQLEXPRESS05)

---

## X. TROUBLESHOOTING

### Lỗi: A network-related or instance-specific error

**Nguyên nhân**:
- Không ping được máy Desktop
- Firewall chặn
- SQL Server service chưa chạy
- Port chưa mở

**Giải pháp**:
1. Ping máy Desktop: `ping 192.168.1.X`
2. Telnet test port: `telnet 192.168.1.X 1433`
3. Kiểm tra firewall trên cả 2 máy
4. Kiểm tra SQL Server service trên máy Desktop

### Lỗi: Login failed for user 'sa'

**Nguyên nhân**:
- Sai mật khẩu
- Tài khoản sa chưa enable
- Mixed Mode chưa bật

**Giải pháp**:
1. Kiểm tra mật khẩu sa trên máy Desktop
2. Kiểm tra Mixed Mode đã bật chưa
3. Kiểm tra tài khoản sa đã enable chưa

### Lỗi: Cannot open database "qldiemsv"

**Nguyên nhân**:
- Database chưa được tạo trên máy Desktop

**Giải pháp**:
1. Vào máy Desktop
2. Chạy lại các script SQL trong thư mục `SQL_Scripts`

---

## XI. SƠ ĐỒ KẾT NỐI

```
┌─────────────────────────────────────────┐
│        MÁY DESKTOP (MÁY MẸ)            │
│        IP: 192.168.1.X                  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ SQLEXPRESS03 (Port 1433)         │  │
│  │ - Server Phân Phối (SV_MAIN)     │  │
│  │ - Lưu trữ dữ liệu gốc QLDiem     │  │
│  │ - Linked Servers: SV_TH, SV_NN   │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ SQLEXPRESS04 (Port 1434)         │  │
│  │ - Server 1 (SV_TH)               │  │
│  │ - Khoa Tin Học                   │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ SQLEXPRESS05 (Port 1435)         │  │
│  │ - Server 2 (SV_NN)               │  │
│  │ - Khoa Ngoại Ngữ                 │  │
│  └──────────────────────────────────┘  │
└─────────────────────────────────────────┘
              ↑
              │ Kết nối remote
              │ qua TCP/IP
              │
┌─────────────────────────────────────────┐
│        MÁY LAPTOP (MÁY CON)            │
│        IP: 192.168.1.10                 │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ Windows Forms App                │  │
│  │ - Kết nối đến máy Desktop        │  │
│  │ - CRUD operations                │  │
│  │ - Tự động đồng bộ                │  │
│  └──────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

---

## XII. DEMO CONNECTION STRING CHO MÁY LAPTOP

**Ví dụ cụ thể** (giả sử IP máy Desktop là 192.168.1.5):

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
        // QUAN TRỌNG: Thay 192.168.1.5 bằng IP thực của máy Desktop!
        // ============================================
        
        public static string ConnectionString_SV_MAIN = @"Data Source=192.168.1.5\SQLEXPRESS03,1433;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        
        public static string ConnectionString_SV_TH = @"Data Source=192.168.1.5\SQLEXPRESS04,1434;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        
        public static string ConnectionString_SV_NN = @"Data Source=192.168.1.5\SQLEXPRESS05,1435;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";

        // Connection string chính (Server Phân Phối)
        public static string MainConnectionString = ConnectionString_SV_MAIN;
        
        // ... phần còn lại giữ nguyên
    }
}
```

---

**Hoàn thành cấu hình cho Máy Laptop!**
**Bây giờ cả 2 máy đã có thể làm việc với hệ thống phân mảnh!**

---

## XIII. CHECKLIST CUỐI CÙNG

### Trên máy Desktop:
- [ ] Đã cài 3 SQL Server instances
- [ ] Đã cấu hình ports 1433, 1434, 1435
- [ ] Đã mở firewall cho 3 ports
- [ ] Đã enable remote connection
- [ ] Đã kích hoạt tài khoản sa
- [ ] Đã tạo database và phân mảnh dữ liệu
- [ ] Đã tạo Linked Servers
- [ ] Ứng dụng chạy được local

### Trên máy Laptop:
- [ ] Ping được máy Desktop
- [ ] Telnet được 3 ports
- [ ] Đã cập nhật Connection String với IP Desktop
- [ ] Đã mở firewall cho 3 ports
- [ ] Ứng dụng kết nối được đến máy Desktop
- [ ] Test CRUD operations thành công
- [ ] Dữ liệu đồng bộ giữa 2 máy

**Nếu tất cả đều ✓ → HỆ THỐNG HOÀN THÀNH!**
