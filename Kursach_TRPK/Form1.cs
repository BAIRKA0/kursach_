using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
        private void LoadComboBox2() //Заполняет возможными артикулами на странице мебели
        { 
            comboBox2.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                comboBox2.Items.Add(dataGridView1[0, i].Value);
            }
        }
        private void LoadComboBox3() //Заполняет возможными расположениями на странице мебели
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

        private void LoadComboBox4_5() //Заполняет возможными id броней 
        {
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                comboBox4.Items.Add(dataGridView2[0, i].Value);
                comboBox5.Items.Add(dataGridView2[0, i].Value);
            }
        }

        private void LoadChange() //Заполняет поля брони
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
        private void Booking_Article(string str) //Заполняет возможными артикулами на странице броней
        {                                     //DESKTOP-MANINV5
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

        private void Booking_Place(string article,string category) //Заполняет возможными расположениями на странице броней
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
        private void Booking() //Создает запись брони
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

        private void LoadData_Furniture(string str) //Заполняет таблицу мебели
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
        private void LoadData_Booking() //Заполняет таблицу броней
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

        private void Delete(string id) //Удаляет запись брони из БД
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = "DELETE FROM booking WHERE id=" + id;
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            myConnection.Close();
            LoadData_Booking();
            LoadComboBox4_5();
        }

        private void Update(string id, string category, string article, int kol, string place, string date) //Редактирует запись брони в БД
        {
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            Dictionary<String, String> dic = new Dictionary<String, String>()
            {
                { "шкаф","SELECT count FROM wardrobe WHERE article="+article },
                { "стул","SELECT count FROM chairs WHERE article="+article },
                { "полка","SELECT count FROM shelf WHERE article="+article },
                { "кресло","SELECT count FROM armchair WHERE article="+article }
            };
            string query = dic[category];
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            string str="";
            while(reader.Read())
            {
                str = reader[0].ToString();
            }
            reader.Close();
            if (kol <= Convert.ToInt32(str))
            {
                query = "UPDATE booking SET booking_category='" + category+ "',booking_article='" + article+ "',booking_count=" + kol+ ",booking_place='" + place+ "',booking_date='" + date+"' WHERE id="+id;
                command = new SqlCommand(query, myConnection);
                reader = command.ExecuteReader();
            }
            else
            {
                MessageBox.Show("Количество больше допустимого", "Ошибка");
            }
            reader.Close();
            myConnection.Close();
            LoadData_Booking();
            LoadComboBox4_5();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadData_Furniture и LoadComboBox2 при выборе категории
        {
            String str = comboBox1.Text;
            LoadData_Furniture(str);
            LoadComboBox2();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadComboBox3 при выборе артикула на странице мебели
        {
            LoadComboBox3();
        }

        private void button1_Click(object sender, EventArgs e) //Вызывает Booking при нажатии на кнопку
        {
            Booking();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadChange при выборе расположения на странице мебели
        {
            LoadChange();

        }

        private void button2_Click(object sender, EventArgs e) //Вызывает Delete при нажатии на кнопку
        {
            Delete(comboBox5.Text);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) //Вызывает Booking_Article при выборе артикула на странице брони
        {
            Booking_Article(comboBox6.Text);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e) //Вызывает Booking_Place при выборе расположения на странице брони
        {
            Booking_Place(comboBox8.Text,comboBox6.Text);
        }

        private void button3_Click(object sender, EventArgs e) //Вызывает Update при нажатии на кнопку
        {
            Update(comboBox4.Text, comboBox6.Text, comboBox8.Text, Convert.ToInt32(textBox2.Text), comboBox7.Text, dateTimePicker1.Value.ToShortDateString());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажатие было на ячейку, а не на заголовок столбца
            if (e.ColumnIndex >= 0)
            {
                comboBox2.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                comboBox3.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажатие было на ячейку, а не на заголовок столбца
            if (e.ColumnIndex >= 0)
            {
                comboBox4.SelectedItem = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                comboBox5.SelectedItem = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                comboBox6.SelectedItem = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox7.SelectedItem = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                comboBox8.SelectedItem = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                string format = "dd.MM.yyyy";
                DateTime selectedDate = DateTime.ParseExact(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString(), format, CultureInfo.InvariantCulture);
                dateTimePicker1.Value = selectedDate;
            }
        }
    }
}
