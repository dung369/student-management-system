using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DatabaseConnection
    {
        // Connection strings cho 3 servers
        public static string ConnectionString_SV_MAIN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS03;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        public static string ConnectionString_SV_TH = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS04;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";
        public static string ConnectionString_SV_NN = @"Data Source=DESKTOP-4EOK9DU\SQLEXPRESS05;Initial Catalog=qldiemsv;User ID=sa;Password=123456;Connect Timeout=30";

      
        public static string MainConnectionString = ConnectionString_SV_MAIN;

        // Kiểm tra kết nối
        public static bool TestConnection(string connectionString, out string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    message = "Kết nối thành công!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi kết nối: " + ex.Message;
                return false;
            }
        }

        // Thực thi câu lệnh SELECT và trả về DataTable
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(MainConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        // Thực thi câu lệnh INSERT, UPDATE, DELETE
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(MainConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = -1;
            }
            return result;
        }

        // Thực thi Stored Procedure
        public static bool ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters, out string message)
        {
            message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(MainConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Lấy giá trị output parameters
                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output)
                            {
                                if (param.ParameterName == "@Message")
                                {
                                    message = param.Value.ToString();
                                }
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi: " + ex.Message;
                return false;
            }
        }

        // Kiểm tra sinh viên tồn tại
        public static bool CheckStudentExists(string maSV, out string serverName)
        {
            serverName = "";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV),
                new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                new SqlParameter("@ServerName", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output }
            };

            string message;
            ExecuteStoredProcedure("SP_CheckStudentExists", parameters, out message);

            bool exists = (bool)parameters[1].Value;
            if (exists)
            {
                serverName = parameters[2].Value.ToString();
            }

            return exists;
        }

        // Thêm sinh viên
        public static bool InsertStudent(string maSV, string hoSV, string tenSV, bool phai, 
            DateTime ngaySinh, string noiSinh, string maKhoa, float hocBong, out string message)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV),
                new SqlParameter("@HoSv", hoSV),
                new SqlParameter("@TenSv", tenSV),
                new SqlParameter("@Phai", phai),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@NoiSinh", noiSinh),
                new SqlParameter("@MaKh", maKhoa),
                new SqlParameter("@HocBong", hocBong),
                new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) { Direction = ParameterDirection.Output }
            };

            ExecuteStoredProcedure("SP_InsertStudent", parameters, out message);

            int result = (int)parameters[8].Value;
            message = parameters[9].Value.ToString();

            return result == 1;
        }

        // Chuyển sinh viên
        public static bool TransferStudent(string maSV, string newMaKhoa, out string message)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV),
                new SqlParameter("@NewMaKhoa", newMaKhoa),
                new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 200) { Direction = ParameterDirection.Output }
            };

            ExecuteStoredProcedure("SP_TransferStudent", parameters, out message);

            int result = (int)parameters[2].Value;
            message = parameters[3].Value.ToString();

            return result == 1;
        }

        // Lấy danh sách tất cả sinh viên
        public static DataTable GetAllStudents()
        {
            string query = "SELECT * FROM V_AllStudents ORDER BY ServerName, Masv";
            return ExecuteQuery(query);
        }

        // Lấy danh sách sinh viên theo khoa
        public static DataTable GetStudentsByKhoa(string maKhoa)
        {
            string query = "SELECT * FROM V_AllStudents WHERE Makh = @MaKhoa ORDER BY Masv";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKhoa", maKhoa)
            };
            return ExecuteQuery(query, parameters);
        }

        // Tìm kiếm sinh viên
        public static DataTable SearchStudents(string keyword)
        {
            string query = @"SELECT * FROM V_AllStudents 
                            WHERE Masv LIKE @keyword 
                            OR HoSv LIKE @keyword 
                            OR tensv LIKE @keyword
                            OR NoiSinh LIKE @keyword
                            ORDER BY ServerName, Masv";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", "%" + keyword + "%")
            };
            
            return ExecuteQuery(query, parameters);
        }

        // Lấy danh sách khoa
        public static DataTable GetAllKhoa()
        {
            string query = "SELECT MaKhoa, TenKhoa FROM SV_MAIN.qldiemsv.dbo.DMkhoa";
            return ExecuteQuery(query);
        }

        // Lấy kết quả học tập của sinh viên
        public static DataTable GetStudentResults(string maSV)
        {
            string query = @"SELECT kq.MaSV, kq.MaMH, mh.tenMh, kq.LanThi, kq.Diem, kq.ServerName
                            FROM V_AllResults kq
                            INNER JOIN SV_MAIN.qldiemsv.dbo.DMMH mh ON kq.MaMH = mh.MaMh
                            WHERE kq.MaSV = @MaSV
                            ORDER BY kq.MaMH, kq.LanThi";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV)
            };
            
            return ExecuteQuery(query, parameters);
        }
    }
}
