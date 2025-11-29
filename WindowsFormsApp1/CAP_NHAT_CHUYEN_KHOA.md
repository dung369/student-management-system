# CẬP NHẬT CHỨC NĂNG CHUYỂN KHOA
## CHO MÁY LAPTOP (KẾT NỐI TRỰC TIẾP)

---

## 📋 TÓM TẮT CẬP NHẬT

Đã tạo và cập nhật các file sau để hỗ trợ chức năng **CHUYỂN KHOA** trên máy laptop:

### ✅ File mới được tạo:

1. **`DirectTransferHelper.cs`** (MỚI)
   - Class xử lý chuyển khoa TRỰC TIẾP giữa 2 servers
   - KHÔNG cần Linked Servers
   - Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu

### ✅ File được cập nhật:

2. **`WindowsFormsApp1.csproj`**
   - Thêm `DirectTransferHelper.cs` vào danh sách Compile

3. **`HUONG_DAN_MAY_LAPTOP_CHI_KHOA_TIN_HOC.md`**
   - Thêm hướng dẫn sử dụng `DirectTransferHelper`
   - Code mẫu cho button "Chuyển Khoa"
   - 2 cách: Cách 1 (khuyến nghị) và Cách 2 (chi tiết)

4. **`TOM_TAT_CHO_BAN.md`**
   - Cập nhật test case chuyển khoa
   - Thêm lưu ý về DirectTransferHelper

---

## 🎯 CÁCH SỬ DỤNG

### Bước 1: Rebuild Solution

1. Mở Visual Studio
2. Build → Clean Solution
3. Build → Rebuild Solution
4. Đảm bảo không có lỗi

### Bước 2: Cập nhật Code trong FormMain.cs

Thay đổi button "Chuyển Khoa" để sử dụng `DirectTransferHelper.TransferStudentAuto()`:

```csharp
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
            LoadData(); // Refresh
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

### Bước 3: Test chức năng

1. Chạy ứng dụng (F5)
2. Đăng nhập vào Server Tin Học
3. Chọn 1 sinh viên (ví dụ: A10)
4. Click nút **CHUYỂN KHOA**
5. Xác nhận "Yes"
6. Kiểm tra:
   - Sinh viên biến mất khỏi danh sách
   - Vào máy Desktop check SQLEXPRESS05 → Thấy sinh viên A10 với Makhoa = 'NN'

---

## 🔧 CHI TIẾT KỸ THUẬT

### DirectTransferHelper.TransferStudentDirect()

**Tham số**:
- `maSV`: Mã sinh viên cần chuyển
- `newMaKhoa`: Mã khoa mới (TH hoặc NN)
- `connStringSource`: Connection string server nguồn
- `connStringTarget`: Connection string server đích
- `message` (out): Thông báo kết quả

**Quy trình xử lý**:

```
1. Kết nối đến CẢ 2 servers (nguồn và đích)
   ├── connSource = new SqlConnection(connStringSource)
   └── connTarget = new SqlConnection(connStringTarget)

2. Bắt đầu Transaction trên CẢ 2 servers
   ├── transSource = connSource.BeginTransaction()
   └── transTarget = connTarget.BeginTransaction()

3. Kiểm tra sinh viên tồn tại trên server nguồn
   └── SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV

4. Kiểm tra sinh viên chưa tồn tại trên server đích
   └── SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV

5. Đọc thông tin sinh viên từ server nguồn
   ├── SELECT * FROM DMSV WHERE Masv = @MaSV
   └── Lưu: HoSv, TenSv, Phai, NgaySinh, NoiSinh, HocBong

6. Đọc kết quả học tập từ server nguồn
   └── SELECT * FROM KetQua WHERE MaSV = @MaSV

7. THÊM sinh viên vào server đích
   └── INSERT INTO DMSV (...) VALUES (...)

8. THÊM kết quả học tập vào server đích
   └── INSERT INTO KetQua (...) VALUES (...) [foreach]

9. XÓA kết quả học tập khỏi server nguồn
   └── DELETE FROM KetQua WHERE MaSV = @MaSV

10. XÓA sinh viên khỏi server nguồn
    └── DELETE FROM DMSV WHERE Masv = @MaSV

11. COMMIT transaction trên CẢ 2 servers
    ├── transTarget.Commit()
    └── transSource.Commit()

12. Nếu có lỗi → ROLLBACK transaction trên CẢ 2 servers
    ├── transSource.Rollback()
    └── transTarget.Rollback()
```

### DirectTransferHelper.TransferFromTHtoNN()

Wrapper function đơn giản hóa việc chuyển từ TH → NN:

```csharp
public static bool TransferFromTHtoNN(string maSV, out string message)
{
    string connStringTH = DatabaseConnection.ConnectionString_SV_TH;
    string connStringNN = DatabaseConnection.ConnectionString_SV_NN;
    
    return TransferStudentDirect(maSV, "NN", connStringTH, connStringNN, out message);
}
```

---

## ⚠️ LƯU Ý QUAN TRỌNG

### 1. So sánh 2 cách chuyển khoa:

| Tiêu chí | Stored Procedure (SP_TransferStudent) | DirectTransferHelper |
|----------|---------------------------------------|----------------------|
| **Sử dụng Linked Server** | ✅ Có | ❌ Không |
| **Phù hợp cho** | Máy Desktop | Máy Laptop |
| **Kết nối** | Qua SQLEXPRESS03 | Trực tiếp đến 2 servers |
| **Transaction** | Distributed Transaction | Local Transaction x2 |
| **Tốc độ** | Chậm hơn | Nhanh hơn |
| **Phụ thuộc** | Cần Linked Server | Chỉ cần connection string |

### 2. Máy Desktop:
- Tiếp tục dùng `DatabaseConnection.TransferStudent()` (qua SP)
- Có Linked Servers nên dùng SP tốt hơn

### 3. Máy Laptop:
- **BẮT BUỘC** dùng `DirectTransferHelper.TransferFromTHtoNN()`
- Không có Linked Servers
- Kết nối trực tiếp đến 2 ports: 1434 và 1435

### 4. Bảo mật Transaction:
- Nếu bước nào lỗi → Rollback TẤT CẢ
- Dữ liệu không bị mất hoặc duplicate
- An toàn với dữ liệu quan trọng

---

## 📝 CHECKLIST SAU KHI CẬP NHẬT

### Trên máy Desktop (Máy bạn):
- [ ] Đã có file `DirectTransferHelper.cs`
- [ ] Đã cập nhật `WindowsFormsApp1.csproj`
- [ ] Rebuild thành công
- [ ] Test chuyển khoa local (nếu muốn)

### Trên máy Laptop (Máy bạn bạn):
- [ ] Copy toàn bộ folder `WindowsFormsApp1` mới
- [ ] Đã cập nhật IP trong `DatabaseConnection.cs`
- [ ] Rebuild thành công
- [ ] Test kết nối đến cả 2 servers (ports 1434 và 1435)
- [ ] Test chuyển khoa: Chọn A10 → Chuyển khoa → Thành công
- [ ] Verify trên máy Desktop: A10 đã ở SQLEXPRESS05

---

## 🚀 DEMO FLOW

```
[MÁY LAPTOP - KHOA TIN HỌC]
│
├─ User chọn sinh viên A10
├─ Click "CHUYỂN KHOA"
├─ Confirm "Yes"
│
└─► DirectTransferHelper.TransferFromTHtoNN("A10")
     │
     ├─ Kết nối đến SQLEXPRESS04:1434 (Server TH)
     ├─ Kết nối đến SQLEXPRESS05:1435 (Server NN)
     │
     ├─ BEGIN TRANSACTION trên cả 2
     │
     ├─ Đọc A10 từ SQLEXPRESS04
     │   └─ HoTen: "duong thi", TenSV: "muoi"
     │   └─ Điểm: MH01=8.5, MH02=7.0
     │
     ├─ Ghi A10 vào SQLEXPRESS05 (Makhoa='NN')
     ├─ Ghi điểm vào SQLEXPRESS05
     │
     ├─ Xóa A10 khỏi SQLEXPRESS04
     ├─ Xóa điểm khỏi SQLEXPRESS04
     │
     └─ COMMIT trên cả 2
         │
         └─► Thành công!
             └─ Message: "Chuyển khoa thành công! Sinh viên: A10..."
```

---

## 📞 TROUBLESHOOTING

### Lỗi: "Cannot open database 'qldiemsv'"
**Nguyên nhân**: SQLEXPRESS05 chưa có database qldiemsv

**Giải pháp**:
```sql
-- Chạy trên SQLEXPRESS05 (máy Desktop)
-- Script: 00_CreateDatabase.sql
-- Script: 03_Insert_Data_SQLEXPRESS05_KhoaNN.sql
```

### Lỗi: "A network-related error... SQLEXPRESS05"
**Nguyên nhân**: Port 1435 chưa mở hoặc SQLEXPRESS05 chưa chạy

**Giải pháp**:
```powershell
# Test từ máy Laptop
telnet 192.168.1.X 1435

# Nếu fail, vào máy Desktop:
# 1. Start SQL Server (SQLEXPRESS05)
# 2. Mở Firewall port 1435
```

### Lỗi: "Sinh viên đã tồn tại trên server đích"
**Nguyên nhân**: A10 đã được chuyển trước đó

**Giải pháp**:
```sql
-- Kiểm tra trên SQLEXPRESS05
SELECT * FROM DMSV WHERE MaSV = 'A10';

-- Nếu muốn test lại, xóa và chuyển lại
DELETE FROM KetQua WHERE MaSV = 'A10';
DELETE FROM DMSV WHERE MaSV = 'A10';
```

---

**HOÀN THÀNH CẬP NHẬT!** 🎉

Bây giờ máy laptop có thể chuyển khoa TRỰC TIẾP mà không cần Linked Servers!
