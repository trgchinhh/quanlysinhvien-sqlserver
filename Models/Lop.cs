using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien_SQLserver {
	internal class Lop {
		private string malop = "";
		private string tenlop = "";

		public Lop() {
			this.malop = "";
			this.tenlop = "";
		}

		public Lop(string malop, string tenlop) {
			this.MaLop = malop;
			this.TenLop = tenlop;
		}

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

		// Phương thức 
		// tên lớp đầy đủ (mã lớp - tên lớp) phục vụ in ra màn hình
		public string LayTenLopDayDu() {
			return $"{this.MaLop} - {this.TenLop}";
		}
	}
}
