-- =============================================
-- Script: Test Views và Stored Procedures
-- CHẠY TRÊN SQLEXPRESS03 (Server Máy Chủ Phân Phối - Port 1433)
-- =============================================

USE qldiemsv
GO

PRINT N'========================================='
PRINT N'BẮT ĐẦU TEST VIEWS VÀ STORED PROCEDURES'
PRINT N'========================================='
GO

-- =============================================
-- Test 1: View V_AllStudents
-- =============================================
PRINT N''
PRINT N'Test 1: Xem tất cả sinh viên từ V_AllStudents'
PRINT N'-----------------------------------------'
SELECT * FROM V_AllStudents
ORDER BY ServerName, Masv
GO

-- =============================================
-- Test 2: View V_AllResults
-- =============================================
PRINT N''
PRINT N'Test 2: Xem tất cả kết quả từ V_AllResults'
PRINT N'-----------------------------------------'
SELECT * FROM V_AllResults
ORDER BY ServerName, MaSV, MaMH
GO

-- =============================================
-- Test 3: SP_CheckStudentExists - Sinh viên tồn tại
-- =============================================
PRINT N''
PRINT N'Test 3: Kiểm tra sinh viên A01 (nên tồn tại trên SV_TH)'
PRINT N'-----------------------------------------'
DECLARE @Exists bit, @ServerName varchar(10)
EXEC SP_CheckStudentExists 'A01', @Exists OUTPUT, @ServerName OUTPUT
SELECT 
    'A01' as MaSV,
    CASE WHEN @Exists = 1 THEN N'Tồn tại' ELSE N'Không tồn tại' END as TrangThai,
    @ServerName as ServerName
GO

-- =============================================
-- Test 4: SP_CheckStudentExists - Sinh viên không tồn tại
-- =============================================
PRINT N''
PRINT N'Test 4: Kiểm tra sinh viên Z99 (không tồn tại)'
PRINT N'-----------------------------------------'
DECLARE @Exists bit, @ServerName varchar(10)
EXEC SP_CheckStudentExists 'Z99', @Exists OUTPUT, @ServerName OUTPUT
SELECT 
    'Z99' as MaSV,
    CASE WHEN @Exists = 1 THEN N'Tồn tại' ELSE N'Không tồn tại' END as TrangThai,
    @ServerName as ServerName
GO

-- =============================================
-- Test 5: SP_InsertStudent - Thêm sinh viên mới vào khoa TH
-- =============================================
PRINT N''
PRINT N'Test 5: Thêm sinh viên mới C01 vào khoa TH'
PRINT N'-----------------------------------------'
DECLARE @Result int, @Message nvarchar(200)
EXEC SP_InsertStudent 
    'C01', 
    N'Trần Văn', 
    N'Cường', 
    1, 
    '2000-05-15', 
    N'Hà Nội', 
    'TH', 
    200000, 
    @Result OUTPUT, 
    @Message OUTPUT
SELECT @Result as Result, @Message as Message
GO

-- Kiểm tra sinh viên vừa thêm
PRINT N'Kiểm tra sinh viên C01 vừa thêm:'
SELECT * FROM SV_TH.qldiemsv.dbo.DMSV WHERE Masv = 'C01'
GO

-- =============================================
-- Test 6: SP_InsertStudent - Thêm sinh viên trùng mã (nên báo lỗi)
-- =============================================
PRINT N''
PRINT N'Test 6: Thêm sinh viên trùng mã A01 (nên báo lỗi)'
PRINT N'-----------------------------------------'
DECLARE @Result int, @Message nvarchar(200)
EXEC SP_InsertStudent 
    'A01', 
    N'Nguyễn Văn', 
    N'Test', 
    1, 
    '2000-01-01', 
    N'Test', 
    'TH', 
    0, 
    @Result OUTPUT, 
    @Message OUTPUT
SELECT @Result as Result, @Message as Message
GO

-- =============================================
-- Test 7: SP_TransferStudent - Chuyển sinh viên từ TH sang NN
-- =============================================
PRINT N''
PRINT N'Test 7: Chuyển sinh viên C01 từ khoa TH sang khoa NN'
PRINT N'-----------------------------------------'

-- Xem thông tin trước khi chuyển
PRINT N'Thông tin C01 TRƯỚC KHI chuyển:'
SELECT * FROM V_AllStudents WHERE Masv = 'C01'

-- Thực hiện chuyển
DECLARE @Result int, @Message nvarchar(200)
EXEC SP_TransferStudent 'C01', 'NN', @Result OUTPUT, @Message OUTPUT
SELECT @Result as Result, @Message as Message

-- Xem thông tin sau khi chuyển
PRINT N'Thông tin C01 SAU KHI chuyển:'
SELECT * FROM V_AllStudents WHERE Masv = 'C01'
GO

-- =============================================
-- Test 8: Xóa dữ liệu test
-- =============================================
PRINT N''
PRINT N'Test 8: Xóa sinh viên test C01'
PRINT N'-----------------------------------------'
DELETE FROM SV_NN.qldiemsv.dbo.DMSV WHERE Masv = 'C01'
PRINT N'Đã xóa sinh viên C01'
GO

PRINT N''
PRINT N'========================================='
PRINT N'HOÀN THÀNH TẤT CẢ TEST CASES'
PRINT N'========================================='
GO
