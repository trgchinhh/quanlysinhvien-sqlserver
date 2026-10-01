using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien_SQLserver {
	internal class QuanLySinhVien {
		private int soluongsinhvien;
		private List<SinhVien> danhsachsinhvien = new List<SinhVien>();
		private readonly Ketnoidulieu db = new Ketnoidulieu();
		// giảng viên đang đăng nhập, mọi thao tác chỉ diễn ra trong lớp của giảng viên này
		private readonly GiangVien giangvien;

		// dữ liệu mẫu sinh viên (giống file sql/script.sql)
		private static readonly List<(string ten, string mssv, float diem, string malop)> danhsachsinhvienmau =
			new List<(string ten, string mssv, float diem, string malop)> {
			// Lớp OOP
			("Nguyễn Trường Chinh", "DH52400001", 7.6f, "OOP"),
			("Trần Minh Gia Bảo", "DH52400002", 7.6f, "OOP"),
			("Trần Minh Anh", "DH52400003", 7.1f, "OOP"),
			("Hoàng Ngọc Hân", "DH52400006", 6.2f, "OOP"),
			("Bùi Đức Anh", "DH52400009", 3.0f, "OOP"),
			("Hồ Thùy Dương", "DH52400012", 9.9f, "OOP"),
			("Phan Mai Phương", "DH52400014", 1.5f, "OOP"),
			("Đoàn Gia Huy", "DH52400043", 7.9f, "OOP"),
			("Kiều Thanh Tâm", "DH52400044", 6.8f, "OOP"),
			("Lương Minh Quân", "DH52400045", 8.1f, "OOP"),
			// Lớp CSDL
			("Lê Hoàng Nam", "DH52400004", 8.3f, "CSDL"),
			("Vũ Võ Tuấn Kiệt", "DH52400007", 6.7f, "CSDL"),
			("Đỗ Khánh Linh", "DH52400010", 5.8f, "CSDL"),
			("Lý Hoàng Long", "DH52400013", 7.8f, "CSDL"),
			("Đặng Văn Lâm", "DH52400017", 10.0f, "CSDL"),
			("Nguyễn Duy Quang", "DH52400019", 9.1f, "CSDL"),
			("Phùng Anh Thư", "DH52400046", 9.4f, "CSDL"),
			("Châu Quốc Việt", "DH52400047", 5.9f, "CSDL"),
			("Thái Ngọc Ánh", "DH52400048", 7.2f, "CSDL"),
			("Quách Đình Phong", "DH52400049", 8.7f, "CSDL"),
			// Lớp IPC
			("Nguyễn Thanh Tùng", "DH52400005", 9.1f, "IPC"),
			("Đặng Phương Thảo", "DH52400008", 3.5f, "IPC"),
			("Ngô Quốc Huy", "DH52400011", 7.6f, "IPC"),
			("Võ Đình Khang", "DH52400015", 6.4f, "IPC"),
			("Trần Văn Đan", "DH52400016", 8.0f, "IPC"),
			("Nguyễn Duy Minh", "DH52400018", 9.2f, "IPC"),
			("Nguyễn Trường An", "DH52400020", 8.5f, "IPC"),
			("Lê Thị Ngọc Tú", "DH52400022", 10.0f, "IPC"),
			("La Thúy Hằng", "DH52400050", 6.6f, "IPC"),
			("Âu Hoàng Sơn", "DH52400051", 7.5f, "IPC"),
			// Lớp MH
			("Phạm Quốc Bảo", "DH52400023", 8.2f, "MH"),
			("Lê Minh Châu", "DH52400024", 7.4f, "MH"),
			("Trần Hải Đăng", "DH52400025", 6.5f, "MH"),
			("Võ Thanh Hà", "DH52400026", 9.0f, "MH"),
			("Huỳnh Gia Hưng", "DH52400027", 5.5f, "MH"),
			("Đinh Ngọc Mai", "DH52400028", 8.8f, "MH"),
			("Dương Anh Khoa", "DH52400029", 7.0f, "MH"),
			("Lâm Thu Hiền", "DH52400030", 9.5f, "MH"),
			("Mai Văn Phúc", "DH52400031", 4.8f, "MH"),
			("Tạ Bảo Ngọc", "DH52400032", 8.0f, "MH"),
			// Lớp KT
			("Phạm Thanh Hà", "DH52400033", 7.7f, "KT"),
			("Lê Gia Hưng", "DH52400034", 6.9f, "KT"),
			("Trần Bảo Ngọc", "DH52400035", 9.3f, "KT"),
			("Bùi Anh Khoa", "DH52400036", 5.2f, "KT"),
			("Hoàng Thu Hiền", "DH52400037", 8.6f, "KT"),
			("Đỗ Minh Châu", "DH52400038", 7.3f, "KT"),
			("Ngô Hải Đăng", "DH52400039", 6.1f, "KT"),
			("Vương Khánh Vy", "DH52400040", 9.7f, "KT"),
			("Cao Đức Thịnh", "DH52400041", 4.5f, "KT"),
			("Trịnh Mỹ Linh", "DH52400042", 8.4f, "KT")
		};

		public QuanLySinhVien(GiangVien giangvien) {
			this.giangvien = giangvien;
			this.soluongsinhvien = 0;
			// database đã được kiểm tra kết nối ở bước đăng nhập giảng viên
			this.Napdulieudanhsach();
		}

		private void Napdulieudanhsach() {
			try {
				this.danhsachsinhvien = db.LayDanhSachCSDL(this.giangvien.MaLop);
				if (this.danhsachsinhvien.Count == 0) {
					var danhsachmau = this.TaoDanhSachSinhVienMau();
					// lớp mới tạo chưa có sinh viên thì không có dữ liệu mẫu để nạp
					if (danhsachmau.Count == 0) {
						Mau.InCanhBao($"Lớp {this.giangvien.LayTenLopDayDu()} chưa có sinh viên, hãy thêm sinh viên !");
					}
					else {
						Mau.InCanhBao($"Lớp {this.giangvien.LayTenLopDayDu()} chưa có sinh viên, đang nạp dữ liệu mẫu ...");
						foreach (var sv in danhsachmau){
							db.ThemCSDL(sv, this.giangvien.MaLop);
						} 
						this.danhsachsinhvien = danhsachmau;
						Mau.InThanhCong("Đã nạp xong !");
					}
				}
			}
			catch (Exception ex) {
				Mau.InThanhCong($"Lỗi kết nối database: {ex.Message}");
			}
			finally {
				this.soluongsinhvien = this.danhsachsinhvien.Count;
			}
		}

		// lấy phần dữ liệu mẫu thuộc đúng lớp của giảng viên đang đăng nhập
		private List<SinhVien> TaoDanhSachSinhVienMau() {
			var danhsachmau = new List<SinhVien>();
			foreach (var sv in danhsachsinhvienmau) {
				if (sv.malop == this.giangvien.MaLop) {
					danhsachmau.Add(new SinhVien {TenSinhVien = sv.ten, MaSoSinhVien = sv.mssv, DiemSinhVien = sv.diem});
				}
			}
			return danhsachmau;
		}

		// Các hàm chính 
		// nhập số thứ tự (phục vụ cho các hàm sửa, xóa)
		public int NhapSoThuTu() {
			int sothutu;
			do {
				Console.Write("Nhập số thứ tự (0 để thoát): ");
				int.TryParse(Console.ReadLine()!, out sothutu);
				if (sothutu < 0 || sothutu > this.soluongsinhvien) {
					Mau.InCanhBao($"Nhập STT trong khoảng 1 -> {this.soluongsinhvien}");
				}
				else if (sothutu == 0) {
					break;
				}
			} while (sothutu < 0 || sothutu > this.soluongsinhvien);
			return sothutu;
		}

		// thêm sinh viên 
		public void ThemThongTinSinhVien() {
			Console.WriteLine($"Thêm thông tin sinh viên vào lớp {this.giangvien.LayTenLopDayDu()}");
			int soluong;
			do {
				Console.Write("Nhập số lượng Sv muốn thêm (0 để thoát): ");
				int.TryParse(Console.ReadLine()!, out soluong);
				if (soluong < 0) {
					Mau.InCanhBao("Số lượng phải từ 1 trở lên !");
				}
				else if (soluong == 0) {
					return;
				}
			} while (soluong <= 0);
			// cập nhật số lượng sv mới 
			//this.soluongsinhvien += soluong;

			int soluongsvdathem = 0;
			for (int i = 0; i < soluong; i++) {
				SinhVien sinhvienmoi = new SinhVien();
				this.InDanhSachSinhVien();
				Console.WriteLine($"Nhập thông tin Sv thứ {i + 1}");
				if(!sinhvienmoi.NhapThongTin()){
					break;
				}
				try {
					db.ThemCSDL(sinhvienmoi, this.giangvien.MaLop);
					this.danhsachsinhvien.Add(sinhvienmoi);
					this.soluongsinhvien++;
					soluongsvdathem++;
				}
				catch (Exception ex) {
					Mau.InCanhBao($"Không thêm được (mã số có thể đã bị trùng): {ex.Message}");
				}
			}
			Console.WriteLine();
			Mau.InThanhCong($"Đã thêm {soluongsvdathem} sinh viên vào danh sách");
			this.InDanhSachSinhVien();
		}

		// xóa sinh viên 
		public void XoaThongTinSinhVien() {
			Console.WriteLine($"Xóa thông tin sinh viên trong lớp {this.giangvien.LayTenLopDayDu()}");
			this.InDanhSachSinhVien();
			int sothutu = this.NhapSoThuTu();
			if (sothutu == 0) {
				return;
			}
			SinhVien sv = this.danhsachsinhvien[sothutu - 1];
			try {
				db.XoaCSDL(sv.MaSoSinhVien, this.giangvien.MaLop);
				this.danhsachsinhvien.RemoveAt(sothutu - 1);
				this.soluongsinhvien--;
				Console.WriteLine();
				Mau.InThanhCong($"Đã xóa thông tin sinh viên: {sv.TenSinhVien}");
			}
			catch (Exception ex) {
				Mau.InLoi($"Lỗi: {ex.Message}");
			}
		}

		// sửa thông tin sinh viên 
		public void SuaThongTinSinhVien() {
			Console.WriteLine($"Sửa thông tin sinh viên trong lớp {this.giangvien.LayTenLopDayDu()}");
			bool thaydoi = false;
			this.InDanhSachSinhVien();
			int sothutu = this.NhapSoThuTu();
			if (sothutu == 0) {
				return;
			}
			SinhVien sinhvien = this.danhsachsinhvien[sothutu - 1];
			string masosinhviencu = sinhvien.MaSoSinhVien;
			Console.WriteLine("Bỏ trống và nhấn Enter để giữ nguyên giá trị cũ");

			Console.Write("Nhập tên: ");
			string tenthay = Console.ReadLine()!;
			if (string.IsNullOrEmpty(tenthay)) {
				tenthay = sinhvien.TenSinhVien;
			}
			else thaydoi = true;
			sinhvien.TenSinhVien = tenthay;

			Console.Write("Nhập mã số: ");
			string masothay = Console.ReadLine()!;
			if (string.IsNullOrEmpty(masothay)) {
				masothay = sinhvien.MaSoSinhVien;
			}
			else thaydoi = true;
			sinhvien.MaSoSinhVien = masothay;

			Console.Write("Nhập điểm: ");
			float.TryParse(Console.ReadLine()!, out float diemthay);
			if (diemthay <= 0 || diemthay > 10) {
				diemthay = sinhvien.DiemSinhVien;
			}
			else thaydoi = true;
			sinhvien.DiemSinhVien = diemthay;
			if (thaydoi) {
				try {
					db.SuaCSDL(masosinhviencu, sinhvien, this.giangvien.MaLop);
				} catch(Exception ex) {
					Mau.InLoi($"Lỗi: {ex.Message}");
					// nếu trùng mã thì nạp lại dữ liệu danh sách 
					this.Napdulieudanhsach();
					return;
				}
			}
			Console.WriteLine();
			Mau.InThanhCong(
				$"{(thaydoi ? "Đã" : "Không")} thay đổi thông tin sinh viên: {sinhvien.TenSinhVien}"
			);
		}

		// lọc thông tin sinh viên
		public void LocThongTinSinhVien() {
			while (true) {
				Console.WriteLine(
					"LỌC SINH VIÊN\n" +
					"1. Lọc theo chữ cái đầu của tên\n" +
					"2. Lọc theo điểm\n" +
					"3. Lọc theo học lực\n" +
					"4. Lọc theo mã số\n" +
					"0. Quay lại"
				);
				Console.Write("Lựa chọn: ");
				string nhap = Console.ReadLine()!;
				if (string.IsNullOrEmpty(nhap)) {
					return;
				}
				if (!int.TryParse(nhap, out int luachon)) {
					Mau.InCanhBao("Vui lòng chọn hợp lệ !");
					continue;
				}
				if (luachon == 0) {
					return;
				}
				// nạp dữ liệu mới nhất trước khi lọc
				this.Napdulieudanhsach();
				Console.WriteLine();
				List<SinhVien> ketqua = new List<SinhVien>();

				if (luachon == 1) {
					Console.Write("Nhập chữ cái đầu của tên: ");
					string chucai = Console.ReadLine()!.Trim().ToLower();
					if (chucai.Length == 0) {
						Mau.InCanhBao("Chưa nhập chữ cái !");
						continue;
					}
					for (int i = 0; i < this.danhsachsinhvien.Count; i++) {
						string tencuoi = HamPhuQuanLySinhVien.LayTenCuoi(this.danhsachsinhvien[i].TenSinhVien).ToLower();
						if (tencuoi.StartsWith(chucai)) {
							ketqua.Add(this.danhsachsinhvien[i]);
						}
					}
				}
				else if (luachon == 2) {
					Console.Write("Từ điểm: ");
					if (!float.TryParse(Console.ReadLine(), out float tu) || tu < 0 || tu > 10) {
						Mau.InCanhBao("Điểm phải từ 0 -> 10 !");
						continue;
					}
					Console.Write("Đến điểm: ");
					if (!float.TryParse(Console.ReadLine(), out float den) || den < 0 || den > 10) {
						Mau.InCanhBao("Điểm phải từ 0 -> 10 !");
						continue;
					}
					if (tu > den) {
						(tu, den) = (den, tu);
					}
					for (int i = 0; i < this.danhsachsinhvien.Count; i++) {
						float diem = this.danhsachsinhvien[i].DiemSinhVien;
						if (diem >= tu && diem <= den) {
							ketqua.Add(this.danhsachsinhvien[i]);
						}
					}
				}
				else if (luachon == 3) {
					Console.WriteLine(
						"1. Giỏi (> 8.5)\n" +
						"2. Khá (> 7.0 - 8.5)\n" +
						"3. Trung bình (> 5.0 - 7.0)\n" +
						"4. Yếu (> 2.0 - 5.0)\n" +
						"5. Kém (<= 2.5)"
					);
					Console.Write("Chọn học lực: ");
					if (!int.TryParse(Console.ReadLine(), out int hocluc) || hocluc < 1 || hocluc > 5) {
						Mau.InCanhBao("Vui lòng chọn 1 -> 5 !");
						continue;
					}
					for (int i = 0; i < this.danhsachsinhvien.Count; i++) {
						if (HamPhuQuanLySinhVien.LayMaHocLuc(this.danhsachsinhvien[i].DiemSinhVien) == hocluc) {
							ketqua.Add(this.danhsachsinhvien[i]);
						}
					}
				}
				else if (luachon == 4) {
					Console.Write("Từ mã số: ");
					string matu = Console.ReadLine()!.Trim();
					Console.Write("Đến mã số: ");
					string maden = Console.ReadLine()!.Trim();
					if (matu.Length == 0 || maden.Length == 0) {
						Mau.InCanhBao("Chưa nhập mã số !");
						continue;
					}
					if (string.Compare(matu, maden, StringComparison.OrdinalIgnoreCase) > 0) {
						(matu, maden) = (maden, matu);
					}
					for (int i = 0; i < this.danhsachsinhvien.Count; i++) {
						string maso = this.danhsachsinhvien[i].MaSoSinhVien;
						if (string.Compare(maso, matu, StringComparison.OrdinalIgnoreCase) >= 0 &&
							string.Compare(maso, maden, StringComparison.OrdinalIgnoreCase) <= 0) {
							ketqua.Add(this.danhsachsinhvien[i]);
						}
					}
				}
				else {
					Mau.InCanhBao("Vui lòng chọn hợp lệ !");
					continue;
				}
				HamPhuQuanLySinhVien.InKetQuaLoc(ketqua);
				break;
			}
		}

		// sắp xếp thông tin sinh viên 
		public void SapXepThongTinSinhVien() {
			bool hiendanhsach = true;
			while (true) {
				Console.WriteLine(
					"SẮP XẾP SINH VIÊN\n" +
					"1. Sắp xếp theo tên (A -> Z)\n" +
					"2. Sắp xếp theo tên (Z -> A)\n" +
					"3. Sắp xếp theo mã số (Thấp -> Cao)\n" +
					"4. Sắp xếp theo điểm (Thấp -> Cao)\n" +
					"5. Sắp xếp theo điểm (Cao -> Thấp)\n" +
					"0. Quay lại"
				);
				Console.Write("Lựa chọn: ");
				string? nhap = Console.ReadLine();
				if (string.IsNullOrEmpty(nhap)) {
					return;
				}
				if (!int.TryParse(nhap, out int luachon)) {
					Mau.InCanhBao("Vui lòng chọn hợp lệ !");
					continue;
				}
				// lưu dữ liệu mới nhất trc khi sắp xếp để tránh sai sót dữ liệu
				this.Napdulieudanhsach();
				Console.WriteLine();
				if (luachon == 1) {
					this.danhsachsinhvien.Sort((a, b)
						=> String.Compare(
							HamPhuQuanLySinhVien.LayTenCuoi(a.TenSinhVien), 
							HamPhuQuanLySinhVien.LayTenCuoi(b.TenSinhVien)
						)
					);
					Mau.InThanhCong("Đã sắp xếp theo tên (A->Z)");
				}
				else if (luachon == 2) {
					this.danhsachsinhvien.Sort((a, b)
						=> String.Compare(
							HamPhuQuanLySinhVien.LayTenCuoi(b.TenSinhVien), 
							HamPhuQuanLySinhVien.LayTenCuoi(a.TenSinhVien)
						)
					);
					Mau.InThanhCong("Đã sắp xếp theo tên (Z->A)");
				}
				else if (luachon == 3){
					this.danhsachsinhvien.Sort((a, b)
						=> (HamPhuQuanLySinhVien.LaySoMaSoSinhVien(a.MaSoSinhVien)).
						CompareTo(HamPhuQuanLySinhVien.LaySoMaSoSinhVien(b.MaSoSinhVien))
					);
					Mau.InThanhCong("Đã sắp xếp theo mã số (Thấp->Cao)");
				}	
				else if (luachon == 4) {
					this.danhsachsinhvien.Sort((a, b)
						=> (a.DiemSinhVien).CompareTo(b.DiemSinhVien));
					Mau.InThanhCong("Đã sắp xếp theo điểm (Thấp->Cao)");
				}
				else if (luachon == 5) {
					this.danhsachsinhvien.Sort((a, b)
						=> (b.DiemSinhVien).CompareTo(a.DiemSinhVien));
					Mau.InThanhCong("Đã sắp xếp theo điểm (Cao->Thấp)");
				}
				else if (luachon == 0) {
					hiendanhsach = false;
					break;
				}
				else {
					Mau.InCanhBao("Vui lòng chọn hợp lệ");
					continue;
				}
				try {
					db.LuuThuTuCSDL(this.danhsachsinhvien, this.giangvien.MaLop);
				}
				catch (Exception ex) {
					Mau.InLoi($"Lỗi lưu thứ tự: {ex.Message}");
				}
				break;
			}
			if (hiendanhsach) {
				this.InDanhSachSinhVien();
			}
		}

		// thống kê 
		public void ThongKeThongTinSinhVien() {
			Console.WriteLine($"Thống kê thông tin sinh viên lớp {this.giangvien.LayTenLopDayDu()}");
			if(this.soluongsinhvien == 0) {
				Mau.InCanhBao("Danh sách rỗng !");
				return;
			}
			int so_hsg = 0, so_hsk = 0, so_hstb = 0, so_hsy = 0;
			float tongdiem = 0f;
			for (int i = 0; i < this.soluongsinhvien; i++) {
				float diemsinhvien = this.danhsachsinhvien[i].DiemSinhVien;
				if (diemsinhvien > 8.5) so_hsg++;
				else if (diemsinhvien > 7.0) so_hsk++;
				else if (diemsinhvien > 5.0) so_hstb++;
				else so_hsy++;
				tongdiem += diemsinhvien;
			}
			float diemtrungbinhlop = tongdiem / this.soluongsinhvien;
			float diemcaonhat = this.danhsachsinhvien.Max(sv => sv.DiemSinhVien);
			float diemthapnhat = this.danhsachsinhvien.Min(sv => sv.DiemSinhVien);
			int soluongsinhviendiemcaonhat = 0;
			int soluongsinhviendiemthapnhat = 0;
			List<SinhVien> danhsachsinhviendiemcaonhat = new List<SinhVien>();
			List<SinhVien> danhsachsinhviendiemthapnhat = new List<SinhVien>();
			foreach(SinhVien sinhvien in this.danhsachsinhvien){
				if(sinhvien.DiemSinhVien == diemcaonhat) {
					soluongsinhviendiemcaonhat++;
					danhsachsinhviendiemcaonhat.Add(sinhvien);
				} 
				if(sinhvien.DiemSinhVien == diemthapnhat) {
					soluongsinhviendiemthapnhat++;
					danhsachsinhviendiemthapnhat.Add(sinhvien);
				}
			}

			Console.WriteLine("Sĩ số: {0}", this.soluongsinhvien);
			Console.WriteLine(
				$"{"Số HSG",-15} {"Số HSK",-15} {"Số HSTB",-15} {"Số HSY",-15}"
			);
			Console.WriteLine(
				$"{so_hsg,-15} {so_hsk,-15} {so_hstb,-15} {so_hsy,-15}"
			);
			Console.WriteLine("Điểm trung bình lớp: {0:F1}", diemtrungbinhlop);
			Console.WriteLine($"Điểm cao nhất: {diemcaonhat:F1} ({soluongsinhviendiemcaonhat} sinh viên)");
			foreach(SinhVien sinhvien in danhsachsinhviendiemcaonhat){
				Mau.ToMau("    -", Mau.danhsachmau[3]);
				Console.WriteLine(sinhvien.TenSinhVien);
			}
			Console.WriteLine($"Điểm thấp nhất: {diemthapnhat:F1} ({soluongsinhviendiemthapnhat} sinh viên)");
			foreach(SinhVien sinhvien in danhsachsinhviendiemthapnhat){
				Mau.ToMau("    -", Mau.danhsachmau[3]);
				Console.WriteLine(sinhvien.TenSinhVien);
			}
			Console.WriteLine();
		}

		// in danh sách sinh viên của lớp đang quản lý
		public void InDanhSachSinhVien() {
			this.Napdulieudanhsach();
			Mau.InThanhCong($"Danh sách sinh viên lớp {this.giangvien.LayTenLopDayDu()}\n");
			Console.WriteLine($"{"STT",-5} {"Tên sinh viên",-25} {"Mã số",-15} {"Điểm",-6} {"Học lực",-10}");
			Console.WriteLine(new string('-', 65));
			for (int i = 0; i < this.danhsachsinhvien.Count; i++) {
				this.danhsachsinhvien[i].XuatThongTin(i + 1);
			}
			Console.WriteLine("");
		}
	}
}
