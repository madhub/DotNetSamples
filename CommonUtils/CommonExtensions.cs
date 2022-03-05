using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class CommonExtensions
    {
        // Retry User supplied  function
        public static void Retry( Action retryAction,int retryCount)
        {
            while (true)
            {
                try
                {
                    retryAction();
                }
                catch when (retryCount-- > 0) { }
            }
        }
        // https://www.danylkoweb.com/Blog/10-extremely-useful-net-extension-methods-8J
        public static string ToFileSize(this long size)
        {
            if (size < 1024) { return (size).ToString("F0") + " bytes"; }
            if (size < Math.Pow(1024, 2)) { return (size / 1024).ToString("F0") + "KB"; }
            if (size < Math.Pow(1024, 3)) { return (size / Math.Pow(1024, 2)).ToString("F0") + "MB"; }
            if (size < Math.Pow(1024, 4)) { return (size / Math.Pow(1024, 3)).ToString("F0") + "GB"; }
            if (size < Math.Pow(1024, 5)) { return (size / Math.Pow(1024, 4)).ToString("F0") + "TB"; }
            if (size < Math.Pow(1024, 6)) { return (size / Math.Pow(1024, 5)).ToString("F0") + "PB"; }
            return (size / Math.Pow(1024, 6)).ToString("F0") + "EB";
        }
        public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                age--;
            return age;
        }
        /// <summary>
        /// Usage
        /// public Test(string input1)
        /// {
        ///     input1.ThrowIfArgumentIsNull(nameof(input1));
        /// }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        public static void ThrowIfArgumentIsNull<T>(this T obj, string parameterName) where T : class
        {
            if (obj == null) throw new ArgumentNullException(parameterName + " not allowed to be null");
        }
        public static void ThrowIfNull<T>(this T item, string name) where T : class
        { if (item == null) throw new ArgumentNullException(name); }

        public static void ThrowIfNull<T>(this T item) where T : class
        { if (item == null) throw new ArgumentNullException(); }

        public static void ThrowIfNull<T>(this T? item, string name) where T : struct
        { if (item == null) throw new ArgumentNullException(name); }

        public static void ThrowIfNull<T>(this T? item) where T : struct
        { if (item == null) throw new ArgumentNullException(); }

        // MyList.EmptyIfNull().Where(....)
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> pSeq)
        {
            return pSeq ?? Enumerable.Empty<T>();
        }
        public static bool IsNullOrEmpty(this ICollection obj)
        {
            return (obj == null || obj.Count == 0);
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return (collection == null || !collection.Any() );
        }


    }

    public static class FrameworkExtensions
    {
        // a map function
        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> mapFunction)
        {
            foreach (var item in @enum) mapFunction(item);
        }
    }
}
