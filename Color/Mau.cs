using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace QuanLySinhVien_SQLserver { 
	internal class Mau {
		public static List<Color> danhsachmau = new List<Color> {
			Color.Red,
			Color.Green,
			Color.Yellow,
			Color.Orange1,
			Color.Aquamarine1,
			Color.Cyan,
			Color.Default
		};

		public static void ToMau(string noidung, Color mau, bool xuongdong = false) {
			Console.ForegroundColor = mau;
			if (xuongdong) Console.WriteLine(noidung);
			else Console.Write(noidung);
			Console.ResetColor();
		}

		public static void InThanhCong(string noidung){
			// in nhập thì dùng màu xanh lá
			Console.Write("(");
			ToMau("*", danhsachmau[1]);
			Console.Write(") ");
			Console.WriteLine(noidung);
			Console.ResetColor();
		}

		public static void InCanhBao(string noidung){
			// in cảnh báo thì dùng màu vàng 
			Console.Write("(");
			ToMau("!", danhsachmau[2]);
			Console.Write(") ");
			Console.WriteLine(noidung);
			Console.ResetColor();
		}

		public static void InLoi(string noidung){
			// in lỗi thì dùng màu đỏ 
			Console.Write("(");
			ToMau("X", danhsachmau[0]);
			Console.Write(") ");
			Console.WriteLine(noidung);
			Console.ResetColor();
		}

	}
}
