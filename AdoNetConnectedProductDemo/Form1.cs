using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AdoNetConnectedProductDemo
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Category> list = new List<Category>();
                string qry = "Select * from Category";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category category = new Category();
                        category.Cid = Convert.ToInt32(reader["Cid"]);
                        category.cname = reader["cname"].ToString();
                        list.Add(category);
                    }
                }
                comboBoxCName.DataSource= list;
                comboBoxCName.ValueMember = "Cid";
                comboBoxCName.DisplayMember = "cname";
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into Product Values(@Name,@Price,@Cid)";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToInt32(textBoxPrice.Text));
                cmd.Parameters.AddWithValue("@Cid", Convert.ToInt32(comboBoxCName.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if(result >= 1)
                {
                    MessageBox.Show("Record Inserted");
                    ClearField();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.* ,c.cname from Product p inner join Category c on c.Cid = p.Cid where p.Pid=@Pid";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Pid", Convert.ToInt32(textBoxPid.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        textBoxName.Text = reader["Name"].ToString();
                        textBoxPrice.Text = reader["Price"].ToString();
                        comboBoxCName.Text = reader["Cname"].ToString();
                    }

                }
                else
                {
                    MessageBox.Show("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Product set Name=@Name,Price=@Price, Cid=@Cid where Pid=@Pid ";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToInt32(textBoxPrice.Text));
                cmd.Parameters.AddWithValue("@Cid", Convert.ToInt32(comboBoxCName.SelectedValue));
                cmd.Parameters.AddWithValue("@Pid", Convert.ToInt32(textBoxPid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record Upadate");
                    ClearField();


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Product where Pid=@Pid ";
                cmd = new SqlCommand(qry, con);

                cmd.Parameters.AddWithValue("@Pid", Convert.ToInt32(textBoxPid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record Delete");
                    GetAllProduct();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }
        public void GetAllProduct()
        {
            string qry = "select p.*,c.Cname from Product p inner join Category c on c.Cid = p.Cid";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();
        }
        private void ClearField()
        {
            textBoxPid.Clear();
            textBoxName.Clear();
            textBoxPrice.Clear();
            
            comboBoxCName.Refresh();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
