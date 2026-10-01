using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
	internal class SinhVien {
		private string tensinhvien;
		private string masosinhvien;
		private float diemsinhvien;
		private string hoclucsinhvien;

		public SinhVien() {
			this.tensinhvien = "";
			this.masosinhvien = "";
			this.diemsinhvien = 0f;
		}

		public SinhVien(string tensinhvien, string masosinhvien, float diemsinhvien) {
			this.TenSinhVien = tensinhvien;
			this.MaSoSinhVien = masosinhvien;
			this.DiemSinhVien = diemsinhvien;
			this.hoclucsinhvien = this.HocLucSinhVien;
		}

		public string TenSinhVien {
			get { return this.tensinhvien; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.tensinhvien = value;
				}
			}
		}

		public string MaSoSinhVien {
			get { return this.masosinhvien; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					this.masosinhvien = value;
				}
			}
		}

		public float DiemSinhVien {
			get { return this.diemsinhvien; }
			set
			{
				if(value > 0 && value <= 10) {
					this.diemsinhvien = value;
				}
			}
		}

		public string HocLucSinhVien {
			get {
				if(this.diemsinhvien > 8.5f){
					return "Giỏi";
				} else if(this.diemsinhvien >= 7.0){
					return "Khá";
				} else if(this.diemsinhvien >= 5.0) {
					return "Trung bình";
				} else if(this.diemsinhvien >= 2.0){
					return "Yếu";
				} else {
					return "Kém";
				}
			}
		}

		// Phương thức 
		// trả về false nếu người dùng bỏ trống (Enter) ở bất kỳ trường nào
		public bool NhapThongTin() {
			try {
				Console.Write("(?) Nhập tên: ");
				string ten = Console.ReadLine()!.Trim();
				if (ten.Length == 0) {
					Mau.InCanhBao("Bỏ trống tên, hủy nhập sinh viên !");
					return false;
				}
				this.TenSinhVien = ten;
				Console.Write("(?) Nhập mã số: DH524000");
				string maso = Console.ReadLine()!.Trim();
				if (maso.Length == 0) {
					Mau.InCanhBao("Bỏ trống mã số, hủy nhập sinh viên !");
					return false;
				}
				this.MaSoSinhVien = "DH524000" + maso;
				float diem;
				while (true) {
					Console.Write("(?) Nhập điểm (dùng ',' thay vì '.'): ");
					string nhapdiem = Console.ReadLine()!.Trim();
					if (nhapdiem.Length == 0) {
						Mau.InCanhBao("Bỏ trống điểm, hủy nhập sinh viên !");
						return false;
					}
					if (!float.TryParse(nhapdiem, out diem) || diem <= 0 || diem > 10) {
						Mau.InCanhBao("Điểm phải là số trong khoảng 1 -> 10 !");
						continue;
					}
					break;
				}
				this.DiemSinhVien = diem;
				this.hoclucsinhvien = this.HocLucSinhVien;
				return true;
			}
			catch (Exception ex) {
				Mau.InLoi($"{ex.Message}");
				return false;
			}
		}

		public void XuatThongTin(int sothutu) {
			Console.WriteLine(
				"{0,-5} {1,-25} {2,-15} {3,-6:F1} {4,-10}",
				sothutu,
				this.TenSinhVien,
				this.MaSoSinhVien,
				this.DiemSinhVien,
				this.hoclucsinhvien
			);
		}
	}
}
