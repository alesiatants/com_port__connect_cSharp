using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com_port
{
    public partial class Form1 : Form
    {
        string dataOut;
        byte byteOut;
        byte[] byteOutMassive;
        string SendWith;
        string DataIN;
        byte[] messByte;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //чтение портов доступных в системе
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.AddRange(ports);
            chBoxDtrEnable.Checked = false;
            serialPort1.DtrEnable = false;
            chBoxRtsEnable.Checked = false;
            serialPort1.RtsEnable = false;
            btnSendData.Enabled = true;
            SendWith = "Both";
            toolStripComboBox1.Text = "Add to Old Data";
            toolStripComboBox2.Text = "Both";
            toolStripComboBox3.Text = "BOTTOM";
            //Очистка содержимого бокса
            /*comboBox1.Items.Clear();
            //Добавление найденных портов в бокс
            comboBox1.Items.AddRange(ports);*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
     

     

     

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (hexInput.Checked)
                {
                    byte[] bytes = Enumerable.Range(0, tBoxDataOut.Text.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(tBoxDataOut.Text.Substring(x, 2), 16))
                         .ToArray().ToArray();
                    //int NumberChars = tBoxDataOut.Text.Length;
                    //byte[] bytes = new byte[NumberChars / 2];
                    // for (int i = 0; i < NumberChars; i += 2)
                    //bytes[i / 2] = Convert.ToByte(tBoxDataOut.Text.Substring(i, 2), 16);
                    // byteOut = Convert.ToByte(tBoxDataOut.Text);
                    //dataOut = tBoxDataOut.Text;
                    //serialPort1.WriteLine(dataOut);
                    if (SendWith == "None")
                    {

                        serialPort1.Write(bytes, 0, 1);
                    }
                    else if (SendWith == "Both")
                    {
                        serialPort1.Write(bytes, 0, 1);
                        //serialPort1.Write(dataOut+"\r\n");
                    }
                    else if (SendWith == "New Line")
                    {
                        serialPort1.Write(bytes, 0, 1);
                        //serialPort1.WriteLine(dataOut+"\n");
                    }
                    else if (SendWith == "Carriage Return")
                    {
                        serialPort1.Write(bytes, 0, 1);
                        //serialPort1.WriteLine(dataOut+"\r");
                    }
                }
                if (stringInput.Checked)
                {
                    dataOut = tBoxDataOut.Text;
                    if (SendWith == "None")
                    {

                        serialPort1.Write(dataOut);
                    }
                    else if (SendWith == "Both")
                    {
                        serialPort1.Write(dataOut+"\r\n");
                    }
                    else if (SendWith == "New Line")
                    {
                        serialPort1.WriteLine(dataOut+"\n");
                    }
                    else if (SendWith == "Carriage Return")
                    {
                        serialPort1.WriteLine(dataOut+"\r");
                    }

                }
                if (dexInput.Checked)
                {
                    int dataOutint = int.Parse(tBoxDataOut.Text);
                    byte[] b = BitConverter.GetBytes(dataOutint);
                    if (SendWith == "None")
                    {

                        serialPort1.Write(b,0,4);
                    }
                    else if (SendWith == "Both")
                    {
                        serialPort1.Write(b, 0, 4);
                    }
                    else if (SendWith == "New Line")
                    {
                        serialPort1.Write(b, 0, 4);
                    }
                    else if (SendWith == "Carriage Return")
                    {
                        serialPort1.Write(b, 0, 4);
                    }
                }
            }

        }

        private void chBoxDtrEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxDtrEnable.Checked)
            {
                serialPort1.DtrEnable = true;
                MessageBox.Show("DTR Enable", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else{
                serialPort1.DtrEnable = false;
            }
        }

        private void chBoxRtsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRtsEnable.Checked)
            {
                serialPort1.RtsEnable = true;
                MessageBox.Show("RTS Enable", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                serialPort1.RtsEnable = false;
            }
        }

        private void tBoxDataOut_TextChanged(object sender, EventArgs e)
        {
            int dataoutlength = tBoxDataOut.TextLength;
            lblDataOutLength.Text = string.Format("{0:00}", dataoutlength);
            

        }

       

        private void tBoxDataOut_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (e.KeyCode == Keys.Enter)
                {
                this.doSomething();
                e.Handled = true;
                e.SuppressKeyPress = true;    
               }
        }

        private void doSomething()
        {
            if (serialPort1.IsOpen)
            {
                dataOut = tBoxDataOut.Text;
                if (SendWith == "None")
                    {
                    serialPort1.Write(dataOut + "\r\n");
                }
                    else if (SendWith == "Both")
                    {
                        //serialPort1.Write(dataOut + "\r\n");
                    }
                    else if (SendWith == "New Line")
                    {
                        //serialPort1.WriteLine(dataOut + "\n");
                    }
                    else if (SendWith == "Carriage Return")
                    {
                        //serialPort1.WriteLine(dataOut + "\r");
                    }
                
            }
        }


        static float ToFloat(byte[] input)
        {
            byte[] newArray = new[] { input[2], input[3], input[0], input[1] };
            return BitConverter.ToSingle(newArray, 0);
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            if (hexOut.Checked || dexOut.Checked || floatOut.Checked)
            {
                int byteRecieved = serialPort1.BytesToRead;
                messByte = new byte[byteRecieved];
                serialPort1.Read(messByte, 0, byteRecieved);
                this.Invoke(new EventHandler(ShowData));
            }
            if (stringOut.Checked)
            {

                DataIN = serialPort1.ReadExisting();
                this.Invoke(new EventHandler(ShowData));
            }
        }

        private void ShowData(object sender, EventArgs e)
        {
            
            if (hexOut.Checked) {
                int dataINLength = messByte.Length;
                lblDataINLength.Text = string.Format("{0:00}", dataINLength);
                if (toolStripComboBox1.Text == "Allways Update")
                {

                    //foreach (byte i in messByte)
                    //   tBoxDataIN.Text = i.ToString();
                    StringBuilder hex = new StringBuilder(messByte.Length * 2);
                    foreach (byte b in messByte)
                        hex.AppendFormat("{0:x2}", b);
                    tBoxDataIN.Text = "0x" + hex.ToString().ToUpper() + " ";
                }
                else if (toolStripComboBox1.Text == "Add to Old Data")
                {
                    if (toolStripComboBox3.Text == "TOP")
                    {
                        //foreach (byte i in messByte)
                        //  tBoxDataIN.Text = tBoxDataIN.Text.Insert(0, i.ToString());
                        StringBuilder hex = new StringBuilder(messByte.Length * 2);
                        foreach (byte b in messByte)
                            hex.AppendFormat("{0:x2}", b);
                        tBoxDataIN.Text = tBoxDataIN.Text.Insert(0, "0x" + hex.ToString().ToUpper() + " ");
                    }
                    else if (toolStripComboBox3.Text == "BOTTOM")
                    {
                        //foreach (byte i in messByte)
                        //tBoxDataIN.Text += i.ToString();
                        StringBuilder hex = new StringBuilder(messByte.Length * 2);
                        foreach (byte b in messByte)
                            hex.AppendFormat("{0:x2}", b);
                        tBoxDataIN.Text += "0x" + hex.ToString().ToUpper() + " ";
                    }
                }

                }
            if (dexOut.Checked)
            {
                int dataINLength = messByte.Length;
                lblDataINLength.Text = string.Format("{0:00}", dataINLength);
                if (toolStripComboBox1.Text == "Allways Update")
                {

                    foreach (byte i in messByte)
                    tBoxDataIN.Text = i.ToString()+" ";
                }
                else if (toolStripComboBox1.Text == "Add to Old Data")
                {
                    if (toolStripComboBox3.Text == "TOP")
                    {
                        foreach (byte i in messByte)
                        tBoxDataIN.Text = tBoxDataIN.Text.Insert(0, i.ToString()+" ");
                        }
                    else if (toolStripComboBox3.Text == "BOTTOM")
                    {
                        foreach (byte i in messByte)
                        tBoxDataIN.Text += i.ToString()+" ";
                    }
                }
            }
            if (floatOut.Checked)
            {
                int dataINLength = messByte.Length;
                lblDataINLength.Text = string.Format("{0:00}", dataINLength);
                if (toolStripComboBox1.Text == "Allways Update")
                {
                    tBoxDataIN.Text = BitConverter.ToSingle(messByte, 0) + "°C ";

                }
                else if (toolStripComboBox1.Text == "Add to Old Data")
                {
                    if (toolStripComboBox3.Text == "TOP")
                    {
                        tBoxDataIN.Text = tBoxDataIN.Text.Insert(0, BitConverter.ToSingle(messByte, 0) + "°C ");
                       

                    }
                    else if (toolStripComboBox3.Text == "BOTTOM")
                    {
                        tBoxDataIN.Text += BitConverter.ToSingle(messByte, 0) + "°C ";
                    }
                }
            }
            if (stringOut.Checked)
            {
                int dataINLength = DataIN.Length;
                lblDataINLength.Text = string.Format("{0:00}", dataINLength);
                if (toolStripComboBox1.Text == "Allways Update")
                {
                    tBoxDataIN.Text = DataIN;
                }
                else if (toolStripComboBox1.Text == "Add to Old Data")
                {
                    if (toolStripComboBox3.Text == "TOP")
                    {
                       
                        tBoxDataIN.Text = tBoxDataIN.Text.Insert(0, DataIN+" ");
                        }
                    else if (toolStripComboBox3.Text == "BOTTOM")
                    {
                        tBoxDataIN.Text += DataIN+" ";
                    }
                }
            }
            }

       
       

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBOUDRATE.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDATABITS.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxSTOPBITS.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxPARITYBITS.Text);
                serialPort1.Open();
                progressBar1.Value = 100;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";
            }
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tBoxDataIN.Text != "")
            {
                tBoxDataIN.Text = "";
            }
        }

        private void toolStripComboBox2_DropDownClosed(object sender, EventArgs e)
        {
            //None
            //Both
            //New Line
            //Carriage Return
            if (toolStripComboBox2.Text == "None")
            {
                SendWith = "None";
            }
            else if (toolStripComboBox2.Text == "Both")
            {
                SendWith = "Both";
            }
            else if (toolStripComboBox2.Text == "New Line")
            {
                SendWith = "New Line";
            }
            else if (toolStripComboBox2.Text == "Carriage Return")
            {
                SendWith = "Carriage Return";
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Alesia", "Creator",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
            }
        }

        private void buttonFloatOutput_Click(object sender, EventArgs e)
        {

        }
    }
}
