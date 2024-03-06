using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceService.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AdjustDateTimeStyle(this DateTime dateTime, DateTimeStyles dateTimeStyle)
        {

            if (dateTime.Kind == DateTimeKind.Unspecified)
            {

                if (dateTimeStyle.HasFlag(System.Globalization.DateTimeStyles.AssumeUniversal))

                {

                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                }

                else if (dateTimeStyle.HasFlag(System.Globalization.DateTimeStyles.AssumeLocal))

                {

                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }

            }

            if (dateTimeStyle.HasFlag(System.Globalization.DateTimeStyles.AdjustToUniversal))

            {
                dateTime = dateTime.ToUniversalTime();
            }
            return dateTime;
        }

        public static DateTime? AdjustDateTimeStyle(this DateTime? dateTime, DateTimeStyles dateTimeStyle)
        {
            return dateTime.HasValue == false ? dateTime:  dateTime.Value.AdjustDateTimeStyle(dateTimeStyle);
        }
    }
}
