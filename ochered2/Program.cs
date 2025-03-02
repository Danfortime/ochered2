using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace ochered2
{
    public partial class MainForm : Form
    {
        private Queue<Client> cashier1 = new Queue<Client>();
        private Queue<Client> cashier2 = new Queue<Client>();
        private int maxQueueLength = 5;
        private int totalClients = 0;
        private int clientsLeft = 0; // Количество ушедших клиентов
        private List<int> processingTimes = new List<int>();
        private List<int> clientStayTimes = new List<int>(); // Время нахождения клиентов
        private Random random = new Random();
        private System.Timers.Timer timer;
        private int time = 0;

        // Элементы интерфейса
        private Label lblCashier1, lblCashier2, lblTime, lblStats;
        private Button btnStart, btnStop;
        private ListBox listBoxCashier1, listBoxCashier2;

        public MainForm()
        {
            InitializeCustomComponents();
            InitializeTimer();
            this.Size = new Size(500, 400); // Уменьшили ширину формы
            this.Text = "Банковские очереди";
        }

        private void InitializeCustomComponents()
        {
            // Метки
            lblTime = new Label { Text = "Тики: 0", Left = 200, Top = 20 };
            lblCashier1 = new Label { Text = "Касса 1: 0", Left = 50, Top = 60 };
            lblCashier2 = new Label { Text = "Касса 2: 0", Left = 250, Top = 60 };
            lblStats = new Label { Text = "Статистика:", Left = 50, Top = 300, Width = 400 };

            // ListBox для касс
            listBoxCashier1 = new ListBox { Left = 50, Top = 80, Width = 150, Height = 100 };
            listBoxCashier2 = new ListBox { Left = 250, Top = 80, Width = 150, Height = 100 };

            // Кнопки (расположены ниже ListBox)
            btnStart = new Button { Text = "Старт", Left = 50, Top = 200, Width = 150 };
            btnStop = new Button { Text = "Стоп", Left = 250, Top = 200, Width = 150 };

            // Привязка событий
            btnStart.Click += (sender, e) => StartSimulation();
            btnStop.Click += (sender, e) => StopSimulation();

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] {
                lblTime, lblCashier1, lblCashier2,
                lblStats, btnStart, btnStop,
                listBoxCashier1, listBoxCashier2
            });
        }

        private void InitializeTimer()
        {
            timer = new System.Timers.Timer(1000); // Таймер с интервалом 1 секунда
            timer.Elapsed += (sender, e) => ProcessStep();
        }

        private void ProcessStep()
        {
            time++; // Увеличиваем количество тиков

            // Гарантированно добавляем хотя бы одного клиента через каждый тик
            AddClient();

            // Обработка клиентов в кассах
            ProcessCashier(ref cashier1);
            ProcessCashier(ref cashier2);

            // Обновление интерфейса
            this.Invoke((MethodInvoker)delegate
            {
                lblTime.Text = $"Тики: {time}";
                lblCashier1.Text = $"Касса 1: {cashier1.Count}";
                lblCashier2.Text = $"Касса 2: {cashier2.Count}";
                lblStats.Text = $"Всего клиентов: {totalClients} | Ушли: {clientsLeft}";

                // Обновляем ListBox для каждой кассы
                UpdateListBox(listBoxCashier1, cashier1);
                UpdateListBox(listBoxCashier2, cashier2);
            });
        }

        private void AddClient()
        {
            if (totalClients >= 100) return; // Лимит клиентов

            totalClients++;

            // Случайный тип документа (1, 2 или 3)
            int docType = random.Next(1, 4);
            int processingTime = docType switch
            {
                1 => random.Next(2, 4),
                2 => random.Next(2, 4),
                3 => 4,
                _ => 1
            };

            Client client = new Client(totalClients, docType, processingTime) // Передаем порядковый номер
            {
                ArrivalTime = time // Сохраняем время прибытия
            };

            bool isAdded = false;

            // Выбираем кассу с наименьшей очередью
            var targetQueue = cashier1.Count <= cashier2.Count ? cashier1 : cashier2;
            if (targetQueue.Count < maxQueueLength)
            {
                targetQueue.Enqueue(client);
                isAdded = true;
            }

            if (!isAdded) clientsLeft++; // Увеличиваем счётчик 
        }

        private void ProcessCashier(ref Queue<Client> queue)
        {
            if (queue.Count > 0)
            {
                Client client = queue.Peek(); // Смотрим первого клиента в очереди
                client.ProcessingTime--; // Уменьшаем оставшееся время обработки

                if (client.ProcessingTime <= 0)
                {
                    queue.Dequeue(); // Удаляем клиента, если обработка завершена
                    processingTimes.Add(client.ProcessingTime); // Сохраняем время обработки
                    clientStayTimes.Add(time - client.ArrivalTime); // Сохраняем время нахождения
                }
            }
        }

        private void StartSimulation()
        {
            ClearAll(); // Очищаем все данные перед стартом
            timer.Start();
        }

        private void StopSimulation()
        {
            timer.Stop();

            // Все клиенты из касс уходят
            clientsLeft += cashier1.Count + cashier2.Count;
            cashier1.Clear();
            cashier2.Clear();

            // Рассчитываем среднее время нахождения
            if (clientStayTimes.Count > 0)
            {
                double averageStayTime = clientStayTimes.Average();
                MessageBox.Show($"Среднее время нахождения клиентов в банке: {averageStayTime:F1} тиков.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет данных для расчёта среднего времени.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateUI();
        }

        private void ClearAll()
        {
            cashier1.Clear();
            cashier2.Clear();
            totalClients = 0;
            clientsLeft = 0; // Сбрасываем счётчик ушедших клиентов
            processingTimes.Clear();
            clientStayTimes.Clear(); // Очищаем список времени нахождения
            time = 0; // Сбрасываем счётчик тиков
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblTime.Text = $"Тики: {time}";
            lblCashier1.Text = $"Касса 1: {cashier1.Count}";
            lblCashier2.Text = $"Касса 2: {cashier2.Count}";
            lblStats.Text = $"Всего клиентов: {totalClients} | Ушли: {clientsLeft}";

            // Обновляем ListBox для каждой кассы
            UpdateListBox(listBoxCashier1, cashier1);
            UpdateListBox(listBoxCashier2, cashier2);
        }

        private void UpdateListBox(ListBox listBox, Queue<Client> queue)
        {
            listBox.Items.Clear();
            foreach (var client in queue)
            {
                listBox.Items.Add($"№{client.Id}: {client.DocumentType}Т-{client.ProcessingTime}Д");
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new MainForm());
        }
    }

    public class Client
    {
        public int Id { get; } // Порядковый номер клиента
        public int DocumentType { get; }
        public int ProcessingTime { get; set; } // Оставшееся время обработки
        public int ArrivalTime { get; set; }

        public Client(int id, int documentType, int processingTime)
        {
            Id = id;
            DocumentType = documentType;
            ProcessingTime = processingTime;
        }
    }
}