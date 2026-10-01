using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
	internal class HamPhuProgram {
		// hàm phụ cho hàm main 
		public static string noidungbanner = @"┌──────────────────────────────┐
│     QUẢN LÝ SINH VIÊN C#     │
│ Tác giả: Trường Chinh        │
│ Github: Github.com/trgchinhh │
└──────────────────────────────┘
		";
		public static void DungChuongTrinh() {
			Console.Write("Nhấn phím bất kỳ để tiếp tục ...");
			Console.ReadKey();
		}

		public static Color ChonMau() {
			Console.Clear();
			Console.WriteLine(noidungbanner);

			// phần hướng dẫn dùng 
			AnsiConsole.Write(
				new Panel(
					"Dùng phím ↑ ↓ để di chuyển\n" +
					"Dùng phím Enter để chọn"
				)
				.Header("Hướng dẫn")
			);
			Console.WriteLine("\n(↑ ↓) Chọn màu menu");
			var luachonmau = AnsiConsole.Prompt(
				new SelectionPrompt<int>()
				.AddChoices(1, 2, 3, 4, 5, 6, 7, 8)
				.WrapAround(true)
				.HighlightStyle(new Style(Mau.danhsachmau[6]))
				.UseConverter(x => x switch {
					1 => Markup.Escape("[01] Màu đỏ"),
					2 => Markup.Escape("[02] Màu xanh lá"),
					3 => Markup.Escape("[03] Màu vàng"),
					4 => Markup.Escape("[04] Màu cam"),
					5 => Markup.Escape("[05] Màu xanh ngọc"),
					6 => Markup.Escape("[06] Màu xanh cyan"),
					7 => Markup.Escape("[07] Màu mặc định"),
					8 => Markup.Escape("[08] Thoát"),
					_ => ""
				})
			);
			if (luachonmau == 8) Environment.Exit(0);
			return Mau.danhsachmau[luachonmau - 1];
		}
	}
}
