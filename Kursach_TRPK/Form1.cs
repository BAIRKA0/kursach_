using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            comboBox1.SelectedIndex = 0;
            String str = comboBox1.Text;
            LoadData_Furniture(str);
            LoadComboBox2();
            LoadComboBox4_5();
        }
        public string con2 = "DESKTOP-MANINV5";
        public string con1 = "BAIRKA";
        public void LoadComboBox2() //Заполняет возможными артикулами на странице мебели
        { 
            comboBox2.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                comboBox2.Items.Add(dataGridView1[0, i].Value);
            }
        }
        public void LoadComboBox3() //Заполняет возможными расположениями на странице мебели
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

        public void LoadComboBox4_5() //Заполняет возможными id броней 
        {
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                comboBox4.Items.Add(dataGridView2[0, i].Value);
                comboBox5.Items.Add(dataGridView2[0, i].Value);
            }
        }

        public void LoadChange() //Заполняет поля брони
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
        public void Booking_Article(string str) //Заполняет возможными артикулами на странице броней
        {                                     
            string connectString = "Data Source="+con2+"\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
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

        public void Booking_Place(string article,string category) //Заполняет возможными расположениями на странице броней
        {
            string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
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
        
        public void Booking() //Создает запись брони
        {
            if (textBox3.Text.Length > 0 && textBox4.Text.Length > 0 && textBox1.Text.Length>0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (comboBox2.Text == dataGridView1[0, i].Value.ToString()
                        && comboBox3.Text == dataGridView1[7, i].Value.ToString())
                    {
                        if (Convert.ToInt32(textBox1.Text) <= Convert.ToInt32(dataGridView1[8, i].Value))
                        {
                            string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
                            SqlConnection myConnection = new SqlConnection(connectString);
                            myConnection.Open();
                            string query = "INSERT INTO booking (booking_category,booking_article,booking_count,booking_place,booking_date,booking_fio,booking_tel) " +
                                "VALUES ('" + comboBox1.Text + "', '" + comboBox2.Text + "', '" + textBox1.Text + "', '" + comboBox3.Text + "', '" + dateTimePicker2.Value.ToShortDateString() + "', '" + textBox3.Text + "', '" + textBox4.Text + "');";
                            SqlCommand command = new SqlCommand(query, myConnection);
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Close();
                            myConnection.Close();
                            LoadData_Booking();
                            LoadComboBox4_5();
                            MessageBox.Show("Бронь добавлена", "Уведомление");
                        }
                        else
                        {
                            MessageBox.Show("Количество больше допустимого", "Ошибка");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Не должно быть пустых значений", "Ошибка");
            }
        }

        public void LoadData_Furniture(string str) //Заполняет таблицу мебели
        {
            string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
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
        public void LoadData_Booking() //Заполняет таблицу броней
        {
            string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = "SELECT * FROM booking";
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();            
            while (reader.Read())
            {
                data.Add(new string[8]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
            }
            reader.Close();
            myConnection.Close();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            foreach (string[] s in data)
                dataGridView2.Rows.Add(s);
        }

        public void Delete(string id) //Удаляет запись брони из БД
        {
            string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
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
        public void Update(string id, string category, string article, string kol, string place, string date, string fio, string tel) //Редактирует запись брони в БД
        {
            if (id.Length > 0 && category.Length > 0 && article.Length > 0 && kol.Length > 0 && place.Length > 0 && date.Length > 0 && fio.Length > 0 && tel.Length > 0)
            {
                string connectString = "Data Source=" + con2 + "\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
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
                string str = "";
                while (reader.Read())
                {
                    str = reader[0].ToString();
                }
                reader.Close();
                if (fio != null && tel != null && fio != "" && tel != "" && kol != null && kol != "")
                {
                    int kol2 = Convert.ToInt32(kol);
                    if (Kol(kol2, Convert.ToInt32(str)) && kol2 > 0)
                    {
                        query = "UPDATE booking SET booking_category='" + category + "',booking_article='" + article + "',booking_count=" + kol2 + ",booking_place='" + place + "',booking_date='" + date + "',booking_fio='" + fio + "', booking_tel='" + tel + "' WHERE id=" + id;
                        command = new SqlCommand(query, myConnection);
                        reader = command.ExecuteReader();
                    }
                    else
                    {
                        MessageBox.Show("Количество больше или меньше допустимого", "Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Не должно быть пустых значений", "Ошибка");
                }
                reader.Close();
                myConnection.Close();
                LoadData_Booking();
                LoadComboBox4_5();
            }
            else
            {
                MessageBox.Show("Не должно быть пустых значений", "Ошибка");
            }
        }
        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadData_Furniture и LoadComboBox2 при выборе категории
        {
            String str = comboBox1.Text;
            LoadData_Furniture(str);
            LoadComboBox2();
        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadComboBox3 при выборе артикула на странице мебели
        {
            LoadComboBox3();
        }

        public void button1_Click(object sender, EventArgs e) //Вызывает Booking при нажатии на кнопку
        {
            Booking();
        }

        public void comboBox4_SelectedIndexChanged(object sender, EventArgs e) //Вызывает LoadChange при выборе расположения на странице мебели
        {
            LoadChange();

        }

        public void button2_Click(object sender, EventArgs e) //Вызывает Delete при нажатии на кнопку
        {
            Delete(comboBox5.Text);
        }

        public void comboBox6_SelectedIndexChanged(object sender, EventArgs e) //Вызывает Booking_Article при выборе артикула на странице брони
        {
            Booking_Article(comboBox6.Text);
        }

        public void comboBox8_SelectedIndexChanged(object sender, EventArgs e) //Вызывает Booking_Place при выборе расположения на странице брони
        {
            Booking_Place(comboBox8.Text,comboBox6.Text);
        }

        public void button3_Click(object sender, EventArgs e) //Вызывает Update при нажатии на кнопку
        {
            Update(comboBox4.Text, comboBox6.Text, comboBox8.Text, textBox2.Text, comboBox7.Text, dateTimePicker1.Value.ToShortDateString(), textBox5.Text, textBox6.Text);
        }

        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажатие было на ячейку, а не на заголовок столбца
            if (e.ColumnIndex >= 0)
            {
                comboBox2.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                comboBox3.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }
        }

        public void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
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
                textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
                textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                string format = "dd.MM.yyyy";
                DateTime selectedDate = DateTime.ParseExact(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString(), format, CultureInfo.InvariantCulture);
                dateTimePicker1.Value = selectedDate;
            }
        }
        public bool Kol(int a, int b)
        {
            if (a <= b)
            {
                return true;
            }
            return false;
        }
    }
}
