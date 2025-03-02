namespace ochered2
{
    public partial class Form1 : Form
    {
        private Queue<Client> cashier1 = new Queue<Client>();
        private Queue<Client> cashier2 = new Queue<Client>();
        private Queue<Client> cashier3 = new Queue<Client>();
        private int totalClients = 0;
        private int clientsLeft = 0;
        private List<int> processingTimes = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cashier1.Clear();
            cashier2.Clear();
            cashier3.Clear();
            totalClients = 0;
            clientsLeft = 0;
            processingTimes.Clear();
            UpdateUI();
            textBoxResults.Text = "";
        }

        private void UpdateUI()
        {
            lblCashier1.Text = $"Касса 1: {cashier1.Count}";
            lblCashier2.Text = $"Касса 2: {cashier2.Count}";
            lblCashier3.Text = $"Касса 3: {cashier3.Count}";
            lblStats.Text = $"Всего клиентов: {totalClients} | Ушли: {clientsLeft}";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}