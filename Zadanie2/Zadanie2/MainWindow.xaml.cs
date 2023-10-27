using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Zadanie2
{
    public partial class MainWindow : Window
    {
        private TaskContext dbContext; // Контекст базы данных

        // Модель данных для задачи
        public class Task
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? DueDate { get; set; } // Nullable DateTime
            public bool IsCompleted { get; set; }
        }

        // Контекст базы данных для Entity Framework Core
        public class TaskContext : DbContext
        {
            public DbSet<Task> Tasks { get; set; } // DbSet для задач

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=task.db"); // Подключение к SQLite базе данных
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new TaskContext(); // Инициализация контекста базы данных
            dbContext.Database.EnsureCreated(); // Создание базы данных, если ее нет
            UpdateTaskList(); // Обновление списка задач при запуске приложения
        }

        // Обработчик события клика на кнопке "Добавить задачу"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                var newTask = new Task
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    DueDate = dpDueDate.SelectedDate,
                    IsCompleted = false,
                };

                dbContext.Tasks.Add(newTask);
                dbContext.SaveChanges();

                UpdateTaskList();
                ClearInputs();
            }
        }

        // Обработчик события клика на кнопке "Удалить задачу"
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedTask = (Task)lstTasks.SelectedItem;
            if (selectedTask != null)
            {
                dbContext.Tasks.Remove(selectedTask); // Удаление выбранной задачи из контекста
                dbContext.SaveChanges(); // Сохранение изменений в базе данных

                UpdateTaskList(); // Обновление списка задач
                ClearInputs(); // Очистка полей ввода
            }
        }

        // Обработчик события клика на кнопке "Обновить задачу"
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedTask = (Task)lstTasks.SelectedItem;
            if (selectedTask != null)
            {
                selectedTask.Title = txtTitle.Text;
                selectedTask.Description = txtDescription.Text;
                selectedTask.DueDate = dpDueDate.SelectedDate;

                dbContext.SaveChanges(); // Сохранение изменений в базе данных

                UpdateTaskList(); // Обновление списка задач
                ClearInputs(); // Очистка полей ввода
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var selectedTask = (Task)lstTasks.SelectedItem;
            if (selectedTask != null)
            {
                selectedTask.IsCompleted = true; // Пометить задачу как выполненную
                dbContext.SaveChanges();
                UpdateTaskList();
                ClearInputs();
            }
        }

        // Метод для обновления списка задач в интерфейсе
        private void UpdateTaskList()
        {
            lstTasks.ItemsSource = dbContext.Tasks.ToList();
        }

        // Метод для очистки полей ввода
        private void ClearInputs()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            dpDueDate.SelectedDate = null;
        }
    }
}
