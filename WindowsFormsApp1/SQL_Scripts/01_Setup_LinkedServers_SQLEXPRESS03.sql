-- =============================================
-- Script: Cấu hình Linked Server trên SQLEXPRESS03 (Server Máy Chủ Phân Phối - Port 1433)
-- Chạy script này TRÊN SQLEXPRESS03
-- =============================================

USE master
GO

-- =============================================
-- Xóa các Linked Server cũ nếu tồn tại
-- =============================================
IF EXISTS (SELECT * FROM sys.servers WHERE name = 'SV_MAIN')
BEGIN
    EXEC sp_dropserver 'SV_MAIN', 'droplogins'
    PRINT N'Đã xóa SV_MAIN'
END
GO

IF EXISTS (SELECT * FROM sys.servers WHERE name = 'SV_TH')
BEGIN
    EXEC sp_dropserver 'SV_TH', 'droplogins'
    PRINT N'Đã xóa SV_TH'
END
GO

IF EXISTS (SELECT * FROM sys.servers WHERE name = 'SV_NN')
BEGIN
    EXEC sp_dropserver 'SV_NN', 'droplogins'
    PRINT N'Đã xóa SV_NN'
END
GO

-- Đợi một chút để đảm bảo đã xóa hoàn toàn
WAITFOR DELAY '00:00:02'
GO

-- =============================================
-- Tạo Linked Server SV_MAIN -> SQLEXPRESS03 (chính nó - Port 1433)
-- Server máy chủ phân phối
-- =============================================
EXEC sp_addlinkedserver   
    @server=N'SV_MAIN',
    @srvproduct=N'',
    @provider=N'SQLOLEDB',
    @datasrc=N'DESKTOP-4EOK9DU\SQLEXPRESS03'
GO

EXEC sp_addlinkedsrvlogin 
    @rmtsrvname=N'SV_MAIN',
    @useself=N'false',
    @rmtuser=N'sa',
    @rmtpassword=N'123456'
GO

-- Cấu hình options cho SV_MAIN
EXEC sp_serveroption N'SV_MAIN', N'rpc', N'true'
EXEC sp_serveroption N'SV_MAIN', N'rpc out', N'true'
EXEC sp_serveroption N'SV_MAIN', N'data access', N'true'
GO

PRINT N'✓ Đã tạo Linked Server SV_MAIN -> DESKTOP-4EOK9DU\SQLEXPRESS03'
GO

-- =============================================
-- Tạo Linked Server SV_TH -> SQLEXPRESS04 (Port 1434)
-- Server 1 - SINHVIEN_TH (Khoa Tin Học)
-- =============================================
EXEC sp_addlinkedserver   
    @server=N'SV_TH',
    @srvproduct=N'',
    @provider=N'SQLOLEDB',
    @datasrc=N'DESKTOP-4EOK9DU\SQLEXPRESS04'
GO

EXEC sp_addlinkedsrvlogin 
    @rmtsrvname=N'SV_TH',
    @useself=N'false',
    @rmtuser=N'sa',
    @rmtpassword=N'123456'
GO

-- Cấu hình options cho SV_TH
EXEC sp_serveroption N'SV_TH', N'rpc', N'true'
EXEC sp_serveroption N'SV_TH', N'rpc out', N'true'
EXEC sp_serveroption N'SV_TH', N'data access', N'true'
GO

PRINT N'✓ Đã tạo Linked Server SV_TH -> DESKTOP-4EOK9DU\SQLEXPRESS04'
GO

-- =============================================
-- Tạo Linked Server SV_NN -> SQLEXPRESS05 (Port 1435)
-- Server 2 - SINHVIEN_NN (Khoa Ngoại Ngữ)
-- =============================================
EXEC sp_addlinkedserver   
    @server=N'SV_NN',
    @srvproduct=N'',
    @provider=N'SQLOLEDB',
    @datasrc=N'DESKTOP-4EOK9DU\SQLEXPRESS05'
GO

EXEC sp_addlinkedsrvlogin 
    @rmtsrvname=N'SV_NN',
    @useself=N'false',
    @rmtuser=N'sa',
    @rmtpassword=N'123456'
GO

-- Cấu hình options cho SV_NN
EXEC sp_serveroption N'SV_NN', N'rpc', N'true'
EXEC sp_serveroption N'SV_NN', N'rpc out', N'true'
EXEC sp_serveroption N'SV_NN', N'data access', N'true'
GO

PRINT N'✓ Đã tạo Linked Server SV_NN -> DESKTOP-4EOK9DU\SQLEXPRESS05'
GO

-- =============================================
-- Kiểm tra kết nối bằng cách query trực tiếp
-- =============================================
PRINT N''
PRINT N'========================================='
PRINT N'KIỂM TRA KẾT NỐI LINKED SERVERS'
PRINT N'========================================='
GO

BEGIN TRY
    SELECT 'SV_MAIN: OK' AS Status, @@SERVERNAME AS ServerName 
    FROM SV_MAIN.master.sys.databases WHERE name = 'master'
END TRY
BEGIN CATCH
    PRINT 'SV_MAIN: FAILED - ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    SELECT 'SV_TH: OK' AS Status, @@SERVERNAME AS ServerName 
    FROM SV_TH.master.sys.databases WHERE name = 'master'
END TRY
BEGIN CATCH
    PRINT 'SV_TH: FAILED - ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    SELECT 'SV_NN: OK' AS Status, @@SERVERNAME AS ServerName 
    FROM SV_NN.master.sys.databases WHERE name = 'master'
END TRY
BEGIN CATCH
    PRINT 'SV_NN: FAILED - ' + ERROR_MESSAGE()
END CATCH
GO

-- Kiểm tra cấu hình Linked Server
PRINT N''
PRINT N'========================================='
PRINT N'THÔNG TIN LINKED SERVERS'
PRINT N'========================================='
SELECT 
    name AS [Linked Server],
    data_source AS [Data Source],
    provider AS [Provider],
    is_rpc_out_enabled AS [RPC Out]
FROM sys.servers 
WHERE name IN ('SV_MAIN', 'SV_TH', 'SV_NN')
ORDER BY name
GO

PRINT N''
PRINT N'========================================='
PRINT N'HOÀN THÀNH!'
PRINT N'========================================='
PRINT N'SV_MAIN (Port 1433) -> DESKTOP-4EOK9DU\SQLEXPRESS03'
PRINT N'SV_TH (Port 1434) -> DESKTOP-4EOK9DU\SQLEXPRESS04'
PRINT N'SV_NN (Port 1435) -> DESKTOP-4EOK9DU\SQLEXPRESS05'
GO
