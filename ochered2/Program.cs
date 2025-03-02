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
        private int clientsLeft = 0; // ���������� ������� ��������
        private List<int> processingTimes = new List<int>();
        private List<int> clientStayTimes = new List<int>(); // ����� ���������� ��������
        private Random random = new Random();
        private System.Timers.Timer timer;
        private int time = 0;

        // �������� ����������
        private Label lblCashier1, lblCashier2, lblTime, lblStats;
        private Button btnStart, btnStop;
        private ListBox listBoxCashier1, listBoxCashier2;

        public MainForm()
        {
            InitializeCustomComponents();
            InitializeTimer();
            this.Size = new Size(500, 400); // ��������� ������ �����
            this.Text = "���������� �������";
        }

        private void InitializeCustomComponents()
        {
            // �����
            lblTime = new Label { Text = "����: 0", Left = 200, Top = 20 };
            lblCashier1 = new Label { Text = "����� 1: 0", Left = 50, Top = 60 };
            lblCashier2 = new Label { Text = "����� 2: 0", Left = 250, Top = 60 };
            lblStats = new Label { Text = "����������:", Left = 50, Top = 300, Width = 400 };

            // ListBox ��� ����
            listBoxCashier1 = new ListBox { Left = 50, Top = 80, Width = 150, Height = 100 };
            listBoxCashier2 = new ListBox { Left = 250, Top = 80, Width = 150, Height = 100 };

            // ������ (����������� ���� ListBox)
            btnStart = new Button { Text = "�����", Left = 50, Top = 200, Width = 150 };
            btnStop = new Button { Text = "����", Left = 250, Top = 200, Width = 150 };

            // �������� �������
            btnStart.Click += (sender, e) => StartSimulation();
            btnStop.Click += (sender, e) => StopSimulation();

            // ���������� ��������� �� �����
            this.Controls.AddRange(new Control[] {
                lblTime, lblCashier1, lblCashier2,
                lblStats, btnStart, btnStop,
                listBoxCashier1, listBoxCashier2
            });
        }

        private void InitializeTimer()
        {
            timer = new System.Timers.Timer(1000); // ������ � ���������� 1 �������
            timer.Elapsed += (sender, e) => ProcessStep();
        }

        private void ProcessStep()
        {
            time++; // ����������� ���������� �����

            // �������������� ��������� ���� �� ������ ������� ����� ������ ���
            AddClient();

            // ��������� �������� � ������
            ProcessCashier(ref cashier1);
            ProcessCashier(ref cashier2);

            // ���������� ����������
            this.Invoke((MethodInvoker)delegate
            {
                lblTime.Text = $"����: {time}";
                lblCashier1.Text = $"����� 1: {cashier1.Count}";
                lblCashier2.Text = $"����� 2: {cashier2.Count}";
                lblStats.Text = $"����� ��������: {totalClients} | ����: {clientsLeft}";

                // ��������� ListBox ��� ������ �����
                UpdateListBox(listBoxCashier1, cashier1);
                UpdateListBox(listBoxCashier2, cashier2);
            });
        }

        private void AddClient()
        {
            if (totalClients >= 100) return; // ����� ��������

            totalClients++;

            // ��������� ��� ��������� (1, 2 ��� 3)
            int docType = random.Next(1, 4);
            int processingTime = docType switch
            {
                1 => random.Next(2, 4),
                2 => random.Next(2, 4),
                3 => 4,
                _ => 1
            };

            Client client = new Client(totalClients, docType, processingTime) // �������� ���������� �����
            {
                ArrivalTime = time // ��������� ����� ��������
            };

            bool isAdded = false;

            // �������� ����� � ���������� ��������
            var targetQueue = cashier1.Count <= cashier2.Count ? cashier1 : cashier2;
            if (targetQueue.Count < maxQueueLength)
            {
                targetQueue.Enqueue(client);
                isAdded = true;
            }

            if (!isAdded) clientsLeft++; // ����������� ������� 
        }

        private void ProcessCashier(ref Queue<Client> queue)
        {
            if (queue.Count > 0)
            {
                Client client = queue.Peek(); // ������� ������� ������� � �������
                client.ProcessingTime--; // ��������� ���������� ����� ���������

                if (client.ProcessingTime <= 0)
                {
                    queue.Dequeue(); // ������� �������, ���� ��������� ���������
                    processingTimes.Add(client.ProcessingTime); // ��������� ����� ���������
                    clientStayTimes.Add(time - client.ArrivalTime); // ��������� ����� ����������
                }
            }
        }

        private void StartSimulation()
        {
            ClearAll(); // ������� ��� ������ ����� �������
            timer.Start();
        }

        private void StopSimulation()
        {
            timer.Stop();

            // ��� ������� �� ���� ������
            clientsLeft += cashier1.Count + cashier2.Count;
            cashier1.Clear();
            cashier2.Clear();

            // ������������ ������� ����� ����������
            if (clientStayTimes.Count > 0)
            {
                double averageStayTime = clientStayTimes.Average();
                MessageBox.Show($"������� ����� ���������� �������� � �����: {averageStayTime:F1} �����.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("��� ������ ��� ������� �������� �������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateUI();
        }

        private void ClearAll()
        {
            cashier1.Clear();
            cashier2.Clear();
            totalClients = 0;
            clientsLeft = 0; // ���������� ������� ������� ��������
            processingTimes.Clear();
            clientStayTimes.Clear(); // ������� ������ ������� ����������
            time = 0; // ���������� ������� �����
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblTime.Text = $"����: {time}";
            lblCashier1.Text = $"����� 1: {cashier1.Count}";
            lblCashier2.Text = $"����� 2: {cashier2.Count}";
            lblStats.Text = $"����� ��������: {totalClients} | ����: {clientsLeft}";

            // ��������� ListBox ��� ������ �����
            UpdateListBox(listBoxCashier1, cashier1);
            UpdateListBox(listBoxCashier2, cashier2);
        }

        private void UpdateListBox(ListBox listBox, Queue<Client> queue)
        {
            listBox.Items.Clear();
            foreach (var client in queue)
            {
                listBox.Items.Add($"�{client.Id}: {client.DocumentType}�-{client.ProcessingTime}�");
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
        public int Id { get; } // ���������� ����� �������
        public int DocumentType { get; }
        public int ProcessingTime { get; set; } // ���������� ����� ���������
        public int ArrivalTime { get; set; }

        public Client(int id, int documentType, int processingTime)
        {
            Id = id;
            DocumentType = documentType;
            ProcessingTime = processingTime;
        }
    }
}