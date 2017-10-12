using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace DMM.HEHE
{
    public partial class Form1 : Form
    {
        private Hashtable Database = new Hashtable();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string querry = textBox1.Text.ToLower();
            string[] querry_identifier = querry.Split(' ');    
              
            if(querry_identifier[0]=="create" && querry_identifier[1]=="table" && querry_identifier[2] != " " && querry_identifier[3] != " ")
            {
                string result = "creating table " + querry_identifier[2] + " with columns ";
                string[] column = querry_identifier[3].Split('"');
                string column_count = null;
                for(int i=0;i<column.Count();i++)
                {
                    if (i % 2 != 0)
                    {
                        if(i < column.Count()-1)
                            column_count = column_count + column[i] + " ";
                        
                        //result = result + "[" + column[i] + "]";
                    }
                }
                string[] columns = column_count.Split(' ');
                createTable(querry_identifier[2], columns);
               // MessageBox.Show(result);
            }
            else 
            if(querry_identifier[0] == "drop" && querry_identifier[1]== "table" )
            {
                if (table_exists(querry_identifier[2]))
                {
                    //MessageBox.Show("Deleting Table");
                    listBox1.Items.Add("Table " + querry_identifier[2]+" Deleted");
                    dropTable(querry_identifier[2]);
                }
                else
                {
                    //MessageBox.Show("Table not found");
                    listBox1.Items.Add("table " + querry_identifier[2] + " not found");
                }
            }
            else
            if (querry_identifier[0] == "select" && querry_identifier[1] == "*" && querry_identifier[2] == "from" && querry_identifier[3] != " ")
            {
                if (table_exists(querry_identifier[3]))
                {                   
                    if(getTable(querry_identifier[3]))
                    {
                        listBox1.Items.Add("Getting Content from " + querry_identifier[3]);
                    }
                }
                else
                {
                    listBox1.Items.Add("table " + querry_identifier[3] + " not found");
                }
            }
            else
            if (querry_identifier[0] == "insert" && querry_identifier[1] == "into" && querry_identifier[2] != " ")
            {
                if (table_exists(querry_identifier[2]))
                {
                    string[] values = querry_identifier[3].Split('"');
                    listBox1.Items.Add("Added new row to:  " + querry_identifier[2]);
                    string column_count = null;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (i < values.Count() - 1)
                                column_count = column_count + values[i] + " ";
                        }
                    }
                    string[] value = column_count.Split(' ');
                    insertTable(querry_identifier[2], value);

                }
                else
                {
                    listBox1.Items.Add("table " + querry_identifier[2] + " not found");
                }
            }
            else
            {
                listBox1.Items.Add("Unrecognized Querry");
            }
        }       
        private void createTable(string table_name, string[] columns)
        {
            if (!table_exists(table_name))
            {
                DeactivateList(listView1);
                List<List<string>> table = new List<List<string>>();
                List<string> row = columns.ToList<string>();
                table.Add(row);
                Database.Add(table_name, table);
                listBox1.Items.Add("table " + table_name + " created");
                //O(1)
            }
            else
            {
                MessageBox.Show("Table already exists");
            }
        }      
        private bool table_exists(string table_name)
        {
            if (Database.ContainsKey(table_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool dropTable(string table_name)
        {
            DeactivateList(listView1);
            if (table_exists(table_name))
            {
                Database.Remove(table_name);
                return true;
            }else
            {
                return false;
            }
            //O(1)
        }
        private bool getTable(string table_name)
        {
            if (table_exists(table_name))
            {
                activateList(listView1);
                List<List<string>> table_values = Database[table_name] as List<List<string>>;

                for(int i=0; i<table_values[0].Count();i++)
                {
                    listView1.Columns.Add(table_values[0][i]);
                }

                for(int j=1;j<table_values.Count();j++)
                {
                    string[] row = table_values[j].Select(i => i.ToString()).ToArray();
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                }
                return true;
            }
            else
                return false;

            //O(n*n)
        }
        private bool insertTable(string table_name, string[] values)
        {
            DeactivateList(listView1);
            listBox1.Visible = true;
            if (table_exists(table_name))
            {
                listBox1.Visible = true;
                List<List<string>> table_values = Database[table_name] as List<List<string>>;
                List<string> new_row = values.ToList();
                table_values.Add(new_row);
                return true;
            }
            else
            {
                return false;
            }

            //O(n)
        }

        private bool DeactivateList(ListView list)
        {
            listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listView1.Items.Clear();
            listView1.Visible = false;
            listBox1.Visible = true;
            return true;
        }

        private bool activateList(ListView list)
        {
            listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            listView1.Items.Clear();
            listView1.Visible = true;
            listBox1.Visible = false;
            return true;
        }
    }
}