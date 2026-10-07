using QuanLySinhVien_SQLserver;
using System;
using System.Text;
using Spectre.Console;

public class Program {        
	public static void Main() {
		QuanLyGiangVien quanlygiangvien = new QuanLyGiangVien();
		var luachonmau = HamPhuProgram.ChonMau();
		while (true) {
			Console.Clear();
			Console.WriteLine(HamPhuProgram.noidungbanner);
			AnsiConsole.Write(
				new Panel(
					$"Chọn [yellow]{Markup.Escape("[03]")}[/] để xem tài khoản đăng nhập mẫu\n" +
					$"Hoặc [yellow]{Markup.Escape("[02]")}[/] để tự tạo tài khoản mới"
				)
				.Header($"Hướng dẫn")
			);
			Console.WriteLine();
			Console.WriteLine("  (↑ ↓) MENU");
			var luachon = AnsiConsole.Prompt(
				new SelectionPrompt<int>()
				.AddChoices(1, 2, 3, 4)
				.WrapAround(true)
				.HighlightStyle(new Style(luachonmau))
				.UseConverter(x => x switch {
					1 => Markup.Escape("[01] Đăng nhập"),
					2 => Markup.Escape("[02] Đăng ký"),
					3 => Markup.Escape("[03] Xem tài khoản giảng viên"),
					4 => Markup.Escape("[04] Thoát"),
					_ => ""
				})
			);
			Console.WriteLine();
			if (luachon == 1) {
				GiangVien? giangvien = quanlygiangvien.DangNhap();
				if (giangvien == null) {
					Mau.InCanhBao("Đăng nhập không thành công, vui lòng thử lại !");
				}
				else {
					MenuQuanLySinhVien(giangvien, ref luachonmau);
				}
			}
			else if (luachon == 2) {
				quanlygiangvien.DangKy();
			}
			else if (luachon == 3) {
				quanlygiangvien.XemThongTinTaiKhoanGiangVien();
			}
			else if (luachon == 4) {
				Mau.InThanhCong("Hẹn gặp lại giảng viên !");
				break;
			}
			HamPhuProgram.DungChuongTrinh();
		}
	}

	// menu quản lý sinh viên
	private static void MenuQuanLySinhVien(GiangVien giangvien, ref Color luachonmau) {
		QuanLySinhVien quanlysinhvien = new QuanLySinhVien(giangvien);
		int luachontruoc = 0;
		while (true) {
			Console.Clear();
			HamPhuQuanLyGiangVien.InTieuDe(giangvien);
			Console.WriteLine("  (↑ ↓) MENU");
			var luachon = AnsiConsole.Prompt(
				new SelectionPrompt<int>()
				.AddChoices(0, 1, 2, 3, 4, 5, 6, 7, 8)
				.WrapAround(true)
				.HighlightStyle(new Style(luachonmau))
				.DefaultValue(luachontruoc)
				.UseConverter(x => x switch {
					0 => Markup.Escape("[00] Thay màu"),
					1 => Markup.Escape("[01] Xem danh sách"),
					2 => Markup.Escape("[02] Thêm sinh viên"),
					3 => Markup.Escape("[03] Sửa thông tin"),
					4 => Markup.Escape("[04] Lọc thông tin"),
					5 => Markup.Escape("[05] Sắp xếp thông tin"),
					6 => Markup.Escape("[06] Xóa thông tin"),
					7 => Markup.Escape("[07] Thống kê"),
					8 => Markup.Escape("[08] Đăng xuất"),
					_ => ""
				})
			);
			luachontruoc = luachon;
			Console.WriteLine();
			if (luachon == 0) {
				luachonmau = HamPhuProgram.ChonMau();
				continue;
			}
			else if (luachon == 1) {
				quanlysinhvien.InDanhSachSinhVien();
			}
			else if (luachon == 2) {
				quanlysinhvien.ThemThongTinSinhVien();
			}
			else if (luachon == 3) {
				quanlysinhvien.SuaThongTinSinhVien();
			}
			else if (luachon == 4) {
				quanlysinhvien.LocThongTinSinhVien();
			}
			else if (luachon == 5) {
				quanlysinhvien.SapXepThongTinSinhVien();
			}
			else if (luachon == 6) {
				quanlysinhvien.XoaThongTinSinhVien();
			}
			else if (luachon == 7) {
				quanlysinhvien.ThongKeThongTinSinhVien();
			}
			else if (luachon == 8) {
				Mau.InThanhCong(
					$"Tạm biệt giảng viên {giangvien.TenGiangVien} !"
				);
				return;
			}
			else {
				Mau.InCanhBao("(!) Vui lòng chọn đúng !");
			}
			HamPhuProgram.DungChuongTrinh();
		}
	}
}
