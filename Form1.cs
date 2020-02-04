using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DomaciZadatak7
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        string connectString;

        public Form1()
        {
            InitializeComponent();
            connectString = ConfigurationManager.ConnectionStrings["DomaciZadatak7.Properties.Settings.StudentiConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            using (connection = new SqlConnection(connectString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Studenti", connection))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Studenti VALUES (@Ime, @Prezime, @DatumRodjenja, @Pol, @Student)";

            using (connection = new SqlConnection(connectString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("Ime", textBox_ime.Text);
                command.Parameters.AddWithValue("Prezime", textBox_prezime.Text);
                command.Parameters.AddWithValue("DatumRodjenja", dateTimePicker1.Value.Date);
                command.Parameters.AddWithValue("Pol", getPol());
                command.Parameters.AddWithValue("Student", ifStudent());


                command.ExecuteNonQuery();
            }

            Run();
        }

        private string getPol()
        {
            if (radioButton_muski.Checked)
            {
                radioButton_zenski.Checked = false;
                return "Muški";
            }
            if (radioButton_zenski.Checked)
            {
                radioButton_muski.Checked = false;
                return "Ženski";
            }
            return "Srednji";
        }

        private bool ifStudent()
        {
            if (checkBox_student.Checked)
            {
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Studenti WHERE Id=" + textBox_id.Text.ToString();

            using (connection = new SqlConnection(connectString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            Run();
        }
    }
}
