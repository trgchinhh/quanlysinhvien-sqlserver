using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace QuanLySinhVien_SQLserver {
	internal class Ketnoidulieu {
		// Bổ sung tên server SQL server của máy bạn
		// ví dụ tenserver = DESKTOP-E...\\SQLEXPRESS
		private static readonly string tenserver = Environment.GetEnvironmentVariable("TENSERVER") ?? "";
		// tên database đã được tích hợp vào file script.sql 
		// nếu đổi tên thì phải đổi trong file script
		private static readonly string tendatabse = Environment.GetEnvironmentVariable("TENDATABASE") ?? "";
		private static readonly string csdl =
			$"Server={tenserver};Database={tendatabse};Trusted_Connection=True;TrustServerCertificate=True;";

		public SqlConnection LayKetNoi() {
			var ketnoi = new SqlConnection(csdl);
			ketnoi.Open();
			return ketnoi;
		}

		public void InThongTinServer(SqlConnection ketnoi){
			Console.WriteLine("" +
				$"Database kết nối thành công !\n" +
				$"Server: {ketnoi.DataSource}\n" +
				$"Tên Database: {ketnoi.Database}\n"
			);
		}

		public bool KiemTra() {
			if(string.IsNullOrEmpty(tenserver) || string.IsNullOrEmpty(tendatabse)){
				Mau.InLoi("Chưa điền tên server/database. Vui lòng điền trong env/.env");
				return false;
			}
			try {
				using var ketnoi = LayKetNoi();
				InThongTinServer(ketnoi);
				Khaibaolog.logketnoidulieu.GhiLog(
					loglevel.SUCCESS,
					"Kết nối database thành công"
				);
				return true;				
			} catch(SqlException ex){
				Mau.InLoi($"Không kết nối được với Database: {ex.Message}");
				Khaibaolog.logketnoidulieu.GhiLog(
					loglevel.CRITICAL,
					"Kết nối database không thành công"
				);
				return false;
			} catch(Exception ex){
				Mau.InLoi($"Lỗi: {ex.Message}");
				Khaibaolog.logketnoidulieu.GhiLog(
					loglevel.ERROR,
					$"Đã xảy ra lỗi {ex.Message}"
				);
				return false;
			}
		}

		// các hàm xử lý bảng lớp 
		
		public List<Lop> LayDanhSachLopCSDL() {
			var danhsach = new List<Lop>();
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"SELECT MaLop, TenLop FROM Lop ORDER BY MaLop", ketnoi
			);
			using var doc = lenh.ExecuteReader();
			while (doc.Read()) {
				danhsach.Add(new Lop(doc.GetString(0), doc.GetString(1)));
			}
			return danhsach;
		}

		// đếm số lớp trong database 
		public int DemSoLopCSDL() {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM Lop", ketnoi);
			return Convert.ToInt32(lenh.ExecuteScalar());
		}

		// kiểm tra có tồn tại mã lớp 
		public bool KiemTraLopCSDL(string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM Lop WHERE MaLop=@m", ketnoi);
			lenh.Parameters.AddWithValue("@m", malop);
			return Convert.ToInt32(lenh.ExecuteScalar()) > 0;
		}

		// thêm lớp mới vào database 
		public void ThemLopCSDL(Lop lop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"INSERT INTO Lop (MaLop, TenLop) VALUES (@m, @t)", ketnoi
			);
			lenh.Parameters.AddWithValue("@m", lop.MaLop);
			lenh.Parameters.AddWithValue("@t", lop.TenLop);
			lenh.ExecuteNonQuery();
		}

		// các hàm xử lý bảng giảng viên 
		public List<GiangVien> LayDanhSachGiangVienCSDL() {
			var danhsach = new List<GiangVien>();
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"SELECT g.MaGiangVien, g.TenGiangVien, g.MaLop, l.TenLop " +
				"FROM GiangVien g INNER JOIN Lop l ON g.MaLop = l.MaLop " +
				"ORDER BY g.MaGiangVien", ketnoi
			);
			using var doc = lenh.ExecuteReader();
			while (doc.Read()) {
				danhsach.Add(
					new GiangVien(
						doc.GetString(0),
						doc.GetString(1),
						"",
						doc.GetString(2),
						doc.GetString(3)
					)
				);
			}
			return danhsach;
		}

		// đếm số giảng viên trong csdl
		public int DemSoGiangVienCSDL() {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM GiangVien", ketnoi);
			return Convert.ToInt32(lenh.ExecuteScalar());
		}

		// thêm giảng viên mới vào database 
		public void ThemGiangVienCSDL(GiangVien giangvien) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"INSERT INTO GiangVien (MaGiangVien, TenGiangVien, MatKhau, MaLop) " +
				"VALUES (@mgv, @tgv, @mk, @ml)", ketnoi
			);
			lenh.Parameters.AddWithValue("@mgv", giangvien.MaGiangVien);
			lenh.Parameters.AddWithValue("@tgv", giangvien.TenGiangVien);
			lenh.Parameters.Add("@mk", System.Data.SqlDbType.VarChar, 64).Value = giangvien.MatKhau;
			lenh.Parameters.AddWithValue("@ml", giangvien.MaLop);
			lenh.ExecuteNonQuery();
		}

		// kiểm tra mã giảng viên đã có trong database chưa (phục vụ đăng ký)
		public bool KiemTraGiangVienCSDL(string magiangvien) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM GiangVien WHERE MaGiangVien=@m", ketnoi);
			lenh.Parameters.AddWithValue("@m", magiangvien);
			return Convert.ToInt32(lenh.ExecuteScalar()) > 0;
		}

		// kiểm tra lớp đã có giảng viên phụ trách chưa (mỗi lớp chỉ 1 giảng viên)
		public bool KiemTraLopDaCoGiangVienCSDL(string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM GiangVien WHERE MaLop=@m", ketnoi);
			lenh.Parameters.AddWithValue("@m", malop);
			return Convert.ToInt32(lenh.ExecuteScalar()) > 0;
		}

		// kiểm tra đăng nhập, trả về giảng viên kèm lớp phụ trách (null nếu sai tài khoản)
		public GiangVien? DangNhapCSDL(string magiangvien, string matkhau) {
			string malop = "";
			string tenlop = "";
			GiangVien? ketqua = null;
			using (var ketnoi = LayKetNoi()) {
				var lenh = new SqlCommand(
					"SELECT g.MaGiangVien, g.TenGiangVien, g.MaLop, l.TenLop " +
					"FROM GiangVien g INNER JOIN Lop l ON g.MaLop = l.MaLop " +
					"WHERE g.MaGiangVien = @mgv AND g.MatKhau = @mk", ketnoi
				);
				lenh.Parameters.AddWithValue("@mgv", magiangvien);
				lenh.Parameters.AddWithValue("@mk", matkhau);
				using var doc = lenh.ExecuteReader();
				if (doc.Read()) {
					malop = doc.GetString(2);
					tenlop = doc.GetString(3);
					ketqua = new GiangVien(
						doc.GetString(0),
						doc.GetString(1),
						"",
						malop,
						tenlop
					);
				}
			}
			if (ketqua != null) {
				// gắn sĩ số của lớp vào giảng viên để in ra màn hình
				ketqua.SoSinhVienLop = this.DemSoSinhVienCSDL(malop);
			}
			return ketqua;
		}

		// số sinh viên đang có trong 1 lớp
		public int DemSoSinhVienCSDL(string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand("SELECT COUNT(*) FROM SinhVien WHERE MaLop=@m", ketnoi);
			lenh.Parameters.AddWithValue("@m", malop);
			return Convert.ToInt32(lenh.ExecuteScalar());
		}

		// các hàm xử lý bảng sinh viên (đều giới hạn theo lớp của giảng viên) 
		// hàm thêm sinh viên và database 
		public void ThemCSDL(SinhVien sinhvien, string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"INSERT INTO SinhVien (MaSoSinhVien, TenSinhVien, DiemSinhVien, MaLop, ThuTu) " +
				"VALUES (@m, @t, @d, @ml, (SELECT ISNULL(MAX(ThuTu), -1) + 1 FROM SinhVien WHERE MaLop=@ml))", ketnoi
			);
			lenh.Parameters.AddWithValue("@m", sinhvien.MaSoSinhVien);
			lenh.Parameters.AddWithValue("@t", sinhvien.TenSinhVien);
			lenh.Parameters.AddWithValue("@d", sinhvien.DiemSinhVien);
			lenh.Parameters.AddWithValue("@ml", malop);
			lenh.ExecuteNonQuery();
		}

		// hàm sửa dữ liệu 
		public bool SuaCSDL(string masosinhviencu, SinhVien sinhvien, string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"UPDATE SinhVien SET MaSoSinhVien=@m, TenSinhVien=@t, DiemSinhVien=@d " +
				"WHERE MaSoSinhVien=@cu AND MaLop=@ml", ketnoi
			);
			lenh.Parameters.AddWithValue("@m", sinhvien.MaSoSinhVien);
			lenh.Parameters.AddWithValue("@t", sinhvien.TenSinhVien);
			lenh.Parameters.AddWithValue("@d", sinhvien.DiemSinhVien);
			lenh.Parameters.AddWithValue("@cu", masosinhviencu);
			lenh.Parameters.AddWithValue("@ml", malop);
			return lenh.ExecuteNonQuery() > 0;
		}

		// hàm xóa dữ liệu 
		public bool XoaCSDL(string masosinhvien, string malop) {
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"DELETE FROM SinhVien WHERE MaSoSinhVien=@m AND MaLop=@ml", ketnoi
			);
			lenh.Parameters.AddWithValue("@m", masosinhvien);
			lenh.Parameters.AddWithValue("@ml", malop);
			return lenh.ExecuteNonQuery() > 0;
		}

		// hàm lấy danh sách theo lớp 
		public List<SinhVien> LayDanhSachCSDL(string malop) {
			var danhsach = new List<SinhVien>();
			using var ketnoi = LayKetNoi();
			var lenh = new SqlCommand(
				"SELECT TenSinhVien, MaSoSinhVien, DiemSinhVien FROM SinhVien " +
				"WHERE MaLop=@ml ORDER BY ThuTu, MaSoSinhVien", ketnoi
			);
			lenh.Parameters.AddWithValue("@ml", malop);
			using var doc = lenh.ExecuteReader();
			while (doc.Read()) {
				danhsach.Add(
					new SinhVien(
						doc.GetString(0),
						doc.GetString(1),
						doc.GetFloat(2)
					)
				);
			}
			return danhsach;
		}

		// ghi thứ tự hiện tại của list xuống database
		public void LuuThuTuCSDL(List<SinhVien> danhsach, string malop) {
			using var ketnoi = LayKetNoi();
			using var battay = ketnoi.BeginTransaction();
			try {
				for (int i = 0; i < danhsach.Count; i++) {
					var lenh = new SqlCommand(
						"UPDATE SinhVien SET ThuTu=@t WHERE MaSoSinhVien=@m AND MaLop=@ml", ketnoi, battay
					);
					lenh.Parameters.AddWithValue("@t", i+1);
					lenh.Parameters.AddWithValue("@m", danhsach[i].MaSoSinhVien);
					lenh.Parameters.AddWithValue("@ml", malop);
					lenh.ExecuteNonQuery();
				}
				battay.Commit();
			}
			catch {
				battay.Rollback();
				throw;				
			}
		}
	}
}
