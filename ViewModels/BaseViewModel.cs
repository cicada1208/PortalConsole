using FluentValidation;
//using FluentValidation.Results;
using GalaSoft.MvvmLight;
using Lib;
using Lib.Wpf;
using Models;
using Params;
using System;
//using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
//using System.Reflection;
//using System.Runtime.CompilerServices;
using ViewModels.FluentValidators;
using static Params.EditParam;

namespace ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase, IDataErrorInfo where T : class
    {
        #region INotifyPropertyChanged

        ///// <summary>
        ///// Property 變更通知機制，通知 Binding 更新，此為委派事件
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        /////  賦值並通知 Property 已經修改 Binding 更新，Property 同步方法
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="field"></param>
        ///// <param name="newValue"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        //{
        //    if (!EqualityComparer<T>.Default.Equals(field, newValue))
        //    {
        //        field = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //        return true;
        //    }
        //    return false;
        //}

        //private IMessenger _messengerInstance;
        ///// <summary>
        ///// Gets the Messenger's default instance
        ///// </summary>
        //protected IMessenger MessengerInstance
        //{
        //    get
        //    {
        //        if (_messengerInstance == null)
        //            _messengerInstance = Messenger.Default;
        //        return _messengerInstance;
        //    }
        //}

        #endregion


        #region IDataErrorInfo(ValidatesOnDataErrors=True) + System.ComponentModel.DataAnnotations.Validator

        ///// <summary>
        ///// Property 驗證錯誤訊息
        ///// </summary>
        ///// <returns>若驗證成功回傳 string.Empty</returns>
        ///// <remarks>若對該 Model 使用 GetProperties( ) 需排除繼承屬性，因為不能存取到 this</remarks>
        //public string this[string propertyName]
        //{
        //    get
        //    {
        //        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>(); // 記錄錯誤
        //        var property = GetType().GetProperty(propertyName); // 取得驗證物件
        //        // 某一特定程式點所需滿足的驗證條件
        //        Contract.Assert(null != property); // 確認有驗證物件對象
        //        var validationContext = new ValidationContext(this)
        //        {
        //            MemberName = propertyName
        //        };
        //        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(
        //            property.GetValue(this), validationContext, validationResults);
        //        string result = string.Empty;
        //        if (!isValid)
        //        {
        //            // 驗證錯誤，回傳錯誤訊息
        //            // result = validationResults.First().ErrorMessage;
        //            result = string.Join(Environment.NewLine, validationResults.Select(rst => rst.ErrorMessage).ToArray());
        //        }
        //        return result;
        //    }
        //}

        ///// <summary>
        ///// 整份 ViewModel 驗證錯誤訊息
        ///// </summary>
        ///// <returns>若驗證成功回傳 string.Empty</returns>
        //public string Error
        //{
        //    get
        //    {
        //        string entityError = string.Empty;
        //        var props = GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);  // 排除繼承屬性
        //        foreach (var prop in props)
        //        {
        //            var propError = this[prop.Name];
        //            if (propError != string.Empty)
        //                entityError += $"{prop.Name}:{Environment.NewLine}{propError}{Environment.NewLine}";
        //        }
        //        return entityError;
        //    }
        //}

        #endregion


        #region FluentValidation

        private ValidatorFactory _validatorFactory;
        /// <summary>
        /// [FluentValidation] ValidatorFactory
        /// </summary>
        private ValidatorFactory ValidatorFactory =>
            _validatorFactory ?? (_validatorFactory = new ValidatorFactory());

        private AbstractValidator<T> _validator;
        /// <summary>
        /// [FluentValidation] Validator
        /// </summary>
        /// <remarks>若定義 public，於 ApiUtil.HttpClientEx會出錯</remarks>
        protected AbstractValidator<T> Validator
        {
            get
            {
                if (_validator == null)
                    _validator = ValidatorFactory.GetValidator(this.GetType().UnderlyingSystemType) as AbstractValidator<T>;
                return _validator;
            }
        }

        ///// <summary>
        ///// [FluentValidation] IsValid(for the entire entity)
        ///// </summary>
        //public bool ValidationIsValid() =>
        //    Validator?.Validate(this as T).IsValid ?? true;

        ///// <summary>
        ///// [FluentValidation] Errors(for the entire entity)
        ///// </summary>
        //public IList<ValidationFailure> ValidationErrors() =>
        //    Validator?.Validate(this as T).Errors;

        /// <summary>
        /// [FluentValidation] Validate
        /// </summary>
        /// <param name="properties">property names to validate</param>
        public FluentValidation.Results.ValidationResult Validate(params string[] properties)
        {
            if (properties.Length == 0)
                return Validator?.Validate(this as T);
            else
                return Validator?.Validate(this as T, options => options.IncludeProperties(properties));
        }

        #endregion


        #region IDataErrorInfo(ValidatesOnDataErrors=True) + FluentValidation

        /// <summary>
        /// [IDataErrorInfo] Property 驗證錯誤訊息
        /// </summary>
        /// <returns>若驗證成功回傳 string.Empty</returns>
        /// <remarks>若對該 Model 使用 GetProperties( ) 需排除繼承屬性，因為不能存取到 this</remarks>
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                //var validationFailure = ValidationErrors?.FirstOrDefault(f => f.PropertyName == propertyName);
                var validationFailure = Validator?.Validate(this as T, options => options.IncludeProperties(propertyName))
                    .Errors.FirstOrDefault();
                result = validationFailure?.ErrorMessage ?? string.Empty;
                return result;
            }
        }

        /// <summary>
        /// [IDataErrorInfo] 整份 ViewModel 驗證錯誤訊息
        /// </summary>
        /// <returns>若驗證成功回傳 string.Empty</returns>
        public string Error
        {
            get
            {
                string entityError = string.Empty;
                var validationResult = Validator?.Validate(this as T);
                if (validationResult != null && validationResult.Errors.Any())
                    entityError = string.Join(Environment.NewLine, validationResult.Errors.Select(f => f.ErrorMessage).ToArray());
                return entityError;
            }
        }

        #endregion


        #region INotifyDataErrorInfo(ValidatesOnNotifyDataErrors=True(default)) + FluentValidation

        ///// <summary>
        ///// [INotifyDataErrorInfo] Occurs when the validation errors have changed for a property or for the entire entity.
        ///// </summary>
        ///// <remarks>microsoft docs: binding engine handles the ErrorsChanged 
        ///// event to monitor for updates.</remarks>
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        ///// <summary>
        ///// [INotifyDataErrorInfo] 
        ///// </summary>
        //protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null) =>
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        ///// <summary>
        ///// [INotifyDataErrorInfo] 僅判斷觸發GetErrors，顧慮效能，統一回傳true
        ///// </summary>
        ///// <remarks>microsoft docs: binding engine never uses the HasErrors property.</remarks>
        //public bool HasErrors => true;

        ///// <summary>
        ///// [INotifyDataErrorInfo] Property 驗證錯誤訊息
        ///// </summary>
        ///// <param name="propertyName">空值回傳 null (因 GetErrors 由 WPF 觸發不可控，
        ///// 顧慮效能，故不做整份 ViewModel 驗證錯誤訊息)</param>
        ///// <remarks>microsoft docs: whenever the errors change for a bound property or entity, 
        ///// the binding engine calls GetErrors to retrieve the updated errors.</remarks>
        //public IEnumerable GetErrors([CallerMemberName] string propertyName = null)
        //{
        //    List<string> result = null;
        //    if (!string.IsNullOrWhiteSpace(propertyName))
        //        result = Validator?.Validate(this as T, options => options.IncludeProperties(propertyName))
        //            .Errors.Select(f => f.ErrorMessage).ToList();
        //    return result;
        //}

        #endregion


        #region Dialog

        private bool? _dialogResult;
        /// <summary>
        /// [Window] DialogResult
        /// </summary>
        public bool? DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        private bool _isOpenDialog;
        /// <summary>
        /// [materialDesign:DialogHost] IsOpen
        /// </summary>
        public bool IsOpenDialog
        {
            get => _isOpenDialog;
            set => Set(ref _isOpenDialog, value);
        }

        private object _dialogContent;
        /// <summary>
        /// [materialDesign:DialogHost] DialogContent
        /// </summary>
        public object DialogContent
        {
            get => _dialogContent;
            set => Set(ref _dialogContent, value);
        }

        /// <summary>
        /// [materialDesign:DialogHost] CloseDialog delegate type
        /// </summary>
        /// <param name="close">CloseDialog</param>
        /// <param name="result">ApiResult.Succ</param>
        public delegate void CloseDialogAction(bool close, bool? result = null);
        /// <summary>
        /// [materialDesign:DialogHost] CloseDialog
        /// </summary>
        public CloseDialogAction CloseDialog;

        /// <summary>
        /// [materialDesign:DialogHost] CloseDialogAsync delegate type
        /// </summary>
        /// <param name="close">CloseDialog</param>
        /// <param name="result">ApiResult.Succ</param>
        public delegate Task CloseDialogActionAsync(bool close, bool? result = null);
        /// <summary>
        /// [materialDesign:DialogHost] CloseDialogAsync
        /// </summary>
        public CloseDialogActionAsync CloseDialogAsync;

        #endregion


        #region ProgressBar

        //private Visibility _progressVisibility = Visibility.Collapsed;
        ///// <summary>
        ///// ProgressBar：Visibility
        ///// </summary>
        //public Visibility ProgressVisibility
        //{
        //    get => _progressVisibility;
        //    set => Set(ref _progressVisibility, value);
        //}

        private bool _progressShow = false;
        /// <summary>
        /// ProgressBar：Show
        /// </summary>
        public bool ProgressShow
        {
            get => _progressShow;
            set => Set(ref _progressShow, value);
        }

        #endregion


        #region Utils

        private UtilLocator _utils;
        public UtilLocator Utils =>
            _utils ?? (_utils = new UtilLocator());

        private WpfUtilLocator _wpfUtils;
        public WpfUtilLocator WpfUtils =>
            _wpfUtils ?? (_wpfUtils = new WpfUtilLocator());

        #endregion


        #region Permission

        private string _viewName = string.Empty;
        /// <summary>
        /// 視圖名稱
        /// </summary>
        public virtual string ViewName
        {
            get => _viewName;
            set => Set(ref _viewName, value);
        }

        private SysFuncPermissionDetailDistinct _permission;
        /// <summary>
        /// 權限
        /// </summary>
        public virtual SysFuncPermissionDetailDistinct Permission
        {
            get
            {
                if (_permission == null)
                {
                    _permission = LoginViewModel.LoginUserPermission.Find(p => p.ViewName == ViewName) ?? new SysFuncPermissionDetailDistinct
                    {
                        QueryAuth = false,
                        AddAuth = false,
                        ModifyAuth = false,
                        DeleteAuth = false,
                        ExportAuth = false,
                        PrintAuth = false
                    };
                }
                return _permission;
            }
            set => Set(ref _permission, value);
        }

        #endregion


        #region Command

        /// <summary>
        /// 匯出標題
        /// </summary>
        protected virtual string ExportTitle =>
            $"{MsgParam.TitleExportRpt} {DateTime.Now.ToString("yyyyMMddHHmmss")}";

        private DelegateCommand<DataGrid> _exportCommand;
        public DelegateCommand<DataGrid> ExportCommand =>
            _exportCommand ?? (_exportCommand = new DelegateCommand<DataGrid>
            (OnExport, (param) => (bool)Permission.ExportAuth));
        /// <summary>
        /// 匯出
        /// </summary>
        protected virtual void OnExport(DataGrid dataGrid) =>
            WpfUtils.Excel.ExportExcel(dataGrid, ExportTitle);

        #endregion


        private EditMode _editMode = EditMode.NONE;
        /// <summary>
        /// 編輯模式
        /// </summary>
        public EditMode EditMode
        {
            get => _editMode;
            set => Set(ref _editMode, value);
        }

    }
}
