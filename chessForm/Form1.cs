using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chessForm
{
    public partial class formChess : Form
    {
        static int[,] tahta = new int[DefineConstants.kareSayisi, DefineConstants.kareSayisi]; //satranc tahtasi 8*8lik
        static int[] alt_x = { 0, 0, 0, 0, 0, 0, 0, 0 }; //gidilebilecek alternatif karelerin X konumlarini tutuyor
        static int[] alt_y = { 0, 0, 0, 0, 0, 0, 0, 0 }; //gidilebilecek alternatif karelerin Y konumlarini tutuyor
        static int[] alt_cikis = { 0, 0, 0, 0, 0, 0, 0, 0 }; //alternatiflerden cikislarin sayilarini tutuyor
        static int[] fark_x = { -2, -1, 1, 2, 2, 1, -1, -2 }; //saat yonunde olusan farklar
        static int[] fark_y = { 1, 2, 2, 1, -1, -2, -2, -1 };
        static Label[,] _arrLbl = new Label[8, 8];//satranc tahtasi icin kullanilan label dizisi 8*8lik
        static int i, j, sayac = 1, g, t = 1, saniye = 3;
        static int xyer, yyer, x, y, hareket, say = 0;


        public formChess()
        {
            InitializeComponent();
        }


        private void btnBaslat_Click(object sender, EventArgs e)
        {
            if (say == 0)
            {
                if (txtlocationX.Text == string.Empty || txtLocationY.Text == string.Empty)

                {
                    MessageBox.Show("Konum Boş olamaz");
                    say--;
                }
                else
                {
                    xyer = Convert.ToInt32(txtlocationX.Text);
                    xyer = xyer - 1;
                    yyer = Convert.ToInt32(txtLocationY.Text);
                    yyer = yyer - 1;
                    if (xyer < 0 || xyer > DefineConstants.kareSayisi - 1 || yyer < 0 || yyer > DefineConstants.kareSayisi - 1)
                    {
                        MessageBox.Show("Lütfen 1 ile 8 arasında tam sayı giriniz...");
                        say--;
                    }
                    else
                    {
                        x = xyer;
                        y = yyer;

                        while ((tahta[xyer, yyer]) == 0) //dongu,gidilecek kare ziyaret edilmedigi surece devam eder
                        {
                            tahta[xyer, yyer] = sayac++;
                            alternatif_bul(xyer, yyer);
                            alt_say_hesapla();
                            g = yk_bul(); //g degiskeni yk_bul fonksiyonundan donen indisi tutuyor
                            if (g != -1)
                            {
                                xyer = alt_x[g];
                                yyer = alt_y[g];
                            }


                            for (i = 0; i < 8; i++) //her adimi ekrana yazdir.
                            {
                                for (j = 0; j < 8; j++)
                                {
                                    _arrLbl[i, j].Text = tahta[i, j].ToString();
                                    lblAdim.Text += tahta[i, j].ToString() + " ";

                                }
                                lblAdim.Text += "\n";
                            }
                            lblAdim.Text += "\n\n";

                            alt_dizi_sifirla();

                        }
                        hareket = sayac - 1;
                        for (i = 0; i < 8; i++) //eger g nin degeri -1 ise ve hala cikis var ise(at sondan bir onceki hamlede ise)
                        {
                            if (xyer + fark_x[i] > -1 && xyer + fark_x[i] < DefineConstants.kareSayisi && yyer + fark_y[i] > -1 && yyer + fark_y[i] < DefineConstants.kareSayisi && tahta[(xyer + fark_x[i]), (yyer + fark_y[i])] == 0)
                            {
                                tahta[(xyer + fark_x[i]), (yyer + fark_y[i])] = sayac; //sayacin son degerini son kareye ata
                                hareket++;
                            }
                        }

                        for (i = 0; i < 8; i++) //son durumu ekrana yaz.
                        {
                            for (j = 0; j < 8; j++)
                            {
                                _arrLbl[i, j].Text = tahta[i, j].ToString();
                                lblAdim.Text += tahta[i, j].ToString() + " ";
                            }
                            lblAdim.Text += "\n";
                        }
                        lblAdim.Text += ("\n\n");

                    }
                }
            }
            else
            {
                Application.Restart();
                 
            }
            say++;
            
        }






        //algoritmalar
        public static void alternatif_bul(int x, int y)
        { //atin bulundugu kareden gidebilecegi konumlari bulur,X konumlarini alt_x dizisine,Y konumlarini alt_y dizisine atar.
            int i;
            for (i = 0; i < 8; i++)
            {
                alt_x[i] = x + fark_x[i];
                alt_y[i] = y + fark_y[i];
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            tahtaGez();
        }

        public static void alt_say_hesapla()
        { //alt_say_hesapla fonksiyonu alternatif karelerden gidilebilecek karelerin sayilarini (kareler daha once gezilmemis ve matris icinde //ise)alt_cikis dizisine atar.
            int tmp_x;
            int tmp_y;
            for (i = 0; i < 8; i++)
            {
                if (alt_x[i] > -1 && alt_y[i] > -1 && alt_x[i] < DefineConstants.kareSayisi && alt_y[i] < DefineConstants.kareSayisi && (tahta[alt_x[i], alt_y[i]]) == 0) //alternatif cikis gezilmemis ve
                {
                    for (j = 0; j < 8; j++) // tahta icinde mi?
                    {
                        tmp_x = alt_x[i] + fark_x[j];
                        tmp_y = alt_y[i] + fark_y[j];
                        if (tmp_x > -1 && tmp_y > -1 && tmp_x < DefineConstants.kareSayisi && tmp_y < DefineConstants.kareSayisi && (tahta[tmp_x, tmp_y]) == 0) // alternatif cikis
                        {
                            alt_cikis[i]++; //gezilmemis ve tahta icinde ise cikisa ait alternatif sayisini        1 arttir
                        }
                    }
                }
            }
        }

        public static int yk_bul()
        { //alt_cikis dizisinin 0 haric en kucuk elemaninin indisini dondurur.
            int sayi = 9;
            int ind = -1;
            for (i = 0; i < 8; i++)
            {
                if (alt_cikis[i] != 0) //cikis 0 ise donguden cik
                {
                    if (sayi > alt_cikis[i])
                    { //cikis 9 dan kucuk ise
                        sayi = alt_cikis[i]; //sayiyi cikis sayisina esitle
                        ind = i; //ind degiskenine indisi ata
                    }
                }
            }
            return ind; //indisi dondur
        }

        public static void alt_dizi_sifirla()
        {
           
            for (i = 0; i < 8; i++)
            {
                alt_cikis[i] = 0;
            }
        }
        //kontroller
        private void txtlocationX_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtLocationY_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //form acilmasi
        private void formChess_Load(object sender, EventArgs e)
        {
            tahtaOlustur();
            timer1.Start();
        }

        //label kullanarak ile satranc tahtası
        public void tahtaOlustur()
        {
            int  yer = 0;
            for (i = 0; i < 8; i++)
            {

                for (j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {

                        if (yer == 1)
                        {
                            _arrLbl[i, j] = new Label();
                            _arrLbl[i, j].BackColor = Color.Black;
                            _arrLbl[i, j].ForeColor = Color.Black;
                            _arrLbl[i, j].Size = new Size(50, 50);
                            _arrLbl[i, j].TextAlign = ContentAlignment.MiddleCenter;
                            _arrLbl[i, j].Location = new Point(j * 50, i * 50);
                            flowLayoutPanel1.Controls.Add(_arrLbl[i, j]);

                            yer = 0;
                        }

                        else
                        {
                            yer++;
                            _arrLbl[i, j] = new Label();
                            _arrLbl[i, j].Size = new Size(50, 50);
                            _arrLbl[i, j].BackColor = Color.White;
                            _arrLbl[i, j].ForeColor = Color.White;
                            _arrLbl[i, j].TextAlign = ContentAlignment.MiddleCenter;
                            _arrLbl[i, j].Location = new Point(j * 50, i * 50);
                            flowLayoutPanel1.Controls.Add(_arrLbl[i, j]);

                        }


                    }
                    else

                    {
                        if (yer == 0)

                        {

                            _arrLbl[i, j] = new Label();
                            _arrLbl[i, j].BackColor = Color.Black;
                            _arrLbl[i, j].TextAlign = ContentAlignment.MiddleCenter;
                            _arrLbl[i, j].ForeColor = Color.Black;
                            _arrLbl[i, j].Size = new Size(50, 50);
                            _arrLbl[i, j].Location = new Point(j * 50, i * 50);
                            flowLayoutPanel1.Controls.Add(_arrLbl[i, j]);
                            yer++;

                        }
                        else
                        {
                            yer = 0;
                            _arrLbl[i, j] = new Label();
                            _arrLbl[i, j].Size = new Size(50, 50);
                            _arrLbl[i, j].BackColor = Color.White;
                            _arrLbl[i, j].TextAlign = ContentAlignment.MiddleCenter;
                            _arrLbl[i, j].ForeColor = Color.White;
                            _arrLbl[i, j].Location = new Point(j * 50, i * 50);
                            flowLayoutPanel1.Controls.Add(_arrLbl[i, j]);
                        }

                    }
                }
            }
        }

        //tahta üzerinde gezme kodu
        public void tahtaGez()
        {

            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (t >= 1 && t <= 64)
                    {
                        if (_arrLbl[i, j].Text == t.ToString())
                        {

                            _arrLbl[i, j].Image = Image.FromFile(Application.StartupPath + @"\horse.png");

                            saniye--;
                            if (saniye == 0)
                            {

                                _arrLbl[i, j].Image = null;
                                saniye = 3;
                                t++;
                            }

                        }
                    }

                }
            }
        }

        static class DefineConstants
        {
            public const int kareSayisi = 8;
        };

    }
}
