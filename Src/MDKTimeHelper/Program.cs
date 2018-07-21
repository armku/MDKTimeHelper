using System;
using System.Collections.Generic;
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
    }
}
