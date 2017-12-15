using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace V4_Jehlicka_Matej_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //proměnné
        private string fName; //privátní proměnná pro cestu k vybranému souboru
        private List<int> listx = new List<int>(); // pro ukládání všech x-ových souřadnic
        private List<int> listy = new List<int>(); // pro ukládání všech y-ových souřadnic
        private const int meritko = 20; // hodnoty jsou malé, proto je násobím dvaceti
        Point A; //krajní bod úsečky
        Point B; //krajní bod úsečky
        //

        //vymaže všechna data ze všech prvků formu a vyprázdní proměnné
        private void cls()
        {
            listx.Clear();
            listy.Clear();
            listBox1.Items.Clear();
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            //odtranění stávajících, starých dat z formu
            //
            cls();
            //
            
            //
            //openfile dialog pro vybrání souboru ke zpracování
            //
            OpenFileDialog openf = new OpenFileDialog();
            openf.Filter = "Text files (*.txt)|*.txt";
            openf.FilterIndex = 1;
            openf.Multiselect = false;

            if (openf.ShowDialog() == DialogResult.OK)
            {
                fName = openf.FileName;
            }
            //

            //
            //čtení textu z vybraného souboru
            //
            try //ošetření, poku ze souboru nelze číst žádná data
            {
                string[] readText = File.ReadAllLines(fName); //čtení dat suboru po jednotlivých řádcích
                foreach (string s in readText)
                {
                    string line = ""; //proměnná pro nahrání textu bez 
                    bool comment = false;
                    //odstraní komentovaný text z řetězce
                    foreach (char ch in s)
                    {
                        if (ch != '#' && comment == false)
                        {
                            line = line + ch;
                        }
                        else
                        {
                            if (ch == '#' && comment == true)
                            {
                                comment = false;
                            }
                            else
                            {
                                comment = true;
                            }
                        }
                    }
                    if (line != "") //pokud je celý řádek zakomentovaný, nevypíše se a nezpracovává
                    {

                        listBox1.Items.Add(line); //vypsání přečtených dat bez komentářů do listboxu
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Soubor nelze přečíst.");
                /*listBox1.Items.Add("Soubor nelze přečíst!");
                listBox1.Items.Add(ex.Message);*/
            }

            //
            //separování dat ze souboru jako souřadnice vhodné ke zpracování
            //
            body();
            //


            //panel1.Refresh();

        }

        private void body() //Výpočet souřadnic bodů úseček
        {
            //
            //Rozdělení na jednotlivé souřadnice
            //
            
            foreach(string s in listBox1.Items)
            {
                string[] usecka = new string[4]; //pro ukládání dat souřadnic právě zpracovávané úsečky
                int x1, x2, y1, y2;
                usecka = s.Split(';');
                try
                {
                    x1 = Convert.ToInt32(Convert.ToDouble(usecka[0]) * meritko); //x-ová souřadnice prvního bodu úsečky
                    x2 = Convert.ToInt32(Convert.ToDouble(usecka[2]) * meritko); //x-ová souřadnice druhého bodu úsečky
                    y1 = Convert.ToInt32(Convert.ToDouble(usecka[1]) * meritko); //y-ová souřadnice prvního bodu úsečky
                    y2 = Convert.ToInt32(Convert.ToDouble(usecka[3]) * meritko); //y-ová souřadnice druhého bodu úsečky


                    listx.Add(x1); //do listu nahraje x-ové souřadnice prvního bodu 
                    listx.Add(x2); //do listu nahraje x-ové souřadnice druhého bodu 
                    listy.Add(y1); //do listu nahraje y-ové souřadnice prvního bodu 
                    listy.Add(y2); //do listu nahraje y-ové souřadnice druhého bodu
                    

                    
                }
                catch
                {
                    MessageBox.Show("Zadali jste neplatnou hodnotu. Můžete zapsat jen desetinné číslo. Jako oddělovač desetinných míst použijte čárku (,), jako oddělovač souřadnic použijte středník (;). \n CHYBNÝ ZÁPIS --> " + s);
                    /*int index = listBox1.FindString(s);
                    if (index != -1) listBox1.SetSelected(index, true);
                    string msg = Convert.ToString(listBox1.Items[index]);
                    listBox1.Items[index] = "! " + msg;*/
                }
            }

            //
            panel1.Refresh();

        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            //int dx1, dx2, dy1, dy2; //proměnné pro hodnoty souřadnic krajích bodů úsečky k vykreslení

            int x1, x2, x3, x4, y1, y2, y3, y4;
            List<int> pr = new List<int>();
            Graphics dw = e.Graphics;

            Pen myPen = new Pen(Color.Black, 2); //pero pro kreslení úseček
            Pen osa = new Pen(Color.Gray, 1); //pero pro kreslení úseček
            Brush myBrush = new SolidBrush(Color.Red); //štětec pro kreslení (zvýraznění) průsečíků

            //
            //Vykreslení os a meřítka
            //
            dw.DrawLine(osa, 500, 0, 500, 600);
            dw.DrawLine(osa, 0, 300, 1000, 300);

            for (int i = 0; i <= 600 / meritko; i++)
            {              
                dw.DrawLine(osa, 490, i * meritko, 510, i * meritko);
            }
            for (int i = 0; i <= 1000 - meritko / meritko; i++)
            {
                dw.DrawLine(osa, i * meritko, 290, i * meritko, 310);
            }
            //

            for (int i = 0; i < listx.Count ; i = i + 2)
            {
                x1 = listx[i];
                x2 = listx[i + 1];
                y1 = listy[i];
                y2 = listy[i + 1];

                A = new Point(x1+500, y1+300);
                B = new Point(x2+500, y2+300);

                dw.DrawLine(myPen, A, B);

                for (int z = i + 2; z < listx.Count; z = z + 2) 
                {                   
                    x3 = listx[z];
                    x4 = listx[z + 1];
                    y3 = listy[z];
                    y4 = listy[z + 1];

                    if (x1 == x3 && x2 == x4 && y1 == y3 && y2 == y4)
                    {
                        MessageBox.Show("Zadali jste dvě stejné úsečky. Jedna bude ignorována! X[" + x1 / meritko + ";" + x2 / meritko);
                    }
                    else
                    {
                        //
                        //Výpočet průsečíků
                        //

                        if (x1 != x2 || x3 != x4)
                        {
                            //výpočet bodů průsečíku, f odpovídá Y souřadici prlůůsečíku a q X souřadnici průsečíku)
                            double f = (((y1 - y2) * (y3 - y4) * (x1 - x3)) + ((x3 - x4) * (y1 - y2) * y3) - ((y3 - y4) * (x1 - x2) * y1)) / (((y1 - y2) * (x3 - x4)) - ((y3 - y4) * (x1 - x2)));

                            double q = (x1 - x2);
                            q = q / (y1 - y2);
                            q = q * (f - y1);
                            q = q + x1;
                            if (q >= (x1 - 7) && q <= (x2 + 7) && q >= (x3 - 7) && q <= (x4 + 7) && f >= (y1 - 7) && f <= (y2 + 7) && f >= (y3 - 7) && f <= (y4 + 7))
                            {
                                pr.Add(Convert.ToInt32(q));
                                pr.Add(Convert.ToInt32(f));
                                listBox2.Items.Add("[" + Math.Round(q / meritko, 2) + ";" + Math.Round(f / meritko, 2) + "]");
                            }
                        }
                        //
                    }
                }
            }

            //
            //Cyklus pro vykreslení průsečíků (aby byly nad úsečkami)
            //
            for (int i = 0; i < pr.Count; i = i + 2)
            {
                dw.FillEllipse(myBrush, pr[i] +497, pr[i + 1] + 297, 6, 6);
            }
            //

            dw.Dispose();

        }
    }
}
//
//POPIS PROGRAMU
//
/*
 * Všechny hodnoty, které program načte ze souboru nejprve validuje. Pokud odpovídají podmínkám, uloží si je do svých interních proměnných pro další zpracování. Všechny hodnoty jsou
 * vinosábony koeficientem (měřítkem), aby byly vůbec vidět. Tento fakt ale pouukazuje na nedostatek mého programu. Pokud zadáme příliš veliké číslo či příliš malé číslo, úsečky téměř neuvidíme
 * a jejich průsečíky také ne.
 * 
 * Průsečíky jsou vypočítávány pomocí dvou vzorců (soustavy rovnic). Souřadnice se počítají z rovnic přímek těchto úseček.
 * Po vypočítání Y-ové složky průsečíku se z ní dopočítá složka X-ová. Dále potřeba ověřit, zda tyto souřadnice leží na zobrazených úsečkách. Může se totiž stát, že některé průsečíky leží mimo
 * ně, tzn. přímky úseček se protínají, ale samotné úsečky nikoliv.
 * 
 * Časová složitost programu je lineární, přímo úměrná zadaným hodnotám.
 */