using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkRepair.Business
{
    public static class CmdExecuteHelper
    {
        public static string Execute(string cmd)
        {
            Process cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";

            cmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            cmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            cmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            cmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            cmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出

            cmdProcess.StartInfo.Arguments = "/c " + cmd;
            cmdProcess.Start();
            string result = cmdProcess.StandardOutput.ReadToEnd();
            result = result.Trim();

            //等待程序执行完退出进程
            cmdProcess.WaitForExit();
            cmdProcess.Close();
            return result;
        }
    }
}
