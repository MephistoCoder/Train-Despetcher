using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainDespetcher
{
    partial class AddPage : Form
    {
        SQLConnector sql;
        Train currTrain;

        public AddPage()
        {
        }

        public AddPage(SQLConnector sql, Train curr)
        {
            InitializeComponent();
            this.sql = sql;
            currTrain = curr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.Text == "Додавання нового рейсу")
            {
                sql.insert(textBox1.Text,
               textBox2.Text,
               maskedTextBox1.Text,
               textBox4.Text,
               Convert.ToInt32(textBox3.Text)
               );
            } else
            {
                sql.update(currTrain, textBox1.Text,
               textBox2.Text,
               maskedTextBox1.Text,
               textBox4.Text,
               Convert.ToInt32(textBox3.Text)
               );
            }
           

            this.Close();
        }

        private void AddPage_Load(object sender, EventArgs e)
        {

        }
    }
}
