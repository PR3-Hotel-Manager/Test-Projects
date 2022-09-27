using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace Hotel_Management__Beta_1._0_
{
    public partial class LookUp_Form : Form
    {
        public LookUp_Form()
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
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Name_TextBox.Text = "";
            LastName_TextBox.Text = "";
        }
        private void LookUp_Form_Load(object sender, EventArgs e)
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = client.Get("Room Number/" + RoomNumber_TextBox.Text);
            customer c = result.ResultAs<customer>();
            Name_TextBox.Text = c.Name;
            LastName_TextBox.Text = c.Lastname;
            MessageBox.Show("Data retrieved successfuly.");
        }


    }
}
