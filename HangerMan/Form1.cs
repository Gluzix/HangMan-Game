﻿using System;
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
        private int comp_results = 0; //Zmienna która okresla czy gracz przegrał/wygrał/wciąz gra
        private game g1; //Obiekt klasy game, czyli klasy która zawiera wszystkie informacje potrzebne graczowi
        private game_db db1; //Obiekt klasy game_db, połączenie z bazą danych.
        public Form1()
        {
            db1 = new game_db("geography");
            InitializeComponent();
            start_game();
        }

        private void start_game()//Funkcja która rozpoczyna grę, tworzy nowy obiekt oraz pokazuje ukryte kontrolki
        {
            db1.rand_nmb();
            //Połączenie z bazą danych          
            g1 = new game(db1.return_string("question"), 6, db1.return_string("tip"));
            //utworzenie obiektu gry. zwracane sa tutaj pytanie oraz podpowiedz, w celu utworzenia obiektu
            //Question, lives, Tip, ID
            label1.Text = g1.return_hidden_quest();
            label2.Text = "Chances: " + g1.return_lives().ToString();
            label3.Text = "Tip: " + g1.return_tip();
            label3.Visible = true;
            comp_results = 0;
            foreach(Control ctrl in panel1.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.Visible = true;
                }
            }
        }

        private void button_Click(object sender, EventArgs e) //Jedna funkcja do obsługi wszystkich buttonów od a do z
        {
            if (comp_results!=1 && comp_results != 2)
            {
                g1.check_entered_char((sender as Button).Text); //Sprawdzenie czy wybrany znak zgadza się z którymkolwiek w stringu
                label1.Text = g1.return_hidden_quest(); //Odświeżenie ukrytego stringa
                (sender as Button).Visible = false; //Ukrycie wybranego buttona w celu eliminacji ponownego klikania w użytą kontrolkę
                label2.Text = "Chances: " + g1.return_lives().ToString(); //Wypisanie ilości żyć
                comp_results = g1.compare_results(); //Sprawdzenie, czy gracz wygrał/przegrał/wciąz gra
                if (comp_results==1)
                {
                    label3.Visible = false;
                    label2.Text = " Gratulacje, Wygrałeś!\n Pozostalo Ci: " + g1.return_lives().ToString() + " Szans!";
                }
                else if (comp_results == 2)
                {
                    label3.Visible = false;
                    label1.Text = g1.return_quest();
                    label2.Text = " Niestety przegrałeś!!";
                }
            }
        }
        private void button27_Click(object sender, EventArgs e)
        {
            g1 = null;//Zniszczenie obiektu w celu zagrania od nowa
            start_game(); // wywolanie funkcji tworzacej rozgrywke
        }
        private void button28_Click(object sender, EventArgs e)
        {
            g1 = null;
            this.Close();
        }
    }
    
}
