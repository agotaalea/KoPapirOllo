using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoPapirOllo
{
    internal class Statisztika
    {
        int szerencsetlenEmberekSzama;
        int mutatasokSzama;
        double korok;

        public double Korok
        {
            get { return korok; }
            set { korok = value; }
        }

        public int SzerencsetlenEmberek
        {
            get { return szerencsetlenEmberekSzama; }
            set { szerencsetlenEmberekSzama = value; }
        }

        public int MutatasokSzama
        {
            get { return mutatasokSzama; }
            set { mutatasokSzama = value;}
        }

        public int EletkorSzamlalas(int eletkor, Jatekos[] versenyzok)
        {
            int db = 0;
            for (int i = 0; i < versenyzok.Length; i++)
            {
                if (versenyzok[i] != null && versenyzok[i].Eletkor == eletkor)
                {
                    db++;
                }
            }

            return db;
        }
    }
}
