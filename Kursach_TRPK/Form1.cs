using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach_TRPK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData_Booking();
            comboBox1.SelectedIndex =0;
            String str = comboBox1.Text;
            LoadData_Furniture(str);
            LoadComboBox2();
            LoadComboBox4_5();
        }
        private void LoadComboBox2()
        { 
            comboBox2.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                comboBox2.Items.Add(dataGridView1[0, i].Value);
            }
        }
        private void LoadComboBox3()
        {
            comboBox3.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if(comboBox2.Text == dataGridView1[0, i].Value.ToString())
                {
                    comboBox3.Items.Add(dataGridView1[7, i].Value);
                }
            }
        }

        private void LoadComboBox4_5()
        {
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                comboBox4.Items.Add(dataGridView2[0, i].Value);
                comboBox5.Items.Add(dataGridView2[0, i].Value);
            }
        }

        private void LoadChange()
        {
            for(int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (comboBox4.Text == dataGridView2[0,i].Value.ToString())
                {
                    comboBox6.SelectedItem = dataGridView2[1, i].Value.ToString();
                    comboBox8.SelectedItem = dataGridView2[2, i].Value.ToString();
                    textBox2.Text = dataGridView2[3, i].Value.ToString();
                    comboBox7.SelectedItem = dataGridView2[4, i].Value.ToString();
                    //dateTimePicker1.Value = dataGridView2[5, i].Value;
                }
            }
        }
        private void Booking_Article(string str)
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            Dictionary<String, String> dic = new Dictionary<String, String>()
            {
                { "шкаф","SELECT article FROM wardrobe" },
                { "стул","SELECT article FROM chairs" },
                { "полка","SELECT article FROM shelf" },
                { "кресло","SELECT article FROM armchair" }
            };
            string query = dic[str];
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> data = new List<string>();
            while (reader.Read())
            {
                data.Add(reader[0].ToString());
            }
            reader.Close();
            myConnection.Close();
            comboBox8.Items.Clear();
            foreach (string s in data)
                comboBox8.Items.Add(s);
        }

        private void Booking_Place(string article,string category)
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            Dictionary<String, String> dic = new Dictionary<String, String>()
            {
                { "шкаф","wardrobe" },
                { "стул","chairs" },
                { "полка","shelf" },
                { "кресло","armchair" }
            };
            string query = "SELECT place FROM " + dic[category] +" WHERE article='"+article+"'";
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> data = new List<string>();
            while (reader.Read())
            {
                data.Add(reader[0].ToString());
            }
            reader.Close();
            myConnection.Close();
            comboBox7.Items.Clear();
            foreach (string s in data)
                comboBox7.Items.Add(s);
        }
        private void Booking()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++) {
                if (comboBox2.Text == dataGridView1[0,i].Value.ToString() 
                    && comboBox3.Text == dataGridView1[7, i].Value.ToString())
                {
                    if(Convert.ToInt32(textBox1.Text) <= Convert.ToInt32(dataGridView1[8, i].Value))
                    {
                        string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
                        SqlConnection myConnection = new SqlConnection(connectString);
                        myConnection.Open();
                        string query = "INSERT INTO booking (booking_category,booking_article,booking_count,booking_place,booking_date) " +
                            "VALUES ('" + comboBox1.Text + "', '" + comboBox2.Text + "', '" + textBox1.Text + "', '" + comboBox3.Text + "', '" + dateTimePicker2.Value.ToShortDateString() + "');";
                        SqlCommand command = new SqlCommand(query, myConnection);
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();
                        myConnection.Close();
                        LoadData_Booking();
                        LoadComboBox4_5();
                    }
                    else
                    {
                        MessageBox.Show("Количество больше допустимого", "Ошибка");
                    }
                }
            }
        }

        private void LoadData_Furniture(string str)
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            Dictionary<String, String> dic = new Dictionary<String, String>()
            {
                { "шкаф","SELECT * FROM wardrobe" },
                { "стул","SELECT * FROM chairs" },
                { "полка","SELECT * FROM shelf" },
                { "кресло","SELECT * FROM armchair" }
            };
            string query = dic[str];
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[9]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
                data[data.Count - 1][8] = reader[8].ToString();
            }
            reader.Close();
            myConnection.Close();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }
        private void LoadData_Booking()
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = "SELECT * FROM booking";
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();            
            while (reader.Read())
            {
                data.Add(new string[6]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
            }
            reader.Close();
            myConnection.Close();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            foreach (string[] s in data)
                dataGridView2.Rows.Add(s);
        }

        private void Delete(string id)
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = "DELETE FROM booking WHERE id="+id;
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            myConnection.Close();
            LoadData_Booking();
            LoadComboBox4_5();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String str = comboBox1.Text;
            LoadData_Furniture(str);
            LoadComboBox2();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComboBox3();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Booking();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChange();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delete(comboBox5.Text);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Booking_Article(comboBox6.Text);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            Booking_Place(comboBox8.Text,comboBox6.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
