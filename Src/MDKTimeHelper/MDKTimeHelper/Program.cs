using MDKTimeHelper.Extension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKTimeHelper
{
    class Program
    {

        static void Main(string[] args)
        {
            var dthelper = new DateTimeHelper();
            Console.WriteLine("Hello {0}\n", dthelper.GetDateTimeStr());
            if (args.Count() > 0)
            {
                dthelper.DealAxf(args[0]);
            }
        }
    }
    class DateTimeHelper
    {
        String strDateTime = "";
        public String GetDateTimeStr()
        {
            strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return strDateTime;
        }
        public void DealAxf(string filename)
        {
            //char[] strbufold = "yyyy-MM-dd HH:mm:ss".ToArray();
            //char[] strnew = this.GetDateTimeStr().ToArray();
            //Console.WriteLine(String.Format("len {0} {1}", strbufold.Length, strnew.Length));
            //if (!File.Exists(filename))
            //{
            //    Console.WriteLine(String.Format("文件{0}不存在", filename));
            //    return;
            //}
            //int pos;
            //int filelen = 0;
            //char[] buffer = new char[1024 * 1024 * 10];
            //using (StreamReader reader = new StreamReader(filename, Encoding.Default))//需要指定编码，否则读到的为乱码
            //{
            //    filelen = reader.Read(buffer, 0, buffer.Length);
            //}

            //for (int jk = 0; jk < 20; jk++)
            //{
            //    pos = this.findPos(buffer, strbufold);
            //    Console.WriteLine(String.Format("位置 {0}", pos));
            //    if (pos >= 0)
            //    {
            //        for (int i = 0; i < strnew.Length; i++)
            //        {
            //            buffer[pos + i] = strnew[i];
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("找到{0}处时间并修改", jk);
            //        break;
            //    }
            //}
            //using (var writer = new StreamWriter(filename, false, Encoding.Default))//需要指定编码，否则读到的为乱码
            //{
            //    writer.Write(buffer, 0, filelen);
            //    writer.Flush();
            //    writer.Close();
            //}

            // 修改编译时间
            var ft = "yyyy-MM-dd HH:mm:ss";
            //var sys = axf.GetFullPath();
            if (!File.Exists(filename)) return;
            var dt = ft.GetBytes();
            using (var fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                if (fs.IndexOf(dt) > 0)
                {
                    fs.Position -= dt.Length;
                    var now = DateTime.Now.ToString(ft);
                    Console.WriteLine("找到编译时间的位置0x{0:X8}，准备写入编译时间{1}", fs.Position, now);
                    fs.Write(now.GetBytes());

                    return ;
                }
            }
        }
        
        public int findPos(char[] buf, char[] bufsub)
        {
            for (int i = 0; i < buf.Length - bufsub.Length; i++)
            {
                int j = 0;
                for (j = 0; j < bufsub.Length; j++)
                {
                    if (buf[i + j] != bufsub[j])
                        break;
                }
                if (j >= bufsub.Length)
                    return i;
            }
            //Console.WriteLine("Length {0}--{1}", buf.Length, bufsub.Length);
            return -1;
        }
#if false
        static Int32 Main1(String[] args)
        {
            var axf = GetAxf(args);
            // 修改编译时间
            if (!String.IsNullOrEmpty(axf) && WriteBuildTime(axf))
            {
                // 修改成功说明axf文件有更新，需要重新生成bin
                // 必须先找到Keil目录，否则没得玩
                var mdk = GetKeil();
                if (!String.IsNullOrEmpty(mdk) && Directory.Exists(mdk))
                {
                    var fromelf = mdk.CombinePath("ARMCC\\bin\\fromelf.exe");
                    var bin = Path.GetFileNameWithoutExtension(axf) + ".bin";
                    var bin2 = bin.GetFullPath();
                    //Process.Start(fromelf, String.Format("--bin {0} -o {1}", axf, bin2));
                    var p = new Process();
                    p.StartInfo.FileName = fromelf;
                    p.StartInfo.Arguments = String.Format("--bin {0} -o {1}", axf, bin2);
                    //p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                    p.WaitForExit(5000);
                    var len = bin2.AsFile().Length;
                    Console.WriteLine("生成ROM文件{0}共{1:n0}字节/{2:n1}KB", bin, len, (Double)len / 1024);
                }
            }

            return 0;
        }

        static String GetAxf(String[] args)
        {
            var axf = args.FirstOrDefault(e => e.EndsWithIgnoreCase(".axf"));
            if (!String.IsNullOrEmpty(axf)) return axf.GetFullPath();

            // 搜索所有axf文件，找到最新的那一个
            var fis = Directory.GetFiles(".", "*.axf", SearchOption.AllDirectories);
            if (fis != null && fis.Length > 0)
            {
                // 按照修改时间降序的第一个
                return fis.OrderByDescending(e => e.AsFile().LastWriteTime).First().GetFullPath();
            }

            Console.WriteLine("未能从参数中找到输出文件.axf，请在命令行中使用参数#L");
            return null;
        }

        static Boolean WriteBuildTime(String axf)
        {
            // 修改编译时间
            var ft = "yyyy-MM-dd HH:mm:ss";
            var sys = axf.GetFullPath();
            if (!File.Exists(sys)) return false;

            var dt = ft.GetBytes();
            // 查找时间字符串，写入真实时间
            using (var fs = File.Open(sys, FileMode.Open, FileAccess.ReadWrite))
            {
                if (fs.IndexOf(dt) > 0)
                {
                    fs.Position -= dt.Length;
                    var now = DateTime.Now.ToString(ft);
                    Console.WriteLine("找到编译时间的位置0x{0:X8}，准备写入编译时间{1}", fs.Position, now);
                    fs.Write(now.GetBytes());

                    return true;
                }
            }

            return false;
        }
#endif
        static String GetKeil()
        {
            var reg = Registry.LocalMachine.OpenSubKey("Software\\Keil\\Products\\MDK");
            if (reg == null) return null;

            return reg.GetValue("Path") + "";
        }
    }
}
