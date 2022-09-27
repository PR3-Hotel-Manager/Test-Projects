using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace Hotel_Management__Beta_1._0_
{
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
           // Use .ShowDialog() method to only keep the new Form active and prevent interference.
        }
        async private void checkConnection() { 
        
            Ping pingSignal = new Ping();
            string hostName = "www.google.com";

            byte[] buffer = new byte[32];
            int timeOut = 4000;

            connectionStatus_Label.Text = "Connecting to Database...";

            try
            {
                PingOptions options = new PingOptions();
                PingReply reply = pingSignal.Send(hostName, timeOut, buffer, options);

                if (reply.Status == IPStatus.Success) 
                {
                    await Task.Delay(TimeSpan.FromSeconds(1)); // Wait 1 second before displaying result
                    connectionStatus_Label.Text = "Connected to Database."; // Update Label text
                    RetryConnecting_Button.Enabled = false;
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    databaseConnectionStatus_Panel.Visible = false; 
                }
  
            }

            catch (Exception)
            {
                await Task.Delay(TimeSpan.FromSeconds(1)); // Wait 1 second before displaying result
                connectionStatus_Label.Text = "Unable to Connect to Database."; // Update Label text
                RetryConnecting_Button.Enabled = true;
                databaseConnectionStatus_Panel.Visible = true;

            }
         
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            Title_Label.FlatStyle = FlatStyle.Standard;
            Title_Label.Parent = Title_Holder;
            Title_Label.BackColor = Color.Transparent;

            checkConnection();

        }

        private void CheckIn_Button_Click(object sender, EventArgs e)
        {
            CheckIn_Form form_CheckIn = new CheckIn_Form();
            form_CheckIn.ShowDialog();
        }

        private void CheckOut_Button_Click(object sender, EventArgs e)
        {
            CheckOut_Form form_CheckOut = new CheckOut_Form();
            form_CheckOut.ShowDialog();
        }

        private void Capacity_Button_Click(object sender, EventArgs e)
        {
            Rooms_Form form_Capacity = new Rooms_Form();
            form_Capacity.ShowDialog();
        }

        private void LookUp_Button_Click(object sender, EventArgs e)
        {
            LookUp_Form form_LookUp = new LookUp_Form();
            form_LookUp.ShowDialog();
        }

        private void Report_Button_Click(object sender, EventArgs e)
        {
            Report_Form form_Report = new Report_Form();
            form_Report.ShowDialog();
        }

        private void Quit_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RetryConnecting_Button_Click(object sender, EventArgs e)
        {
            checkConnection();
        }
    }
}