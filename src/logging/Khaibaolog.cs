using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace QuanLySinhVien_SQLserver {
    internal class Khaibaolog {
        public static Logging logsinhvien = new Logging(tenfile:"sinhvien.log");
        public static Logging loggiangvien = new Logging(tenfile:"giangvien.log");
        public static Logging logketnoidulieu = new Logging(tenfile:"ketnoidulieu.log");
    }
}