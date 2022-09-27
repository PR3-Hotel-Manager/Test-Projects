using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Reflection;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace Hotel_Management__Beta_1._0_
{
    public partial class CheckIn_Form : Form
    {
        public CheckIn_Form()
        {
            InitializeComponent();
        }

        // 3. Copy AuthSecret and BasePath from Firebase Project
        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "13qazvxvMETHRB4C4CWjlCkbQeHan9ePUz2Mg4Rs",
            BasePath = "https://cfireeng-6297d-default-rtdb.firebaseio.com/"

        };
        // 4. Type this
        IFirebaseClient client;
        private void CheckIn_Form_Load(object sender, EventArgs e)
        {

            try
            {
                client = new FireSharp.FirebaseClient(fcon);
            }
            catch
            {
                MessageBox.Show("There was a problem with the interenet");
            }

        }

        bool verifyInputs()  // returns false if conditions are not met.
        {
            if (LastName_TextBox.Text == "" || Name_TextBox.Text == "")
            {
                return false;  // Check if TextBoxes are empty.
            }

            if (Name_TextBox.Text.All(Char.IsLetter) == false || LastName_TextBox.Text.All(Char.IsLetter) == false)
            {
                return false;   // Check if TextBoxes contain only letters.
            }

            else
                return true;

        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string getHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();

        }

        string transactionTime()
        {
            return DateTime.Now.ToString("mm/dd/yyyy h:mm:ss tt"); 

        }
        string retrievePaymentMethod()
        {
            if (Cash_RadioButton.Checked == true) {
                return "Cash";
            }
            else
                return "Credit/Debit";
        }
        public string pmtMethodLabel(string text)
        {
            if (text == "Credit/Debit")
                return "C/D";

            else
                return "C";
        }
        public void performCheckIn()
        {
            // Prepare Confirmation Number
            string guestDetails = Name_TextBox.Text + LastName_TextBox.Text + transactionTime();
            string temp = getHashString(guestDetails);
            string confNumber = temp.Substring(temp.Length - (temp.Length / 4));

            // Get Name, Last Name, Age, Bed, Price, Room#, Stay Length
            string Name = Name_TextBox.Text;
            string LastName = LastName_TextBox.Text;
            string Age = Age_Selector.Value.ToString();
            string Bed = BedConfig_Selector.Value.ToString();
            string Price = Price_Selector.Value.ToString();
            string Room = Room_Selector.Value.ToString();
            string Stay = StayLength_Selector.Value.ToString();

            customer c = new customer()
            {
                Name = Name_TextBox.Text,
                Lastname = LastName_TextBox.Text,
                Age = Age_Selector.Value.ToString(),
                Bed = BedConfig_Selector.Value.ToString(),
                Price = Price_Selector.Value.ToString(),
                Room = Room_Selector.Value.ToString(),
                Stay = StayLength_Selector.Value.ToString()
            };   
        

            // Get Payment Method
            string pmtMethod;
            pmtMethod = retrievePaymentMethod();

            // Add fields to Database(to be implemented).


            CheckInConfirmation_Form form = new();  // pass confirmation number to the label in #CheckInConfirmation_Form 
            form.changeLabel(confNumber);
            this.Hide();
            form.ShowDialog(); // Display #CheckInConfirmation_Form
            this.Close();

            // Save to Log File
            
          
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            File.AppendAllText(filePath, DateTime.Now.ToString("HH:mm:ss") + "|Chk-in|  " + Name.PadRight(15,' ') + " " + LastName.PadRight(20,' ') + " " + Age.PadLeft(2) + "  #" + Room.PadRight(2) + " - " + pmtMethodLabel(pmtMethod) + "\n");

            var setter = client.Set("Room Number/" + Room_Selector.Value.ToString(), c);
            MessageBox.Show("Data inserted successfuly.");

        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close(); // Cancel & Exit CheckIn Form
        }
   
        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (verifyInputs() == true) // If input fields are verified, perform check-in.
            {
                performCheckIn();            

            }

            else   // Else; Display Error Message.
            {
                MessageBox.Show("Input fields are missing or contain numbers. Please try again.", " Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
