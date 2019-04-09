using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HangerMan
{
    public partial class Form1 : Form
    {
        private int time_counter = 30;
        private System.Timers.Timer couter_timer;
        private int comp_results = 0; //Zmienna która okresla czy gracz przegrał/wygrał/wciąz gra
        private game g1; //Obiekt klasy game, czyli klasy która zawiera wszystkie informacje potrzebne graczowi
        private game_db db1; //Obiekt klasy game_db, połączenie z bazą danych.
        private Bitmap[] img;
        private int mode=0;
        private int img_counter = 0;

        public Form1()
        {
            img = new Bitmap[6];
            img[0] = Properties.Resources.Hang_0;
            img[1] = Properties.Resources.Hang_1;
            img[2] = Properties.Resources.Hang_2;
            img[3] = Properties.Resources.Hang_3;
            img[4] = Properties.Resources.Hang_4;
            img[5] = Properties.Resources.Hang_5;
            db1 = new game_db("cars");
            InitializeComponent();
        }

        private void count_the_time()
        {
            time_counter = 30;
            couter_timer = new System.Timers.Timer();
            couter_timer.Interval = 1000;
            couter_timer.Elapsed += count_tt;
            couter_timer.Start();
        }

        private void count_tt(object info, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(()=>
            {
                if(time_counter<=0)
                {
                    g1.if_time_is_out(2);
                    label3.Visible = false;
                    label1.Text = g1.return_quest();
                    label2.Text = " Zabrakło czasu!!!";
                }
                else
                {
                    time_counter--;
                    label4.Text = "Czas: " + time_counter.ToString();
                }
            }));
        }

        private void hide_show_controls(bool one, bool two)
        {
            label1.Visible = one;
            label2.Visible = one;
            label3.Visible = one;
            panel1.Visible = one;
            panel2.Visible = one;
            button27.Visible = one;
            button39.Visible = one;
            panel3.Visible = two;
        }

        private void hide_img()
        {
            pictureBox1.Image = null;
            img_counter = 0;
        }

        private void start_game()//Funkcja która rozpoczyna grę, tworzy nowy obiekt oraz pokazuje ukryte kontrolki
        {
            db1.rand_nmb();
            //Połączenie z bazą danych          
            g1 = new game(db1.return_string("question"), 6, db1.return_string("tip"));
            g1.set_mode(mode); //ustawienie trybu gry
                               //utworzenie obiektu gry. zwracane sa tutaj pytanie oraz podpowiedz, w celu utworzenia obiektu
                               //Question, lives, Tip, ID
            label1.Text = g1.return_hidden_quest();
            label2.Text = "Chances: " + g1.return_lives().ToString();
            label3.Text = "Tip: " + g1.return_tip();
            hide_show_controls(true, false); //ukryj kontrolki menu, pokaz kontrolki rozgrywki
            comp_results = 0;
            foreach(Control ctrl in panel1.Controls)//Pętla pokazująca wszystkie buttony które zostały zakryte podczas rozgrywki.
            {
                if (ctrl is Button)
                {
                    ctrl.Visible = true;
                }
            }
            foreach (Control ctrl in panel2.Controls)//Pętla pokazująca reszte buttonow które zostały zakryte podczas rozgrywki.
            {
                if (ctrl is Button)
                {
                    ctrl.Visible = true;
                }
            }
        }

        private void button_Click(object sender, EventArgs e) //Jedna funkcja do obsługi wszystkich buttonów od a do z
        {
            comp_results = g1.compare_results();
            if (comp_results!=1 && comp_results != 2)
            {
                if(g1.check_entered_char((sender as Button).Text)==0)
                {
                    pictureBox1.Image = img[img_counter];
                    img_counter++;
                } //Sprawdzenie czy wybrany znak zgadza się z którymkolwiek w stringu oraz rysowanie wisielca na ekranie
                label1.Text = g1.return_hidden_quest(); //Odświeżenie ukrytego stringa
                (sender as Button).Visible = false; //Ukrycie wybranego buttona w celu eliminacji ponownego klikania w użytą kontrolkę
                label2.Text = "Chances: " + g1.return_lives().ToString(); //Wypisanie ilości żyć
                comp_results = g1.compare_results(); //Sprawdzenie, czy gracz wygrał/przegrał/wciąz gra
                if (comp_results==1)
                {
                    label3.Visible = false;
                    label2.Text = " Gratulacje, Wygrałeś!";
                    label4.Visible = false;
                }
                else if (comp_results == 2)
                {
                    label3.Visible = false;
                    label1.Text = g1.return_quest();
                    label2.Text = " Niestety przegrałeś!!";
                    label4.Visible = false;
                }
            }
        }
        //Restart
        private void button27_Click(object sender, EventArgs e)
        {
            g1 = null;//Zniszczenie obiektu w celu zagrania od nowa
            start_game(); // wywolanie funkcji tworzacej rozgrywke
            hide_img();
            if (g1.ret_mode() == 1) //Przy restarcie, counter musi zostac zresetowany, dlatego trzeba go najpierw zatrzymac.
            {
                label4.Visible = true;
                couter_timer.Stop();
                couter_timer = null;
                count_the_time();
                label4.Text = "Czas: " + time_counter.ToString();
            }
        }
        private void button28_Click(object sender, EventArgs e)//END
        {
            g1 = null;
            this.Close();
        } 

        //Funkcja pokazująca menu
        private void button39_Click(object sender, EventArgs e)
        {
            if (g1.ret_mode() == 1) // Przy wyjsciu do menu, counter musi zostac zatrzymany oraz label z napisem czas musi zostac ukryty
            {
                couter_timer.Stop();
                label4.Visible = false;
            }
            g1 = null;
            hide_show_controls(false, true); //pokaz kontrolki menu, ukryj kontrolki rozgrywki
            hide_img();
        }
        //Funkcja rozpoczynająca grę
        private void button41_Click(object sender, EventArgs e)
        {
            start_game();
            if(g1.ret_mode()==1) //Przy rozpoczynaniu nowej gry, Musimy stworzyć obiekt klasy Timer, pokazać label4, oraz wpisac do niego odpowiedni ciag znakow
            {
                count_the_time();
                label4.Visible = true;
                label4.Text = "Czas: " + time_counter.ToString();
            }
        }

        //Wybranie nowej kategorii
        private void button40_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            hide_show_controls(false, false);
            comboBox1.Items.Clear();
            for(int i=0; i<db1.return_nmb_of_tables(); i++)
            {
                comboBox1.Items.Add(db1.return_tablename_by_index(i));
            }
        }

        //Kod zostanie wykonany po zmianie indexu ComboBoxa
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            hide_show_controls(false, true);
            db1.change_category(comboBox1.SelectedItem.ToString()); //Wywloanie funkcji w celu zmiany kategorii
            db1.restart_id_tab(); //zresetowanie tablicy przechowujacaej tymczasowe pytania
        }

        //Funkcja odpowiadająca za zmianę trybów gry.
        private void button50_Click(object sender, EventArgs e)
        {
            hide_show_controls(false, false);
            panel4.Visible = true;
        }

        //Tryb normalny
        private void button51_Click(object sender, EventArgs e)
        {
            mode = 0;
            hide_show_controls(false, true);
            panel4.Visible = false;
        }

        //Tryb na czas
        private void button52_Click(object sender, EventArgs e)
        {
            mode = 1;
            hide_show_controls(false, true);
            panel4.Visible = false;
        }

        //Tryb Online
        private void button53_Click(object sender, EventArgs e)
        {
            hide_show_controls(false, true);
            panel4.Visible = false;
        }
    }
}
