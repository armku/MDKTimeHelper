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
            if (args.Count() > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (!dthelper.WriteBuildTime(args[0]))
                        break;
                }
            }
        }
    }
    class DateTimeHelper
    {
        /// <summary>
        /// 写入当前时间
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Boolean WriteBuildTime(string filename)
        {
            // 修改编译时间
            var ft = "yyyy-MM-dd HH:mm:ss";
            if (!File.Exists(filename))
                return false;
            var dt = ft.GetBytes();
            using (var fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
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
        /// <summary>
        /// 获取Keil路径
        /// </summary>
        /// <returns></returns>
        static String GetKeil()
        {
            var reg = Registry.LocalMachine.OpenSubKey("Software\\Keil\\Products\\MDK");
            if (reg == null) return null;

            return reg.GetValue("Path") + "";
        }
    }
}
