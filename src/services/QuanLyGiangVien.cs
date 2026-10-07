using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien_SQLserver {
	internal class QuanLyGiangVien {
		private List<GiangVien> danhsachgiangvien = new List<GiangVien>();
		private List<Lop> danhsachlop = new List<Lop>();
		private readonly Ketnoidulieu db = new Ketnoidulieu();
		private const int solandangnhaptoida = 3;

		// dữ liệu mẫu giảng viên + lớp phụ trách (giống file sql/script.sql)
		private static readonly List<Lop> danhsachlopmau = new List<Lop> {
			new Lop {MaLop = "OOP",  TenLop = "Lập trình hướng đối tượng"},
			new Lop {MaLop = "CSDL", TenLop = "Cơ sở dữ liệu"},
			new Lop {MaLop = "IPC",  TenLop = "Mạng máy tính"},
			new Lop {MaLop = "MH",   TenLop = "Mã hóa ứng dụng"},
			new Lop {MaLop = "KT",   TenLop = "Kỹ thuật lập trình"}
		};
		private static readonly List<GiangVien> danhsachgiangvienmau = new List<GiangVien> {
			new GiangVien {MaGiangVien = "GV001", TenGiangVien = "Nguyễn Văn An",   MatKhau = "123456", MaLop = "OOP"},
			new GiangVien {MaGiangVien = "GV002", TenGiangVien = "Trần Minh Bình",  MatKhau = "123456", MaLop = "CSDL"},
			new GiangVien {MaGiangVien = "GV003", TenGiangVien = "Lê Thị Cúc",      MatKhau = "123456", MaLop = "IPC"},
			new GiangVien {MaGiangVien = "GV004", TenGiangVien = "Phạm Quang Dũng", MatKhau = "123456", MaLop = "MH"},
			new GiangVien {MaGiangVien = "GV005", TenGiangVien = "Hoàng Thị Em",    MatKhau = "123456", MaLop = "KT"}
		};

		public QuanLyGiangVien() {
			Console.Clear();
			Console.WriteLine(HamPhuProgram.noidungbanner);
			// Nếu ko kết nối được với database thì exit 
			if(!db.KiemTra()){
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					"Khởi tạo quản lý giảng viên. Không kết nối được database, thoát chương trình"
				);
				Environment.Exit(0);
			}
			this.Napdulieulopvagiangvien();
			HamPhuProgram.DungChuongTrinh();
		}

		// nạp lại lớp + giảng viên từ database (au khi đăng ký tài khoản mới)
		public void NapLaiDanhSach() {
			this.danhsachlop = db.LayDanhSachLopCSDL();
			this.danhsachgiangvien = db.LayDanhSachGiangVienCSDL();
		}

		// nạp lớp + giảng viên, nếu database chưa có thì nạp dữ liệu mẫu
		private void Napdulieulopvagiangvien() {
			try {
				this.NapLaiDanhSach();
				if (this.danhsachlop.Count == 0) {
					Mau.InCanhBao("Database chưa có lớp và giảng viên, đang nạp dữ liệu mẫu ...");
					foreach (var lop in danhsachlopmau) {
						db.ThemLopCSDL(lop);
					}
					foreach (var gv in danhsachgiangvienmau) {
						db.ThemGiangVienCSDL(gv);
					}
					this.NapLaiDanhSach();
					Mau.InThanhCong("Đã nạp xong !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.INFO,
						$"Nạp dữ liệu mẫu. Đã nạp {danhsachlopmau.Count} lớp và {danhsachgiangvienmau.Count} giảng viên"
					);
				}
				else {
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.INFO,
						$"Nạp dữ liệu. Đã nạp {this.danhsachlop.Count} lớp và {this.danhsachgiangvien.Count} giảng viên từ database"
					);
				}
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi nạp dữ liệu giảng viên: {ex.Message}");
				Mau.InCanhBao("Hãy chạy file sql/script.sql để tạo bảng lớp và giảng viên trước !");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi nạp dữ liệu lớp và giảng viên: {ex.Message}"
				);
			}
		}

		// đăng ký tài khoản cho giảng viên mới kèm lớp phụ trách
		public GiangVien? DangKy() {
			Console.Clear();
			Console.WriteLine(HamPhuProgram.noidungbanner);
			HamPhuQuanLyGiangVien.InHuongDanDangKy();
			Khaibaolog.loggiangvien.GhiLog(
				loglevel.INFO,
				"Bắt đầu đăng ký tài khoản giảng viên"
			);
			string magiangvien;
			while (true) {
				Console.Write("(?) Nhập mã giảng viên: ");
				magiangvien = Console.ReadLine()!.Trim().ToUpper();
				if (magiangvien.Length == 0) {
					Mau.InCanhBao("Bỏ trống mã giảng viên, hủy đăng ký !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						"Hủy đăng ký. Bỏ trống mã giảng viên"
					);
					return null;
				}
				if (this.TonTaiGiangVien(magiangvien)) {
					Mau.InCanhBao("Mã giảng viên đã tồn tại, hãy nhập mã khác !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Đăng ký. Mã giảng viên {magiangvien} đã tồn tại"
					);
					continue;
				}
				break;
			}
			Console.Write("(?) Nhập tên giảng viên: ");
			string tengiangvien = Console.ReadLine()!.Trim();
			if (tengiangvien.Length == 0) {
				Mau.InCanhBao("Bỏ trống tên giảng viên, hủy đăng ký !");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.WARNING,
					$"Hủy đăng ký {magiangvien}. Bỏ trống tên giảng viên"
				);
				return null;
			}
			string matkhau;
			// nhâp mật khẩu lần 1 
			while (true) {
				matkhau = HamPhuQuanLyGiangVien.NhapMatKhau(1);
				if (matkhau.Length == 0) {
					Mau.InCanhBao("Bỏ trống mật khẩu, hủy đăng ký !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Hủy đăng ký {magiangvien}. Bỏ trống mật khẩu"
					);
					return null;
				}
				if (matkhau.Length < 6) {
					Mau.InCanhBao("Mật khẩu phải có tối thiểu 6 ký tự !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Đăng ký {magiangvien}. Mật khẩu ngắn hơn 6 ký tự"
					);
					continue;
				}
				break;
			}
			// nhập mật khẩu lần 2 xác nhận MK
			while (true) {
				string nhaplai = HamPhuQuanLyGiangVien.NhapMatKhau(2);
				if (nhaplai.Length == 0) {
					Mau.InCanhBao("Bỏ trống mật khẩu nhập lại, hủy đăng ký !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Hủy đăng ký {magiangvien}. Bỏ trống mật khẩu nhập lại"
					);
					return null;
				}
				if (nhaplai == matkhau) {
					break;
				}
				Mau.InCanhBao("Hai mật khẩu không khớp, hãy nhập lại !");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.WARNING,
					$"Đăng ký {magiangvien}. Hai mật khẩu không khớp"
				);
			}

			Lop? lop = this.NhapLopPhuTrach();
			if (lop == null) {
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.WARNING,
					$"Hủy đăng ký {magiangvien}. Không có lớp phụ trách"
				);
				return null;
			}

			var giangvienmoi = new GiangVien(magiangvien, tengiangvien, matkhau, lop.MaLop, lop.TenLop);
			giangvienmoi.SoSinhVienLop = 0;
			try {
				db.ThemGiangVienCSDL(giangvienmoi);
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi đăng ký: {ex.Message}");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi đăng ký giảng viên {magiangvien}: {ex.Message}"
				);
				return null;
			}
			this.danhsachgiangvien.Add(giangvienmoi);
			Khaibaolog.loggiangvien.GhiLog(
				loglevel.SUCCESS,
				$"Đăng ký thành công giảng viên {giangvienmoi.MaGiangVien} ({giangvienmoi.TenGiangVien}), phụ trách lớp {giangvienmoi.MaLop}"
			);
			Console.WriteLine();
			Mau.InThanhCong(
				$"Đăng ký thành công !\n" +
				$"    - Tài khoản: {giangvienmoi.MaGiangVien}\n" +
				$"    - Giảng viên: {giangvienmoi.TenGiangVien}\n" +
				$"    - Quản lý lớp: {giangvienmoi.LayTenLopDayDu()}\n" +
				$"    - Lớp mới chưa có sinh viên, hãy thêm sinh viên ở menu quản lý\n" +
				$"Hãy đăng nhập lại bằng tài khoản vừa tạo !"
			);
			return giangvienmoi;
		}

		// Các hàm chính 
		// đăng nhập bằng tài khoản giảng viên, trả về null nếu đăng nhập thất bại
		public GiangVien? DangNhap() {
			for (int lanthu = 1; lanthu <= solandangnhaptoida; lanthu++) {
				Console.Clear();
				Console.WriteLine(HamPhuProgram.noidungbanner);
				HamPhuQuanLyGiangVien.InHuongDanDangNhap(lanthu, solandangnhaptoida);

				Console.Write("(?) Nhập mã giảng viên: ");
				string magiangvien = Console.ReadLine()!.Trim().ToUpper();
				if (magiangvien.Length == 0) {
					Mau.InCanhBao("Bỏ trống mã giảng viên, hủy đăng nhập !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Hủy đăng nhập (lần {lanthu}/{solandangnhaptoida}). Bỏ trống mã giảng viên"
					);
					return null;
				}

				string matkhau = HamPhuQuanLyGiangVien.NhapMatKhau(1);
				if (matkhau.Length == 0) {
					Mau.InCanhBao("Bỏ trống mật khẩu, hủy đăng nhập !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Hủy đăng nhập {magiangvien} (lần {lanthu}/{solandangnhaptoida}). Bỏ trống mật khẩu"
					);
					return null;
				}
				string hash_matkhau = Sha256.Hash(matkhau);
				try {
					GiangVien? giangvien = db.DangNhapCSDL(magiangvien, hash_matkhau);
					if (giangvien == null) {
						Mau.InLoi("Sai mã giảng viên hoặc mật khẩu !");
						Khaibaolog.loggiangvien.GhiLog(
							loglevel.WARNING,
							$"Đăng nhập thất bại {magiangvien} (lần {lanthu}/{solandangnhaptoida}). Sai mã giảng viên hoặc mật khẩu"
						);
						HamPhuProgram.DungChuongTrinh();
						continue;
					}
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.SUCCESS,
						$"Đăng nhập thành công giảng viên {giangvien.MaGiangVien} ({giangvien.TenGiangVien}), lớp {giangvien.MaLop}"
					);
					Console.WriteLine();
					Mau.InThanhCong(
						$"Đăng nhập thành công !\n" +
						$"Chào mừng\n" +
						$"    - Giảng viên: {giangvien.TenGiangVien}\n" +
						$"    - Quản lý lớp: {giangvien.LayTenLopDayDu()}"
					);
					HamPhuProgram.DungChuongTrinh();
					return giangvien;
				}
				catch (Exception ex) {
					Mau.InLoi($"Lỗi đăng nhập: {ex.Message}");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.ERROR,
						$"Lỗi đăng nhập {magiangvien} (lần {lanthu}/{solandangnhaptoida}): {ex.Message}"
					);
					HamPhuProgram.DungChuongTrinh();
				}
			}
			Console.WriteLine();
			Mau.InLoi($"Sai quá {solandangnhaptoida} lần, vui lòng thử lại !");
			Khaibaolog.loggiangvien.GhiLog(
				loglevel.ERROR,
				$"Đăng nhập thất bại quá {solandangnhaptoida} lần"
			);
			return null;
		}

		// nhập lớp phụ trách, lớp đã có sẵn và chưa có giảng viên thì dùng luôn, còn lại tạo lớp mới
		private Lop? NhapLopPhuTrach() {
			string malop;
			while (true) {
				Console.Write("(?) Nhập mã lớp: ");
				malop = Console.ReadLine()!.Trim().ToUpper();
				if (malop.Length == 0) {
					Mau.InCanhBao("Bỏ trống mã lớp, hủy đăng ký !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						"Hủy đăng ký. Bỏ trống mã lớp"
					);
					return null;
				}
				if (this.LopDaCoGiangVien(malop)) {
					Mau.InCanhBao("Lớp đã có giảng viên phụ trách, hãy nhập mã lớp khác !");
					Khaibaolog.loggiangvien.GhiLog(
						loglevel.WARNING,
						$"Đăng ký. Lớp {malop} đã có giảng viên phụ trách"
					);
					continue;
				}
				break;
			}
			Console.Write("(?) Nhập tên lớp: ");
			string tenlop = Console.ReadLine()!.Trim();
			if (tenlop.Length == 0) {
				Mau.InCanhBao("Bỏ trống tên lớp, hủy đăng ký !");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.WARNING,
					$"Hủy đăng ký. Bỏ trống tên lớp {malop}"
				);
				return null;
			}
			var lopmoi = new Lop(malop, tenlop);
			try {
				db.ThemLopCSDL(lopmoi);
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi tạo lớp: {ex.Message}");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi tạo lớp {malop}: {ex.Message}"
				);
				return null;
			}
			this.danhsachlop.Add(lopmoi);
			Khaibaolog.loggiangvien.GhiLog(
				loglevel.SUCCESS,
				$"Tạo lớp mới thành công: {lopmoi.LayTenLopDayDu()}"
			);
			Mau.InThanhCong($"Đã tạo lớp mới {lopmoi.LayTenLopDayDu()}");
			return lopmoi;
		}

		// mã giảng viên đã tồn tại trong database chưa
		private bool TonTaiGiangVien(string magiangvien) {
			for(int i = 0; i < this.danhsachgiangvien.Count; i++){
				if(this.danhsachgiangvien[i].MaGiangVien == magiangvien){
					return true;
				}
			}
			try {
				return db.KiemTraGiangVienCSDL(magiangvien);
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi kiểm tra mã giảng viên: {ex.Message}");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi kiểm tra mã giảng viên {magiangvien}: {ex.Message}"
				);
				return true;
			}
		}

		//lớp đã có giảng viên phụ trách chưa (mỗi lớp chỉ 1 giảng viên)
		private bool LopDaCoGiangVien(string malop) {
			for(int i = 0; i < this.danhsachgiangvien.Count; i++){
				if(this.danhsachgiangvien[i].MaLop == malop){
					return true;
				}
			}
			try {
				return db.KiemTraLopDaCoGiangVienCSDL(malop);
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi kiểm tra lớp: {ex.Message}");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi kiểm tra lớp {malop}: {ex.Message}"
				);
				return true;
			}
		}

		// in thông tin tài khoản của các giảng viên kèm lớp phụ trách và sĩ số lớp
		public void XemThongTinTaiKhoanGiangVien() {
			Console.Clear();
			Console.WriteLine(HamPhuProgram.noidungbanner);
			try {
				this.NapLaiDanhSach();
				Mau.InThanhCong("Thông tin tài khoản giảng viên\n");
				Console.WriteLine(
					$"{"STT",-5} {"Mã GV",-10} {"Tên giảng viên",-25} {"Mã lớp",-10} {"Tên lớp",-28} {"Sĩ số lớp",-12}"
				);
				Console.WriteLine(new string('-', 95));
				for (int i = 0; i < this.danhsachgiangvien.Count; i++) {
					this.danhsachgiangvien[i].XuatThongTinTaiKhoan(
						i + 1, 
						db.DemSoSinhVienCSDL(this.danhsachgiangvien[i].MaLop)
					);
				}
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi xem thông tin tài khoản: {ex.Message}");
				Khaibaolog.loggiangvien.GhiLog(
					loglevel.ERROR,
					$"Lỗi xem thông tin tài khoản giảng viên: {ex.Message}"
				);
				return;
			}
			Khaibaolog.loggiangvien.GhiLog(
				loglevel.INFO,
				$"Xem thông tin tài khoản giảng viên. {this.danhsachlop.Count} lớp, {this.danhsachgiangvien.Count} tài khoản"
			);
			Console.WriteLine();
			Mau.InThanhCong(
				$"Tổng cộng {this.danhsachlop.Count} lớp và {this.danhsachgiangvien.Count} tài khoản giảng viên"
			);
		}
	}
}