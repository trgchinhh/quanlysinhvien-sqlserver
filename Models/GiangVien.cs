using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
	internal class GiangVien {
		private string magiangvien = "";
		private string tengiangvien = "";
		private string matkhau = "";
		private string malop = "";
		private string tenlop = "";
		private int sosinhvienlop = 0;

		public GiangVien() {
			this.magiangvien = "";
			this.tengiangvien = "";
			this.matkhau = "";
			this.malop = "";
			this.tenlop = "";
			this.sosinhvienlop = 0;
		}

		public GiangVien(string magiangvien, string tengiangvien, string matkhau, string malop, string tenlop) {
			this.MaGiangVien = magiangvien;
			this.TenGiangVien = tengiangvien;
			this.MatKhau = matkhau;
			this.MaLop = malop;
			this.TenLop = tenlop;
		}

		public string MaGiangVien {
			get { return this.magiangvien; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.magiangvien = value;
				}
			}
		}

		public string TenGiangVien {
			get { return this.tengiangvien; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.tengiangvien = value;
				}
			}
		}

		// mật khẩu chỉ dùng lúc đăng nhập, không in ra màn hình
		public string MatKhau {
			get { return this.matkhau; }
			set
			{
				//if (!string.IsNullOrEmpty(value)) {
				//	this.matkhau = value;
				//}
				if (!string.IsNullOrEmpty(value)) {
					this.matkhau = Sha256.Hash(value);
				}
			}
		}

		// khóa ngoại nối sang bảng lớp
		public string MaLop {
			get { return this.malop; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.malop = value;
				}
			}
		}

		public string TenLop {
			get { return this.tenlop; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.tenlop = value;
				}
			}
		}

		// số sinh viên đang có trong lớp của giảng viên
		public int SoSinhVienLop {
			get { return this.sosinhvienlop; }
			set
			{
				if (value >= 0) {
					this.sosinhvienlop = value;
				}
			}
		}

		// Phương thức 
		// lớp mà giảng viên phụ trách
		public string LayTenLopDayDu() {
			return $"{this.MaLop} - {this.TenLop}";
		}

		// in thông tin giảng viên đang đăng nhập (dùng ở đầu chương trình)
		public void XuatThongTin() {
			Console.WriteLine("Giảng viên");
			Console.Write("Tên: ");
			Mau.ToMau($"{this.TenGiangVien}", Mau.danhsachmau[1], true);
			Console.Write("Lớp: ");
			Mau.ToMau(this.LayTenLopDayDu(), Mau.danhsachmau[3], true);
			Console.Write("Sĩ số: ");
			Mau.ToMau($"{this.SoSinhVienLop}", Mau.danhsachmau[4]);
			Console.WriteLine(" (sinh viên)");

		}

		// in thông tin tài khoản giảng viên + lớp phụ trách (dùng ở màn hình đăng nhập)
		public void XuatThongTinTaiKhoan(int sothutu, int sosinhvienlop) {
			Console.WriteLine(
				"{0,-5} {1,-10} {2,-25} {3,-10} {4,-28} {5,-12}",
				sothutu,
				this.MaGiangVien,
				this.TenGiangVien,
				this.MaLop,
				this.TenLop,
				$"{sosinhvienlop} sinh viên"
			);
		}
	}
}
