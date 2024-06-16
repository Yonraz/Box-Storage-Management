using BoxStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Services
{
    internal class HelperFunctions
    {
        public static bool IsInDeviationRange(double value, double originalValue)
        {
            double range = double.Parse(UserSettings.DeviationPercentage.ToString()) / 100 + 1;
            return originalValue * range >= value && !(value < originalValue);
        }
    }
}
