# QUICK START GUIDE - HƯỚNG DẪN NHANH

## 📋 DANH SÁCH KIỂM TRA NHANH

---

## 🖥️ MÁY DESKTOP (Server Mẹ)

### BƯỚC 1: CÀI ĐẶT SQL SERVER (30-45 phút)
```
□ Tải SQL Server Express
□ Cài instance SQLEXPRESS03
□ Cài instance SQLEXPRESS04  
□ Cài instance SQLEXPRESS05
□ Cài SQL Server Management Studio (SSMS)
```

### BƯỚC 2: CẤU HÌNH PORTS (10 phút)
```
□ Mở SQL Server Configuration Manager
□ SQLEXPRESS03: Enable TCP/IP, Port = 1433
□ SQLEXPRESS04: Enable TCP/IP, Port = 1434
□ SQLEXPRESS05: Enable TCP/IP, Port = 1435
□ Restart tất cả 3 services
```

### BƯỚC 3: CẤU HÌNH REMOTE ACCESS (15 phút)
```
Làm cho cả 3 instances:
□ Allow remote connections
□ Enable Mixed Mode Authentication
□ Enable tài khoản sa, password: 123456
□ Restart services
```

### BƯỚC 4: MỞ FIREWALL (5 phút)
```
□ Mở Windows Firewall (wf.msc)
□ Inbound Rule: Port 1433 (TCP)
□ Inbound Rule: Port 1434 (TCP)
□ Inbound Rule: Port 1435 (TCP)
□ Outbound Rules tương tự (optional)
```

### BƯỚC 5: TẠO DATABASE (5 phút)
```
Trong SSMS, kết nối đến mỗi instance và chạy:
□ SQLEXPRESS03: 00_CreateDatabase.sql
□ SQLEXPRESS04: 00_CreateDatabase.sql
□ SQLEXPRESS05: 00_CreateDatabase.sql
```

### BƯỚC 6: CẤU HÌNH LINKED SERVERS (5 phút)
```
Chỉ trên SQLEXPRESS03:
□ Sửa password trong script nếu cần
□ Chạy: 01_Setup_LinkedServers_SQLEXPRESS03.sql
□ Test: EXEC sp_testlinkedserver 'SV_TH'
□ Test: EXEC sp_testlinkedserver 'SV_NN'
```

### BƯỚC 7: INSERT DỮ LIỆU (5 phút)
```
□ SQLEXPRESS04: 02_Insert_Data_SQLEXPRESS04_KhoaTH.sql
□ SQLEXPRESS05: 03_Insert_Data_SQLEXPRESS05_KhoaNN.sql
□ SQLEXPRESS03: 04_Create_Views_And_StoredProcedures.sql
```

### BƯỚC 8: CHẠY ỨNG DỤNG (5 phút)
```
□ Mở Visual Studio
□ Open solution: WindowsFormsApp1.sln
□ Build (Ctrl+Shift+B)
□ Run (F5)
□ Login: DESKTOP-4EOK9DU\SQLEXPRESS03,1433 / sa / 123456
```

### BƯỚC 9: LẤY IP CỦA MÁY (2 phút)
```
□ Mở Command Prompt
□ Gõ: ipconfig
□ Ghi nhớ IPv4 Address (ví dụ: 192.168.1.5)
□ Cung cấp IP này cho máy Laptop
```

**✅ HOÀN TẤT MÁY DESKTOP - Tổng thời gian: ~85 phút**

---

## 💻 MÁY LAPTOP (Client)

### BƯỚC 1: KIỂM TRA KẾT NỐI (5 phút)
```
□ Lấy IP máy Desktop từ người cấu hình
□ Ping: ping 192.168.1.X (thay X bằng IP Desktop)
□ Bật Telnet Client (Windows Features)
□ Test: telnet 192.168.1.X 1433
□ Test: telnet 192.168.1.X 1434
□ Test: telnet 192.168.1.X 1435
```

### BƯỚC 2: MỞ FIREWALL (5 phút)
```
□ Mở Windows Firewall (wf.msc)
□ Inbound Rule: Port 1433 (TCP)
□ Inbound Rule: Port 1434 (TCP)
□ Inbound Rule: Port 1435 (TCP)
```

### BƯỚC 3: COPY PROJECT (5 phút)
```
□ Copy thư mục WindowsFormsApp1 từ máy Desktop
□ Paste vào máy Laptop (ví dụ: C:\Projects\)
```

### BƯỚC 4: CÀI VISUAL STUDIO (Optional - 30 phút)
```
Nếu chưa có Visual Studio:
□ Tải Visual Studio Community
□ Cài với workload: .NET desktop development
```

### BƯỚC 5: CẬP NHẬT CONNECTION STRING (5 phút)
```
□ Mở Visual Studio
□ Open: WindowsFormsApp1.sln
□ Mở file: DatabaseConnection.cs
□ Thay DESKTOP-4EOK9DU bằng IP máy Desktop (192.168.1.X)

Ví dụ:
ConnectionString_SV_MAIN = @"Data Source=192.168.1.5\SQLEXPRESS03,1433;...";
ConnectionString_SV_TH = @"Data Source=192.168.1.5\SQLEXPRESS04,1434;...";
ConnectionString_SV_NN = @"Data Source=192.168.1.5\SQLEXPRESS05,1435;...";
```

### BƯỚC 6: BUILD VÀ RUN (5 phút)
```
□ Build: Ctrl+Shift+B
□ Run: F5
□ Login: 192.168.1.X\SQLEXPRESS03,1433 / sa / 123456
```

### BƯỚC 7: KIỂM TRA ĐỒNG BỘ (5 phút)
```
□ Trên Desktop: Thêm sinh viên mới
□ Trên Laptop: Click LÀM MỚI → Thấy sinh viên mới
□ Trên Laptop: Thêm sinh viên mới
□ Trên Desktop: Click LÀM MỚI → Thấy sinh viên mới
```

**✅ HOÀN TẤT MÁY LAPTOP - Tổng thời gian: ~30 phút (hoặc ~60 phút nếu cài VS)**

---

## 🔍 KIỂM TRA NHANH

### Test 1: Kết nối Local (Máy Desktop)
```sql
-- Trong SSMS, kết nối đến SQLEXPRESS03
-- Chạy:
SELECT * FROM V_AllStudents
-- Kết quả: Thấy sinh viên từ cả SV_TH và SV_NN
```

### Test 2: Kết nối Remote (Máy Laptop)
```
Trong SSMS trên Laptop:
Server: 192.168.1.X\SQLEXPRESS03,1433
Login: sa / 123456
→ Kết nối thành công = OK
```

### Test 3: Phân mảnh
```
Trên app:
1. Thêm sinh viên Khoa TH → Kiểm tra SQLEXPRESS04 (SV_TH)
2. Thêm sinh viên Khoa NN → Kiểm tra SQLEXPRESS05 (SV_NN)
3. Chuyển sinh viên từ TH sang NN → Kiểm tra dữ liệu đã chuyển
```

### Test 4: Đồng bộ
```
1. Desktop thêm sinh viên
2. Laptop click LÀM MỚI → Thấy ngay
3. Laptop thêm sinh viên
4. Desktop click LÀM MỚI → Thấy ngay
```

---

## ⚡ TROUBLESHOOTING NHANH

### ❌ Lỗi: Cannot connect
```
✅ Check: SQL Server service đang chạy?
✅ Check: Port đúng chưa? (1433, 1434, 1435)
✅ Check: Firewall đã mở chưa?
✅ Check: Ping được chưa?
```

### ❌ Lỗi: Login failed
```
✅ Check: Mixed Mode đã bật?
✅ Check: Tài khoản sa đã enable?
✅ Check: Password đúng chưa?
```

### ❌ Lỗi: Linked Server không hoạt động
```
✅ Chạy lại: 01_Setup_LinkedServers_SQLEXPRESS03.sql
✅ Test: EXEC sp_testlinkedserver 'SV_TH'
✅ Check: Credential đúng chưa?
```

### ❌ Dữ liệu không đồng bộ
```
✅ Click: Nút LÀM MỚI
✅ Check: Kết nối mạng ổn định?
✅ Check: Linked Servers hoạt động?
```

---

## 📊 THÔNG TIN QUAN TRỌNG

### Connection Strings:
```
Desktop (Local):
- DESKTOP-4EOK9DU\SQLEXPRESS03,1433
- DESKTOP-4EOK9DU\SQLEXPRESS04,1434
- DESKTOP-4EOK9DU\SQLEXPRESS05,1435

Laptop (Remote):
- 192.168.1.X\SQLEXPRESS03,1433
- 192.168.1.X\SQLEXPRESS04,1434
- 192.168.1.X\SQLEXPRESS05,1435
```

### Credentials:
```
Username: sa
Password: 123456
(Đổi password trong production!)
```

### Phân mảnh:
```
Server Phân Phối → SQLEXPRESS03 (Port 1433) - SV_MAIN
Khoa Tin Học (TH) → SQLEXPRESS04 (Port 1434) - SV_TH
Khoa Ngoại Ngữ (NN) → SQLEXPRESS05 (Port 1435) - SV_NN
```

---

## 📞 KHI CẦN TRỢ GIÚP

1. ✅ Đọc lại file hướng dẫn chi tiết:
   - Máy Desktop: `HUONG_DAN_MAY_DESKTOP.md`
   - Máy Laptop: `HUONG_DAN_MAY_LAPTOP.md`

2. ✅ Kiểm tra Troubleshooting section

3. ✅ Kiểm tra Checklist cuối mỗi hướng dẫn

4. ✅ Test từng bước một, không bỏ qua

---

## 🎯 MỤC TIÊU CUỐI CÙNG

```
✅ Máy Desktop: 3 instances chạy, database đã tạo, app chạy được
✅ Máy Laptop: Kết nối được đến Desktop, app chạy được
✅ Đồng bộ: Thêm/sửa/xóa trên máy nào cũng thấy trên máy kia
✅ Phân mảnh: Sinh viên TH ở SV_TH, sinh viên NN ở SV_NN
✅ Chuyển khoa: Chuyển được sinh viên giữa các server
```

---

**⏱️ TỔNG THỜI GIAN DỰ KIẾN**
- Máy Desktop: ~85 phút (1.5 giờ)
- Máy Laptop: ~30-60 phút (0.5-1 giờ)
- Tổng cộng: ~2-2.5 giờ cho cả hệ thống

**🚀 BẮT ĐẦU NGAY!**
