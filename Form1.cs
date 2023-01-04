using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void redirect(string a)
        {
            try
            {
                // Veritabanına bağlantı kurun
                string connectionString = "server=your_server;user=your_user;database=your_database;password=your_password";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                // Kısaltılmış linki kullanarak veritabanından orijinal linki alın
                string selectQuery = "SELECT original_link FROM links WHERE s = @a";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@a", a);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                string originalLink = reader["original_link"].ToString();

                // Tarayıcıda orijinal linki açın
                System.Diagnostics.Process.Start(originalLink);
            }
            catch (MySqlException ex)
            {
                // Veritabanına bağlantı kurulamadı veya veritabanından veri okunurken hata oluştu
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException ex)
            {
                // System.Diagnostics.Process.Start() metoduna null değerleri geçirildi
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanına bağlantı kurun
                string connectionString = "server=your_server;user=your_user;database=your_database;password=your_password";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                // Orijinal linki alın ve veritabanına yeni bir kayıt ekleyin
                string originalLink = textBox1.Text;
                string insertQuery = "INSERT INTO links (original_link) VALUES (@originalLink)";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@originalLink", originalLink);
                command.ExecuteNonQuery();
                // Yeni kaydın ID'sini alın ve kısaltılmış link oluşturun
                long id = command.LastInsertedId;
                string a = Convert.ToBase64String(BitConverter.GetBytes(id));

                // Kısaltılmış linki veritabanına ekleyin
                string updateQuery = "UPDATE links SET s = @a WHERE id = @id";
                command = new MySqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@a", a);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                // Kısaltılmış linki gösterin ve kopyalaması için bir button ekleyin
                textBox2.Text = "http://fkdk.ml/a.php?a=" + a;
                button2.Visible = true;








            }
            catch (MySqlException ex)
            {
                // Veritabanına bağlantı kurulamadı veya veritabanına kayıt eklenirken hata oluştu
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException ex)
            {
                // Convert.ToBase64String() veya Clipboard.SetText() metodlarına null değerleri geçirildi
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kısaltılmış linki kopyalayın
                Clipboard.SetText(textBox2.Text);
            }
            catch (ArgumentNullException ex)
            {
                // Clipboard.SetText() metoduna null değerleri geçirildi
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Kısaltılmış linki kopyalayın
                Clipboard.SetText(textBox4.Text);
            }
            catch (ArgumentNullException ex)
            {
                // Clipboard.SetText() metoduna null değerleri geçirildi
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void button4_Click(object sender, EventArgs e)
        {

            // Veritabanına bağlantı kurun
            string connectionString = "server=your_server;user=your_user;database=your_database;password=your_password";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();


            // Veritabanındaki s değerini alıp original_link değerini güncelleyin
            string s = textBox3.Text;
         // Uri uri = new Uri(s);
         // string query = uri.Query;
         // query = query.Replace("http://fkdk.ml/a.php?a=", "");
            string newOriginalLink = textBox4.Text;
            string updateQuery = "UPDATE links SET s = @newOriginalLink WHERE s = @s";
            MySqlCommand command = new MySqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@newOriginalLink", newOriginalLink);
            command.Parameters.AddWithValue("@s", s);
            command.ExecuteNonQuery();
            textBox4.Text = "http://fkdk.ml/a.php?a=" + textBox4.Text;
            button4.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}