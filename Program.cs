namespace Fp2_Polinomio
{
    internal class Program
    {
        const int N = 100;
        const double EPS = 1e-15;
        struct Monomio
        {
            public double coef; // Coeficiente
            public int exp; // Exponente
        }

        struct Polinomio
        {
            public Monomio[] mon; // Array de monomios
            public int nMon; // Primer espacio libre en el array mon
        }
        static void Main(string[] args)
        {
            Polinomio pol;
            PidePolinomio(out pol);
            //LeeArchivo(out pol);
            GuardaArchivo(pol);
            EscribePolinomio(pol);
        }

        static void LeeMonomio(out Monomio m)
        {
            m.exp = 0;
            Console.WriteLine("Monomio: ");
            Console.Write(" coef: ");
            m.coef = double.Parse(Console.ReadLine());
            if (!EqDouble(m.coef, 0))
            {
                Console.Write(" exp: ");
                m.exp = int.Parse(Console.ReadLine());
            }
        }

        static void PidePolinomio(out Polinomio p)
        {
            // Se declara el array de monomios
            p.mon = new Monomio[N];
            p.nMon = 0; // Se inicializa nMon a 0
            Console.Write("Introduce monomios (coef = 0 para terminar):\n");
            Monomio m; 
            LeeMonomio(out m);
            while (!EqDouble(m.coef, 0))
            {
                // Metodo inserta
                Inserta(m, ref p);
                LeeMonomio(out m);
            }

        }

        static bool EqDouble(double c1, double c2) { return Math.Abs(c1 - c2) < EPS; }

        static void Inserta(Monomio m, ref Polinomio p)
        {
            if (!EqDouble(m.coef, 0))
            {
                int i = 0;
                while (i < p.nMon && m.exp != p.mon[i].exp) i++;
                if (i < p.nMon) 
                { 
                    double c = p.mon[i].coef + m.coef;
                    if (EqDouble(c, 0))
                    {
                        p.mon[i] = p.mon[p.nMon - 1];
                        p.nMon--;
                    }
                    else p.mon[i].coef = c;

                }
                else
                {
                    if (p.nMon == N) Console.WriteLine("Error: Polinomio lleno");
                    else
                    {
                        p.mon[p.nMon] = m;
                        p.nMon++;
                    }
                }

            }
        }

        static Polinomio Copia(Polinomio p)
        {
            Polinomio copia;
            copia.mon = new Monomio[p.mon.Length];
            copia.nMon = p.nMon;
            for (int i = 0; i < copia.mon.Length; i++) copia.mon[i] = p.mon[i];
            return copia;
        }
        
        static Polinomio Suma(Polinomio p1, Polinomio p2)
        {
            Polinomio p3, p4;
            if (p1.nMon < p2.nMon)
            {
                p3 = Copia(p2);
                p4 = p1;
            }
            else
            {
                p3 = Copia(p1);
                p4 = p2;
            }
            for (int i = 0; i < p4.nMon; i++) Inserta(p4.mon[i], ref p3);
            return p3;
        }

        static void GuardaArchivo(Polinomio p) // Se guarda cada monomio en una linea de esta forma: coef exp
        {
            StreamWriter guardar = new StreamWriter("Polinomio.txt");
            for (int i = 0; i < p.nMon; i++) guardar.WriteLine(p.mon[i].coef + " " + p.mon[i].exp);
            guardar.Close();
        }

        static void LeeArchivo(out Polinomio p)
        {
            StreamReader leer = new StreamReader("Polinomio.txt");
            p.mon = new Monomio[N];
            p.nMon = 0;
            while (!leer.EndOfStream)
            {
                string[] line = leer.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (line.Length == 2)
                {
                    Monomio m;
                    m.coef = double.Parse(line[0]);
                    m.exp = int.Parse(line[1]);
                    Inserta(m, ref p);
                }
            }
            leer.Close();
        }

        static void EscribePolinomio(Polinomio polinomio)
        {
            for (int i = 0; i < polinomio.nMon; i++)
            {
                Console.Write(" + " + polinomio.mon[i].coef + "x^" + polinomio.mon[i].exp);
            }
        }
    }
}
