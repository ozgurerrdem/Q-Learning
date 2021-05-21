using QLearning.MachineLearning;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QLearning
{
    public partial class qLearningForm : Form
    {
        public qLearningForm()
        {
            InitializeComponent();
        }
        Button butonBul(String btn_name)
        {
            return panel1.Controls[btn_name] as Button;
        }
        public static string baslangicNokta;
        public static string bitisNokta;
        public int a = 0;
        public int karesayisi = 20;
        public int kareKenarBoyutu = 30;
        FileStream fs;
        StreamWriter sw;
        Random rnd = new Random();
        public int[,] qTable;
        private void Form1_Load(object sender, EventArgs e)
        {
            fs = new FileStream(@"engel.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fs);
            panel1.Width = karesayisi * kareKenarBoyutu;
            panel1.Height = karesayisi * kareKenarBoyutu;
            Size = new Size(karesayisi * kareKenarBoyutu + 14, karesayisi * kareKenarBoyutu + 37);
            int rastgele;
            for (int i = 0; i < karesayisi; i++)
            {
                for (int j = 0; j < karesayisi; j++)
                {
                    Button buton = new Button();
                    buton.Width = kareKenarBoyutu;
                    buton.Height = kareKenarBoyutu;
                    buton.Name = i.ToString() + "," + j.ToString();
                    buton.Location = new Point(j * kareKenarBoyutu, i * kareKenarBoyutu);
                    rastgele = rnd.Next(101);
                    if (rastgele <= 30)
                    {
                        buton.BackColor = Color.OrangeRed;
                        buton.Enabled = false;
                        sw.WriteLine("(" + (i + 1).ToString() + "," + (j + 1).ToString() + ",K)\n");
                    }
                    else
                    {
                        buton.BackColor = Color.White;
                        sw.WriteLine("(" + (i + 1).ToString() + "," + (j + 1).ToString() + ",B)\n");
                    }
                    this.panel1.Controls.Add(buton);
                    buton.Click += new System.EventHandler(clickedButton);
                }
            }
            MessageBox.Show("İlk tıklayacağınız nokta başlangıç noktası, ikinci tıklayacağınız nokta bitiş noktası olacak", "Başlamak için");
        }
        public string basilanButon;
        public string basla;
        public string bit;
        public void clickedButton(object sender, EventArgs e)
        {
            basilanButon = (sender as Button).Name;
            Button btn = new Button();
            string[] split;
            if (a == 0)
            {
                baslangicNokta = basilanButon;
                btn = butonBul(baslangicNokta);
                btn.BackColor = Color.LightSkyBlue;
                MessageBox.Show("Başlangıç Noktası Seçildi");
                split = baslangicNokta.Split(",");
                basla = "(" + (Convert.ToInt32(split[0]) + 1).ToString() + "," + (Convert.ToInt32(split[1]) + 1).ToString() + ",M)";
            }
            else if (a == 1)
            {
                bitisNokta = basilanButon;
                btn = butonBul(bitisNokta);
                btn.BackColor = Color.GreenYellow; ;
                MessageBox.Show("Bitiş Noktası Seçildi");
                btnEnable(false);
                split = bitisNokta.Split(",");
                bit = "(" + (Convert.ToInt32(split[0]) + 1).ToString() + "," + (Convert.ToInt32(split[1]) + 1).ToString() + ",Y)";
                sw.WriteLine("Başlangıç ve Bitiş Noktaları\n\n" + basla + "\n\n" + bit);
                sw.Flush();
                sw.Close();
                fs.Close();
                qLearning();
            }
            a++;
        }
        (int, int) kordinatBul(int a)
        {
            int x = 0;
            while (a >= karesayisi)
            {
                x++;
                a -= karesayisi;
            }
            return (x, a);
        }
        private void qLearning()
        {
            int[,] rTable = createRTable();
            string[] split;
            split = bitisNokta.Split(",");
            int bitisKacinciNokta = Convert.ToInt32(split[0]) * karesayisi + Convert.ToInt32(split[1]);
            var room = new RoomsProblem();
            room.odul = rTable;
            room.hedef = bitisKacinciNokta;
            var qL = new QLearningFunctions(0.8, room);
            qL.AjanEgit(2500);
            split = baslangicNokta.Split(",");
            int baslangicKacinciNokta = Convert.ToInt32(split[0]) * karesayisi + Convert.ToInt32(split[1]);
            var qLearningStats = qL.Run(baslangicKacinciNokta);
            for (int i = 0; i < qLearningStats.Actions.Length - 1; i++)
            {
                Button sonuc = butonBul(kordinatBul(qLearningStats.Actions[i]).Item1.ToString() + "," + kordinatBul(qLearningStats.Actions[i]).Item2.ToString());
                sonuc.BackColor = Color.Bisque;
            }
            MessageBox.Show("En kısa yol bulundu!");
        }
        private int[,] createRTable()
        {
            int[,] rTable = new int[karesayisi * karesayisi, karesayisi * karesayisi];
            int x, y, nx, ny;
            Button btn = new Button();
            for (int i = 0; i < karesayisi * karesayisi; i++)
            {
                for (int j = 0; j < karesayisi * karesayisi; j++)
                {
                    rTable[i, j] = -1;
                }
            }
            for (int i = 0; i < karesayisi * karesayisi; i++)
            {
                nx = kordinatBul(i).Item1;
                ny = kordinatBul(i).Item2;
                for (int j = 0; j < karesayisi * karesayisi; j++)
                {
                    if (i != j)
                    {
                        btn = butonBul(nx.ToString() + "," + ny.ToString());
                        if (btn.BackColor == Color.White || btn.BackColor == Color.LightSkyBlue || btn.BackColor == Color.GreenYellow)
                        {
                            x = kordinatBul(j).Item1;
                            y = kordinatBul(j).Item2;
                            btn = butonBul(x.ToString() + "," + y.ToString());
                            if (btn.BackColor != Color.OrangeRed)
                            {
                                if (Math.Abs(nx - x) <= 1 && Math.Abs(ny - y) <= 1)
                                {
                                    for (int a = x - 1; a < x + 1; a++)
                                    {
                                        for (int b = y - 1; b < y + 1; b++)
                                        {
                                            btn = butonBul(a.ToString() + "," + b.ToString());
                                            if (btn != null)
                                            {
                                                if (btn.BackColor != Color.OrangeRed)
                                                {
                                                    if (btn.BackColor == Color.GreenYellow)
                                                    {
                                                        rTable[i, j] = 100;
                                                    }
                                                    else
                                                    {
                                                        rTable[i, j] = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rTable;
        }
        private void btnEnable(bool v)
        {
            Button btn = new Button();
            for (int i = 0; i < karesayisi; i++)
            {
                for (int j = 0; j < karesayisi; j++)
                {
                    btn = butonBul(i.ToString() + "," + j.ToString());
                    btn.Enabled = v;
                }
            }
        }
    }
}
