using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
    internal class HamPhuQuanLyGiangVien {
        // in tiêu đề chương trình kèm lớp và giảng viên đang đăng nhập
        public static void InTieuDe(GiangVien giangvien) {
            Console.WriteLine(HamPhuProgram.noidungbanner);
            giangvien.XuatThongTin();
            //Console.WriteLine(new string('-', 65));
            Console.WriteLine();
        }

        public static void InHuongDanDangNhap(int lanthu, int soluonglanthu) {
            AnsiConsole.Write(
                new Panel(
                    "[yellow]Tài khoản mẫu:[/]\n" +
                    "[yellow]Mã tài khoản:[/] GV001 / GV002 / GV003\n" + 
                    "[yellow]Mật khẩu:[/] 123456"
                )
                .Header($"Đăng nhập giảng viên (lần {lanthu}/[red]{soluonglanthu}[/])")
            );
            Console.WriteLine();
        }

        // panel hướng dẫn ở màn hình đăng ký giảng viên
        public static void InHuongDanDangKy() {
            AnsiConsole.Write(
                new Panel(
                    "Giảng viên mới tạo tài khoản và lớp phụ trách\n" +
                    "Nhập mã giáo viên, mã lớp, tên lớp, môn phụ trách\n" +
                    "Lớp mới tạo chưa có sinh viên, thêm sinh viên ở menu quản lý"
                )
                .Header("Đăng ký giảng viên")
            );
            Console.WriteLine();
        }

        // nhập mật khẩu mà không hiện ký tự lên màn hình
        public static string NhapMatKhau(int lan) {
            if(lan == 1) Console.Write("(?) Nhập mật khẩu: ");
            if(lan == 2) Console.Write("(?) Xác nhận mật khẩu: ");
            if (Console.IsInputRedirected) {
                return Console.ReadLine()!.Trim();
            }
            var matkhau = new StringBuilder();
            while (true) {
                ConsoleKeyInfo phim = Console.ReadKey(true);
                if (phim.Key == ConsoleKey.Enter) {
                    break;
                }
                if (phim.Key == ConsoleKey.Backspace) {
                    if (matkhau.Length > 0) {
                        matkhau.Remove(matkhau.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(phim.KeyChar)) {
                    matkhau.Append(phim.KeyChar);
                    Console.Write('*');
                }
            }
            Console.WriteLine();
            return matkhau.ToString();
        }
    }
}