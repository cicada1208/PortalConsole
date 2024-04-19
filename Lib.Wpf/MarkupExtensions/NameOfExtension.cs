using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Lib.Wpf.MarkupExtensions
{
    [ContentProperty(nameof(Member))]
    public class NameOfExtension : MarkupExtension
    {
        //usage
        ////<viewmodels:SysAppViewModel ViewName="{extensions:NameOf Type={x:Type local:SysAppPage}}"/>
        //<viewmodels:SysAppViewModel ViewName="{extensions:NameOf Type={x:Type local:SysAppPage}, Member=Title}"/>

        public Type Type { get; set; }
        public string Member { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            if (Type == null)
                throw new ArgumentException("Syntax for x:NameOf is Type={x:Type [className]}");

            if (!string.IsNullOrEmpty(Member))
            {
                if (Member.Contains("."))
                    throw new ArgumentException("Syntax for x:NameOf is Type={x:Type [className]} Member=[propertyName]");

                var pinfo = Type.GetRuntimeProperties().FirstOrDefault(pi => pi.Name == Member);
                var finfo = Type.GetRuntimeFields().FirstOrDefault(fi => fi.Name == Member);

                if (pinfo == null && finfo == null)
                    throw new ArgumentException($"No property or field found for {Member} in {Type}");

                return Member;
            }
            else
                return Type.Name;
        }
    }
}
