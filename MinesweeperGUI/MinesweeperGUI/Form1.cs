using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

   

        private void button1_Click(object sender, EventArgs e)
        {
            string choice = "";
            if (radioButton1.Checked) choice = "easy";
            if (radioButton2.Checked) choice = "medium";
            if (radioButton3.Checked) choice = "hard";
            Form2 form = new Form2(choice);
            this.Hide();
            form.Show();
            
        }



      
    }
}
