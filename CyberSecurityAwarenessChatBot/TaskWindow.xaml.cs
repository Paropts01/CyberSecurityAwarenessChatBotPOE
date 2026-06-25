using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
    public partial class TaskWindow : Window
    {
        private TaskManager taskManager;
        private CyberTask selectedTask;

        public TaskWindow()
        {
            InitializeComponent();
            taskManager = new TaskManager();
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = taskManager.GetAllTasks();
            lstTasks.ItemsSource = tasks;
            lstTasks.Items.Refresh();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
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

        private void LstTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTask = lstTasks.SelectedItem as CyberTask;
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void BtnClearReminder_Click(object sender, RoutedEventArgs e)
        {
            dpReminder.SelectedDate = null;
        }
    }

    // =========================================================
    // VALUE CONVERTERS (Nested safely inside the namespace)
    // =========================================================

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

    public class OverdueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return Brushes.DarkSlateBlue; // Default non-overdue task color

            if (DateTime.TryParse(value.ToString(), out DateTime reminderDate))
            {
                // Returns Red if the current date has passed the deadline
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


