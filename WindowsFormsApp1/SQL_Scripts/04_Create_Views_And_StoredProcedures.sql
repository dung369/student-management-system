-- =============================================
-- Script: Tạo lại Views và Stored Procedures - PHƯƠNG PHÁP MỚI
-- CHẠY TRÊN SQLEXPRESS03 (Server Máy Chủ Phân Phối - Port 1433)
-- =============================================

USE master
GO

-- Bật RPC cho Linked Servers (quan trọng!)
EXEC sp_serveroption 'SV_TH', 'rpc', 'true'
EXEC sp_serveroption 'SV_TH', 'rpc out', 'true'
EXEC sp_serveroption 'SV_NN', 'rpc', 'true'
EXEC sp_serveroption 'SV_NN', 'rpc out', 'true'
GO

PRINT N'Đã bật RPC cho Linked Servers'
GO

USE qldiemsv
GO

-- =============================================
-- XÓA CÁC ĐỐI TƯỢNG CŨ (nếu tồn tại)
-- =============================================
IF OBJECT_ID('SP_TransferStudent', 'P') IS NOT NULL
    DROP PROCEDURE SP_TransferStudent
GO

IF OBJECT_ID('SP_InsertStudent', 'P') IS NOT NULL
    DROP PROCEDURE SP_InsertStudent
GO

IF OBJECT_ID('SP_CheckStudentExists', 'P') IS NOT NULL
    DROP PROCEDURE SP_CheckStudentExists
GO

IF OBJECT_ID('V_AllResults', 'V') IS NOT NULL
    DROP VIEW V_AllResults
GO

IF OBJECT_ID('V_AllStudents', 'V') IS NOT NULL
    DROP VIEW V_AllStudents
GO

PRINT N'Đã xóa các đối tượng cũ'
GO

-- =============================================
-- TẠO VIEW 1: V_AllStudents (sử dụng UNION ALL với bảng tạm)
-- =============================================
PRINT N'Đang tạo View V_AllStudents...'
GO

EXEC sp_executesql N'
CREATE VIEW V_AllStudents
WITH VIEW_METADATA
AS
    SELECT Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong, ''SV_TH'' as ServerName
    FROM SV_TH.qldiemsv.dbo.DMSV
    UNION ALL
    SELECT Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong, ''SV_NN'' as ServerName
    FROM SV_NN.qldiemsv.dbo.DMSV
'
GO

IF OBJECT_ID('V_AllStudents', 'V') IS NOT NULL
    PRINT N'✓ View V_AllStudents tạo thành công'
ELSE
    PRINT N'✗ View V_AllStudents THẤT BẠI'
GO

-- =============================================
-- TẠO VIEW 2: V_AllResults
-- =============================================
PRINT N'Đang tạo View V_AllResults...'
GO

EXEC sp_executesql N'
CREATE VIEW V_AllResults
WITH VIEW_METADATA
AS
    SELECT MaSV, MaMH, LanThi, Diem, ''SV_TH'' as ServerName
    FROM SV_TH.qldiemsv.dbo.KetQua
    UNION ALL
    SELECT MaSV, MaMH, LanThi, Diem, ''SV_NN'' as ServerName
    FROM SV_NN.qldiemsv.dbo.KetQua
'
GO

IF OBJECT_ID('V_AllResults', 'V') IS NOT NULL
    PRINT N'✓ View V_AllResults tạo thành công'
ELSE
    PRINT N'✗ View V_AllResults THẤT BẠI'
GO

-- =============================================
-- TẠO SP 1: SP_CheckStudentExists
-- =============================================
PRINT N'Đang tạo SP_CheckStudentExists...'
GO

EXEC sp_executesql N'
CREATE PROCEDURE SP_CheckStudentExists
    @MaSV char(3),
    @Exists bit OUTPUT,
    @ServerName varchar(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM SV_TH.qldiemsv.dbo.DMSV WHERE Masv = @MaSV)
    BEGIN
        SET @Exists = 1
        SET @ServerName = ''SV_TH''
        RETURN
    END
    
    IF EXISTS (SELECT 1 FROM SV_NN.qldiemsv.dbo.DMSV WHERE Masv = @MaSV)
    BEGIN
        SET @Exists = 1
        SET @ServerName = ''SV_NN''
        RETURN
    END
    
    SET @Exists = 0
    SET @ServerName = NULL
END
'
GO

IF OBJECT_ID('SP_CheckStudentExists', 'P') IS NOT NULL
    PRINT N'✓ SP_CheckStudentExists tạo thành công'
ELSE
    PRINT N'✗ SP_CheckStudentExists THẤT BẠI'
GO

-- =============================================
-- TẠO SP 2: SP_InsertStudent
-- =============================================
PRINT N'Đang tạo SP_InsertStudent...'
GO

EXEC sp_executesql N'
CREATE PROCEDURE SP_InsertStudent
    @MaSV char(3),
    @HoSv nvarchar(30),
    @TenSv nvarchar(10),
    @Phai bit,
    @NgaySinh datetime,
    @NoiSinh nvarchar(25),
    @MaKh char(2),
    @HocBong float,
    @Result int OUTPUT,
    @Message nvarchar(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Exists bit, @ServerName varchar(10)
    
    EXEC SP_CheckStudentExists @MaSV, @Exists OUTPUT, @ServerName OUTPUT
    
    IF @Exists = 1
    BEGIN
        SET @Result = 0
        SET @Message = N''Mã sinh viên '' + @MaSV + N'' đã tồn tại trên '' + @ServerName
        RETURN
    END
    
    IF @MaKh = ''TH''
    BEGIN
        INSERT INTO SV_TH.qldiemsv.dbo.DMSV(Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong)
        VALUES(@MaSV, @HoSv, @TenSv, @Phai, @NgaySinh, @NoiSinh, @MaKh, @HocBong)
        
        SET @Result = 1
        SET @Message = N''Thêm sinh viên thành công vào SV_TH (Khoa Tin Học)''
    END
    ELSE IF @MaKh = ''NN''
    BEGIN
        INSERT INTO SV_NN.qldiemsv.dbo.DMSV(Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong)
        VALUES(@MaSV, @HoSv, @TenSv, @Phai, @NgaySinh, @NoiSinh, @MaKh, @HocBong)
        
        SET @Result = 1
        SET @Message = N''Thêm sinh viên thành công vào SV_NN (Khoa Ngoại Ngữ)''
    END
    ELSE
    BEGIN
        SET @Result = 0
        SET @Message = N''Mã khoa không hợp lệ''
    END
END
'
GO

IF OBJECT_ID('SP_InsertStudent', 'P') IS NOT NULL
    PRINT N'✓ SP_InsertStudent tạo thành công'
ELSE
    PRINT N'✗ SP_InsertStudent THẤT BẠI'
GO

-- =============================================
-- TẠO SP 3: SP_TransferStudent
-- =============================================
PRINT N'Đang tạo SP_TransferStudent...'
GO

EXEC sp_executesql N'
CREATE PROCEDURE SP_TransferStudent
    @MaSV char(3),
    @NewMaKhoa char(2),
    @Result int OUTPUT,
    @Message nvarchar(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    DECLARE @OldMaKhoa char(2), @OldServer varchar(10), @NewServer varchar(10)
    DECLARE @HoSv nvarchar(30), @TenSv nvarchar(10), @Phai bit
    DECLARE @NgaySinh datetime, @NoiSinh nvarchar(25), @HocBong float
    
    SELECT @OldMaKhoa = Makh, @HoSv = HoSv, @TenSv = tensv, @Phai = phai,
           @NgaySinh = NgaySinh, @NoiSinh = NoiSinh, @HocBong = HocBong
    FROM SV_TH.qldiemsv.dbo.DMSV WHERE Masv = @MaSV
    
    IF @OldMaKhoa IS NOT NULL
        SET @OldServer = ''SV_TH''
    ELSE
    BEGIN
        SELECT @OldMaKhoa = Makh, @HoSv = HoSv, @TenSv = tensv, @Phai = phai,
               @NgaySinh = NgaySinh, @NoiSinh = NoiSinh, @HocBong = HocBong
        FROM SV_NN.qldiemsv.dbo.DMSV WHERE Masv = @MaSV
        
        IF @OldMaKhoa IS NOT NULL
            SET @OldServer = ''SV_NN''
    END
    
    IF @OldMaKhoa IS NULL
    BEGIN
        SET @Result = 0
        SET @Message = N''Không tìm thấy sinh viên có mã '' + @MaSV
        ROLLBACK TRANSACTION
        RETURN
    END
    
    IF @NewMaKhoa = @OldMaKhoa
    BEGIN
        SET @Result = 0
        SET @Message = N''Sinh viên đã thuộc khoa '' + @NewMaKhoa
        ROLLBACK TRANSACTION
        RETURN
    END
    
    IF @NewMaKhoa = ''TH''
        SET @NewServer = ''SV_TH''
    ELSE IF @NewMaKhoa = ''NN''
        SET @NewServer = ''SV_NN''
    ELSE
    BEGIN
        SET @Result = 0
        SET @Message = N''Mã khoa mới không hợp lệ''
        ROLLBACK TRANSACTION
        RETURN
    END
    
    BEGIN TRY
        IF @OldServer = ''SV_TH'' AND @NewServer = ''SV_NN''
        BEGIN
            INSERT INTO SV_NN.qldiemsv.dbo.KetQua(MaSV, MaMH, LanThi, Diem)
            SELECT MaSV, MaMH, LanThi, Diem FROM SV_TH.qldiemsv.dbo.KetQua WHERE MaSV = @MaSV
        END
        ELSE IF @OldServer = ''SV_NN'' AND @NewServer = ''SV_TH''
        BEGIN
            INSERT INTO SV_TH.qldiemsv.dbo.KetQua(MaSV, MaMH, LanThi, Diem)
            SELECT MaSV, MaMH, LanThi, Diem FROM SV_NN.qldiemsv.dbo.KetQua WHERE MaSV = @MaSV
        END
        
        IF @NewServer = ''SV_TH''
        BEGIN
            INSERT INTO SV_TH.qldiemsv.dbo.DMSV(Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong)
            VALUES(@MaSV, @HoSv, @TenSv, @Phai, @NgaySinh, @NoiSinh, @NewMaKhoa, @HocBong)
        END
        ELSE IF @NewServer = ''SV_NN''
        BEGIN
            INSERT INTO SV_NN.qldiemsv.dbo.DMSV(Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong)
            VALUES(@MaSV, @HoSv, @TenSv, @Phai, @NgaySinh, @NoiSinh, @NewMaKhoa, @HocBong)
        END
        
        IF @OldServer = ''SV_TH''
            DELETE FROM SV_TH.qldiemsv.dbo.KetQua WHERE MaSV = @MaSV
        ELSE IF @OldServer = ''SV_NN''
            DELETE FROM SV_NN.qldiemsv.dbo.KetQua WHERE MaSV = @MaSV
        
        IF @OldServer = ''SV_TH''
            DELETE FROM SV_TH.qldiemsv.dbo.DMSV WHERE Masv = @MaSV
        ELSE IF @OldServer = ''SV_NN''
            DELETE FROM SV_NN.qldiemsv.dbo.DMSV WHERE Masv = @MaSV
        
        COMMIT TRANSACTION
        
        SET @Result = 1
        SET @Message = N''Chuyển sinh viên '' + @MaSV + N'' từ '' + @OldServer + N'' sang '' + @NewServer + N'' thành công''
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Result = 0
        SET @Message = N''Lỗi: '' + ERROR_MESSAGE()
    END CATCH
END
'
GO

IF OBJECT_ID('SP_TransferStudent', 'P') IS NOT NULL
    PRINT N'✓ SP_TransferStudent tạo thành công'
ELSE
    PRINT N'✗ SP_TransferStudent THẤT BẠI'
GO

-- =============================================
-- KIỂM TRA KẾT QUẢ
-- =============================================
PRINT N''
PRINT N'========================================='
PRINT N'KẾT QUẢ TẠO CÁC ĐỐI TƯỢNG:'
PRINT N'========================================='

SELECT 
    type_desc as [Loại],
    name as [Tên đối tượng],
    create_date as [Ngày tạo]
FROM sys.objects
WHERE name IN ('V_AllStudents', 'V_AllResults', 'SP_CheckStudentExists', 'SP_InsertStudent', 'SP_TransferStudent')
ORDER BY type_desc, name
GO

PRINT N'========================================='
PRINT N'HOÀN THÀNH!'
PRINT N'========================================='
