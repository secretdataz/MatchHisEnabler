using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace MatchHisEnabler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool enabled = false;
        private string propsPath;
        private void SetState(string msg)
        {
            label2.Text = msg;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            var Processes = Process.GetProcessesByName("LolClient");
            if(Processes.Length > 0)
            {
                var Prc = Processes[0];
                var LoLPath = Prc.MainModule.FileName;
                LoLPath = Path.GetDirectoryName(LoLPath);
                var PropsPath = Path.Combine(LoLPath, "lol.properties");
                var content = File.ReadAllLines(PropsPath);
                foreach(var line in content)
                {
                    if (line.StartsWith("matchHistoryTest"))
                    {
                        SetState("พบ LoL / มี Match history แบบใหม่อยู่แล้ว");
                        enabled = true;
                        btnRemove();
                        propsPath = PropsPath;
                        timer1.Stop();
                        return;
                    }
                }
                // Match history not found. Fall thru
                SetState("พบ LoL");
                btnEnable();
                propsPath = PropsPath;
                timer1.Stop();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(propsPath))
            {
                MessageBox.Show("Path variable is empty. How?");
                return;
            }
            var content = new List<string>();
            content.AddRange(File.ReadAllLines(propsPath));
            if (enabled)
            {
                content.Remove("matchHistoryTest=true");
                btnEnable();
                SetState("ปิดระบบ Match history แบบใหม่แล้ว กรุณารีสตาร์ทเกม");
                enabled = false;
            }
            else
            {
                content.Add("matchHistoryTest=true");
                btnRemove();
                SetState("เปิดระบบ Match history แบบใหม่แล้ว กรุณารีสตาร์ทเกม");
                enabled = true;
            }
            File.WriteAllLines(propsPath ,content.ToArray());
        }

        private void btnEnable()
        {
            button1.Text = "เปิด match his ใหม่";
            button1.Enabled = true;
        }

        private void btnRemove()
        {
            button1.Text = "ปิด match his ใหม่";
            button1.Enabled = true;
        }

    }
}