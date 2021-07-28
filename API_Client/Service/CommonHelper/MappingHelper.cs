using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Service.CommonHelper
{
    public class MappingHelper
    {
        public static R Mapping<R, T>(T model)
        {
            R result = Activator.CreateInstance<R>();
            foreach (PropertyInfo info in typeof(R).GetProperties())
            {
                PropertyInfo pro = typeof(T).GetProperty(info.Name);
                if (pro != null)
                    info.SetValue(result, pro.GetValue(model));
            }
            return result;
        }
    }
}
