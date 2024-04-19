using GongSolutions.Wpf.DragDrop;
using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using MaterialDesignThemes.Wpf;
using Models;
using MoreLinq;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static Params.FuncParam;
using static Params.SysAppParam;
using static Params.SysLogParam;

namespace ViewModels
{
    public class FuncListViewModel : BaseViewModel<FuncListViewModel>, IDragSource, IDropTarget
    {
        public FuncListViewModel()
        {
            PropertyChanged += FuncListViewModelPropertyChanged;
        }

        private ObservableCollection<FuncGroup> _userFuncList;
        /// <summary>
        /// 使用者功能選單
        /// </summary>
        public ObservableCollection<FuncGroup> UserFuncList
        {
            get => _userFuncList ?? (_userFuncList = new ObservableCollection<FuncGroup>());
            set
            {
                Set(ref _userFuncList, value);
                if (_userFuncList != null)
                    SetUserFuncCollectionView(_userFuncList);
            }
        }

        private ObservableCollection<FuncGroup> _funcUserFavoriteList;
        /// <summary>
        /// 功能使用者最愛：選單
        /// </summary>
        public ObservableCollection<FuncGroup> FuncUserFavoriteList
        {
            get => _funcUserFavoriteList ?? (_funcUserFavoriteList = new ObservableCollection<FuncGroup>());
            set => Set(ref _funcUserFavoriteList, value);
        }

        private bool _funcUserFavoriteLoading = false;
        /// <summary>
        /// 功能使用者最愛：加載狀態
        /// </summary>
        public bool FuncUserFavoriteLoading
        {
            get => _funcUserFavoriteLoading;
            set => Set(ref _funcUserFavoriteLoading, value);
        }

        /// <summary>
        /// 功能使用者最愛：排序 Task
        /// </summary>
        public Task batchPatchFuncUserFavoriteSeqTask;

        private Func<Task> _batchPatchFuncUserFavoriteSeqDebounce;
        /// <summary>
        /// 功能使用者最愛：排序 Debounce
        /// </summary>
        private Func<Task> BatchPatchFuncUserFavoriteSeqDebounce => _batchPatchFuncUserFavoriteSeqDebounce ??
            (_batchPatchFuncUserFavoriteSeqDebounce = ((Func<CancellationToken, Task>)BatchPatchFuncUserFavoriteSeq).Debounce(4000));

        private ObservableCollection<FuncGroup> _contentFuncList;
        /// <summary>
        /// 功能頁籤列表
        /// </summary>
        public ObservableCollection<FuncGroup> ContentFuncList
        {
            get => _contentFuncList ?? (_contentFuncList = new ObservableCollection<FuncGroup>());
            set => Set(ref _contentFuncList, value);
        }

        private FuncGroup _selectedFunc;
        /// <summary>
        /// 選取的功能頁籤
        /// </summary>
        public FuncGroup SelectedFunc
        {
            get => _selectedFunc;
            set => Set(ref _selectedFunc, value);
        }

        private string _searchFunc;
        /// <summary>
        /// 篩選功能
        /// </summary>
        public string SearchFunc
        {
            get => _searchFunc;
            set => Set(ref _searchFunc, value);
        }

        /// <summary>
        /// 顯示功能頁籤畫面
        /// </summary>
        public Action ShowPageWindow;

        /// <summary>
        /// 隱藏功能頁籤畫面
        /// </summary>
        public Action HidePageWindow;

        public PopupBox MainPopupBox;


        private bool SelectFuncExisted(FuncGroup selectedFunc)
        {
            FuncGroup targetFunc = ContentFuncList.FirstOrDefault(f => f.FuncId == selectedFunc.FuncId);
            bool existed = targetFunc != null;
            if (existed)
                SelectedFunc = targetFunc;
            return existed;
        }

        private DelegateCommand<FuncGroup> _selectFuncCommand;
        public DelegateCommand<FuncGroup> SelectFuncCommand =>
            _selectFuncCommand ?? (_selectFuncCommand = new DelegateCommand<FuncGroup>(OnSelectFunc));
        private void OnSelectFunc(FuncGroup selectedFunc)
        {
            Mouse.Capture(MainPopupBox, CaptureMode.SubTree);

            if (selectedFunc == null) return;

            //if (ContentFuncList.Contains(selectedFunc))
            //    SelectedFunc = selectedFunc;
            if (!SelectFuncExisted(selectedFunc))
            {
                if (selectedFunc.FuncType == FuncParam.FuncType.Root ||
                    selectedFunc.FuncType == FuncParam.FuncType.Catalog ||
                    selectedFunc.BasePath.IsNullOrWhiteSpace()) return;

                switch (selectedFunc.FuncType)
                {
                    case FuncParam.FuncType.WpfPage:
                        Frame frame = new Frame();
                        frame.Navigate(new Uri($"{selectedFunc.BasePath}.xaml", UriKind.RelativeOrAbsolute));
                        selectedFunc.ViewInstance = frame;
                        ContentFuncList.Add(selectedFunc);
                        SelectedFunc = selectedFunc;
                        break;
                    case FuncParam.FuncType.WpfWindow:
                        Window win = WpfUtils.Ctrl.GetWindow(selectedFunc.BasePath, selectedFunc.Assembly, selectedFunc.FuncName);
                        win?.ShowDialog();
                        break;
                    case FuncParam.FuncType.WpfMethod:
                        Utils.Common.InvokeMethodFromAssembly(selectedFunc.SubPath, selectedFunc.BasePath, selectedFunc.Assembly);
                        break;
                    default:
                        // 未實做
                        break;
                }
            }

            switch (selectedFunc.FuncType)
            {
                case FuncParam.FuncType.WpfMethod:
                    break;
                default:
                    ShowPageWindow?.Invoke();
                    break;
            }
        }

        private DelegateCommand<FuncGroup> _closeFuncCommand;
        public DelegateCommand<FuncGroup> CloseFuncCommand =>
             _closeFuncCommand ?? (_closeFuncCommand = new DelegateCommand<FuncGroup>(OnCloseFunc));
        private void OnCloseFunc(FuncGroup selectedFunc)
        {
            if (selectedFunc == null) return;
            selectedFunc.DisposeViewInstance();
            ContentFuncList.Remove(selectedFunc);
        }

        public async Task GetFuncUserFavorite()
        {
            var funcUserFavoriteResult = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<FuncGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetFavoriteFuncGroup,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new { userId = LoginViewModel.LoginUser.EmpId, sysId = SysId.Portal });

            FuncUserFavoriteList = funcUserFavoriteResult.Data;
        }

        private DelegateCommand<FuncGroup> _addRemoveFuncUserFavoriteCommand;
        public DelegateCommand<FuncGroup> AddRemoveFuncUserFavoriteCommand =>
            _addRemoveFuncUserFavoriteCommand ?? (_addRemoveFuncUserFavoriteCommand = new DelegateCommand<FuncGroup>
            (OnAddRemoveFuncUserFavorite, param => !FuncUserFavoriteLoading));
        /// <summary>
        /// 加入移除功能使用者最愛
        /// </summary>
        private async void OnAddRemoveFuncUserFavorite(FuncGroup selectedFunc)
        {
            try
            {
                FuncUserFavoriteLoading = true;
                ApiResult<object> result;

                if (selectedFunc.Favorite)
                {
                    result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                        UAACRoute.Service(), UAACRoute.FuncUserFavorite.Controller,
                        method: ApiParam.HttpVerbs.Post,
                        body: new FuncUserFavorite
                        {
                            UserId = LoginViewModel.LoginUser.EmpId,
                            SysId = selectedFunc.SysId,
                            FuncId = selectedFunc.FuncId,
                            Activate = true,
                            MUserId = LoginViewModel.LoginUser.EmpId
                        });
                }
                else
                {
                    result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                        UAACRoute.Service(), UAACRoute.FuncUserFavorite.PatchDeactivate,
                        method: ApiParam.HttpVerbs.Patch,
                        body: new FuncUserFavorite
                        {
                            FuncUserFavoriteId = selectedFunc.FuncUserFavoriteId,
                            UserId = LoginViewModel.LoginUser.EmpId,
                            SysId = selectedFunc.SysId,
                            FuncId = selectedFunc.FuncId,
                            MUserId = LoginViewModel.LoginUser.EmpId
                        });
                }

                if (!result.Succ)
                {
                    selectedFunc.Favorite = !selectedFunc.Favorite;
                    Global.MainSnackbar.MessageEnqueue(result.Msg);
                }
                else
                {
                    await GetFuncUserFavorite();
                    UserFuncList.Flatten(f => f.Funcs).ForEach(f =>
                    {
                        if (f.FuncId == selectedFunc.FuncId)
                            f.Favorite = selectedFunc.Favorite;
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
                        ControllerClass = nameof(FuncListViewModel),
                        ActionMethod = nameof(OnAddRemoveFuncUserFavorite),
                        State = false,
                        Msg = ex.ToString()
                    }).FireAndForget();
            }
            finally
            {
                FuncUserFavoriteLoading = false;
                AddRemoveFuncUserFavoriteCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 排序功能使用者最愛
        /// </summary>
        private async Task BatchPatchFuncUserFavoriteSeq(CancellationToken cancellationToken = default)
        {
            List<FuncUserFavorite> funcUserFavorites = new List<FuncUserFavorite>();
            short seq = 1;

            FuncUserFavoriteList.ForEach(f => funcUserFavorites.Add(new FuncUserFavorite
            {
                FuncUserFavoriteId = f.FuncUserFavoriteId,
                Seq = seq++,
                MUserId = LoginViewModel.LoginUser.EmpId,
            }));

            await ApiUtil.HttpClientExAsync<ApiResult<List<FuncUserFavorite>>>(
              UAACRoute.Service(), UAACRoute.FuncUserFavorite.BatchPatchSeq,
              method: ApiParam.HttpVerbs.Patch,
              body: funcUserFavorites,
              cancellationToken: cancellationToken);
        }

        private void SetUserFuncCollectionView(ObservableCollection<FuncGroup> collection)
        {
            CollectionView view = CollectionViewSource.GetDefaultView(collection) as CollectionView;
            view.Filter = UserFuncFilter;
            collection.Flatten(f => f.Funcs).ForEach(f =>
            {
                view = CollectionViewSource.GetDefaultView(f.Funcs) as CollectionView;
                view.Filter = UserFuncFilter;
            });
        }

        private void FuncListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchFunc))
            {
                CollectionViewSource.GetDefaultView(UserFuncList)?.Refresh();
                UserFuncList.Flatten(f => f.Funcs).ForEach(f =>
                {
                    CollectionViewSource.GetDefaultView(f.Funcs)?.Refresh();
                });
            }
        }

        private bool UserFuncFilter(object item)
        {
            bool result = true;

            if (!SearchFunc.IsNullOrWhiteSpace())
            {
                FuncGroup userFunc = item as FuncGroup;
                result = userFunc.FuncName.ToLower().Contains(SearchFunc.ToLower()) ||
                    (userFunc.FuncType != FuncType.Catalog && (userFunc.ViewName.ToLower().Contains(SearchFunc.ToLower()) || userFunc.SubPath.ToLower().Contains(SearchFunc.ToLower()))) ||
                    userFunc.Funcs.Flatten(f => f.Funcs).Any(f =>
                    f.FuncName.ToLower().Contains(SearchFunc.ToLower()) ||
                    (f.FuncType != FuncType.Catalog && (f.ViewName.ToLower().Contains(SearchFunc.ToLower()) || f.SubPath.ToLower().Contains(SearchFunc.ToLower()))));
            }

            return result;
        }

        #region 登入科室資料夾

        /// <summary>
        /// 是否登入科室資料夾
        /// </summary>
        public static bool IsLoginDeptFolder = false;

        /// <summary>
        /// 登入科室資料夾
        /// </summary>
        private static void LoginDeptFolder()
        {
            try
            {
                string deptPath = @"\\cych.org.tw\share";
                string adAccount = string.Format("cych.org.tw\\{0}", LoginViewModel.LoginUser.EmpId);
                string adPassword = LoginViewModel.LoginUser.Password;

                RemoveConnDeptFolder();
                ConnDeptFolder(deptPath, adAccount, adPassword);

                // 暫停 1 sec
                System.Threading.Thread.Sleep(1000);

                // 開啟 \\cych.org.tw\share
                Process.Start(new ProcessStartInfo()
                {
                    FileName = deptPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception) { }
        }

        public static void RemoveConnDeptFolder()
        {
            try
            {
                ExecuteCmd(@"net use * /delete /yes");
                IsLoginDeptFolder = false;
            }
            catch (Exception) { }
        }

        private static void ConnDeptFolder(string deptPath, string adAccount, string adPassword)
        {
            try
            {
                ExecuteCmd($@"net use {deptPath} /user:{adAccount} {adPassword}");
                IsLoginDeptFolder = true;
            }
            catch (Exception) { }
        }

        private static void ExecuteCmd(string cmd)
        {
            ManagementClass processClass = new ManagementClass("Win32_Process");
            object[] methodArgs = { cmd, null, null, 0 };
            object result = processClass.InvokeMethod("Create", methodArgs);
        }

        #endregion

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
                batchPatchFuncUserFavoriteSeqTask = BatchPatchFuncUserFavoriteSeqDebounce();
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

    }
}
