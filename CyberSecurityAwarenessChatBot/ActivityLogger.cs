using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatBot
{
    public static class ActivityLogger
    {
        private static readonly List<string> _activities = new List<string>();
        private const int MaxDisplayActivities = 10;

        public static void AddActivity(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _activities.Insert(0, $"[{timestamp}] {action}");

            // Limit the stored activities to prevent memory issues
            if (_activities.Count > 100)
            {
                _activities.RemoveRange(50, _activities.Count - 50);
            }
        }

        public static List<string> GetRecentActivities(int count = MaxDisplayActivities)
        {
            return _activities.Take(count).ToList();
        }

        public static List<string> GetAllActivities()
        {
            return new List<string>(_activities);
        }

        public static void ClearActivities()
        {
            _activities.Clear();
            AddActivity("Activity log cleared");
        }

        public static string GetActivitySummary()
        {
            var activities = GetRecentActivities();
            if (activities.Count == 0)
            {
                return "No recent activities to display.";
            }

            string summary = "Here's a summary of recent actions:\n\n";
            for (int i = 0; i < activities.Count; i++)
            {
                summary += $"{i + 1}. {activities[i]}\n";
            }

            return summary.TrimEnd();
        }
    }
}
