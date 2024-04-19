using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lib.Wpf
{
    public class BrowserUtil
    {
        public enum BrowserType
        {
            None = 0,
            Chrome = 1,
            Firefox = 2,
            Edge = 3,
            OldEdge = 4,
            IE = 5,
            //Safari = 6,
        }

        public readonly Dictionary<string, BrowserType> BrowserUserChoiceProgidToType = new Dictionary<string, BrowserType>
        {
            { "chrome", BrowserType.Chrome },
            { "firefox", BrowserType.Firefox },
            { "edge", BrowserType.Edge },
            { "appxq0fevzme2pys62n3e0fbqa7peapykr8v", BrowserType.OldEdge },
            { "ie.http", BrowserType.IE },
            //{ "safari", BrowserType.Safari },
        };

        public const string chromeExe = "chrome.exe";
        public const string firefoxExe = "firefox.exe";
        public const string edgeExe = "msedge.exe";
        public const string ieExe = "iexplore.exe";

        public readonly Dictionary<BrowserType, string> BrowserTypeToExeName = new Dictionary<BrowserType, string>
        {
            { BrowserType.Chrome, chromeExe},
            { BrowserType.Firefox, firefoxExe },
            { BrowserType.Edge, edgeExe },
            { BrowserType.OldEdge, edgeExe },
            { BrowserType.IE, ieExe },
        };


        public BrowserType GetDefaultBrowserType()
        {
            const string userChoiceName = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoiceName))
            {
                if (userChoiceKey == null)
                    return BrowserType.None;

                string progId = userChoiceKey.GetValue("Progid").NullableToStr().ToLower();

                foreach (var browser in BrowserUserChoiceProgidToType)
                {
                    if (progId.Contains(browser.Key))
                        return browser.Value;
                }

                return BrowserType.None;
            }
        }

        public bool CheckBrowserInstalled(string browserExeName)
        {
            const string appPathsName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            using (RegistryKey appPathsKey = Registry.LocalMachine.OpenSubKey(appPathsName))
            {
                if (appPathsKey != null)
                    using (RegistryKey browserExeKey = appPathsKey.OpenSubKey(browserExeName))
                    {
                        if (browserExeKey != null)
                            return true;
                    }
            }
            return false;
        }

        public ProcessStartInfo GetRunningBrowser(string authWhitelist = "")
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = chromeExe,
                UseShellExecute = true,
            };

            HashSet<string> browserExeNames = new HashSet<string> {
                chromeExe,
                firefoxExe,
                edgeExe,
                ieExe
            };

            BrowserType defaultBrowser = GetDefaultBrowserType();

            if (defaultBrowser == BrowserType.IE || defaultBrowser == BrowserType.None)
            {
                foreach (var browserExeName in browserExeNames)
                {
                    if (CheckBrowserInstalled(browserExeName))
                    {
                        processStartInfo.FileName = browserExeName;
                        SetProcessArg(processStartInfo, authWhitelist);
                        break;
                    }
                }
            }
            else
            {
                if (BrowserTypeToExeName.TryGetValue(defaultBrowser, out string browserExeName))
                {
                    processStartInfo.FileName = browserExeName;
                    SetProcessArg(processStartInfo, authWhitelist);
                }
            }

            return processStartInfo;
        }

        /// <summary>
        /// 設定處理序參數
        /// </summary>
        /// <param name="authWhitelist">e.g. *.cych.org.tw</param>
        private void SetProcessArg(ProcessStartInfo processStartInfo, string authWhitelist = "")
        {
            switch (processStartInfo.FileName)
            {
                case chromeExe:
                    processStartInfo.Arguments = $" --new-window --auth-server-whitelist=\"{authWhitelist}\" --auth-negotiate-delegate-whitelist=\"{authWhitelist}\" --auth-schemes=\"ntlm\" ";
                    break;
            }
        }

    }
}
