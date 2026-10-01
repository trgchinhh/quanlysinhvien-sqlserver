using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
    internal class HamPhuQuanLySinhVien {
        // hàm phụ cho file quản lý sinh viên 
        public static int LayMaHocLuc(float diem) {
            if (diem > 8.5f) return 1;
            if (diem > 7.0f) return 2;
            if (diem > 5.0f) return 3;
            if (diem > 2.0f) return 4;
            return 5;
        }

        // in kết quả lọc phục vụ cho lọc thông tin sinh viên 
        public static void InKetQuaLoc(List<SinhVien> ketqua) {
            Console.WriteLine();
            if (ketqua.Count == 0) {
                Mau.InCanhBao("Không có sinh viên nào phù hợp !");
                return;
            }
            Mau.InThanhCong($"Tìm thấy {ketqua.Count} sinh viên\n");
            Console.WriteLine($"{"STT",-5} {"Tên sinh viên",-25} {"Mã số",-15} {"Điểm",-6} {"Học lực",-10}");
            Console.WriteLine(new string('-', 65));
            for (int i = 0; i < ketqua.Count; i++) {
                ketqua[i].XuatThongTin(i + 1);
            }
            Console.WriteLine();
        }

        // hàm lấy tên (chữ cuối) phục vụ sort tên 
        public static string LayTenCuoi(string hotensinhvien) {
            var hotenkhongkhoangtrang = hotensinhvien.Trim().Split(' ');
            return hotenkhongkhoangtrang[hotenkhongkhoangtrang.Length - 1];
        }

        // hàm lấy số cuối mssv phục vụ sort mssv
        public static int LaySoMaSoSinhVien(string masosinhvien) {
            string socuoi = masosinhvien.Substring(masosinhvien.Length - 2, 2);
            return int.Parse(socuoi);
        }
    }
}