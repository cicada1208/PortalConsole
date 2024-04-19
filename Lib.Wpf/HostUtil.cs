using Params;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows;

namespace Lib.Wpf
{
    public class HostUtil
    {
    }

    public static class HostExUtil
    {
        /// <summary>
        /// 取得處理程序的使用者
        /// </summary>
        /// <returns>domain\user</returns>
        public static string GetProcessOwner(this Process process)
        {
            string owner = string.Empty;

            try
            {
                var query = $"Select * From Win32_Process Where ProcessID = '{process.Id}'";
                var searcher = new ManagementObjectSearcher(query);
                var processObj = searcher.Get().OfType<ManagementObject>().FirstOrDefault();
                if (processObj == null) return owner;
                var args = new string[2]; // argList[0]：user、argList[1]：domain
                int returnVal = Convert.ToInt32(processObj.InvokeMethod("GetOwner", args));
                if (returnVal == 0)
                    owner = string.Join(@"\", args.Reverse().ToArray());
            }
            catch (Exception) { }

            return owner;
        }

        /// <summary>
        /// 取得同名處理程序的數量
        /// </summary>
        /// <param name="sameOwner">是否判斷該處理程式使用者</param>
        public static int GetSameNameProcessNum(this Process process, bool sameOwner = true)
        {
            int num = 0;

            try
            {
                string processName = $"{process.ProcessName}.exe";
                var query = $"Select * From Win32_Process Where Name = '{processName}'";
                var searcher = new ManagementObjectSearcher(query);
                var processObjList = searcher.Get().OfType<ManagementObject>();
                if (processObjList == null) return num;

                if (sameOwner)
                {
                    string owner = process.GetProcessOwner();
                    string[] args;
                    int returnVal;
                    num = processObjList.Select(processObj =>
                    {
                        args = new string[2]; // argList[0]：user、argList[1]：domain
                        returnVal = Convert.ToInt32(processObj.InvokeMethod("GetOwner", args));
                        return returnVal == 0 ? string.Join(@"\", args.Reverse().ToArray()) : string.Empty;
                    }).Count(o => o == owner);
                }
                else
                    num = processObjList.Count();
            }
            catch (Exception) { }

            return num;
        }

        /// <summary>
        /// 當超過開啟限制時應用程式關閉
        /// </summary>
        /// <param name="limit">開啟限制數量</param>
        /// <param name="sameOwner">是否判斷該處理程式使用者</param>
        public static void AppShutdownWhenLimitReached(this Process process, int limit, bool sameOwner = true)
        {
            if (process.GetSameNameProcessNum(sameOwner) > limit)
            {
                MessageBox.Show($"系統已達開啟限制 {limit} 次。", MsgParam.TitlePrompt);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 取得處理程序的子程序
        /// </summary>
        public static List<Process> GetChildProcesses(this Process process) =>
            new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={process.Id}")
            .Get().Cast<ManagementObject>()
            .Select(mo => Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]))).ToList();

    }
}
