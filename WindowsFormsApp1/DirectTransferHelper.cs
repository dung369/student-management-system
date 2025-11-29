using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Class xử lý chuyển khoa TRỰC TIẾP giữa 2 servers
    /// Dùng cho máy laptop không kết nối qua Server Phân Phối (SQLEXPRESS03)
    /// </summary>
    public class DirectTransferHelper
    {
        /// <summary>
        /// Chuyển sinh viên từ Server này sang Server khác (KHÔNG qua Linked Server)
        /// </summary>
        /// <param name="maSV">Mã sinh viên cần chuyển</param>
        /// <param name="newMaKhoa">Mã khoa mới (TH hoặc NN)</param>
        /// <param name="connStringSource">Connection string của server nguồn</param>
        /// <param name="connStringTarget">Connection string của server đích</param>
        /// <param name="message">Thông báo kết quả</param>
        /// <returns>True nếu thành công, False nếu thất bại</returns>
        public static bool TransferStudentDirect(string maSV, string newMaKhoa, 
            string connStringSource, string connStringTarget, out string message)
        {
            SqlConnection connSource = null;
            SqlConnection connTarget = null;
            SqlTransaction transSource = null;
            SqlTransaction transTarget = null;

            try
            {
                // Bước 1: Kết nối đến cả 2 servers
                connSource = new SqlConnection(connStringSource);
                connTarget = new SqlConnection(connStringTarget);
                
                connSource.Open();
                connTarget.Open();

                // Bắt đầu transaction trên cả 2 servers
                transSource = connSource.BeginTransaction();
                transTarget = connTarget.BeginTransaction();

                // Bước 2: Kiểm tra sinh viên có tồn tại trên server nguồn không
                string checkQuery = "SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, connSource, transSource);
                cmdCheck.Parameters.AddWithValue("@MaSV", maSV);
                
                int count = (int)cmdCheck.ExecuteScalar();
                
                if (count == 0)
                {
                    message = $"Không tìm thấy sinh viên {maSV} trên server nguồn!";
                    return false;
                }

                // Bước 3: Kiểm tra sinh viên đã tồn tại trên server đích chưa
                string checkTargetQuery = "SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdCheckTarget = new SqlCommand(checkTargetQuery, connTarget, transTarget);
                cmdCheckTarget.Parameters.AddWithValue("@MaSV", maSV);
                
                int targetCount = (int)cmdCheckTarget.ExecuteScalar();
                
                if (targetCount > 0)
                {
                    message = $"Sinh viên {maSV} đã tồn tại trên server đích!";
                    return false;
                }

                // Bước 4: Lấy thông tin sinh viên từ server nguồn
                string selectQuery = "SELECT * FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdSelect = new SqlCommand(selectQuery, connSource, transSource);
                cmdSelect.Parameters.AddWithValue("@MaSV", maSV);
                
                SqlDataReader reader = cmdSelect.ExecuteReader();
                
                if (!reader.Read())
                {
                    reader.Close();
                    message = "Lỗi đọc dữ liệu sinh viên!";
                    return false;
                }

                // Lưu thông tin sinh viên
                string hoSV = reader["HoSv"].ToString();
                string tenSV = reader["tensv"].ToString();
                bool phai = Convert.ToBoolean(reader["phai"]);
                DateTime ngaySinh = Convert.ToDateTime(reader["NgaySinh"]);
                string noiSinh = reader["NoiSinh"].ToString();
                float hocBong = Convert.ToSingle(reader["HocBong"]);
                
                reader.Close();

                // Bước 5: Lấy kết quả học tập từ server nguồn
                string selectKetQuaQuery = "SELECT * FROM KetQua WHERE MaSV = @MaSV";
                SqlCommand cmdSelectKQ = new SqlCommand(selectKetQuaQuery, connSource, transSource);
                cmdSelectKQ.Parameters.AddWithValue("@MaSV", maSV);
                
                DataTable ketQuaData = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmdSelectKQ);
                adapter.Fill(ketQuaData);

                // Bước 6: Thêm sinh viên vào server đích với khoa mới
                string insertQuery = @"
                    INSERT INTO DMSV (Masv, HoSv, tensv, phai, NgaySinh, NoiSinh, Makh, HocBong)
                    VALUES (@MaSV, @HoSv, @TenSv, @Phai, @NgaySinh, @NoiSinh, @MaKh, @HocBong)";
                
                SqlCommand cmdInsert = new SqlCommand(insertQuery, connTarget, transTarget);
                cmdInsert.Parameters.AddWithValue("@MaSV", maSV);
                cmdInsert.Parameters.AddWithValue("@HoSv", hoSV);
                cmdInsert.Parameters.AddWithValue("@TenSv", tenSV);
                cmdInsert.Parameters.AddWithValue("@Phai", phai);
                cmdInsert.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmdInsert.Parameters.AddWithValue("@NoiSinh", noiSinh);
                cmdInsert.Parameters.AddWithValue("@MaKh", newMaKhoa);
                cmdInsert.Parameters.AddWithValue("@HocBong", hocBong);
                
                cmdInsert.ExecuteNonQuery();

                // Bước 7: Chuyển kết quả học tập sang server đích
                int ketQuaCount = 0;
                foreach (DataRow row in ketQuaData.Rows)
                {
                    string insertKQQuery = @"
                        INSERT INTO KetQua (MaSV, MaMH, Diem)
                        VALUES (@MaSV, @MaMH, @Diem)";
                    
                    SqlCommand cmdInsertKQ = new SqlCommand(insertKQQuery, connTarget, transTarget);
                    cmdInsertKQ.Parameters.AddWithValue("@MaSV", maSV);
                    cmdInsertKQ.Parameters.AddWithValue("@MaMH", row["MaMH"]);
                    cmdInsertKQ.Parameters.AddWithValue("@Diem", row["Diem"]);
                    
                    cmdInsertKQ.ExecuteNonQuery();
                    ketQuaCount++;
                }

                // Bước 8: Xóa kết quả học tập khỏi server nguồn
                string deleteKQQuery = "DELETE FROM KetQua WHERE MaSV = @MaSV";
                SqlCommand cmdDeleteKQ = new SqlCommand(deleteKQQuery, connSource, transSource);
                cmdDeleteKQ.Parameters.AddWithValue("@MaSV", maSV);
                cmdDeleteKQ.ExecuteNonQuery();

                // Bước 9: Xóa sinh viên khỏi server nguồn
                string deleteQuery = "DELETE FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdDelete = new SqlCommand(deleteQuery, connSource, transSource);
                cmdDelete.Parameters.AddWithValue("@MaSV", maSV);
                cmdDelete.ExecuteNonQuery();

                // Bước 10: Commit transaction trên cả 2 servers
                transTarget.Commit();
                transSource.Commit();

                message = $"Chuyển khoa thành công!\n" +
                          $"Sinh viên: {maSV} - {hoSV} {tenSV}\n" +
                          $"Khoa mới: {newMaKhoa}\n" +
                          $"Số điểm đã chuyển: {ketQuaCount}";
                
                return true;
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                try
                {
                    if (transSource != null && transSource.Connection != null)
                        transSource.Rollback();
                    
                    if (transTarget != null && transTarget.Connection != null)
                        transTarget.Rollback();
                }
                catch { }

                message = $"Lỗi chuyển khoa: {ex.Message}";
                return false;
            }
            finally
            {
                // Đóng kết nối
                if (transSource != null) transSource.Dispose();
                if (transTarget != null) transTarget.Dispose();
                if (connSource != null && connSource.State == ConnectionState.Open) connSource.Close();
                if (connTarget != null && connTarget.State == ConnectionState.Open) connTarget.Close();
            }
        }

        /// <summary>
        /// Chuyển sinh viên từ Khoa Tin Học (TH) sang Khoa Ngoại Ngữ (NN)
        /// Dùng cho máy laptop chỉ kết nối đến SQLEXPRESS04 và SQLEXPRESS05
        /// </summary>
        public static bool TransferFromTHtoNN(string maSV, out string message)
        {
            // Connection string đến Server Tin Học (nguồn)
            string connStringTH = DatabaseConnection.ConnectionString_SV_TH;
            
            // Connection string đến Server Ngoại Ngữ (đích)
            string connStringNN = DatabaseConnection.ConnectionString_SV_NN;

            return TransferStudentDirect(maSV, "NN", connStringTH, connStringNN, out message);
        }

        /// <summary>
        /// Chuyển sinh viên từ Khoa Ngoại Ngữ (NN) sang Khoa Tin Học (TH)
        /// Dùng cho máy laptop chỉ kết nối đến SQLEXPRESS04 và SQLEXPRESS05
        /// </summary>
        public static bool TransferFromNNtoTH(string maSV, out string message)
        {
            // Connection string đến Server Ngoại Ngữ (nguồn)
            string connStringNN = DatabaseConnection.ConnectionString_SV_NN;
            
            // Connection string đến Server Tin Học (đích)
            string connStringTH = DatabaseConnection.ConnectionString_SV_TH;

            return TransferStudentDirect(maSV, "TH", connStringNN, connStringTH, out message);
        }

        /// <summary>
        /// TỰ ĐỘNG phát hiện khoa hiện tại và chuyển sang khoa còn lại
        /// TH → NN hoặc NN → TH
        /// </summary>
        public static bool TransferStudentAuto(string maSV, out string message)
        {
            SqlConnection connTH = null;
            SqlConnection connNN = null;

            try
            {
                // Bước 1: Kiểm tra sinh viên đang ở server nào
                connTH = new SqlConnection(DatabaseConnection.ConnectionString_SV_TH);
                connNN = new SqlConnection(DatabaseConnection.ConnectionString_SV_NN);

                connTH.Open();
                connNN.Open();

                // Kiểm tra trên Server Tin Học
                string checkTH = "SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdCheckTH = new SqlCommand(checkTH, connTH);
                cmdCheckTH.Parameters.AddWithValue("@MaSV", maSV);
                int countTH = (int)cmdCheckTH.ExecuteScalar();

                // Kiểm tra trên Server Ngoại Ngữ
                string checkNN = "SELECT COUNT(*) FROM DMSV WHERE Masv = @MaSV";
                SqlCommand cmdCheckNN = new SqlCommand(checkNN, connNN);
                cmdCheckNN.Parameters.AddWithValue("@MaSV", maSV);
                int countNN = (int)cmdCheckNN.ExecuteScalar();

                // Đóng kết nối tạm
                connTH.Close();
                connNN.Close();

                // Bước 2: Xác định hướng chuyển
                if (countTH > 0 && countNN == 0)
                {
                    // Sinh viên đang ở Tin Học → Chuyển sang Ngoại Ngữ
                    return TransferFromTHtoNN(maSV, out message);
                }
                else if (countNN > 0 && countTH == 0)
                {
                    // Sinh viên đang ở Ngoại Ngữ → Chuyển sang Tin Học
                    return TransferFromNNtoTH(maSV, out message);
                }
                else if (countTH > 0 && countNN > 0)
                {
                    message = $"Lỗi: Sinh viên {maSV} tồn tại trên CẢ 2 servers! Cần kiểm tra dữ liệu.";
                    return false;
                }
                else
                {
                    message = $"Không tìm thấy sinh viên {maSV} trên bất kỳ server nào!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = $"Lỗi kiểm tra khoa: {ex.Message}";
                return false;
            }
            finally
            {
                if (connTH != null && connTH.State == ConnectionState.Open) connTH.Close();
                if (connNN != null && connNN.State == ConnectionState.Open) connNN.Close();
            }
        }
    }
}
