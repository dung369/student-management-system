-- =============================================
-- Script: Tạo Database QL_DiemSV trên tất cả các servers
-- Chạy script này trên cả 3 instances: SQLEXPRESS03, SQLEXPRESS04, SQLEXPRESS05
-- =============================================

use master
GO

-- Xóa database nếu tồn tại (đóng tất cả kết nối trước)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'qldiemsv')
BEGIN
    ALTER DATABASE qldiemsv SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE qldiemsv
    PRINT N'Đã xóa database qldiemsv cũ'
END
GO

-- Tạo database mới
CREATE DATABASE qldiemsv
GO

USE qldiemsv
GO

-- =============================================
-- Tạo các bảng
-- =============================================

-- Bảng Khoa
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMkhoa]') AND type in (N'U'))
BEGIN
    CREATE TABLE DMkhoa
    (
        MaKhoa char(2) PRIMARY KEY,
        TenKhoa nVarChar(20),
    )
    PRINT N'Đã tạo bảng DMkhoa'
END
ELSE
    PRINT N'Bảng DMkhoa đã tồn tại'
GO

-- Bảng Môn Học
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMMH]') AND type in (N'U'))
BEGIN
    CREATE TABLE DMMH
    (
        MaMh char(2) PRIMARY KEY,
        tenMh nvarchar(30),
        sotiet tinyint,
    )
    PRINT N'Đã tạo bảng DMMH'
END
ELSE
    PRINT N'Bảng DMMH đã tồn tại'
GO

-- Bảng Sinh Viên
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMSV]') AND type in (N'U'))
BEGIN
    CREATE TABLE DMSV
    (
       Masv char(3) PRIMARY KEY,
       HoSv nvarchar(30),
       tensv nvarchar(10),
       phai bit,
       NgaySinh datetime,
       NoiSinh nvarchar(25),
       Makh char(2),
       HocBong float,      
    )
    PRINT N'Đã tạo bảng DMSV'
END
ELSE
    PRINT N'Bảng DMSV đã tồn tại'
GO

-- Bảng Kết Quả
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KetQua]') AND type in (N'U'))
BEGIN
    CREATE TABLE KetQua
    (
       MaSV char(3),
       MaMH char(2),
       LanThi tinyint,
       Diem decimal(4,2),
       CONSTRAINT PK_KetQua PRIMARY KEY(MaSv, MaMh, LanThi)
    )
    PRINT N'Đã tạo bảng KetQua'
END
ELSE
    PRINT N'Bảng KetQua đã tồn tại'
GO

-- =============================================
-- Tạo các ràng buộc khóa ngoại
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SinhVien_DMkhoa')
BEGIN
    ALTER TABLE DMSV ADD CONSTRAINT FK_SinhVien_DMkhoa FOREIGN KEY(Makh) REFERENCES DMkhoa(MaKhoa)
    PRINT N'Đã tạo khóa ngoại FK_SinhVien_DMkhoa'
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_KetQua_DMSV')
BEGIN
    ALTER TABLE KetQua ADD CONSTRAINT FK_KetQua_DMSV FOREIGN KEY(MaSv) REFERENCES DMSV(MaSV)
    PRINT N'Đã tạo khóa ngoại FK_KetQua_DMSV'
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_KetQua_DMMH')
BEGIN
    ALTER TABLE KetQua ADD CONSTRAINT FK_KetQua_DMMH FOREIGN KEY(MaMH) REFERENCES DMMH(MaMh)
    PRINT N'Đã tạo khóa ngoại FK_KetQua_DMMH'
END
GO

-- =============================================
-- Insert dữ liệu Môn Học (giống nhau trên tất cả servers)
-- =============================================
IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '01')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('01',N'Cơ sở dữ liệu',45)

IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '02')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('02',N'Trí Tuệ Nhân Tạo',45)

IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '03')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('03',N'Truyền Tin',45)

IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '04')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('04',N'Đồ Họa',60)

IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '05')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('05',N'Văn Phạm',60)

IF NOT EXISTS (SELECT * FROM DMMH WHERE MaMh = '06')
    INSERT INTO DMMH (MaMh,tenMh,sotiet) VALUES('06',N'Kỹ Thuật Lập Trình',45)

PRINT N'Đã insert dữ liệu Môn Học'
GO

-- =============================================
-- Insert dữ liệu Khoa (giống nhau trên tất cả servers)
-- =============================================
IF NOT EXISTS (SELECT * FROM DMkhoa WHERE MaKhoa = 'TH')
    INSERT INTO DMkhoa(MaKhoa,TenKhoa) VALUES('TH',N'Tin Học')

IF NOT EXISTS (SELECT * FROM DMkhoa WHERE MaKhoa = 'NN')
    INSERT INTO DMkhoa(MaKhoa,TenKhoa) VALUES('NN',N'Ngoại Ngữ')

PRINT N'Đã insert dữ liệu Khoa'
GO

PRINT N'Tạo database và bảng thành công!'
GO
