using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class StatsPage : ContentPage
    {
        private DatabaseHelper _db;
        private int _currentUserId;

        public StatsPage()
        {
            InitializeComponent();
            _db = new DatabaseHelper();
            _currentUserId = Preferences.Get("UserId", -1);

            if (_currentUserId == -1)
            {
                DisplayAlert("Error", "User not found! Please log in again.", "OK");
                Application.Current.MainPage = new Login();
            }
            else
            {
                LoadSummary();
                LoadPieChart();
                LoadLineChart();
            }
        }

        private void LoadSummary()
        {
            var allTasks = _db.GetTasksByUser(_currentUserId);

            int total = allTasks.Count;
            int completed = allTasks.Count(t => t.IsDone);
            int incomplete = total - completed;

        }

        private void LoadPieChart()
        {
            var allTasks = _db.GetTasksByUser(_currentUserId);

            var grouped = allTasks
                          .GroupBy(t => t.Category)
                          .Select(g => new { Category = g.Key, Count = g.Count() })
                          .ToList();

            var entries = grouped.Select(g => new ChartEntry(g.Count)
            {
                Label = g.Category,
                ValueLabel = g.Count.ToString(),
                Color = SKColor.Parse("#" + Guid.NewGuid().ToString("N").Substring(0, 6)) // random-ish color
            }).ToList();

            pieChartView.Chart = entries.Count > 0
                ? new PieChart { Entries = entries }
                : null;
        }

        private void LoadLineChart()
        {
            var allTasks = _db.GetTasksByUser(_currentUserId);
            var today = DateTime.Today;

            var past7Days = Enumerable.Range(0, 7)
                                      .Select(i => today.AddDays(-6 + i)) // last 7 days, oldest to newest
                                      .ToList();

            var completedEntries = new List<ChartEntry>();
            var incompleteEntries = new List<ChartEntry>();

            foreach (var day in past7Days)
            {
                int completed = allTasks.Count(t => t.TaskDate.Date == day && t.IsDone);
                int incomplete = allTasks.Count(t => t.TaskDate.Date == day && !t.IsDone);

                var dayLabel = day.ToString("dd/MM"); // e.g., 04/04

                completedEntries.Add(new ChartEntry(completed)
                {
                    Label = dayLabel,
                    ValueLabel = completed.ToString(),
                    Color = SKColors.Green
                });

                incompleteEntries.Add(new ChartEntry(incomplete)
                {
                    Label = dayLabel,
                    ValueLabel = incomplete.ToString(),
                    Color = SKColors.Red
                });
            }

            // Merge completed + incomplete in one dataset using multiple LineChartSeries
            lineChartView.Chart = new LineChart
            {
                LineMode = LineMode.Straight,
                LineSize = 4,
                PointMode = PointMode.Circle,
                PointSize = 8,
                Entries = completedEntries, // only one set at a time, so we use multi-series instead below if needed
            };

           
        }

    }
}
