using GongSolutions.Wpf.DragDrop;
using Lib;
using Lib.Wpf;
using Lib.Wpf.PInvokes;
using Lib.Wpf.Routes;
using MaterialDesignThemes.Wpf;
using Models;
using MoreLinq;
using Params;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static Lib.Wpf.PInvokes.WindowPInvoke;
using static Params.FuncParam;
using static Params.SysAppParam;
using static Params.SysLogParam;

namespace ViewModels
{
    public class MainViewModel : BaseViewModel<MainViewModel>, IDragSource, IDropTarget
    {
        public MainViewModel()
        {
            PropertyChanged += MainViewModelPropertyChanged;
            Init().FireAndForget();
            IdleLockTimer.Elapsed += CheckIdleLock;
            IdleLockTimer.AutoReset = true;
            IdleLockTimer.Enabled = true;
        }

        public double ContentWidth => SystemParameters.WorkArea.Width - 160;

        public double ContentHeight => SystemParameters.WorkArea.Height - 100;

        /// <summary>
        /// 閒置鎖定：Time (milliseconds)
        /// </summary>
        private readonly double idleLockTime = 5 * 60 * 1000;

        private System.Timers.Timer _idleLockTimer;
        /// <summary>
        /// 閒置鎖定：Timer
        /// </summary>
        private System.Timers.Timer IdleLockTimer => _idleLockTimer ?? (_idleLockTimer = new System.Timers.Timer(idleLockTime));

        /// <summary>
        /// 閒置鎖定：CancellationTokenSource
        /// <para>使用者繼續使用 => when idleLockCancelTokenSource.IsCancellationRequested is true</para>
        /// </summary>
        private System.Threading.CancellationTokenSource idleLockCancelTokenSource = null;

        ///// <summary>
        ///// 閒置鎖定：使用者繼續使用
        ///// </summary>
        //private bool idleLockContinueToUse = false;

        private bool _idleLockMenuVisible = true;
        /// <summary>
        /// 閒置鎖定：選單顯示
        /// </summary>
        public bool IdleLockMenuVisible
        {
            get => _idleLockMenuVisible;
            set => Set(ref _idleLockMenuVisible, value);
        }

        private string _userIp;
        public string UserIp
        {
            get
            {
                if (_userIp == null)
                    _userIp = Lib.HostUtil.GetHostAddress();
                return _userIp;
            }
            set => Set(ref _userIp, value);
        }

        private string _userHostName;
        public string UserHostName
        {
            get
            {
                if (_userHostName == null)
                    _userHostName = Lib.HostUtil.GetHostName();
                return _userHostName;
            }
            set => Set(ref _userHostName, value);
        }

        //private string _userPhoto;
        //public string UserPhoto
        //{
        //    get
        //    {
        //        if (_userPhoto == null)
        //        {
        //            _userPhoto = $"https://web09.cych.org.tw/ASPNET/CychWebSites/NET2012/CychEip/Photos/{LoginViewModel.LoginUser.EmpId}.jpg";
        //        }
        //        return _userPhoto;
        //    }
        //    set => Set(ref _userPhoto, value);
        //}

        private BitmapImage _userBitmapImage;
        public BitmapImage UserBitmapImage
        {
            get
            {
                if (_userBitmapImage == null)
                {
                    _userBitmapImage = new BitmapImage();
                    _userBitmapImage.BeginInit();
                    _userBitmapImage.UriSource = new Uri($"https://web09.cych.org.tw/ASPNET/CychWebSites/NET2012/CychEip/Photos/{LoginViewModel.LoginUser.EmpId}.jpg");
                    _userBitmapImage.DecodePixelWidth = 200;
                    _userBitmapImage.EndInit();
                }
                return _userBitmapImage;
            }
            set => Set(ref _userBitmapImage, value);
        }

        private ObservableCollection<SysGroup> _userSysList;
        /// <summary>
        /// 使用者系統選單
        /// </summary>
        public ObservableCollection<SysGroup> UserSysList
        {
            get => _userSysList ?? (_userSysList = new ObservableCollection<SysGroup>());
            set
            {
                Set(ref _userSysList, value);
                if (_userSysList != null)
                    SetUserSysCollectionView(_userSysList);
            }
        }

        private ObservableCollection<SysGroup> _sysUserFavoriteList;
        /// <summary>
        /// 系統使用者最愛：選單
        /// </summary>
        public ObservableCollection<SysGroup> SysUserFavoriteList
        {
            get => _sysUserFavoriteList ?? (_sysUserFavoriteList = new ObservableCollection<SysGroup>());
            set => Set(ref _sysUserFavoriteList, value);
        }

        private bool _sysUserFavoriteLoading = false;
        /// <summary>
        /// 系統使用者最愛：加載狀態
        /// </summary>
        public bool SysUserFavoriteLoading
        {
            get => _sysUserFavoriteLoading;
            set => Set(ref _sysUserFavoriteLoading, value);
        }

        /// <summary>
        /// 系統使用者最愛：排序 Task
        /// </summary>
        private Task batchPatchSysUserFavoriteSeqTask;

        private Func<Task> _batchPatchSysUserFavoriteSeqDebounce;
        /// <summary>
        /// 系統使用者最愛：排序 Debounce
        /// </summary>
        private Func<Task> BatchPatchSysUserFavoriteSeqDebounce => _batchPatchSysUserFavoriteSeqDebounce ??
            (_batchPatchSysUserFavoriteSeqDebounce = ((Func<CancellationToken, Task>)BatchPatchSysUserFavoriteSeq).Debounce(4000));

        private List<USERMISSYSTEM> _userCychMisSysList;
        /// <summary>
        /// 使用者嘉基資訊系統清單
        /// </summary>
        public List<USERMISSYSTEM> UserCychMisSysList
        {
            get
            {
                if (_userCychMisSysList == null)
                {
                    _userCychMisSysList = ApiUtil.HttpClientEx<ApiResult<List<USERMISSYSTEM>>>(
                    UAACRoute.Service(), UAACRoute.MISSYSTEM.GetUSERMISSYSTEM,
                    method: ApiParam.HttpVerbs.Get,
                    queryParams: new { EMPNO = LoginViewModel.LoginUser.EmpId }).Data;
                }
                return _userCychMisSysList;
            }
            set => Set(ref _userCychMisSysList, value);
        }

        private IDictionary<string, string> _hisEnvironmentVariables;
        public IDictionary<string, string> HisEnvironmentVariables
        {
            get
            {
                if (_hisEnvironmentVariables == null)
                {
                    _hisEnvironmentVariables = new Dictionary<string, string>();
                    _hisEnvironmentVariables.Add("USER", "hmisa");
                    _hisEnvironmentVariables.Add("USERNAME", "hmisa");
                    _hisEnvironmentVariables.Add("SYBASE", @"C:\Syb157");
                    _hisEnvironmentVariables.Add("DSQUERY", "SYBASE1");
                    _hisEnvironmentVariables.Add("PATH", @"C:\Syb157\DataAccess\ADONET\dll;C:\Syb157\DataAccess\ODBC\dll;C:\Syb157\DataAccess\OLEDB\dll;C:\Syb157\OCS-15_0\lib3p64;C:\Syb157\OCS-15_0\lib3p;C:\Syb157\OCS-15_0\dll;C:\Syb157\OCS-15_0\bin;%PATH%;C:\Acu62;C:\CWIN;C:\HCA2.0;");
                }
                return _hisEnvironmentVariables;
            }
            set => Set(ref _hisEnvironmentVariables, value);
        }

        private List<SysProcess> _sysProcesses;
        /// <summary>
        /// 開啟的系統及對應的 Process
        /// </summary>
        private List<SysProcess> SysProcesses
        {
            get => _sysProcesses ?? (_sysProcesses = new List<SysProcess>());
            set => Set(ref _sysProcesses, value);
        }

        private ConcurrentDictionary<string, bool> _sysLoading;
        /// <summary>
        /// 開啟的系統及其狀態
        /// </summary>
        private ConcurrentDictionary<string, bool> SysLoading
        {
            get => _sysLoading ?? (_sysLoading = new ConcurrentDictionary<string, bool>());
            set => Set(ref _sysLoading, value);
        }

        private bool _sysTabIsSelected = true;
        /// <summary>
        /// 使用者系統選單：頁籤是否選取
        /// </summary>
        public bool SysTabIsSelected
        {
            get => _sysTabIsSelected;
            set => Set(ref _sysTabIsSelected, value);
        }

        private bool _userFavoriteTabIsSelected = false;
        /// <summary>
        /// 使用者最愛：頁籤是否選取
        /// </summary>
        public bool UserFavoriteTabIsSelected
        {
            get => _userFavoriteTabIsSelected;
            set => Set(ref _userFavoriteTabIsSelected, value);
        }

        private string _searchSysFunc;
        /// <summary>
        /// 篩選系統功能
        /// </summary>
        public string SearchSysFunc
        {
            get => _searchSysFunc;
            set
            {
                Set(ref _searchSysFunc, value);
                FuncListViewModel.SearchFunc = _searchSysFunc;
            }
        }

        private FuncListViewModel _funcListViewModel;
        public FuncListViewModel FuncListViewModel
        {
            get => _funcListViewModel ?? (_funcListViewModel = new FuncListViewModel());
        }

        private PageViewModel _pageViewModel;
        public PageViewModel PageViewModel
        {
            get => _pageViewModel ?? (_pageViewModel = new PageViewModel() { FuncListViewModel = FuncListViewModel });
        }

        /// <summary>
        /// 顯示登入畫面
        /// </summary>
        public Func<bool?> ShowLoginWindow;

        public PopupBox MainPopupBox;


        private async Task Init()
        {
            Task userFavoriteTasks = Task.WhenAll(GetSysUserFavorite(), FuncListViewModel.GetFuncUserFavorite());
            GetUserSys().FireAndForget();
            GetUserFunc().FireAndForget();
            await userFavoriteTasks;
            if (SysUserFavoriteList.Any() || FuncListViewModel.FuncUserFavoriteList.Any())
                UserFavoriteTabIsSelected = true;
        }

        /// <summary>
        /// 使用者資訊初始讀取
        /// </summary>
        private void InitUserInfo()
        {
            LoginViewModel.LoginUserPermission = null;
            //UserPhoto = null;
            UserBitmapImage = null;
            SysUserFavoriteList = null;
            FuncListViewModel.FuncUserFavoriteList = null;
            UserSysList = null;
            FuncListViewModel.UserFuncList = null;
            UserCychMisSysList = null;
            SearchSysFunc = string.Empty;
            Init().FireAndForget();
        }

        private async Task GetSysUserFavorite()
        {
            var sysUserFavoriteResult = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetFavoriteSysGroup,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new { userId = LoginViewModel.LoginUser.EmpId });

            SysUserFavoriteList = sysUserFavoriteResult.Data;
        }

        private async Task GetUserSys()
        {
            var userSysResult = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetSysGroup,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new { userId = LoginViewModel.LoginUser.EmpId, rootId = SysId.BasicRoot });

            UserSysList = userSysResult.Data;

            if (!(SysUserFavoriteList.Any() || FuncListViewModel.FuncUserFavoriteList.Any()) && UserSysList.Any())
                SysTabIsSelected = true;
        }

        private async Task GetUserFunc()
        {
            var userFuncResult = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<FuncGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetFuncGroup,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new
                {
                    userId = LoginViewModel.LoginUser.EmpId,
                    sysId = SysId.Portal,
                    rootId = FuncId.PortalRoot
                });

            FuncListViewModel.UserFuncList = userFuncResult.Data;
        }

        /// <summary>
        /// 查詢系統是否加載中
        /// </summary>
        /// <param name="selectedSys">選取的系統</param>
        /// <returns>when loading return true</returns>
        private bool GetSysLoading(SysGroup selectedSys)
        {
            return SysLoading.GetOrAdd(selectedSys.SysId, false);
        }

        /// <summary>
        /// 設定系統是否加載中
        /// </summary>
        /// <param name="selectedSys">選取的系統</param>
        /// <param name="loading">設定加載狀態</param>
        /// <returns>回傳的也是設定加載狀態</returns>
        private bool SetSysLoading(SysGroup selectedSys, bool loading)
        {
            selectedSys.Loading = loading; // 僅用於顯示 loading icon
            return SysLoading.AddOrUpdate(selectedSys.SysId, loading, (key, oldValue) => loading);
        }

        /// <summary>
        /// 查詢第一個加載中的系統
        /// </summary>
        /// <param name="loadingSysNameOrId">加載中系統的名稱或代碼</param>
        /// <returns>when existing any loading system then return true</returns>
        private bool GetFirstLoadingSys(out string loadingSysNameOrId)
        {
            bool loading = false;
            loadingSysNameOrId = string.Empty;

            string loadingSysId = SysLoading.FirstOrDefault(s => s.Value).Key;
            if (loadingSysId != null)
            {
                loading = true;
                SysGroup loadingSys = UserSysList.Flatten(s => s.SysApps).FirstOrDefault(s => s.SysId == loadingSysId);
                if (loadingSys == null)
                    loadingSys = SysUserFavoriteList.FirstOrDefault(s => s.SysId == loadingSysId);
                loadingSysNameOrId = loadingSys?.SysName ?? loadingSysId;
            }
            return loading;
        }

        private DelegateCommand<SysGroup> _selectSysCommand;
        public DelegateCommand<SysGroup> SelectSysCommand =>
            _selectSysCommand ?? (_selectSysCommand = new DelegateCommand<SysGroup>(OnSelectSys));
        /// <summary>
        /// 選取系統
        /// </summary>
        private async void OnSelectSys(SysGroup selectedSys)
        {
            Mouse.Capture(MainPopupBox, CaptureMode.SubTree);

            if (selectedSys == null || selectedSys.SysType == SysType.Catalog) return;

            if (GetSysLoading(selectedSys))
            {
                Global.MainSnackbar.MessageEnqueue("系統載入中，請稍候。");
                return;
            }

            bool limitReached = CheckSysLimitReached(selectedSys);
            if (limitReached)
            {
                Global.MainSnackbar.MessageEnqueue($"系統已達開啟限制 {selectedSys.Limit} 次。",
                    actionContent: $"關閉所有 {selectedSys.SysName}",
                    actionHandler: CloseSys,
                    actionArgument: selectedSys);
                return;
            }

            RunSysResult result = new RunSysResult();
            bool runSys = true;
            try
            {
                SetSysLoading(selectedSys, true);

                switch (selectedSys.SysType)
                {
                    case SysType.VersionExe:
                        result = await Task.Run(() => RunVersionExe(selectedSys));
                        break;
                    case SysType.CychMisExe:
                        result = await Task.Run(() => RunCychMisExe(selectedSys));
                        break;
                    case SysType.HISExe:
                        result = await Task.Run(() => RunHISExe(selectedSys));
                        break;
                    case SysType.Site:
                        result = await Task.Run(() => RunSite(selectedSys));
                        break;
                    default:
                        runSys = false;
                        break;
                }

                if (!result.Msg.IsNullOrWhiteSpace())
                    Global.MainSnackbar.MessageEnqueue(result.Msg, durationOverride: TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                result.Succ = false;
                result.Msg = ex.ToString();
                Global.MainSnackbar.MessageEnqueue("系統執行異常。",
                    actionContent: "詳細訊息",
                    actionHandler: (m) => MessageBox.Show(m, MsgParam.TitlePrompt),
                    actionArgument: result.Msg);
            }
            finally
            {
                SetSysLoading(selectedSys, false);

                if (runSys)
                {
                    ApiUtil.HttpClientExAsync<ApiResult<SysLog>>(
                        UAACRoute.Service(), UAACRoute.SysLog.Controller,
                        method: ApiParam.HttpVerbs.Post,
                        body: new SysLog
                        {
                            UserId = LoginViewModel.LoginUser.EmpId,
                            SysId = SysId.Portal,
                            ProcId = Process.GetCurrentProcess().Id.ToString(),
                            ActionType = ActionType.SysRun,
                            ActionTarget = selectedSys.SysId,
                            ControllerClass = nameof(MainViewModel),
                            ActionMethod = nameof(OnSelectSys),
                            State = result.Succ,
                            Msg = result.Msg
                        }).FireAndForget();
                }
            }
        }

        //private RunSysResult RunVersionExe(SysGroup selectedSys)
        //{
        //    RunSysResult result = new RunSysResult();

        //    bool serverAvalible = Utils.Host.CheckServerAvalible(selectedSys.BasePath.TrimStart('\\')) && Directory.Exists(selectedSys.Path);

        //    string cychAppRoot = @"C:\CychApp";
        //    string clientSysSite = $@"{cychAppRoot}\{selectedSys.SysId}"; //$@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\CychApp\{selectedSys.SysId}";
        //    //CreateCychAppRoot(cychAppRoot);
        //    if (!Directory.Exists(clientSysSite)) Directory.CreateDirectory(clientSysSite);

        //    // 取得 client 最大版本
        //    List<string> clientSysVers = Directory.GetDirectories(clientSysSite).ToList();
        //    Regex clientReg = new Regex(@"\d+$");
        //    string clientMaxVer = clientSysVers.Max(dir => clientReg.Match(dir).Value) ?? string.Empty;
        //    string clientMaxDirPath = string.Empty;
        //    if (clientMaxVer != string.Empty)
        //        clientMaxDirPath = clientSysVers.Where(dir => clientReg.Match(dir).Value == clientMaxVer).FirstOrDefault() ?? string.Empty;

        //    if (serverAvalible)
        //    {
        //        // 取得 server 最大版本
        //        List<string> serverSysVers = Directory.GetFileSystemEntries(selectedSys.Path).ToList();
        //        Regex serverReg = new Regex(@"\d+(\.zip)?$");
        //        string serverMaxVer = serverSysVers.Max(entry => serverReg.Match(entry).Value.Replace(".zip", string.Empty)) ?? string.Empty;
        //        string serverMaxEntryPath = string.Empty;
        //        if (serverMaxVer != string.Empty)
        //            serverMaxEntryPath = serverSysVers.Where(entry => serverReg.Match(entry).Value.Replace(".zip", string.Empty) == serverMaxVer).FirstOrDefault() ?? string.Empty;
        //        Regex zipReg = new Regex(@"(\.zip)$");
        //        bool isZip = File.Exists(serverMaxEntryPath) && zipReg.IsMatch(serverMaxEntryPath);

        //        // 依 server 最大版本為主
        //        if (serverMaxVer != string.Empty)
        //        {
        //            string serverMaxVerPathFinded = clientSysVers.Where(dir => clientReg.Match(dir).Value == serverMaxVer).FirstOrDefault() ?? string.Empty;
        //            if (serverMaxVerPathFinded != string.Empty)
        //            {
        //                clientMaxVer = serverMaxVer;
        //                clientMaxDirPath = serverMaxVerPathFinded;
        //            }
        //        }

        //        if (clientMaxVer != serverMaxVer)
        //        {
        //            string clientMaxEntryTmpRootPath = $@"{clientSysSite}\tmp\{LoginViewModel.LoginUser.EmpId}";
        //            if (!Directory.Exists(clientMaxEntryTmpRootPath)) Directory.CreateDirectory(clientMaxEntryTmpRootPath);
        //            string clientMaxEntryTmpPath = serverMaxEntryPath.Replace(selectedSys.Path, clientMaxEntryTmpRootPath);
        //            string clientMaxEntryPath = serverMaxEntryPath.Replace(selectedSys.Path, clientSysSite);
        //            WpfUtils.File.CopyFilesRecursively(serverMaxEntryPath, clientMaxEntryTmpPath);

        //            if (isZip)
        //            {
        //                string zipFilePath = clientMaxEntryTmpPath;
        //                clientMaxEntryTmpPath = zipReg.Replace(clientMaxEntryTmpPath, string.Empty);
        //                clientMaxEntryPath = zipReg.Replace(clientMaxEntryPath, string.Empty);
        //                ZipFile.ExtractToDirectory(zipFilePath, clientMaxEntryTmpPath);
        //                DeleteLegacyFile(zipFilePath);
        //            }

        //            WpfUtils.File.CopyFilesRecursively(clientMaxEntryTmpPath, clientMaxEntryPath);
        //            DeleteTmpEntries(clientMaxEntryTmpRootPath);
        //            clientMaxDirPath = clientMaxEntryPath;
        //        }
        //    }

        //    if (!serverAvalible)
        //        result.Msg += "伺服器異常或目錄不存在，無法確認系統版本。";

        //    string exeFile = $@"{clientMaxDirPath}\{selectedSys.Assembly}";
        //    if (File.Exists(exeFile))
        //    {
        //        InvokeSys(selectedSys, new ProcessStartInfo()
        //        {
        //            FileName = exeFile,
        //            UseShellExecute = false
        //        });

        //        DeleteLegacySys(clientSysSite, selectedSys.Assembly, clientMaxDirPath);
        //    }
        //    else if (!clientMaxDirPath.IsNullOrWhiteSpace())
        //    {
        //        result.Succ = false;
        //        result.Msg += "尚無執行檔，可能更新中，請稍候執行。";
        //        //DeleteClientMaxDir(clientMaxDirPath);
        //    }
        //    else
        //    {
        //        result.Succ = false;
        //        result.Msg += "無執行檔可執行。";
        //    }

        //    if (!result.Msg.IsNullOrWhiteSpace())
        //        Global.MainSnackbar.MessageEnqueue(result.Msg, durationOverride: TimeSpan.FromSeconds(10));

        //    return result;
        //}

        private async Task<RunSysResult> RunVersionExe(SysGroup selectedSys)
        {
            RunSysResult result = new RunSysResult();
            GetClientMaxVerResult getClientMaxVerResult;
            GetServerMaxVerResult getServerMaxVerResult;
            RunSysResult updateVersionExeResult;
            RunVersionExeConfig config = new RunVersionExeConfig();

            config.ClientSysSite = $@"{config.CychAppRoot}\{selectedSys.SysId}";
            //CreateCychAppRoot(config.CychAppRoot);
            if (!Directory.Exists(config.ClientSysSite)) Directory.CreateDirectory(config.ClientSysSite);
            config.SelectedSysPath = selectedSys.Path.TrimEnd('\\', '/');

            getClientMaxVerResult = GetClientMaxVer(config);
            bool serverAvalible = await CheckServerAvalible(selectedSys.BasePath, selectedSys.Path);
            if (!serverAvalible)
            {
                result.Succ = false;
                result.Msg += "伺服器異常或目錄不存在，無法確認系統版本。";
            }
            else
            {
                getServerMaxVerResult = await GetServerMaxVer(config);
                if (!getServerMaxVerResult.Succ)
                {
                    result.Succ = getServerMaxVerResult.Succ;
                    result.Msg += getServerMaxVerResult.Msg;
                }
                else
                {
                    updateVersionExeResult = await UpdateVersionExe(config, getServerMaxVerResult, getClientMaxVerResult);
                    if (!updateVersionExeResult.Succ)
                    {
                        result.Succ = updateVersionExeResult.Succ;
                        result.Msg += updateVersionExeResult.Msg;
                    }
                }
            }

            string exeFile = $@"{getClientMaxVerResult.ClientMaxDirPath}\{selectedSys.Assembly}";
            if (File.Exists(exeFile))
            {
                InvokeSys(selectedSys, new ProcessStartInfo()
                {
                    FileName = exeFile,
                    UseShellExecute = false
                });

                DeleteLegacySys(config.ClientSysSite, selectedSys.Assembly, getClientMaxVerResult.ClientMaxDirPath, config);
            }
            else if (getClientMaxVerResult.ClientMaxDirPath.IsNullOrWhiteSpace())
            {
                result.Succ = false;
                result.Msg += "執行檔目錄不存在。";
            }
            else
            {
                result.Succ = false;
                result.Msg += "執行檔不存在。";
            }

            return result;
        }

        private async Task<bool> CheckServerAvalible(string selectedSysBasePath, string selectedSysPath)
        {
            bool serverAvalible;

            if (selectedSysBasePath.StartsWith(@"\\"))
                serverAvalible = Utils.Host.CheckServerAvalible(selectedSysBasePath.Trim('\\')) && Directory.Exists(selectedSysPath);
            else // http(s)
                serverAvalible = (await ApiUtil.CheckHttpAvalible(selectedSysPath)).Succ;

            return serverAvalible;
        }

        /// <summary>
        /// 取得 client 最大版本
        /// </summary>
        private GetClientMaxVerResult GetClientMaxVer(RunVersionExeConfig config)
        {
            GetClientMaxVerResult result = new GetClientMaxVerResult
            {
                ClientSysVerDirs = Directory.GetDirectories(config.ClientSysSite).ToList()
            };

            string maxVerFilePath = $@"{config.ClientSysSite}\{config.MaxVerFileName}";
            if (File.Exists(maxVerFilePath))
                result.ClientMaxVer = config.ClientReg.Match(WpfUtils.File.ReadFile(maxVerFilePath).Trim()).Value ?? string.Empty;
            else
                result.ClientMaxVer = result.ClientSysVerDirs.Max(dir => config.ClientReg.Match(dir).Value) ?? string.Empty;

            if (result.ClientMaxVer != string.Empty)
                result.ClientMaxDirPath = result.ClientSysVerDirs.Where(dir =>
                config.ClientReg.Match(dir).Value == result.ClientMaxVer).FirstOrDefault() ?? string.Empty;

            return result;
        }

        /// <summary>
        /// 取得 server 最大版本
        /// </summary>
        private async Task<GetServerMaxVerResult> GetServerMaxVer(RunVersionExeConfig config)
        {
            GetServerMaxVerResult result = new GetServerMaxVerResult();

            if (config.SelectedSysPath.StartsWith(@"\\"))
            {
                List<string> serverSysVerEntries = Directory.GetFileSystemEntries(config.SelectedSysPath).ToList();

                string maxVerFilePath = $@"{config.SelectedSysPath}\{config.MaxVerFileName}";
                if (File.Exists(maxVerFilePath))
                    result.ServerMaxVer = config.ClientReg.Match(WpfUtils.File.ReadFile(maxVerFilePath).Trim()).Value ?? string.Empty;
                else
                    result.ServerMaxVer = serverSysVerEntries.Max(entry => config.ServerReg.Match(entry).Value.Replace(".zip", string.Empty)) ?? string.Empty;

                if (result.ServerMaxVer != string.Empty)
                    result.ServerMaxEntryPath = serverSysVerEntries.Where(entry =>
                    config.ServerReg.Match(entry).Value.Replace(".zip", string.Empty) == result.ServerMaxVer).FirstOrDefault() ?? string.Empty;
            }
            else // http(s)
            {
                string maxVerFileValue = (await ApiUtil.HttpClientGetString($"{config.SelectedSysPath}/{config.MaxVerFileName}")).Data.Trim();
                result.ServerMaxVer = config.ClientReg.Match(maxVerFileValue).Value ?? string.Empty;

                if (result.ServerMaxVer != string.Empty)
                    result.ServerMaxEntryPath = $@"{config.SelectedSysPath}/{maxVerFileValue}.zip";
            }

            if (result.ServerMaxVer == string.Empty)
            {
                result.Succ = false;
                result.Msg = "無法取得最新系統版本。";
            }

            return result;
        }

        /// <summary>
        /// 更版
        /// </summary>
        private async Task<RunSysResult> UpdateVersionExe(RunVersionExeConfig config,
            GetServerMaxVerResult getServerMaxVerResult, GetClientMaxVerResult getClientMaxVerResult)
        {
            RunSysResult result = new RunSysResult();
            string clientMaxEntryTmpRootPath = $@"{config.ClientSysSite}\{config.TempFolderName}\{LoginViewModel.LoginUser.EmpId}";

            try
            {
                Regex zipReg = new Regex(@"(\.zip)$");
                bool isZip = zipReg.IsMatch(getServerMaxVerResult.ServerMaxEntryPath);

                Regex maxVerFileValueReg = new Regex(@"([^\\/]+)$"); // iemr.cmbuild_7091.zip
                string maxVerFileValue = maxVerFileValueReg.Match(getServerMaxVerResult.ServerMaxEntryPath).Value.Replace(".zip", string.Empty);  // iemr.cmbuild_7091

                // 依 server 最大版本為主
                string serverMaxVerFindedInClient = getClientMaxVerResult.ClientSysVerDirs.Where(dir =>
                config.ClientReg.Match(dir).Value == getServerMaxVerResult.ServerMaxVer).FirstOrDefault() ?? string.Empty;
                if (serverMaxVerFindedInClient != string.Empty)
                {
                    if (getServerMaxVerResult.ServerMaxVer != getClientMaxVerResult.ClientMaxVer)
                        WpfUtils.File.WriteFile(maxVerFileValue, $@"{config.ClientSysSite}\{config.MaxVerFileName}", false);
                    getClientMaxVerResult.ClientMaxVer = getServerMaxVerResult.ServerMaxVer;
                    getClientMaxVerResult.ClientMaxDirPath = serverMaxVerFindedInClient;
                }

                // 版本不同則更版
                if (getServerMaxVerResult.ServerMaxVer != getClientMaxVerResult.ClientMaxVer)
                {
                    bool succ;
                    string clientMaxEntryTmpPath = string.Empty;
                    string clientMaxEntryPath = string.Empty;

                    // 下載檔案至用戶端暫存區
                    if (!Directory.Exists(clientMaxEntryTmpRootPath)) Directory.CreateDirectory(clientMaxEntryTmpRootPath);
                    if (getServerMaxVerResult.ServerMaxEntryPath.StartsWith(@"\\"))
                    {
                        clientMaxEntryTmpPath = getServerMaxVerResult.ServerMaxEntryPath.Replace(config.SelectedSysPath, clientMaxEntryTmpRootPath);
                        // clientMaxEntryTmpPath = C:\CychApp\iEMR\tmp\10964\iemr.cmbuild_7091.zip or iemr.cmbuild_7091
                        clientMaxEntryPath = getServerMaxVerResult.ServerMaxEntryPath.Replace(config.SelectedSysPath, config.ClientSysSite);
                        // clientMaxEntryPath = C:\CychApp\iEMR\iemr.cmbuild_7091.zip or iemr.cmbuild_7091
                        succ = WpfUtils.File.CopyFilesRecursively(getServerMaxVerResult.ServerMaxEntryPath, clientMaxEntryTmpPath);
                        // serverMaxEntryPath = \\tfsrepo\iemr2\iemr.cmbuild_7091.zip or iemr.cmbuild_7091
                    }
                    else // http(s)
                    {
                        clientMaxEntryTmpPath = getServerMaxVerResult.ServerMaxEntryPath.Replace($"{config.SelectedSysPath}/", $@"{clientMaxEntryTmpRootPath}\");
                        // clientMaxEntryTmpPath = C:\CychApp\iEMR\tmp\10964\iemr.cmbuild_7091.zip
                        clientMaxEntryPath = getServerMaxVerResult.ServerMaxEntryPath.Replace($"{config.SelectedSysPath}/", $@"{config.ClientSysSite}\");
                        // clientMaxEntryPath = C:\CychApp\iEMR\iemr.cmbuild_7091.zip
                        succ = (await ApiUtil.DownloadFile(getServerMaxVerResult.ServerMaxEntryPath, clientMaxEntryTmpRootPath)).Succ;
                    }
                    if (!succ)
                    {
                        result.Succ = false;
                        result.Msg = "下載檔案至用戶端暫存區失敗。";
                        return result;
                    }

                    if (isZip)
                    {
                        string zipFilePath = clientMaxEntryTmpPath;
                        clientMaxEntryTmpPath = zipReg.Replace(clientMaxEntryTmpPath, string.Empty);
                        // clientMaxEntryTmpPath = C:\CychApp\iEMR\tmp\10964\iemr.cmbuild_7091
                        clientMaxEntryPath = zipReg.Replace(clientMaxEntryPath, string.Empty);
                        //clientMaxEntryPath = C:\CychApp\iEMR\iemr.cmbuild_7091
                        ZipFile.ExtractToDirectory(zipFilePath, clientMaxEntryTmpPath);
                        DeleteLegacyFile(zipFilePath);
                    }

                    succ = WpfUtils.File.CopyFilesRecursively(clientMaxEntryTmpPath, clientMaxEntryPath);
                    if (!succ)
                    {
                        result.Succ = false;
                        result.Msg = "複製檔案至用戶端系統目錄失敗。";
                        return result;
                    }
                    else
                    {
                        WpfUtils.File.WriteFile(maxVerFileValue, $@"{config.ClientSysSite}\{config.MaxVerFileName}", false);
                        getClientMaxVerResult.ClientMaxVer = getServerMaxVerResult.ServerMaxVer;
                        getClientMaxVerResult.ClientMaxDirPath = clientMaxEntryPath;
                    }
                }
            }
            finally
            {
                DeleteTmpEntries(clientMaxEntryTmpRootPath);
            }

            return result;
        }

        private RunSysResult RunCychMisExe(SysGroup selectedSys)
        {
            RunSysResult result = new RunSysResult();
            string serverName = Utils.Config.ReadINI("ServerNow", "S0", @"\\SQL01\MIS$\Ini\Default.INI");

            WORKERS worker = ApiUtil.HttpClientEx<ApiResult<WORKERS>>(
                UAACRoute.Service(), UAACRoute.WORKERS.GetWORKER,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new { EMPNO = LoginViewModel.LoginUser.EmpId }).Data;

            USERMISSYSTEM userCychMisSys = UserCychMisSysList.Find(s => s.SYSID == selectedSys.SysId);

            string formName = "NONE";
            string arg = $"{worker?.EMPNO}^{worker?.NAME}^{userCychMisSys?.SYSNAME}^{userCychMisSys?.ROLE}^{userCychMisSys?.SYSID}^{formName}^{serverName}";

            InvokeSys(selectedSys, new ProcessStartInfo()
            {
                FileName = @"C:\CYCHMIS\UPDEXE.exe ",
                Arguments = arg,
                UseShellExecute = false
            });

            return result;
        }

        private RunSysResult RunHISExe(SysGroup selectedSys)
        {
            RunSysResult result = new RunSysResult();
            string arg = @"-c \\s1\progs95\zsy.dir\cblconfi.syb -7 zpclisf";

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = selectedSys.Path,
                Arguments = arg,
                UseShellExecute = false
            };

            foreach (KeyValuePair<string, string> e in HisEnvironmentVariables)
            {
                if (processStartInfo.EnvironmentVariables.ContainsKey(e.Key))
                    processStartInfo.EnvironmentVariables[e.Key] = e.Value;
                else
                    processStartInfo.EnvironmentVariables.Add(e.Key, e.Value);
            }

            InvokeSys(selectedSys, processStartInfo);

            return result;
        }

        private RunSysResult RunSite(SysGroup selectedSys)
        {
            RunSysResult result = new RunSysResult();

            ProcessStartInfo processStartInfo = WpfUtils.Browser.GetRunningBrowser("*.cych.org.tw");
            processStartInfo.Arguments += selectedSys.Path;
            InvokeSys(selectedSys, processStartInfo);

            return result;
        }

        private void InvokeSys(SysGroup selectedSys, ProcessStartInfo startInfo)
        {
            Process process = null;

            switch (selectedSys.SysType)
            {
                case SysType.VersionExe:
                case SysType.HISExe:
                case SysType.Site:
                    process = Process.Start(startInfo);
                    break;
                case SysType.CychMisExe:
                    process = new Process
                    {
                        StartInfo = startInfo,
                        EnableRaisingEvents = true
                    };
                    process.Exited += ProcessExited;
                    process.Start();
                    process.WaitForExit();
                    process.Exited -= ProcessExited;
                    process = process.GetChildProcesses().FirstOrDefault();
                    break;
            }

            if (process != null)
                SysProcesses.Add(new SysProcess { SysId = selectedSys.SysId, Process = process });
        }

        private void ProcessExited(Object sender, EventArgs e) =>
             Console.WriteLine("Process Exited");

        private bool CheckSysLimitReached(SysGroup selectedSys) =>
           selectedSys.Limit != null && SysProcesses.Count(sp => sp.SysId == selectedSys.SysId && !sp.Process.HasExited) >= selectedSys.Limit;

        /// <summary>
        /// 建立 CychApp 目錄並授權
        /// </summary>
        /// <remarks>改以 WinNexus 執行</remarks>
        private bool CreateCychAppRoot(string rootPath)
        {
            bool succ;

            try
            {
                string userName = "";
                string password = "";
                SecureString securePassword = new SecureString();

                for (int i = 0; i < password.Length; i++)
                    securePassword.AppendChar(Convert.ToChar(password[i]));
                securePassword.MakeReadOnly();

                string cmdMkdir = $@"mkdir {rootPath}";
                string cmdIcacls = $@"icacls ""{rootPath}"" /grant ""Domain Users"":(OI)(CI)F /T";
                string cmd = $@"/C if not exist ""{rootPath}"" {cmdMkdir} & {cmdIcacls}"; // use & to join commands
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmd,
                    UseShellExecute = false,
                    Domain = "cych",
                    UserName = userName,
                    Password = securePassword,
                });
                succ = true;
            }
            catch (Exception)
            {
                succ = false;
            }

            return succ;
        }

        private void DeleteLegacySys(string clientSysSite, string assembly, string clientMaxDirPath, RunVersionExeConfig config)
        {
            try
            {
                if (clientMaxDirPath.IsNullOrWhiteSpace()) return;

                List<string> clientDelEntries = Directory.GetFileSystemEntries(clientSysSite)
                    .Where(entry => entry != clientMaxDirPath
                    && entry != $@"{clientSysSite}\{config.TempFolderName}"
                    && entry != $@"{clientSysSite}\{config.MaxVerFileName}").ToList();

                clientDelEntries.ForEach(entry =>
                {
                    if (Directory.Exists(entry))
                    {
                        if (DeleteLegacyFile($@"{entry}\{assembly}"))
                            DeleteLegacyDirectory(entry);
                    }
                    else if (File.Exists(entry)) DeleteLegacyFile(entry);
                });
            }
            catch (Exception) { }
        }

        private bool DeleteLegacyFile(string path)
        {
            bool succ;

            try
            {
                File.Delete(path);
                succ = true;
            }
            catch (Exception)
            {
                succ = false;
            }

            return succ;
        }

        private bool DeleteLegacyDirectory(string path)
        {
            bool succ;

            try
            {
                Directory.Delete(path, true);
                succ = true;
            }
            catch (Exception)
            {
                succ = false;
            }

            return succ;
        }

        /// <summary>
        /// 刪除使用者暫存資料夾
        /// </summary>
        private void DeleteTmpEntries(string path)
        {
            try
            {
                List<string> clientDelEntries = Directory.GetFileSystemEntries(path).ToList();
                clientDelEntries.ForEach(entry =>
                {
                    if (Directory.Exists(entry)) DeleteLegacyDirectory(entry);
                    else if (File.Exists(entry)) DeleteLegacyFile(entry);
                });
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 若執行異常並已超過目錄建立 3 秒，可以此刪除 clientMaxDir
        /// </summary>
        /// <param name="path">clientMaxDir</param>
        private void DeleteClientMaxDir(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    DateTime creationTime = Directory.GetCreationTime(path);
                    // 3 秒應已複製完畢 ( clientMaxEntryTmpPath to clientMaxEntryPath )
                    if (DateTime.Now > creationTime.AddSeconds(3))
                        DeleteLegacyDirectory(path);
                }
            }
            catch (Exception) { }
        }

        private bool CheckAlivedOrLoading()
        {
            //return SysProcesses.Exists(sp => !sp.Process.HasExited) || FuncListViewModel.ContentFuncList.Any() || (UserSysList != null && UserSysList.Flatten(s => s.SysApps).Any(s => s.Loading));

            bool notIdle = LastInputInfoPInvoke.GetIdleTime() < idleLockTime;
            //return notIdle || (UserSysList != null && UserSysList.Flatten(s => s.SysApps).Any(s => s.Loading));
            return notIdle || SysLoading.Any(s => s.Value);
        }

        /// <summary>
        /// 閒置鎖定：確認
        /// </summary>
        private async void CheckIdleLock(object source, System.Timers.ElapsedEventArgs e)
        {
            IdleLockTimer.Stop();

            if (!CheckAlivedOrLoading())
            {
                int waitSeconds = 60;

                idleLockCancelTokenSource = new System.Threading.CancellationTokenSource();

                Global.MainSnackbar.MessageEnqueue($"已達閒置時間，系統將於 {waitSeconds} 秒後鎖定。",
                    actionContent: $"繼續使用",
                    actionHandler: () =>
                    {
                        //idleLockContinueToUse = true;
                        idleLockCancelTokenSource?.Cancel();
                    },
                    durationOverride: TimeSpan.FromSeconds(waitSeconds));

                //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(waitTime * 1000));
                await Task.Delay((waitSeconds + 2) * 1000, idleLockCancelTokenSource.Token).ContinueWith(task => { });

                //if ((!CheckAlivedOrLoading()) && (!idleLockContinueToUse))
                if ((!CheckAlivedOrLoading()) && (!idleLockCancelTokenSource.IsCancellationRequested))
                    LockAllSysFunc();
                else
                    IdleLockTimer.Start();

                idleLockCancelTokenSource?.Dispose();
                idleLockCancelTokenSource = null;
                //idleLockContinueToUse = false;
            }
            else
                IdleLockTimer.Start();
        }

        private DelegateCommand _lockCommand;
        public DelegateCommand LockCommand =>
            _lockCommand ?? (_lockCommand = new DelegateCommand(LockAllSysFunc));
        /// <summary>
        /// 鎖定所有系統功能
        /// </summary>
        private void LockAllSysFunc()
        {
            IdleLockTimer.Stop();

            IdleLockMenuVisible = false;

            Application.Current.Dispatcher.Invoke(() => FuncListViewModel.HidePageWindow?.Invoke());

            List<IntPtr> hwnds;
            SysProcesses.ForEach(sp =>
            {
                hwnds = sp.Process.GetProcessHWnds(true);
                if (hwnds.Any())
                {
                    sp.Hwnds = hwnds;
                    sp.Hwnds.ForEach(h => ShowWindowAsync(h, (int)ShowWindowCmds.SW_HIDE));
                }
            });
        }

        private DelegateCommand<FrameworkElement> _unLockCommand;
        public DelegateCommand<FrameworkElement> UnLockCommand =>
            _unLockCommand ?? (_unLockCommand = new DelegateCommand<FrameworkElement>(OnUnLock));
        /// <summary>
        /// 密碼解鎖所有系統功能
        /// </summary>
        private void OnUnLock(FrameworkElement dialogContent)
        {
            var vm = new UnLockViewModel();
            vm.CloseDialog = CloseUnLockDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseUnLockDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) UnLockAllSysFunc();
        }

        /// <summary>
        /// 解鎖所有系統功能
        /// </summary>
        private void UnLockAllSysFunc()
        {
            IdleLockMenuVisible = true;

            if (FuncListViewModel.ContentFuncList.Any())
                FuncListViewModel.ShowPageWindow?.Invoke();

            SysProcesses.ForEach(sp =>
            {
                sp.Hwnds?.ForEach(h => ShowWindowAsync(h, (int)ShowWindowCmds.SW_RESTORE));
                sp.Hwnds?.Clear();
            });

            IdleLockTimer.Start();
        }

        /// <summary>
        /// 關閉指定的系統
        /// </summary>
        private void CloseSys(SysGroup selectedSys)
        {
            SysProcesses.RemoveAll(sp =>
            {
                if (sp.SysId == selectedSys.SysId && !sp.Process.HasExited)
                    sp.Process.Kill();
                return sp.SysId == selectedSys.SysId;
            });
        }

        /// <summary>
        /// 關閉所有開啟的系統及功能
        /// </summary>
        private void CloseAllSysFunc()
        {
            SysProcesses.RemoveAll(sp =>
            {
                if (!sp.Process.HasExited)
                    sp.Process.Kill();
                return true;
            });

            FuncListViewModel?.ContentFuncList.Clear();
        }

        private bool CheckLoadingWhenSwitchOrLogout(string action, out string msg)
        {
            msg = string.Empty;
            //SysGroup loadingSys = UserSysList.Flatten(s => s.SysApps).FirstOrDefault(s => s.Loading);
            bool loading = GetFirstLoadingSys(out string loadingSysNameOrId);
            if (loading)
            {
                msg = $"{loadingSysNameOrId} 載入中，請稍候{action}。";
                goto exit;
            }

            loading = SysUserFavoriteLoading || FuncListViewModel.FuncUserFavoriteLoading;
            if (loading)
            {
                msg = $"我的最愛加入移除中，請稍候{action}。";
                goto exit;
            }

            loading = batchPatchSysUserFavoriteSeqTask?.IsCompleted == false ||
                FuncListViewModel.batchPatchFuncUserFavoriteSeqTask?.IsCompleted == false;
            if (loading)
            {
                msg = $"我的最愛排序儲存中，請稍候{action}。";
                goto exit;
            }

        exit:
            return loading;
        }

        private void CheckCloseAllSysAndSwitchUser()
        {
            bool loading = CheckLoadingWhenSwitchOrLogout("切換", out string msg);

            if (loading)
                Global.MainSnackbar.MessageEnqueue(msg, durationOverride: TimeSpan.FromSeconds(10));
            else
                CloseAllSysAndSwitchUser();
        }

        private void CloseAllSysAndSwitchUser()
        {
            IdleLockTimer.Stop();

            CloseAllSysFunc();

            string logoutUserId = LoginViewModel.LoginUser.EmpId;
            bool? dialogResult = ShowLoginWindow?.Invoke();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ApiUtil.HttpClientEx<ApiResult<SysLog>>(
                    UAACRoute.Service(), UAACRoute.SysLog.Controller,
                    method: ApiParam.HttpVerbs.Post,
                    body: new SysLog
                    {
                        UserId = logoutUserId,
                        SysId = SysId.Portal,
                        ProcId = Process.GetCurrentProcess().Id.ToString(),
                        ActionType = ActionType.Logout,
                        ControllerClass = MethodBase.GetCurrentMethod().DeclaringType.Name,
                        ActionMethod = MethodBase.GetCurrentMethod().Name,
                        State = true
                    });

                InitUserInfo();
                UnLockAllSysFunc();

                if (FuncListViewModel.IsLoginDeptFolder)
                    FuncListViewModel.RemoveConnDeptFolder();
            }
            else
            {
                if (IdleLockMenuVisible)
                    IdleLockTimer.Start();
            }
        }

        private DelegateCommand _switchUserCommand;
        public DelegateCommand SwitchUserCommand =>
            _switchUserCommand ?? (_switchUserCommand = new DelegateCommand(OnSwitchUser));
        /// <summary>
        /// 切換使用者
        /// </summary>
        private void OnSwitchUser()
        {
            string msg = string.Empty;
            if (SysProcesses.Exists(sp => !sp.Process.HasExited) || (FuncListViewModel?.ContentFuncList.Any() ?? false))
                msg = "系統功能執行中，請存檔後再點選確認關閉所有系統並切換。";
            else
                msg = "點選確認切換。";

            Global.MainSnackbar.MessageEnqueue(msg,
                actionContent: "確認",
                actionHandler: CheckCloseAllSysAndSwitchUser,
                durationOverride: TimeSpan.FromSeconds(10));
        }

        private void CheckCloseAllSysAndShutdown()
        {
            bool loading = CheckLoadingWhenSwitchOrLogout("登出", out string msg);
            if (loading)
                Global.MainSnackbar.MessageEnqueue(msg, durationOverride: TimeSpan.FromSeconds(10));
            else
                CloseAllSysAndShutdown();
        }

        private void CloseAllSysAndShutdown()
        {
            IdleLockTimer.Stop();

            SysProcesses.RemoveAll(sp =>
            {
                if (!sp.Process.HasExited)
                    sp.Process.Kill();
                return true;
            });

            ApiUtil.HttpClientEx<ApiResult<SysLog>>(
                UAACRoute.Service(), UAACRoute.SysLog.Controller,
                method: ApiParam.HttpVerbs.Post,
                body: new SysLog
                {
                    UserId = LoginViewModel.LoginUser.EmpId,
                    SysId = SysId.Portal,
                    ProcId = Process.GetCurrentProcess().Id.ToString(),
                    ActionType = ActionType.Logout,
                    ControllerClass = MethodBase.GetCurrentMethod().DeclaringType.Name,
                    ActionMethod = MethodBase.GetCurrentMethod().Name,
                    State = true
                });

            if (FuncListViewModel.IsLoginDeptFolder)
                FuncListViewModel.RemoveConnDeptFolder();

            Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
        }

        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(OnLogout));
        /// <summary>
        /// 登出
        /// </summary>
        private void OnLogout()
        {
            string msg = string.Empty;
            if (SysProcesses.Exists(sp => !sp.Process.HasExited) || (FuncListViewModel?.ContentFuncList.Any() ?? false))
                msg = "系統功能執行中，請存檔後再點選確認關閉所有系統並登出。";
            else
                msg = "點選確認登出。";

            Global.MainSnackbar.MessageEnqueue(msg,
                actionContent: "確認",
                actionHandler: CheckCloseAllSysAndShutdown,
                durationOverride: TimeSpan.FromSeconds(10));
        }

        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            OnLogout();
        }

        private DelegateCommand<FrameworkElement> _minimizeCommand;
        public DelegateCommand<FrameworkElement> MinimizeCommand =>
            _minimizeCommand ?? (_minimizeCommand = new DelegateCommand<FrameworkElement>(OnMinimize));
        /// <summary>
        /// 最小化
        /// </summary>
        private void OnMinimize(FrameworkElement win)
        {
            (win as Window).WindowState = WindowState.Minimized;
        }

        private DelegateCommand<SysGroup> _addRemoveSysUserFavoriteCommand;
        public DelegateCommand<SysGroup> AddRemoveSysUserFavoriteCommand =>
            _addRemoveSysUserFavoriteCommand ?? (_addRemoveSysUserFavoriteCommand = new DelegateCommand<SysGroup>
            (OnAddRemoveSysUserFavorite, param => !SysUserFavoriteLoading));
        /// <summary>
        /// 加入移除系統使用者最愛
        /// </summary>
        private async void OnAddRemoveSysUserFavorite(SysGroup selectedSys)
        {
            try
            {
                SysUserFavoriteLoading = true;
                ApiResult<object> result;

                if (selectedSys.Favorite)
                {
                    result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                        UAACRoute.Service(), UAACRoute.SysUserFavorite.Controller,
                        method: ApiParam.HttpVerbs.Post,
                        body: new SysUserFavorite
                        {
                            UserId = LoginViewModel.LoginUser.EmpId,
                            SysId = selectedSys.SysId,
                            Activate = true,
                            MUserId = LoginViewModel.LoginUser.EmpId
                        });
                }
                else
                {
                    result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                        UAACRoute.Service(), UAACRoute.SysUserFavorite.PatchDeactivate,
                        method: ApiParam.HttpVerbs.Patch,
                        body: new SysUserFavorite
                        {
                            SysUserFavoriteId = selectedSys.SysUserFavoriteId,
                            UserId = LoginViewModel.LoginUser.EmpId,
                            SysId = selectedSys.SysId,
                            MUserId = LoginViewModel.LoginUser.EmpId
                        });
                }

                if (!result.Succ)
                {
                    selectedSys.Favorite = !selectedSys.Favorite;
                    Global.MainSnackbar.MessageEnqueue(result.Msg);
                }
                else
                {
                    await GetSysUserFavorite();
                    UserSysList.Flatten(s => s.SysApps).ForEach(s =>
                    {
                        if (s.SysId == selectedSys.SysId)
                            s.Favorite = selectedSys.Favorite;
                    });
                }
            }
            catch (Exception ex)
            {
                ApiUtil.HttpClientExAsync<ApiResult<SysLog>>(
                    UAACRoute.Service(), UAACRoute.SysLog.Controller,
                    method: ApiParam.HttpVerbs.Post,
                    body: new SysLog
                    {
                        UserId = LoginViewModel.LoginUser.EmpId,
                        SysId = SysId.Portal,
                        ProcId = Process.GetCurrentProcess().Id.ToString(),
                        ActionType = ActionType.Exception,
                        ControllerClass = nameof(MainViewModel),
                        ActionMethod = nameof(OnAddRemoveSysUserFavorite),
                        State = false,
                        Msg = ex.ToString()
                    }).FireAndForget();
            }
            finally
            {
                SysUserFavoriteLoading = false;
                AddRemoveSysUserFavoriteCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 排序系統使用者最愛
        /// </summary>
        private async Task BatchPatchSysUserFavoriteSeq(CancellationToken cancellationToken = default)
        {
            List<SysUserFavorite> sysUserFavorites = new List<SysUserFavorite>();
            short seq = 1;

            SysUserFavoriteList.ForEach(f => sysUserFavorites.Add(new SysUserFavorite
            {
                SysUserFavoriteId = f.SysUserFavoriteId,
                Seq = seq++,
                MUserId = LoginViewModel.LoginUser.EmpId,
            }));

            await ApiUtil.HttpClientExAsync<ApiResult<List<SysUserFavorite>>>(
              UAACRoute.Service(), UAACRoute.SysUserFavorite.BatchPatchSeq,
              method: ApiParam.HttpVerbs.Patch,
              body: sysUserFavorites,
              cancellationToken: cancellationToken);
        }

        private void SetUserSysCollectionView(ObservableCollection<SysGroup> collection)
        {
            CollectionView view = CollectionViewSource.GetDefaultView(collection) as CollectionView;
            view.Filter = UserSysFilter;
            collection.Flatten(s => s.SysApps).ForEach(s =>
            {
                view = CollectionViewSource.GetDefaultView(s.SysApps) as CollectionView;
                view.Filter = UserSysFilter;
            });
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchSysFunc))
            {
                CollectionViewSource.GetDefaultView(UserSysList)?.Refresh();
                UserSysList.Flatten(s => s.SysApps).ForEach(s =>
                {
                    CollectionViewSource.GetDefaultView(s.SysApps)?.Refresh();
                });
            }
        }

        private bool UserSysFilter(object item)
        {
            bool result = true;

            if (!SearchSysFunc.IsNullOrWhiteSpace())
            {
                SysGroup userSys = item as SysGroup;
                result = userSys.SysName.ToLower().Contains(SearchSysFunc.ToLower()) ||
                    (userSys.SysType != SysType.Catalog && userSys.SysId.ToLower().Contains(SearchSysFunc.ToLower())) ||
                    userSys.SysApps.Flatten(s => s.SysApps).Any(s =>
                    s.SysName.ToLower().Contains(SearchSysFunc.ToLower()) ||
                    (s.SysType != SysType.Catalog && s.SysId.ToLower().Contains(SearchSysFunc.ToLower())));
            }

            return result;
        }


        #region DragDrop

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            //Console.WriteLine($"CanStartDrag / Mouse.Captured.1/ {Mouse.Captured}");
            Mouse.Capture(dragInfo.VisualSourceItem, CaptureMode.SubTree);
            //Console.WriteLine($"CanStartDrag / Mouse.Captured.2/ {Mouse.Captured}");
            return GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.CanStartDrag(dragInfo);
        }

        public void StartDrag(IDragInfo dragInfo) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.StartDrag(dragInfo);

        public void Dropped(IDropInfo dropInfo) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.Dropped(dropInfo);

        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            //Console.WriteLine($"DragDropOperationFinished / Mouse.Captured.1 / {Mouse.Captured}");
            Mouse.Capture(MainPopupBox, CaptureMode.SubTree);
            //Console.WriteLine($"DragDropOperationFinished / Mouse.Captured.2 / {Mouse.Captured}");
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.DragDropOperationFinished(operationResult, dragInfo);

            if (operationResult != DragDropEffects.None)
                batchPatchSysUserFavoriteSeqTask = BatchPatchSysUserFavoriteSeqDebounce();
        }

        public void DragCancelled() =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.DragCancelled();

        public bool TryCatchOccurredException(Exception exception) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.TryCatchOccurredException(exception);

        public void DragEnter(IDropInfo dropInfo) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragEnter(dropInfo);

        public void DragOver(IDropInfo dropInfo)
        {
            if (!dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter))
                GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo);
        }

        public void DragLeave(IDropInfo dropInfo) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragLeave(dropInfo);

        public void Drop(IDropInfo dropInfo) =>
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);

        #endregion


        class RunSysResult
        {
            public bool Succ { get; set; } = true;
            public string Msg { get; set; } = string.Empty;
        }

        class RunVersionExeConfig
        {
            public string CychAppRoot { get; } = @"C:\CychApp";
            public string ClientSysSite { get; set; } = string.Empty;
            /// <summary>
            /// XXX7091
            /// </summary>
            public Regex ClientReg { get; } = new Regex(@"\d+$");
            public string SelectedSysPath { get; set; } = string.Empty;
            /// <summary>
            /// zip file: XXX7091.zip or directory: XXX7091
            /// </summary>
            public Regex ServerReg { get; } = new Regex(@"\d+(\.zip)?$");
            public string MaxVerFileName { get; } = "maxver.txt";
            public string TempFolderName { get; } = "tmp";
        }

        class GetClientMaxVerResult
        {
            public bool Succ { get; set; } = true;
            public string Msg { get; set; } = string.Empty;
            public string ClientMaxVer { get; set; } = string.Empty;
            public string ClientMaxDirPath { get; set; } = string.Empty;
            public List<string> ClientSysVerDirs { get; set; }
        }

        class GetServerMaxVerResult
        {
            public bool Succ { get; set; } = true;
            public string Msg { get; set; } = string.Empty;
            public string ServerMaxVer { get; set; } = string.Empty;
            public string ServerMaxEntryPath { get; set; } = string.Empty;
        }

    }
}
