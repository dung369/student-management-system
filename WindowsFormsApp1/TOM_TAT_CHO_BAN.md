# TÓM TẮT NHANH - CHO BẠN BẠN (MÁY LAPTOP)

---

## 🎯 MỤC ĐÍCH
Máy laptop của bạn sẽ:
- **Kết nối CHỦ YẾU** đến Server Khoa Tin Học (SQLEXPRESS04, port 1434)
- **Xem và quản lý** sinh viên Khoa Tin Học
- **CÓ CHỨC NĂNG ĐẶC BIỆT**: Chuyển sinh viên từ Khoa Tin Học sang Khoa Ngoại Ngữ

---

## ⚡ CẤU HÌNH NHANH (5 BƯỚC)

### Bước 1: Lấy thông tin từ tôi
```
IP máy Desktop của tôi: 192.168.1._____ (tôi sẽ cho bạn)
Computer Name: DESKTOP-4EOK9DU
Username: sa
Password: 123456
```

### Bước 2: Test kết nối
```powershell
# Mở PowerShell và chạy:
ping 192.168.1._____

# Test 2 ports:
telnet 192.168.1._____ 1434
telnet 192.168.1._____ 1435
```

### Bước 3: Copy ứng dụng
- Tôi sẽ copy thư mục `WindowsFormsApp1` cho bạn (qua USB hoặc mạng)
- Bạn paste vào máy (ví dụ: `C:\Projects\WindowsFormsApp1`)

### Bước 4: Sửa file DatabaseConnection.cs
Mở file `DatabaseConnection.cs` và **THAY ĐỔI IP**:

```csharp
// Thay 192.168.1.X bằng IP thực của máy tôi
public static string ConnectionString_SV_TH = @"Data Source=192.168.1.X\SQLEXPRESS04,1434;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";

public static string ConnectionString_SV_NN = @"Data Source=192.168.1.X\SQLEXPRESS05,1435;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
```

### Bước 5: Chạy ứng dụng
1. Mở Visual Studio
2. File → Open → Project/Solution → Chọn `WindowsFormsApp1.sln`
3. Nhấn **F5** để chạy
4. Đăng nhập:
   ```
   Server: 192.168.1.X\SQLEXPRESS04,1434
   Username: sa
   Password: 123456
   ```

---

## ✅ CHỨC NĂNG BẠN CÓ THỂ DÙNG

### 1. Xem danh sách sinh viên Khoa Tin Học ✅
- Bạn sẽ thấy: A01, A02, A03, A04, A10 (sinh viên Khoa TH)
- **KHÔNG** thấy: B01, B02, B03, B04 (sinh viên Khoa NN)

### 2. Thêm/Sửa/Xóa sinh viên Khoa Tin Học ✅
- Chỉ trong Khoa Tin Học
- Không được thêm vào Khoa Ngoại Ngữ

### 3. Tìm kiếm ✅
- Chỉ tìm trong Khoa Tin Học

### 4. CHUYỂN KHOA (Chức năng đặc biệt) ✅
- **TỰ ĐỘNG** phát hiện sinh viên đang ở khoa nào
- Chuyển sang khoa còn lại:
  - Đang ở **Tin Học (TH)** → Chuyển sang **Ngoại Ngữ (NN)**
  - Đang ở **Ngoại Ngữ (NN)** → Chuyển sang **Tin Học (TH)**
- Quy trình:
  1. Chọn sinh viên (ví dụ: A10 - Khoa TH)
  2. Click nút **CHUYỂN KHOA**
  3. Xác nhận "Yes"
  4. Sinh viên A10 sẽ BIẾN MẤT khỏi danh sách (đã chuyển sang Khoa NN)

**⚠️ LƯU Ý**: Sau khi chuyển khoa, bạn sẽ KHÔNG còn thấy sinh viên đó trong danh sách (nếu bạn chỉ xem 1 khoa)!

---

## ❌ CHỨC NĂNG BẠN KHÔNG THỂ DÙNG

- ❌ Xem sinh viên Khoa Ngoại Ngữ (B01, B02, ...)
- ❌ Thêm/Sửa/Xóa sinh viên Khoa Ngoại Ngữ
- ❌ Chuyển ngược từ Ngoại Ngữ → Tin Học (chỉ 1 chiều)

---

## 🔧 TROUBLESHOOTING

### Lỗi: "A network-related error"
**Giải pháp**:
1. Kiểm tra ping: `ping 192.168.1.X`
2. Hỏi tôi đã bật SQL Server chưa
3. Hỏi tôi đã mở Firewall port 1434 và 1435 chưa

### Lỗi: "Login failed"
**Giải pháp**:
- Kiểm tra lại password: `123456`
- Hỏi tôi đã enable tài khoản `sa` chưa

### Lỗi: "Cannot open database"
**Giải pháp**:
- Hỏi tôi đã tạo database `qldiemsv` trên SQLEXPRESS04 chưa

### Lỗi chuyển khoa: "Không kết nối được SQLEXPRESS05"
**Giải pháp**:
1. Test port 1435: `telnet 192.168.1.X 1435`
2. Hỏi tôi đã bật SQL Server (SQLEXPRESS05) chưa

---

## 📋 CHECKLIST (Hỏi tôi nếu chưa làm)

### Tôi (máy Desktop) phải làm:
- [ ] Bật SQL Server (SQLEXPRESS04) - port 1434
- [ ] Bật SQL Server (SQLEXPRESS05) - port 1435
- [ ] Mở Firewall port 1434
- [ ] Mở Firewall port 1435
- [ ] Enable Remote Connection
- [ ] Enable tài khoản sa với password 123456
- [ ] Tạo database qldiemsv trên cả 2 servers
- [ ] Insert dữ liệu mẫu

### Bạn (máy Laptop) phải làm:
- [ ] Cài Visual Studio 2019/2022
- [ ] Cài SQL Server Management Studio (tùy chọn)
- [ ] Copy ứng dụng từ tôi
- [ ] Sửa IP trong DatabaseConnection.cs
- [ ] Build và Run ứng dụng

---

## 🎓 DEMO NHANH

### Test 1: Xem sinh viên
1. Chạy ứng dụng → Đăng nhập
2. Bạn sẽ thấy 4-5 sinh viên Khoa Tin Học

### Test 2: Thêm sinh viên
1. Click **THÊM**
2. Nhập: MaSV=A99, Tên=Nguyen Van Test, Khoa=Tin Học
3. Lưu → Refresh → Thấy sinh viên A99

### Test 3: Chuyển khoa (quan trọng!)

**Test 3a: Chuyển từ Tin Học → Ngoại Ngữ**
1. Chọn sinh viên A10 (Khoa TH)
2. Click **CHUYỂN KHOA**
3. Xác nhận "Yes"
4. **Sinh viên A10 biến mất** (đã chuyển sang Khoa NN)
5. Hỏi tôi kiểm tra trên máy Desktop:
   ```sql
   -- Chạy trên SQLEXPRESS05 (máy Desktop)
   SELECT * FROM DMSV WHERE MaSV = 'A10' AND Makhoa = 'NN';
   -- Kết quả: Thấy sinh viên A10 với Khoa = 'NN'
   ```

**Test 3b: Chuyển từ Ngoại Ngữ → Tin Học** (nếu có quyền xem Khoa NN)
1. Chọn sinh viên B01 (Khoa NN)
2. Click **CHUYỂN KHOA**
3. Xác nhận "Yes"
4. **Sinh viên B01 biến mất** (đã chuyển sang Khoa TH)
5. Hỏi tôi kiểm tra trên máy Desktop:
   ```sql
   -- Chạy trên SQLEXPRESS04 (máy Desktop)
   SELECT * FROM DMSV WHERE MaSV = 'B01' AND Makhoa = 'TH';
   -- Kết quả: Thấy sinh viên B01 với Khoa = 'TH'
   ```

**Lưu ý**: 
- Chức năng này sử dụng method `DirectTransferHelper.TransferStudentAuto()`
- **TỰ ĐỘNG** phát hiện khoa hiện tại và chuyển sang khoa còn lại
- Kết nối TRỰC TIẾP đến 2 servers (không qua Linked Server)
- Dữ liệu được chuyển an toàn với Transaction

---

## 📞 HỎI TÔI KHI NÀO?

1. **Không ping được máy tôi** → Hỏi tôi IP chính xác
2. **Telnet port 1434/1435 không được** → Hỏi tôi mở Firewall
3. **Login failed** → Hỏi tôi password sa
4. **Không thấy dữ liệu** → Hỏi tôi đã insert dữ liệu chưa
5. **Chuyển khoa lỗi** → Hỏi tôi đã bật SQLEXPRESS05 chưa

---

## 📄 FILE CHI TIẾT

Nếu cần hướng dẫn chi tiết hơn, đọc file:
**`HUONG_DAN_MAY_LAPTOP_CHI_KHOA_TIN_HOC.md`**

File này có:
- Code mẫu đầy đủ cho chức năng chuyển khoa
- Sơ đồ kết nối
- Troubleshooting chi tiết
- Demo code từng chức năng

---

**CHÚC BẠN CẤU HÌNH THÀNH CÔNG!** 🎉

Có gì không hiểu nhắn cho tôi nhé!
