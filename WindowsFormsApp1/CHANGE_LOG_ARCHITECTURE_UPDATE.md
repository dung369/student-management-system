# CHANGE LOG - CẬP NHẬT KIẾN TRÚC HỆ THỐNG

## Ngày cập nhật: 2024
## Loại thay đổi: MAJOR ARCHITECTURE UPDATE

---

## 📋 TỔNG QUAN THAY ĐỔI

Hệ thống đã được cập nhật toàn diện để phản ánh đúng kiến trúc phân tán thực tế:

### Thay đổi chính:

1. **Đổi tên Khoa**: Anh Văn (AV) → Ngoại Ngữ (NN)
2. **Đổi tên Linked Servers**: SV1, SV2, SV3 → SV_MAIN, SV_TH, SV_NN
3. **Làm rõ vai trò các server**:
   - SQLEXPRESS03: Server phân phối chính (lưu dữ liệu gốc QLDiem)
   - SQLEXPRESS04: Server 1 - Khoa Tin Học (TH)
   - SQLEXPRESS05: Server 2 - Khoa Ngoại Ngữ (NN)

---

## 📂 CÁC FILE ĐÃ CẬP NHẬT

### 1. SQL Scripts (5 files)

#### ✅ `SQL_Scripts/00_CreateDatabase.sql`
**Thay đổi:**
- Bảng `DMkhoa`: Thay 'AV' → 'NN'
- TenKhoa: 'Anh Văn' → 'Ngoại Ngữ'

```sql
-- TRƯỚC:
INSERT into DMkhoa(MaKhoa,TenKhoa) values('AV',N'Anh Văn')

-- SAU:
INSERT into DMkhoa(MaKhoa,TenKhoa) values('NN',N'Ngoại Ngữ')
```

---

#### ✅ `SQL_Scripts/01_Setup_LinkedServers_SQLEXPRESS03.sql`
**Thay đổi:**
- Đổi tên Linked Server: SV1, SV2, SV3 → SV_MAIN, SV_TH, SV_NN
- Cập nhật mô tả vai trò từng server

```sql
-- TRƯỚC:
EXEC sp_addlinkedserver @server='SV1'
EXEC sp_addlinkedserver @server='SV2'

-- SAU:
EXEC sp_addlinkedserver @server='SV_TH', @datasrc='DESKTOP-4EOK9DU\SQLEXPRESS04,1434'
EXEC sp_addlinkedserver @server='SV_NN', @datasrc='DESKTOP-4EOK9DU\SQLEXPRESS05,1435'
```

---

#### ✅ `SQL_Scripts/02_Insert_Data_SQLEXPRESS04_KhoaTH.sql` (MỚI)
**Thay đổi:**
- File mới: Dữ liệu sinh viên Khoa Tin Học
- Sinh viên: A01, A03 với Makh='TH'
- Chạy trên: SQLEXPRESS04

---

#### ✅ `SQL_Scripts/03_Insert_Data_SQLEXPRESS05_KhoaNN.sql` (MỚI)
**Thay đổi:**
- File mới: Dữ liệu sinh viên Khoa Ngoại Ngữ
- Sinh viên: A02, A04, B01, B02 với Makh='NN'
- Chạy trên: SQLEXPRESS05

---

#### ✅ `SQL_Scripts/04_Create_Views_And_StoredProcedures.sql`
**Thay đổi:**

**Views:**
```sql
-- V_AllStudents: SV1, SV2 → SV_TH, SV_NN
SELECT * FROM SV_TH.qldiemsv.dbo.DMSV
UNION ALL
SELECT * FROM SV_NN.qldiemsv.dbo.DMSV

-- V_AllResults: SV1, SV2 → SV_TH, SV_NN
```

**Stored Procedures:**

1. `SP_CheckStudentExists`:
```sql
-- Kiểm tra trên SV_TH và SV_NN thay vì SV1 và SV2
IF EXISTS (SELECT 1 FROM SV_TH.qldiemsv.dbo.DMSV WHERE Masv = @MaSV)
    SET @ServerName = 'SV_TH'
```

2. `SP_InsertStudent`:
```sql
-- Thay đổi logic routing:
IF @MaKh = 'TH'
    INSERT INTO SV_TH.qldiemsv.dbo.DMSV...
ELSE IF @MaKh = 'NN'
    INSERT INTO SV_NN.qldiemsv.dbo.DMSV...
```

3. `SP_TransferStudent`:
```sql
-- Cập nhật transfer logic giữa SV_TH và SV_NN
-- Copy dữ liệu giữa SV_TH ↔ SV_NN
```

---

### 2. C# Application Code (2 files)

#### ✅ `DatabaseConnection.cs`
**Thay đổi:**

```csharp
// TRƯỚC:
public static string ConnectionString_SV1 = @"...SQLEXPRESS03,1433...";
public static string ConnectionString_SV2 = @"...SQLEXPRESS04,1434...";
public static string ConnectionString_SV3 = @"...SQLEXPRESS05,1435...";
public static string MainConnectionString = ConnectionString_SV1;

// SAU:
public static string ConnectionString_SV_MAIN = @"...SQLEXPRESS03,1433...";
public static string ConnectionString_SV_TH = @"...SQLEXPRESS04,1434...";
public static string ConnectionString_SV_NN = @"...SQLEXPRESS05,1435...";
public static string MainConnectionString = ConnectionString_SV_MAIN;
```

**Methods cập nhật:**
- `GetAllKhoa()`: SV1 → SV_MAIN
- `GetStudentResults()`: SV1 → SV_MAIN

---

#### ✅ `FormMain.cs`
**Thay đổi:**

```csharp
// TRƯỚC:
string targetServer = (maKhoa == "TH") ? "SV1" : "SV2";

// SAU:
string targetServer = (maKhoa == "TH") ? "SV_TH" : "SV_NN";
```

---

### 3. Documentation (4 files)

#### ✅ `README.md`
**Thay đổi:**
- Sơ đồ kiến trúc: Cập nhật với vai trò rõ ràng cho từng server
- Phân mảnh: TH → SV_TH, NN → SV_NN
- Demo data: Chỉ rõ server cho mỗi khoa
- Tên file SQL scripts

---

#### ✅ `HUONG_DAN_MAY_DESKTOP.md`
**Thay đổi:**
- Mô tả kiến trúc: Server phân phối, Server 1, Server 2
- Linked Server configuration: SV_MAIN, SV_TH, SV_NN
- Tên file SQL scripts: 02_KhoaTH.sql, 03_KhoaNN.sql
- Test commands: sp_testlinkedserver 'SV_TH'

---

#### ✅ `HUONG_DAN_MAY_LAPTOP.md`
**Thay đổi:**
- Connection strings: SV_MAIN, SV_TH, SV_NN
- Sơ đồ kiến trúc remote
- Ví dụ code: DatabaseConnection với tên server mới

---

#### ✅ `QUICK_START.md`
**Thay đổi:**
- Checklist: Test linked servers với SV_TH, SV_NN
- Insert data: Files mới cho TH và NN
- Test cases: Phân mảnh TH → SV_TH, NN → SV_NN
- Connection strings: SV_MAIN, SV_TH, SV_NN

---

## 🔍 KIỂM TRA SAU KHI CẬP NHẬT

### Bước 1: Rebuild Database
```sql
-- Trên cả 3 instances, chạy lại:
1. 00_CreateDatabase.sql
2. Trên SQLEXPRESS03: 01_Setup_LinkedServers_SQLEXPRESS03.sql
3. Trên SQLEXPRESS04: 02_Insert_Data_SQLEXPRESS04_KhoaTH.sql
4. Trên SQLEXPRESS05: 03_Insert_Data_SQLEXPRESS05_KhoaNN.sql
5. Trên SQLEXPRESS03: 04_Create_Views_And_StoredProcedures.sql
```

### Bước 2: Test Linked Servers
```sql
-- Trên SQLEXPRESS03:
EXEC sp_testlinkedserver 'SV_TH'      -- Phải OK
EXEC sp_testlinkedserver 'SV_NN'      -- Phải OK
```

### Bước 3: Test Views
```sql
-- Trên SQLEXPRESS03:
SELECT * FROM V_AllStudents   -- Phải thấy sinh viên từ cả TH và NN
SELECT * FROM V_AllResults    -- Phải thấy kết quả từ cả 2 servers
```

### Bước 4: Rebuild Application
```
1. Mở Visual Studio
2. Clean Solution (Ctrl+Shift+C)
3. Rebuild Solution (Ctrl+Shift+B)
4. Run (F5)
```

### Bước 5: Test Application Functions
```
✅ Login thành công
✅ Xem danh sách sinh viên (có cả TH và NN)
✅ Thêm sinh viên TH → Check SQLEXPRESS04
✅ Thêm sinh viên NN → Check SQLEXPRESS05
✅ Sửa thông tin sinh viên
✅ Chuyển khoa TH → NN (kiểm tra dữ liệu đã chuyển server)
✅ Xóa sinh viên
✅ Tìm kiếm sinh viên
✅ Xem kết quả học tập
```

---

## ⚠️ LƯU Ý QUAN TRỌNG

### Về Database:
1. **Phải chạy lại tất cả SQL scripts** theo đúng thứ tự
2. **Linked Servers cũ (SV1, SV2) phải xóa** trước khi tạo mới:
```sql
-- Trên SQLEXPRESS03:
EXEC sp_dropserver 'SV1', 'droplogins'
EXEC sp_dropserver 'SV2', 'droplogins'
```

### Về Application:
1. Phải **Clean + Rebuild** để đảm bảo code mới được compile
2. Kiểm tra `DatabaseConnection.cs` đã cập nhật connection strings
3. Test kỹ function CHUYỂN KHOA (transfer logic đã thay đổi)

### Về Network:
1. Firewall rules vẫn giữ nguyên (ports 1433, 1434, 1435)
2. Remote connection settings không thay đổi
3. Laptop client chỉ cần update connection strings trong code

---

## 📊 TÓM TẮT THỐNG KÊ

| Loại thay đổi | Số lượng |
|--------------|---------|
| SQL Scripts | 5 files |
| C# Files | 2 files |
| Documentation | 4 files |
| Views cập nhật | 2 views |
| Stored Procedures cập nhật | 3 SPs |
| Linked Servers | 2 servers (renamed) |

---

## ✅ TRẠNG THÁI HỆ THỐNG SAU CẬP NHẬT

### Kiến trúc mới:
```
SQLEXPRESS03:1433 (SV_MAIN - Server Phân Phối)
    ├── Lưu trữ dữ liệu gốc QLDiem
    ├── Quản lý Linked Servers
    ├── Chứa Views tổng hợp
    └── Chứa Stored Procedures phân mảnh
    
SQLEXPRESS04:1434 (SV_TH - Server 1)
    └── Dữ liệu Khoa Tin Học (TH)
    
SQLEXPRESS05:1435 (SV_NN - Server 2)
    └── Dữ liệu Khoa Ngoại Ngữ (NN)
```

### Phân mảnh dữ liệu:
- **Khoa TH**: Sinh viên A01, A03 → SQLEXPRESS04 (SV_TH)
- **Khoa NN**: Sinh viên A02, A04, B01, B02 → SQLEXPRESS05 (SV_NN)

---

## 🎯 KẾT LUẬN

Tất cả các files trong hệ thống đã được cập nhật đồng bộ để phản ánh đúng kiến trúc phân tán:
- **Server phân phối** (SQLEXPRESS03) quản lý và lưu trữ dữ liệu gốc
- **Server 1** (SQLEXPRESS04) lưu dữ liệu Khoa Tin Học
- **Server 2** (SQLEXPRESS05) lưu dữ liệu Khoa Ngoại Ngữ

Hệ thống giờ đây có tên gọi rõ ràng hơn (SV_MAIN, SV_TH, SV_NN) và mã khoa phù hợp hơn (NN thay vì AV).

**📌 Trước khi sử dụng, PHẢI chạy lại tất cả SQL scripts và rebuild application!**
