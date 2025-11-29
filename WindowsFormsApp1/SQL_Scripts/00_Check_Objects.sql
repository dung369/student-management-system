-- =============================================
-- Script: Kiểm tra các đối tượng đã tạo
-- CHẠY TRÊN SQLEXPRESS03
-- =============================================

USE qldiemsv
GO

PRINT N'========================================='
PRINT N'KIỂM TRA CÁC ĐỐI TƯỢNG TRONG DATABASE'
PRINT N'========================================='
GO

-- Kiểm tra Views
PRINT N''
PRINT N'1. VIEWS:'
SELECT 
    name as ViewName,
    create_date as CreatedDate,
    modify_date as ModifiedDate
FROM sys.views
WHERE name LIKE 'V_%'
ORDER BY name
GO

-- Kiểm tra Stored Procedures
PRINT N''
PRINT N'2. STORED PROCEDURES:'
SELECT 
    name as ProcedureName,
    create_date as CreatedDate,
    modify_date as ModifiedDate
FROM sys.procedures
WHERE name LIKE 'SP_%'
ORDER BY name
GO

-- Kiểm tra tất cả objects
PRINT N''
PRINT N'3. TẤT CẢ OBJECTS:'
SELECT 
    type_desc as ObjectType,
    name as ObjectName,
    create_date as CreatedDate
FROM sys.objects
WHERE name IN ('V_AllStudents', 'V_AllResults', 'SP_CheckStudentExists', 'SP_InsertStudent', 'SP_TransferStudent')
ORDER BY type_desc, name
GO

PRINT N''
PRINT N'========================================='
PRINT N'HOÀN THÀNH KIỂM TRA'
PRINT N'========================================='
