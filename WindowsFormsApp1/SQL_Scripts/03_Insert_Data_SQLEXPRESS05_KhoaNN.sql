-- =============================================
-- Script: Insert dữ liệu phân mảnh theo Khoa  
-- CHẠY TRÊN SQLEXPRESS05 (Port 1435) - Server 2: SINHVIEN_NN (Khoa Ngoại Ngữ)
-- =============================================
USE qldiemsv
GO

set dateformat dmy
GO

-- Sinh viên Khoa Ngoại Ngữ
INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('A02',N'Trần Văn',N'Chính',0,'24/12/1992',N'Bình Định','NN',1500000)

INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('A04',N'Trần Anh',N'Tuấn',0,'20/12/1994',N'Hà Nội','NN',800000)

INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('B01',N'Trần Thanh',N'Mai',1,'12/08/1993',N'Hải Phòng','NN',0)

INSERT into DMSV(Masv,HoSv,tensv,phai,NgaySinh,NoiSinh,Makh,HocBong)
values('B02',N'Trần Thị Thu',N'Thủy',1,'02/01/1994',N'TP HCM','NN',0)
GO

-- Kết quả học tập sinh viên Khoa NN
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A02','01',1,4.5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A02','01',2,7)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A02','03',1,10)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A02','05',1,9)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('A04','05',2,10)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('B01','01',1,7)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('B01','03',1,2.5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('B01','03',2,5)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('B02','02',1,6)
INSERT into KetQua(Masv,MaMh,LanThi,Diem) values('B02','04',1,10)
GO

PRINT N'Insert dữ liệu Khoa Ngoại Ngữ (NN) trên SQLEXPRESS05 thành công!'
GO
