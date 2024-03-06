using System;

namespace TraceService.Application.Helpers
{
    public static class EnumHelper<T>
    {
        public static T ConvertToEnum(dynamic value)
        {
            T result = default;
            int tempType = 0;

            //see Note below
            if (value != null &&
                int.TryParse(value.ToString(), out tempType) &&
                Enum.IsDefined(typeof(T), tempType))
            {
                result = (T)Enum.ToObject(typeof(T), tempType);
            }
            return result;
        }
    }
}
