using KoPapirOllo;
using System;
using System.IO;

namespace teszt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hány fős legyen egy csapat?");
            int N = int.Parse(Console.ReadLine());
            while (!KettohatvanyE(N))
            {
                Console.WriteLine("Kettő hatványt adj meg csapatlétszámnak please");
                N = int.Parse(Console.ReadLine());
            }
            Statisztika statisztika = new Statisztika();
            string[,] besorolas = Besorolas("kategoriak.txt", "jatekosok.txt");
            int[] letszamok = JatekosSzamaKategoriankent(besorolas);
            Jatekos[] jatekosok = CsapatBesorolas(statisztika, "kategoriak.txt", "jatekosok.txt", besorolas, "taktikak.txt", N);
            int kategoriakSzama = besorolas.GetLength(0);
            string kimenet = JatekLebonyolitasa(statisztika, jatekosok, kategoriakSzama, N, letszamok);
            kimenet += "\n";

            for (int i = 0; i < 120; i++)
            {
                int darab = statisztika.EletkorSzamlalas(i, jatekosok);
                if (darab != 0)
                {
                    kimenet += "\nEnnyi ember versenyzett " + i + " évesen: " + statisztika.EletkorSzamlalas(i, jatekosok);
                }
            }

            kimenet += "\n";
            kimenet += "Nem kerültek be egy csoportba sem: " + statisztika.SzerencsetlenEmberek;
            kimenet += "\n";
            kimenet += "Átlagos mutatás: " + statisztika.Korok / statisztika.MutatasokSzama;
            Console.WriteLine(kimenet);
            FajlKiiras(kimenet);
        }

        static void FajlKiiras(string kimenet)
        {
            StreamWriter sw = new StreamWriter("kimenet.txt", false);
            sw.Write(kimenet);
            sw.Close();
        }

        static string JatekLebonyolitasa(Statisztika statisztika, Jatekos[] versenyzok, int kategoriakSzama, int csoportLetszam, int[] letszamokKategoriankent)
        {
            string fajlKiirashoz = "";
            int szorzo = 0;
            int kezdoJatekosACsoportban = 0;
            // ennyi kategória
            //egy kategória csoportjaiben győztes
            for (int i = 0; i < kategoriakSzama; i++)
            {
                string kategoria = versenyzok[kezdoJatekosACsoportban].Kategoria;

                // ennyi csoport egy kategóriában
                for (int j = 0; j < letszamokKategoriankent[i] / csoportLetszam; j++)
                {
                    int csoport = versenyzok[kezdoJatekosACsoportban].Csoport;
                    fajlKiirashoz += "##########################";
                    fajlKiirashoz += "\n";
                    fajlKiirashoz += "Kategoria: " + kategoria;
                    fajlKiirashoz += "\n";
                    fajlKiirashoz += "Csoport: " + csoport.ToString();
                    fajlKiirashoz += "\n";
                    fajlKiirashoz += "##########################";

                    // ennyi csata van egy csoportban: csoportLetszam - 1
                    // ennyiszer kell végigmennie egy csoport minden játékosán
                    for (int k = 0; k < Kettohatvany(csoportLetszam); k++)
                    {
                        fajlKiirashoz += "\n";
                        fajlKiirashoz += $"Fordulo{k + 1}: [";
                        int kivalasztottIndex = csoportLetszam * szorzo;

                        while (!versenyzok[kivalasztottIndex].JatekbanVan)
                        {
                            kivalasztottIndex++;
                        }

                        int ellenfelIndex = kivalasztottIndex + 1;
                        int utolsoJatekosACsoportban = kezdoJatekosACsoportban + csoportLetszam - 1;

                        while (ellenfelIndex <= utolsoJatekosACsoportban)
                        {
                            if (versenyzok[kivalasztottIndex].JatekbanVan)
                            {
                                while (ellenfelIndex <= utolsoJatekosACsoportban && !versenyzok[ellenfelIndex].JatekbanVan)
                                {
                                    ellenfelIndex++;
                                }
                                if (ellenfelIndex <= utolsoJatekosACsoportban)
                                {
                                    fajlKiirashoz += $"{versenyzok[kivalasztottIndex].Id} - {versenyzok[ellenfelIndex].Id}, ";
                                    Csata(statisztika, versenyzok[kivalasztottIndex], versenyzok[ellenfelIndex]);
                                    statisztika.Korok++;
                                }
                            }

                            kivalasztottIndex += 1;
                            while (kivalasztottIndex <= utolsoJatekosACsoportban && (!versenyzok[kivalasztottIndex].JatekbanVan || (versenyzok[kivalasztottIndex].JatekbanVan && versenyzok[kivalasztottIndex].Fordulo != k)))
                            {
                                kivalasztottIndex++;
                            }

                            ellenfelIndex = kivalasztottIndex + 1;
                            while (ellenfelIndex <= utolsoJatekosACsoportban && !versenyzok[ellenfelIndex].JatekbanVan && versenyzok[ellenfelIndex].Fordulo != k)
                            {
                                ellenfelIndex++;
                            }
                        }

                        fajlKiirashoz = fajlKiirashoz.Remove(fajlKiirashoz.Length - 2);
                        fajlKiirashoz += "]";
                    }

                    int nyertesIndex = kezdoJatekosACsoportban;
                    while (!versenyzok[nyertesIndex].JatekbanVan)
                    {
                        nyertesIndex++;
                    }

                    fajlKiirashoz += "\n";
                    fajlKiirashoz += "Nyertes: " + versenyzok[nyertesIndex].Nev;
                    fajlKiirashoz += "\n";
                    szorzo += 1;
                    kezdoJatekosACsoportban = csoportLetszam * szorzo;
                }
            }

            return fajlKiirashoz;
        }

        static void Csata(Statisztika stat, Jatekos jatekos1, Jatekos jatekos2)
        {
            bool dontetlen = true;
            int kor = 0;
            while (dontetlen && kor < 3)
            {
                string mutatas1 = jatekos1.Mutat();
                string mutatas2 = jatekos2.Mutat();
                if ((mutatas1 == "kő" && mutatas2 == "kő")
                    || (mutatas1 == "papír" && mutatas2 == "papír")
                    || (mutatas1 == "olló" && mutatas2 == "olló"))
                {
                    kor++;
                }
                else if ((mutatas1 == "kő" && mutatas2 == "olló")
                    || (mutatas1 == "papír" && mutatas2 == "kő")
                    || (mutatas1 == "olló" && mutatas2 == "papír"))
                {
                    jatekos1.Fordulo += 1;
                    jatekos2.JatekbanVan = false;
                    jatekos2.Fordulo -= 1;
                    dontetlen = false;
                }
                else
                {
                    jatekos2.Fordulo += 1;
                    jatekos1.JatekbanVan = false;
                    jatekos1.Fordulo -= 1;
                    dontetlen = false;
                }
            }

            stat.MutatasokSzama += kor;
            if (kor == 3)
            {
                Random rnd = new Random();
                if (rnd.Next(1, 3) == 1)
                {
                    jatekos1.Fordulo += 1;
                    jatekos2.Fordulo -= 1;
                    jatekos2.JatekbanVan = false;
                }
                else
                {
                    jatekos2.Fordulo += 1;
                    jatekos1.Fordulo -= 1;
                    jatekos1.JatekbanVan = false;
                }
            }
        }

        static int[] JatekosSzamaKategoriankent(string[,] besorolas)
        {
            int[] letszamok = new int[besorolas.GetLength(0)];
            int db = 0;
            for (int i = 0; i < besorolas.GetLength(0); i++)
            {
                for (int j = 1; j < besorolas.GetLength(1); j++)
                {
                    if (besorolas[i, j] != "")
                    {
                        db++;
                    }
                }

                letszamok[i] = db;
                db = 0;
            }

            return letszamok;
        }

        static string[] Beolvas(string fileNev)
        {
            string[] sorok = File.ReadAllLines(fileNev);
            return sorok;
        }

        static string[,] Besorolas(string kategoriak, string jatekosok)
        {
            string[] Kategoriak = Beolvas(kategoriak);
            string[] Seged = Beolvas(jatekosok);
            string[] Jatekosok = new string[Seged.Length - 1];

            for (int i = 1; i < Seged.Length; i++)
            {
                Jatekosok[i-1] = Seged[i];
            }

            string[,] eredmeny = new string[Kategoriak.Length, Jatekosok.Length + 1];

            for (int i = 0; i < eredmeny.GetLength(0); i++)
            {
                for (int j = 0; j < eredmeny.GetLength(1); j++)
                {
                    eredmeny[i, j] = "";
                }
            }

            for (int i = 0; i < Kategoriak.Length; i++)
            {
                eredmeny[i, 0] = Kategoriak[i].Split('|')[0];
            }

            int db1 = 1;
            int db2 = 1;
            int db3 = 1;

            for (int i = 0; i < Jatekosok.Length; i++)
            {
                if (JatekosKora(Jatekosok[i]) <= Felsohatar(Kategoriak[0]) &&
                    JatekosKora(Jatekosok[i]) >= Alsohatar(Kategoriak[0]))
                {
                    eredmeny[0, db1] = JatekosID(Jatekosok[i]) + ',' + JatekosNeve(Jatekosok[i]) + ',' + JatekosKora(Jatekosok[i]);
                    db1++;
                }
                else if (JatekosKora(Jatekosok[i]) <= Felsohatar(Kategoriak[1]) &&
                    JatekosKora(Jatekosok[i]) >= Alsohatar(Kategoriak[1]))
                {
                    eredmeny[1, db2] = JatekosID(Jatekosok[i] + ',' + JatekosNeve(Jatekosok[i]) + ',' + JatekosKora(Jatekosok[i]));
                    db2++;
                }
                else
                {
                    eredmeny[2, db3] = JatekosID(Jatekosok[i] + ',' + JatekosNeve(Jatekosok[i]) + ',' + JatekosKora(Jatekosok[i]));
                    db3++;
                }
            }
            return eredmeny;
        }

        //szégyen metódusok :c
        static double JatekosKora(string jatekos)
        {
            double kor = double.Parse(jatekos.Split('|')[1]);
            return kor;
        }
        static double Alsohatar(string kategoria)
        {
            double alsohatar = double.Parse(kategoria.Split('|')[1].Split('-')[0]);
            return alsohatar;
        }
        static double Felsohatar(string kategoria)
        {
            double felsohatar = double.Parse(kategoria.Split('|')[1].Split('-')[1]);
            return felsohatar;
        }
        static string JatekosID(string jatekos)
        {
            string id = jatekos.Split('|')[2];
            return id;
        }
        static string JatekosNeve(string jatekos)
        {
            return jatekos.Split('|')[0];
        }
        static string KategoriaMax(string kategoria)
        {
            string kategmax = kategoria.Split('|')[1].Split('-')[1];
            return kategmax;
        }
        static string KategoriaMin(string kategoria)
        {
            string kategmin = kategoria.Split('|')[1].Split('-')[0];
            return kategmin;
        }
        static string TaktikaID(string taktika)
        {
            string id = taktika.Split('|')[0];
            return id;
        }
        static string Ko(string taktika)
        {
            return taktika.Split('|')[1].Split('-')[0];
        }
        static string Papir(string taktika)
        {
            return taktika.Split('|')[1].Split('-')[1];
        }
        static string Ollo(string taktika)
        {
            return taktika.Split('|')[1].Split('-')[2];
        }


        //segéd metódusok
        static bool KettohatvanyE(double N)
        {
            double seged = N;

            while (seged != 1 && seged > 1)
            {
                seged = seged / 2;
            }
            return seged == 1;
        }
        static int Kettohatvany(double N)
        {
            double seged = N;
            int db = 0;

            while (seged != 1 && seged > 1)
            {
                seged = seged / 2;
                db++;
            }
            return db;
        }
        static int HeadCount(string[,] eredmeny, int i)
        {
            int db = 0;
            for (int j = 0; j < eredmeny.GetLength(1); j++)
            {
                if (eredmeny[i,j] != "")
                {
                    db++;
                }
            }
            return db - 1; //kategória nevét levonjuk, első cella
        }

        //segéd vége
        static bool AtfedesEllenorzes(string kategoriak)
        {
            //kategórák között van átfedés?
            string[] Kategoriak = Beolvas(kategoriak);
            bool joeMinden = true;
            for (int i = 0; i < Kategoriak.Length; i++)
            {
                for (int j = 0; j < Kategoriak.Length; j++)
                {
                    if (i != j)
                    {
                        //kiválasztott kategória minimuma > másik maximumánál				
                        if ((int.Parse(KategoriaMin(Kategoriak[i])) > (int.Parse(KategoriaMax(Kategoriak[j])))))
                        {
                            // jó
                            joeMinden = true;
                        }
                        //kiválasztott kategória minimuma == másik maximumánál				
                        else if (Kategoriak[i] == Kategoriak[j])
                        {
                            //nem jó (átfedés van)
                            joeMinden = false;
                        }
                        //kiválasztott kategória minimuma < másik maximumánál				
                        else
                        {
                            //kiválasztott kategória maximuma < másik minimumámál
                            if ((int.Parse(KategoriaMax(Kategoriak[i])) < (int.Parse(KategoriaMin(Kategoriak[j])))))
                            {
                                // jó
                                joeMinden = true;
                            }
                            //kiválasztott kategória maximuma <= másik minimumámál
                            else if ((int.Parse(KategoriaMax(Kategoriak[i])) <= (int.Parse(KategoriaMin(Kategoriak[j])))))
                            {
                                // nem jó
                                joeMinden = false;
                            }
                        }
                    }
                }
            }

            return joeMinden;
        }
        
        static Jatekos[] CsapatBesorolas(Statisztika stat, string kategoriakTxtFajl, string jatekosokTxtFajl, string[,] besorolas, string taktikakTxtFajl, int N)
        {
            string[] Kategoriak = Beolvas(kategoriakTxtFajl);
            string[] Seged = Beolvas(jatekosokTxtFajl);
            string[] Jatekosok = new string[Seged.Length - 1];
            for (int i = 1; i < Seged.Length; i++)
            {
                Jatekosok[i - 1] = Seged[i];
            }

            Jatekos[] jatekosokTomb = new Jatekos[Jatekosok.Length];
            int jatekosIndex = 0;

            for (int i = 0; i < Kategoriak.Length; i++)
            {
                int Headcount = HeadCount(besorolas, i);

                //ők nem kerülnek sehova
                int Z = Headcount % N;
                stat.SzerencsetlenEmberek += Z;

                int csapatIndex = 0;
                for (int j = 1; j <= Headcount - Z; j++)
                {
                    if (j % N == 1)
                    {
                        csapatIndex++;
                    }

                    Jatekos ujJatekos = new Jatekos();
                    // id
                    ujJatekos.Id = besorolas[i, j].Split(',')[0];
                    // kategória neve
                    ujJatekos.Kategoria = besorolas[i, 0].Split('|')[0];
                    // csapat száma
                    ujJatekos.Csoport = csapatIndex;
                    // játékban van-e
                    ujJatekos.JatekbanVan = true;
                    // név
                    ujJatekos.Nev = besorolas[i, j].Split(',')[1];
                    ujJatekos.Fordulo = 0;
                    ujJatekos.Eletkor = int.Parse(besorolas[i, j].Split(',')[2]);


                    string[] Taktikak = Beolvas(taktikakTxtFajl);
                    int k = 0;
                    bool talalat = false;
                    while (k < Taktikak.Length && !talalat)
                    {
                        if (TaktikaID(Taktikak[k]) == ujJatekos.Id)
                        {
                            ujJatekos.Ko = double.Parse(Ko(Taktikak[k]));
                            ujJatekos.Papir = double.Parse(Papir(Taktikak[k]));
                            ujJatekos.Ollo = double.Parse(Ollo(Taktikak[k]));
                            talalat = true;
                        }

                        k++;
                    }

                    int l = 0;
                    while (JatekosID(Jatekosok[l]) != ujJatekos.Id)   
                    {
                        l++;
                    }

                    jatekosokTomb[jatekosIndex] = ujJatekos;
                    jatekosIndex++;
                }
            }

            return jatekosokTomb;
        }
    }
}