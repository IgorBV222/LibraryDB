using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Library
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SQLiteConnection conn;        
        private DataTable dtAutors;
        private DataTable dtBooks;

        //private DataSet dtAutors;
        //private DataSet dtBooks;

        private SQLiteDataAdapter adAutors;
        private SQLiteDataAdapter adBooks;
        private void selectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                conn = new SQLiteConnection("DataSource=" + filename);
                conn.Open(); // создаст файл 
                string sqltext = "select name from sqlite_master where type='table';";
                SQLiteCommand cmd = new SQLiteCommand(sqltext, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                createTablesToolStripMenuItem.Enabled = !reader.HasRows; // отключаем кнопку создания таблиц
                //conn.Close();
                //dataGridViewFill();
                dataGridViewFillAutors();
                dataGridViewFillBooks();
                conn.Close();
            }
            else
            {
                //указать что будет если база не откроится Library.db
            }
        }

        private void createTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sqltext = "create table Autors(id int PRIMARY KEY, [name] varchar(20), description varchar(100));" +
                "create table Books(id int PRIMARY KEY, id_autors int, [name] varchar(20), description varchar(100));";
            SQLiteCommand cmd = new SQLiteCommand(sqltext, conn);
            //cmd.Connection = conn; // эту строку можно убрать conn на строке выше
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            createTablesToolStripMenuItem.Enabled = false; // отключаем кнопку создания таблиц
        }

        //private void dataGridViewFill()
        //{
        //    string sqltext = "select * from Autors;";
        //    adAutors = new SQLiteDataAdapter(sqltext, conn);
        //    SQLiteCommandBuilder cbAutors = new SQLiteCommandBuilder(adAutors);
        //    dtAutors = new DataTable();
        //    //dtAutors = new DataSet();
        //    adAutors.Fill(dtAutors);
        //    dGVAutors.DataSource = dtAutors;

        //    sqltext = "select * from Books;";
        //    adBooks = new SQLiteDataAdapter(sqltext, conn);
        //    SQLiteCommandBuilder cbBooks = new SQLiteCommandBuilder(adBooks);
        //    dtBooks = new DataTable();
        //    //dtBooks = new DataSet();
        //    adBooks.Fill(dtBooks);
        //    dGVBooks.DataSource = dtBooks;
        //    // dGVBooks..DataSource = dtBooks.Tables[0];

        //}

        private void dataGridViewFillAutors()
        {
            string sqltext = "select * from Autors;";
            adAutors = new SQLiteDataAdapter(sqltext, conn);
            SQLiteCommandBuilder cbAutors = new SQLiteCommandBuilder(adAutors);
            dtAutors = new DataTable();            
            adAutors.Fill(dtAutors);
            dGVAutors.DataSource = dtAutors;            

        }
        private void dataGridViewFillBooks()
        {
            string sqltext = "select * from Books;";
            adBooks = new SQLiteDataAdapter(sqltext, conn);
            SQLiteCommandBuilder cbBooks = new SQLiteCommandBuilder(adBooks);
            dtBooks = new DataTable();            
            adBooks.Fill(dtBooks);
            dGVBooks.DataSource = dtBooks;           

        }
        private void saveChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtAutors.AcceptChanges();
            adAutors.Update(dtAutors);
            dtBooks.AcceptChanges();
            adBooks.Update(dtBooks);
        }

        
    }
}
