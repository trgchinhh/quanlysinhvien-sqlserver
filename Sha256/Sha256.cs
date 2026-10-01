using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.Encodings;

// hash sha256 cho mật khẩu tài khoản giảng viên 
// hash trước khi lưu vào database 
namespace QuanLySinhVien_SQLserver {
	internal class Sha256 {
		public static string Hash(string matkhau) {
			byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(matkhau));
			return Convert.ToHexString(hash).ToLower();
		}
	}
}
