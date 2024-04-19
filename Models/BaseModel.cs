using FluentValidation;
//using FluentValidation.Results;
using GalaSoft.MvvmLight;
using Models.FluentValidators;
//using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics.Contracts;
using System.Linq;
//using System.Reflection;
//using System.Runtime.CompilerServices;

namespace Models
{
    public abstract class BaseModel<T> : ViewModelBase, IDataErrorInfo where T : class
    {
        protected new bool IsInDesignMode =>
            base.IsInDesignMode;


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

        /// <summary>
        /// [FluentValidation] Validate
        /// </summary>
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
        /// [IDataErrorInfo] 顧慮效能，不實做此功能
        /// </summary>
        public string Error { get; set; }

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


        //private UtilLocator _util;
        //protected UtilLocator Util =>
        //    _util ?? (_util = new UtilLocator());

    }
}
