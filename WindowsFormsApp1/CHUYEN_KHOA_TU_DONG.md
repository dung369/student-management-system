# CHUYỂN KHOA TỰ ĐỘNG - CẬP NHẬT MỚI NHẤT

---

## 🎯 TÍNH NĂNG MỚI

### ✨ Tự động phát hiện khoa và chuyển

Button **CHUYỂN KHOA** bây giờ sẽ:

1. **Tự động kiểm tra** sinh viên đang ở khoa nào
2. **Tự động chuyển** sang khoa còn lại:
   - Đang ở **Tin Học (TH)** → Chuyển sang **Ngoại Ngữ (NN)**
   - Đang ở **Ngoại Ngữ (NN)** → Chuyển sang **Tin Học (TH)**

---

## 📝 CODE MỚI NHẤT

### FormMain.cs - Button "Chuyển Khoa"

```csharp
private void btnChuyenKhoa_Click(object sender, EventArgs e)
{
    try
    {
        // Kiểm tra đã chọn sinh viên chưa
        if (dataGridView1.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn sinh viên cần chuyển khoa!", 
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        // Lấy mã sinh viên
        string maSV = dataGridView1.SelectedRows[0].Cells["Mã SV"].Value.ToString();
        
        // Xác nhận với user
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
        
        // GỌI FUNCTION TỰ ĐỘNG
        string message;
        bool success = DirectTransferHelper.TransferStudentAuto(maSV, out message);
        
        // Hiển thị kết quả
        if (success)
        {
            MessageBox.Show(message, "Thành công", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadStudents(); // Refresh danh sách
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
```

---

## 🔄 QUY TRÌNH TỰ ĐỘNG

```
User chọn sinh viên A10 → Click "CHUYỂN KHOA"
                               ↓
              TransferStudentAuto("A10")
                               ↓
         ┌────────────────────────────────┐
         │ Bước 1: Kiểm tra trên SV_TH    │
         │ SELECT COUNT(*) FROM DMSV      │
         │ WHERE Masv = 'A10'             │
         │ → Kết quả: 1 (có sinh viên)    │
         └────────────────────────────────┘
                               ↓
         ┌────────────────────────────────┐
         │ Bước 2: Kiểm tra trên SV_NN    │
         │ SELECT COUNT(*) FROM DMSV      │
         │ WHERE Masv = 'A10'             │
         │ → Kết quả: 0 (không có)        │
         └────────────────────────────────┘
                               ↓
         ┌────────────────────────────────┐
         │ Bước 3: Xác định hướng         │
         │ A10 ở TH, không ở NN           │
         │ → Chuyển từ TH sang NN         │
         └────────────────────────────────┘
                               ↓
         ┌────────────────────────────────┐
         │ Bước 4: Thực hiện chuyển       │
         │ TransferFromTHtoNN("A10")      │
         │ - Đọc thông tin từ SV_TH       │
         │ - Ghi vào SV_NN (Makhoa='NN')  │
         │ - Xóa khỏi SV_TH               │
         └────────────────────────────────┘
                               ↓
                    ✅ THÀNH CÔNG!
         "Chuyển khoa thành công!
          Sinh viên: A10 - duong thi muoi
          Khoa mới: NN
          Số điểm đã chuyển: 2"
```

---

## 🧪 TEST CASE

### Test 1: Sinh viên A10 (Khoa TH) → Chuyển sang NN

**Before:**
```
Server TH: A10 - duong thi muoi - Khoa TH
Server NN: (không có A10)
```

**After chuyển khoa:**
```
Server TH: (không có A10)
Server NN: A10 - duong thi muoi - Khoa NN
```

### Test 2: Sinh viên B01 (Khoa NN) → Chuyển sang TH

**Before:**
```
Server TH: (không có B01)
Server NN: B01 - Tran Thanh Mai - Khoa NN
```

**After chuyển khoa:**
```
Server TH: B01 - Tran Thanh Mai - Khoa TH
Server NN: (không có B01)
```

---

## ⚠️ XỬ LÝ LỖI

### Trường hợp 1: Sinh viên không tồn tại
```
Input: Chuyển khoa A99
Kết quả: "Không tìm thấy sinh viên A99 trên bất kỳ server nào!"
```

### Trường hợp 2: Sinh viên tồn tại trên cả 2 servers (dữ liệu lỗi)
```
Input: Chuyển khoa A10
Check: A10 có trên cả SV_TH và SV_NN
Kết quả: "Lỗi: Sinh viên A10 tồn tại trên CẢ 2 servers! Cần kiểm tra dữ liệu."
```

### Trường hợp 3: Lỗi kết nối server
```
Input: Chuyển khoa A10
Check: Không kết nối được đến SV_NN
Kết quả: "Lỗi kiểm tra khoa: A network-related error..."
```

---

## 📋 CHECKLIST CẬP NHẬT

### Đã có sẵn:
- ✅ `DirectTransferHelper.cs` với method `TransferStudentAuto()`
- ✅ `WindowsFormsApp1.csproj` đã include file mới
- ✅ File hướng dẫn đã cập nhật

### Bạn cần làm:
1. ✅ Copy code mới vào `FormMain.cs` (button `btnChuyenKhoa_Click`)
2. ✅ Build → Rebuild Solution
3. ✅ Test với sinh viên A10 (TH → NN)
4. ✅ Test với sinh viên B01 (NN → TH) nếu có quyền

---

## 💡 SO SÁNH 3 METHODS

| Method | Mô tả | Khi nào dùng |
|--------|-------|--------------|
| `TransferStudentAuto()` | **TỰ ĐỘNG** phát hiện và chuyển | ✅ **KHUYẾN NGHỊ** - Dùng cho cả 2 hướng |
| `TransferFromTHtoNN()` | Chỉ chuyển TH → NN | Chỉ dùng khi chắc chắn hướng |
| `TransferFromNNtoTH()` | Chỉ chuyển NN → TH | Chỉ dùng khi chắc chắn hướng |

---

## 🚀 DEMO NHANH

```csharp
// CÁCH CŨ (phải biết trước hướng)
DirectTransferHelper.TransferFromTHtoNN("A10", out message);

// CÁCH MỚI (tự động phát hiện) ✨
DirectTransferHelper.TransferStudentAuto("A10", out message);
// ↑ Tự động biết A10 đang ở TH → Chuyển sang NN
```

---

**HOÀN THÀNH!** 🎉

Bây giờ chức năng chuyển khoa đã **THÔNG MINH** hơn - tự động phát hiện và chuyển!
