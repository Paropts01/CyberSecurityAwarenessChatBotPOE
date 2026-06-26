using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
 
    // Window for managing tasks (add, complete, delete, view).
  
    public partial class TaskWindow : Window
    {
        private TaskManager taskManager;
        private CyberTask selectedTask;
        private string _userName;

        //Initialises the task window with the given user name.
        public TaskWindow(string userName)
        {
            InitializeComponent();
            _userName = userName;
            taskManager = new TaskManager();
            LoadTasks();
        }

        //Fetches all tasks and updates the ListBox.
        private void LoadTasks()
        {
            var tasks = taskManager.GetAllTasks();
            lstTasks.ItemsSource = tasks;
            lstTasks.Items.Refresh();
        }

        // ----- Add Task -----
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string description = txtTaskDescription.Text.Trim();
            string reminderDate = dpReminder.SelectedDate?.ToString("yyyy-MM-dd");

            int id = taskManager.AddTask(title, description, reminderDate);
            if (id > 0)
            {
                ActivityLogger.AddActivity($"Task added: '{title}'" +
                    (reminderDate != null ? $" (Reminder: {reminderDate})" : ""));

                txtTaskTitle.Clear();
                txtTaskDescription.Clear();
                dpReminder.SelectedDate = null;
                LoadTasks();

                MessageBox.Show("Task added successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add task.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Selection changed -----
        private void LstTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTask = lstTasks.SelectedItem as CyberTask;
        }

        // ----- Mark as Complete -----
        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Please select a task first.", "Selection Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedTask.IsCompleted)
            {
                MessageBox.Show("This task is already completed.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Mark '{selectedTask.Title}' as complete?",
                "Complete Task", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (taskManager.MarkTaskAsComplete(selectedTask.Id))
                {
                    ActivityLogger.AddActivity($"Task completed: '{selectedTask.Title}'");
                    LoadTasks();
                    MessageBox.Show("Task marked as complete!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update task.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ----- Delete Task -----
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Please select a task first.", "Selection Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{selectedTask.Title}'?",
                "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (taskManager.DeleteTask(selectedTask.Id))
                {
                    ActivityLogger.AddActivity($"Task deleted: '{selectedTask.Title}'");
                    selectedTask = null;
                    LoadTasks();
                    MessageBox.Show("Task deleted successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete task.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ----- Refresh -----
        private void btnRefresh_Click(object sender, RoutedEventArgs e) => LoadTasks();

        // ----- Clear Date -----
        private void btnClearReminder_Click(object sender, RoutedEventArgs e) => dpReminder.SelectedDate = null;

        // ----- Back / Close -----
        private void btnBack_Click(object sender, RoutedEventArgs e) => this.Close();
    }

    // =========================================================
    // VALUE CONVERTERS (used in XAML bindings)
    // =========================================================

    //Converts bool to "✓" or "○" for task status.
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isCompleted = (bool)value;
            return isCompleted ? "✓" : "○";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Converts null to Visibility.Collapsed, non‑null to Visible.
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Converts reminder date to a brush: Red if overdue, otherwise DarkSlateBlue.
    public class OverdueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return Brushes.DarkSlateBlue;

            if (DateTime.TryParse(value.ToString(), out DateTime reminderDate))
            {
                return reminderDate < DateTime.Today ? Brushes.Red : Brushes.DarkSlateBlue;
            }
            return Brushes.DarkSlateBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


