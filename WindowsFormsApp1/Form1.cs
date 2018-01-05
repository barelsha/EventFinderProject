using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            string url = "http://localhost:54273/service1.svc/Events/1";
            WebRequest req = (WebRequest)WebRequest.Create(@url);
            req.Method = "GET";
            req.ContentType = "application/json; charset=utf-8";

            WebResponse resp = (WebResponse)req.GetResponse();

            Encoding enc = Encoding.GetEncoding(1252);  // Windows default Code Page

            StreamReader loResponseStream =
               new StreamReader(resp.GetResponseStream(), enc);

            string lcHtml = loResponseStream.ReadToEnd();

            MessageBox.Show(lcHtml);
            resp.Close();
        }
    }
}
