using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditTask
{
    public partial class Form1 : Form
    {
        private Encoding encoding { get; set; }

        private string fileLoadName { get; set; }

        private string fileSaveName { get; set; }

        private string DirectorySaveName { get; set; }

        private char[] punctMarks { get; set; }

        private int amountSymbols { get; set; }


        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) textBox1.Enabled = true;
            else
            {
                textBox1.Text = null;
                textBox1.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) textBox2.Enabled = true;
            else
            {
                textBox2.Text = null;
                textBox2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            this.fileLoadName = openFileDialog1.FileName;
            this.encoding = Encoding.GetEncoding("Windows-1251");

            MessageBox.Show("File selected");
        }


        private bool checkCorrect()
        {
            try
            {
                if (!String.IsNullOrEmpty(textBox1.Text)) this.amountSymbols = Convert.ToInt16(textBox1.Text);
                else this.amountSymbols = -1;
                if (!String.IsNullOrEmpty(textBox2.Text))
                    punctMarks = textBox2.Text.ToCharArray();
                if (String.IsNullOrEmpty(textBox1.Text) && String.IsNullOrEmpty(textBox2.Text)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void editText()
        {
            string fileEditText = "";

            string fileLoadName = this.fileLoadName;

            char[] punctMarks = this.punctMarks;

            int amountSymbols = this.amountSymbols;

           


            System.IO.StreamWriter file = new System.IO.StreamWriter(DirectorySaveName);

             if (punctMarks!=null && punctMarks.Length!=0 && amountSymbols > 0) // sort punctMarks && amountSymbol
             {
                    foreach (var line in File.ReadLines(fileLoadName,this.encoding))
                    {
                        string[] lineWords = line.Split(' ');
                        foreach (var word in lineWords)
                        {
                            string editWord = word;
                            editWord = word.Trim(punctMarks);
                            if (editWord.Length > amountSymbols) fileEditText += editWord + ' ';
                        }
                    fileEditText =  fileEditText.Remove(fileEditText.LastIndexOf(' '), 1);
                    file.WriteLine(fileEditText, false, this.encoding);
                    fileEditText = "";
                    }
              }

             else if (amountSymbols < 1)   // sort of puctMarks
             {
                    foreach (string line in File.ReadLines(fileLoadName, this.encoding))
                    {
                    string[] lineWords = line.Split(' ');
                    foreach (var word in lineWords)
                    {
                        string editWord = word;
                        editWord = word.Trim(punctMarks);
                        fileEditText += editWord + ' ';
                    }
                    fileEditText = fileEditText.Remove(fileEditText.LastIndexOf(' '), 1);
                    file.WriteLine(fileEditText, DirectorySaveName, this.encoding);
                    fileEditText = "";
                }
             }

            else if (punctMarks==null || punctMarks.Length==0) // sort of amountSymbol
            {
                foreach (var line in File.ReadLines(fileLoadName, this.encoding))
                {
                    string[] lineWords = line.Split(' ');
                    foreach (var word in lineWords)
                    {
                        string editWord = word;
                        if (editWord.Length > amountSymbols) fileEditText += editWord + ' ';
                    }
                    fileEditText = fileEditText.Remove(fileEditText.LastIndexOf(' '), 1);
                    file.WriteLine(fileEditText, false, this.encoding);
                    fileEditText = "";
                }
            }
            file.Close(); //Encoding UTF-8
}
        public delegate void InvokeDelegate();

        public void InvokeMethod()     
        {
            var indexItem = listView1.FindItemWithText(fileSaveName).Index;
            while (listView1.Items[indexItem].SubItems[1].Text == "Complete")
                indexItem++;
                listView1.Items[indexItem].SubItems[1].Text = "Complete";                   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            this.DirectorySaveName = saveFileDialog1.FileName;
            this.fileSaveName = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf('\\'));

            if (this.fileLoadName != null && fileSaveName!= null && checkCorrect())
            {
                ListViewItem item = new ListViewItem(fileSaveName);
                item.SubItems.Add("Processing");
                listView1.Items.Add(item);
                ThreadPool.QueueUserWorkItem(new WaitCallback((s) =>
                {
                          editText();
                          listView1.BeginInvoke(new InvokeDelegate(InvokeMethod));
                }));
            }
            else
            {
                MessageBox.Show("Input data is not correct");
            }
        }


    }
}
