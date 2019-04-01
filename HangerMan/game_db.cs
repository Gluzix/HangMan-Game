using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace HangerMan
{
    class game_db
    {
        private int[] id_tab;
        private int tab_iterator;
        private int nmb_of_rows;
        private int nmb_of_tables;
        private string sql;
        private int id;
        private string category;
        private SQLiteConnection m_dbConn;
        private SQLiteCommand command;
        private SQLiteDataReader reader;
        private Random rand;

        //Konstruktor klasy dbającej o połączenie z bazą danych
        public game_db(string cat)
        {
            category = cat;
            nmb_of_rows = 0;//liczba rekordów w danej tabeli
            nmb_of_tables = 0;
            id_tab =  new int[10]; //Tablica przechowująca 5 ostatnich pytań, w celu eliminacji częstych powtórzeń
            tab_iterator = 0; //Iterator do w.w. tablicy
            for (int i = 0; i < id_tab.Length; i++)
            {
                id_tab[i] = -1; //Uzupełniane tablicy początkowym stanem
            }
            m_dbConn = new SQLiteConnection("Data Source=HangerManDB.db;Version=3");//Ścieżka do bazy danych
            m_dbConn.Open(); //Otwarcie bazy danych
            sql = "select * from " + category; //Wpisanie polecenia do stringa
            command = new SQLiteCommand(sql, m_dbConn); //Utworzenie polecenia dla bazy danych
            reader = command.ExecuteReader();//Wykonanie polecenia
            while (reader.Read())
            {
                nmb_of_rows+=1;
            } //Pętla zliczająca ilośc rekordów w tabeli
            command = null; //Wyzerowanie zmiennej przechowującej polecenie dla bazy danych
            reader = null; // Wyzerowanie zmiennej przechowującej wykonywane polecenie
            sql = null; //WYzerowanie stringa który zawiera polecenie       
            sql = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name"; //Polecenie do odczytania tabel w bazie danych
            command = new SQLiteCommand(sql, m_dbConn); 
            reader = command.ExecuteReader(); 
            while (reader.Read())
            {
                nmb_of_tables++; //Zliczanie ilości tabel
            }
        }
        ~game_db()
        {
            m_dbConn.Close(); //Destruktor, GC wywołuje zamknięcie połaczenia z baza danych w momencie niszczenia obiektu
        }
        public void rand_nmb()
        {
            rand = new Random(); //Obiekt liczb pseudolosowych
            bool if_was = true; //Pomocnicza zmienna typu bool w celu sprawdzenia, czy zadanie które zostało wylosowane, nie zostało zagrane chwilę wcześniej
            while(if_was)
            {
                id = rand.Next(0, nmb_of_rows); //Losowanie id
                for(int i=0; i<5; i++)
                {
                    if(id_tab[i]==id)
                    {
                        if_was = true;
                        break;
                    }
                    else
                    {
                        if_was = false;
                    }
                }
            }
            id_tab[tab_iterator] = id; //przypisanie wylosowanego zadania do tablicy.
            if (tab_iterator > 3)
            {
                tab_iterator = 0;
            }
            else tab_iterator++;
            rand = null; //wyzerowanie randa
        }
        public string return_string(string what_return)
        { //Funkcja zwracająca z bazy danych dany string.
            sql = "select * from "+category+" where id=" + id; //utworzenie zapytania
            command = new SQLiteCommand(sql, m_dbConn); //utworzenie polecenia wykonania dla SQL
            reader = command.ExecuteReader(); //wykonanie polecenia
            reader.Read(); //odczytanie wartosci z db 
            return (string)reader[what_return]; //zwrocenie wartosci
        }
        public void change_category(string cat)
        {
            sql = null;
            command = null;
            reader = null;
            nmb_of_rows = 0;
            category = cat; //przypisanie nowej wartosci do zmiennej kategoria
            sql = "select * from " + category; //Wpisanie polecenia do stringa
            command = new SQLiteCommand(sql, m_dbConn); //Utworzenie polecenia dla bazy danych
            reader = command.ExecuteReader();//Wykonanie polecenia
            while (reader.Read())
            {
                nmb_of_rows += 1;
            } //Pętla zliczająca ilośc rekordów w tabeli
        }
        public void restart_id_tab() //zresetowanie tablicy pamietanych pytan
        {
            for (int i = 0; i < id_tab.Length; i++)
            {
                id_tab[i] = -1;
            }
        }
        public string return_tablename_by_index(int index) //zwrocenie nazwy tablicy po indeksie
        {
            int i = 0; //zmienna pomocnicza
            command = null; 
            reader = null;
            sql = null;//wyzerowanie zmiennych SQLite
            sql = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name"; //WAŻNE: Polecenie dzięki któremu możemy otrzymać nazwy tabel w bazie danych
            command = new SQLiteCommand(sql, m_dbConn); 
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                if(i==index) //jeżeli index jest równy i to zwracamy nazwę tabeli
                {
                    return reader.GetString(0); //Tutaj jest zwracana nazwa tabeli
                }
                else
                {
                    i++;
                }
            }
            return "";
        }
        public int return_nmb_of_tables()
        {
            return nmb_of_tables; //Zwrot ilości tabel w bazie danych w celu wpisywania do comboBoxa nazw tabel w Form1
        }
    }
}
