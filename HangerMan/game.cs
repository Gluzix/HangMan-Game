using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Klasa zawiera praktycznie wszystkie elementy potrzebne
 * do poprawnej rozgrywki.*/

namespace HangerMan
{
    class game
    {
        private int length_of_question;
        private string question;
        private char[] hidden_question;
        private int lives;
        private string tip;
        private int if_win;
        private int mode;
        public game(string str, int liv, string tp) 
        {
            mode = 0; //0 normalny tryb, 1 na czas
            if_win = 0; 
            question = str; 
            lives = liv;
            tip = tp;
            length_of_question = str.Length;
            hidden_question = new char[length_of_question];
            for (int i=0; i<length_of_question; i++)
            {
                hidden_question[i] = '-';
                if(question[i]==' ') hidden_question[i] = ' ';
            }
        }
        ~game(){}
        public int check_entered_char(string str)
        {
            char znak = str[0];
            int shooted = 0;
            for(int i=0; i < length_of_question; i++)
            {
                if(question[i]==znak)
                {
                    hidden_question[i] = znak;
                    shooted = 1;
                }
                else if(question[i]==Char.ToUpper(znak))
                {
                    hidden_question[i] = Char.ToUpper(znak);
                    shooted = 1;
                }
            }
            if(shooted==0)
            {
                lives--;
                return 0;
            }
            return 1;
        }
        public string return_hidden_quest()
        {
            string chain = new string(hidden_question);
            return chain;
        }
        public string return_quest()
        {
            return question;
        }
        public int return_lives()
        {
            return lives;
        }
        public string return_tip()
        {
            return tip;
        }
        public int compare_results()
        {
            string hidden_question_str = new string(hidden_question);
            if(hidden_question_str == question)
            {
                if_win = 1; //1 oznacza wygrana
            }
            else if(lives<=0)
            {
                if_win = 2; //2 oznacza koniec szans
            }
            return if_win;
        }
        public int ret_mode()
        {
            return mode;
        }
        public void set_mode(int x)
        {
            if(x==0 || x==1)
            {
                mode = x;
            }
        }
        public void if_time_is_out(int x)
        {
            if_win = x;
        }
    };
}
