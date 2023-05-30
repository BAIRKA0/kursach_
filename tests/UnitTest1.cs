using Kursach_TRPK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var form = new Form1();
            form.tabControl1.SelectedTab = form.tabPage2;
            DataGridView table = form.dataGridView2;
            int initialRowCount = table.Rows.Count;
            form.tabControl1.SelectedTab = form.tabPage1;
            form.comboBox2.SelectedItem = "10101";
            form.comboBox3.SelectedItem = "Лермонтово, 8";
            form.textBox1.Text = "1";
            form.textBox3.Text = "Жалсанов Юрий";
            form.textBox4.Text = "123456";
            form.button1.PerformClick();
            form.Booking();
            form.tabControl1.SelectedTab = form.tabPage2;
            Thread.Sleep(100);
            form.LoadData_Booking();
            table = form.dataGridView2;
            int rowCountAfterClick = table.Rows.Count;
            Assert.AreEqual(initialRowCount + 1, rowCountAfterClick);
        }
        [TestMethod]
        public void TestMethod2()
        {
            string id = "10";
            var form = new Form1();
            string answer = "";
            string connectString = "Data Source=BAIRKA\\SQLEXPRESS; Initial Catalog=furniture_store; Integrated Security=true;";
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = "SELECT * FROM booking WHERE id="+id;
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.Write(reader[0].ToString());
            form.Delete(id);
            try
            {
                reader = command.ExecuteReader();
                reader.Read();
            }
            catch (Exception e)
            {
                answer = "записи нет";
            }
            Assert.AreEqual(answer, "записи нет");
        }
        [TestMethod]
        public void TestMethod3()
        {
            var form = new Form1();
            Assert.IsTrue(form.Kol(1, 2));
        }
    }
}
