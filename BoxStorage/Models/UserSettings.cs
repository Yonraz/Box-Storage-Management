using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models
{
    public static class UserSettings
    {
        static int _maxAmount = 5000;
        public static int MaxAmount
        {
            get { return _maxAmount; }
        }
        static int _minAmount = 10;
        public static int MinAmount
        {
            get { return _minAmount; }
        }
        static int _timeToExpire = 30;
        public static int DaysToExpire
        {
            get { return _timeToExpire; }
        }
        static int _deviationPercentage = 50;
        public static int DeviationPercentage
        {
            get { return _deviationPercentage; }
        }
        static int _expiryTimeForBoxView = 30;
        public static int ExpiryTimeForBoxView
        {
            get { return _expiryTimeForBoxView; }
        }
        public static void ResetSettings()
        {
            _maxAmount = 5000;
            _minAmount = 5;
            _timeToExpire = 30;
            _deviationPercentage = 50;
            _expiryTimeForBoxView = 30;
        }
        public static void SaveSettings(int maxAmount, int minAmount, int timeToExpire, int deviationPercentage, int expiryTimeForBoxView)
        {
            _maxAmount = maxAmount;
            _minAmount = minAmount;
            _timeToExpire = timeToExpire;
            _deviationPercentage = deviationPercentage;
            _expiryTimeForBoxView = expiryTimeForBoxView;
        }
    }
}
