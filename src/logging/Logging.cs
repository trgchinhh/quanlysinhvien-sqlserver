using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

// Logging ghi lại log vào folder logs/ 
// dùng để xem chương trình có lỗi khi nào 

namespace QuanLySinhVien_SQLserver {
    public enum loglevel {
        TRACE = 0,
        DEBUG = 1,
        INFO = 2, 
        SUCCESS = 3, 
        WARNING = 4, 
        ERROR = 5,
        CRITICAL = 6
    };
    internal class Logging {
        private string tenfolder = "";
        private string tenfile = "";
        loglevel minlevel = loglevel.TRACE;         
        
        public Logging(string tenfolder = "logs", string tenfile = "file.log"){
            this.tenfolder = tenfolder;
            this.tenfile = tenfile;
            this.TaoFolderLogs(this.tenfolder);
            this.TaoFileLog(this.tenfile);
        }

        public void TaoFolderLogs(string tenfolder){
            try{
                if(string.IsNullOrEmpty(tenfolder)){
                    Console.WriteLine($"Tên folder không được rỗng. Không tạo folder {tenfolder}");
                    return;
                }
                if(!Directory.Exists(tenfolder)){
                    Console.WriteLine($"Chưa có thư mục {tenfolder}. Đang tạo...");
                    Directory.CreateDirectory(tenfolder);
                } 
            } catch(Exception ex){
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        public void TaoFileLog(string tenfile){
            try {
                if(string.IsNullOrEmpty(tenfile)){
                    Console.WriteLine($"Tên file không được để trống. Không tạo folder {tenfolder}");
                }
                string duongdan = Path.Combine(this.tenfolder, tenfile);
                if(!File.Exists(duongdan)){
                    Console.WriteLine($"Chưa có file {tenfile}. Đang tạo...");
                    File.Create(duongdan).Dispose();
                } else {
                    Console.WriteLine($"File {duongdan} đã tồn tại");
                }
            } catch(Exception ex){
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private string ThoiGianHienTai(){
            return DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private string LevelString(loglevel level){
            switch(level){
                case loglevel.TRACE:     return "TRACE";
                case loglevel.DEBUG:     return "DEBUG";
                case loglevel.INFO:      return "INFO";
                case loglevel.SUCCESS:   return "SUCCESS";
                case loglevel.WARNING:   return "WARNING";
                case loglevel.ERROR:     return "ERROR";
                case loglevel.CRITICAL:  return "CRITICAL";
                default:                 return "LOG";
            } 
        }

        // public int SoMauLevel(loglevel level){
        //     switch(level){
        //         case loglevel.TRACE:     return 7;
        //         case loglevel.DEBUG:     return 5;
        //         case loglevel.INFO:      return 8;
        //         case loglevel.SUCCESS:   return 1;
        //         case loglevel.WARNING:   return 2;
        //         case loglevel.ERROR:     return 0;
        //         case loglevel.CRITICAL:  return 9;
        //         default:                 return 6;
        //     }
        // }

        public void GhiLog(loglevel level, string noidung){
            try {
                string thoigian = this.ThoiGianHienTai();
                string levelstr = this.LevelString(level);
                string noidunglog = $"[{thoigian}] [{levelstr}] {noidung}\n";
                string duongdandaydu = Path.Combine(this.tenfolder, this.tenfile);
                File.AppendAllText(duongdandaydu, noidunglog);
            } catch(Exception ex){
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }       
    }
}