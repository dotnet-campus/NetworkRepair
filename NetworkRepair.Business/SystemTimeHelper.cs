using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkRepair.Business
{
    public static class SystemTimeHelper
    {
        public static string UpdateTime()
        {
            try
            {
                // 记录开始的时间   
                DateTime startDT = DateTime.Now;

                //建立IPAddress对象与端口，创建IPEndPoint节点:   
                int port = 13;
                string[] whost = { "5time.nist.gov", "time-nw.nist.gov", "time-a.nist.gov", "time-b.nist.gov", "tick.mit.edu", "time.windows.com", "clock.sgi.com" };

                IPHostEntry iphostinfo;
                IPAddress ip;
                IPEndPoint ipe;
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket   

                c.ReceiveTimeout = 10 * 1000;//设置超时时间   

                string sEX = "";// 接受错误信息   

                // 遍历时间服务器列表   
                foreach (string strHost in whost)
                {
                    try
                    {
                        iphostinfo = Dns.GetHostEntry(strHost);
                        ip = iphostinfo.AddressList[0];
                        ipe = new IPEndPoint(ip, port);

                        c.Connect(ipe);//连接到服务器   
                        if (c.Connected) break;// 如果连接到服务器就跳出   
                    }
                    catch (Exception ex)
                    {
                        sEX = ex.Message;
                    }
                }

                if (!c.Connected)
                {
                    return $"时间服务器连接失败！/r错误信息：{sEX}系统提示";
                }

                //SOCKET同步接受数据   
                byte[] RecvBuffer = new byte[1024];
                int nBytes, nTotalBytes = 0;
                StringBuilder sb = new StringBuilder();
                System.Text.Encoding myE = Encoding.UTF8;

                while ((nBytes = c.Receive(RecvBuffer, 0, 1024, SocketFlags.None)) > 0)
                {
                    nTotalBytes += nBytes;
                    sb.Append(myE.GetString(RecvBuffer, 0, nBytes));
                }

                //关闭连接   
                c.Close();

                string[] o = sb.ToString().Split(' '); // 打断字符串   

                TimeSpan k = new TimeSpan();
                k = (TimeSpan)(DateTime.Now - startDT);// 得到开始到现在所消耗的时间   

                DateTime SetDT = Convert.ToDateTime(o[1] + " " + o[2]).Subtract(-k);// 减去中途消耗的时间   

                //处置北京时间 +8时   
                SetDT = SetDT.AddHours(8);

                //转换System.DateTime到SystemTime   
                SYSTEMTIME st = new SYSTEMTIME();
                st.FromDateTime(SetDT);

                //调用Win32 API设置系统时间   
                Win32API.SetLocalTime(ref st);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "时间已同步";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        /// <summary>
        /// 从System.DateTime转换。
        /// </summary>
        /// <param name="time">System.DateTime类型的时间。</param>
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
        /// <summary>
        /// 转换为System.DateTime类型。
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        /// <summary>
        /// 静态方法。转换为System.DateTime类型。
        /// </summary>
        /// <param name="time">SYSTEMTIME类型的时间。</param>
        /// <returns></returns>
        public static DateTime ToDateTime(SYSTEMTIME time)
        {
            return time.ToDateTime();
        }
    }
    public class Win32API
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME Time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME Time);
    }
}
