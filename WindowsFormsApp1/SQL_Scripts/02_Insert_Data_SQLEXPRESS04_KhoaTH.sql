-- =============================================
-- Script: Insert dữ liệu phân mảnh theo Khoa
-- CHẠY TRÊN SQLEXPRESS04 (Port 1434) - Server 1: SINHVIEN_TH (Khoa Tin Học)
-- =============================================
USE qldiemsv
GO

set dateformat dmy
GO

-- Sinh viên Khoa Tin Học
INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('A01',N'Nguyễn Thị',N'Hải',1,'23/02/1993',N'Hà Nội','TH',1300000)

INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('A03',N'Lê Thu Bạch',N'Yến',1,'21/02/1993',N'TP HCM','TH',1700000)
GO

-- Kết quả học tập sinh viên Khoa TH
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A01','01',1,3)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A01','01',2,6)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A01','02',2,6)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A01','03',1,5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A03','01',1,2)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A03','01',2,5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A03','03',1,2.5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A03','03',2,4)
GO

PRINT N'Insert dữ liệu Khoa Tin Học (TH) trên SQLEXPRESS04 thành công!'
GO
