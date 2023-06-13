using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoPapirOllo
{
    internal class Jatekos
    {
        static Random rnd = new Random();

        string id;
        string nev;
        int eletkor;
        string kategoria;
        int csoport;
        double ko;
        double papir;
        double ollo;
        int fordulo;
        bool jatekbanVan;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nev
        {
            get { return nev; }
            set { nev = value; }
        }

        public int Eletkor
        {
            get { return eletkor; }
            set { eletkor = value; }
        }
        public string Kategoria
        {
            get { return kategoria; }
            set { kategoria = value; }
        }

        public bool JatekbanVan
        {
            get { return jatekbanVan; }
            set { jatekbanVan = value; }
        }

        public double Ollo
        {
            get { return ollo; }
            set { ollo = value; }
        }

        public double Ko
        {
            get { return ko; }
            set { ko = value; }
        }

        public double Papir
        {
            get { return papir; }
            set { papir = value; }
        }

        public int Csoport
        {
            get { return csoport; }
            set { csoport = value; }
        }

        public int Fordulo  //nehogy többször harcoljon egy fordulóban
        {
            get { return fordulo; }
            set { fordulo = value; }
        }

        public string Mutat()
        {
            int valasztas = rnd.Next(1, 101);

            //ha papír a legkisebb
            if (this.Papir <= this.Ko && this.Papir <= this.Ollo)
            {
                if (valasztas <= this.Papir)
                {
                    return "papír";
                }

                //ez a második legkisebb?
                if (this.Ko <= this.Ollo)
                {
                    if (valasztas <= this.Ko)
                    {
                        return "kő";
                    }

                    return "olló";
                }
                else
                {
                    if (valasztas <= this.Ollo)
                    {
                        return "olló";
                    }

                    return "kő";
                }
            }
            if (this.Ko <= this.Papir && this.Ko <= this.Ollo)
            {
                if (valasztas <= this.Ko)
                {
                    return "kő";
                }

                if (this.Papir <= this.Ollo)
                {
                    if (valasztas <= this.Papir)
                    {
                        return "papír";
                    }

                    return "olló";
                }
                else
                {
                    if (valasztas <= this.Ollo)
                    {
                        return "olló";
                    }

                    return "papír";
                }
            }
            if (this.Ollo <= this.Ko && this.Ollo <= this.Papir)
            {
                if (valasztas <= this.Ollo)
                {
                    return "olló";
                }

                if (this.Ko <= this.Papir)
                {
                    if (valasztas <= this.Ko)
                    {
                        return "kő";
                    }

                    return "papír";
                }
                else
                {
                    if (valasztas <= this.Papir)
                    {
                        return "papír";
                    }

                    return "kő";
                }
            }

            return "hiba";
        }
    }
}
