using Microsoft.AspNetCore.Http;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Helper
{
    public class Utils
    {
        public static DateTime? ConvertStringToDateTime(string? dateString)
        {
            DateTime _date = DateTime.Now;
            try
            {
                if (!string.IsNullOrEmpty(dateString))
                {
                    DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _date);
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return _date;
        }
        public static string getDayFromDate(DateTime? date)
        {
            string _day = null;
            if(date.HasValue)
            {
                _day = date.Value.Day.ToString();
            }
            return _day;
        }
        public static string getMonthFromDate(DateTime? date)
        {
            string _day = null;
            if(date.HasValue)
            {
                _day = date.Value.Month.ToString();
            }
            return _day;
        }
        public static string getYearFromDate(DateTime? date)
        {
            string _day = null;
            if(date.HasValue)
            {
                _day = date.Value.Year.ToString();
            }
            return _day;
        }
    }
}
