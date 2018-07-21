using System;
using System.Collections.Generic;
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
            if(args.Count()>0)
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
            char[] strbufold = "yyyy-MM-dd HH:mm:ss".ToArray();
            char[] strnew = this.GetDateTimeStr().ToArray();
            Console.WriteLine(String.Format("len {0} {1}",strbufold.Length,strnew.Length));
            if(!File.Exists(filename))
            {
                Console.WriteLine(String.Format("文件{0}不存在",filename));
                return;
            }
            int pos;
            int filelen = 0;
            char[] buffer = new char[1024*1024*10];
            using (StreamReader reader = new StreamReader(filename, Encoding.Default))//需要指定编码，否则读到的为乱码
            {
                filelen=reader.Read(buffer, 0, buffer.Length);
            }
            
            for (int jk = 0; jk < 20; jk++)
            {
                pos = this.findPos(buffer, strbufold);
                Console.WriteLine(String.Format("位置 {0}", pos));
                if (pos >= 0)
                {
                    for (int i = 0; i < strnew.Length; i++)
                    {
                        buffer[pos + i] = strnew[i];
                    }
                }
                else
                {
                    Console.WriteLine("找到{0}处时间并修改", jk);
                    break;
                }
            }
            using (var writer = new StreamWriter(filename, false, Encoding.Default))//需要指定编码，否则读到的为乱码
            {
                writer.Write(buffer, 0, filelen);
                writer.Flush();
                writer.Close();
            }
        }
        public int findPos(char[] buf,char[] bufsub)
        {
            for(int i=0;i<buf.Length-bufsub.Length;i++)
            {
                int j = 0;
                for(j=0;j<bufsub.Length;j++)
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
    }

}
