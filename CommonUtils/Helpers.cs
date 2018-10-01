using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    public class Helpers
    {
        public static readonly Regex CGroupPathPattern = new Regex("cpu.+/([^/]*)$", RegexOptions.Multiline | RegexOptions.CultureInvariant);
        public const string CGroupPathPatternString = "cpu.+/([^/]*)$";
        internal static string ParseContainerId(string pathToCGroup = "/proc/self/cgroup")
        {
            if (!File.Exists(pathToCGroup))
                throw new FileNotFoundException("File contains container id does not exist.", pathToCGroup);

            string content = File.ReadAllText(pathToCGroup);
            if (!string.IsNullOrEmpty(content))
            {
                MatchCollection matchCollection = CGroupPathPattern.Matches(content);
                if (matchCollection.Count >= 1 && matchCollection[0].Groups.Count >= 2)
                    return matchCollection[0].Groups[1].Value;
            }
            // throw new InvalidCastException(StringUtils.Invariant(FormattableStringFactory.Create("Can't figure out container id. Input: {0}. Pattern: {1}", (object)content, (object)"cpu.+/([^/]*)$")));
            return string.Empty;
        }

        
    }
}
