using System.Globalization;
using System.IO;

namespace RestAsReward.utils;

public class SimpleLeftTimeVariableManager {
    public static void Save(TimeSpan timeSpan) {
        File.WriteAllText("LeftTime.txt",timeSpan.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
    }

    public static TimeSpan Load() {
        try {
            var leftTime = File.ReadAllText("LeftTime.txt");
            return TimeSpan.FromMilliseconds(double.Parse(leftTime,CultureInfo.InvariantCulture));
        } catch (Exception e) {
            return TimeSpan.Zero;
        }
    }
    
}