#region biblioteki
using System;
using System.Diagnostics;
using System.IO;
#endregion

namespace Zadanie_10_Podział_Liczb_konsola
{
    class Program
    {
        #region zmienne
        public static long counter;                 //licznik rekurencji
        public static long counterDiv;              //licznik podziałów
        public static int number;                   //liczba poddawana rozkładowi
        public static int[] tab = new int[1000];    //wielkość tablicy 
        #endregion
        static void Main(string[] args)
        {
            Console.Title = "Zadanie 10 - Podział Liczb";

            #region sekcja odpowiedzialna za zapis do pliku
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                //zapisuje się w lokalizacji pliku .exe
                ostrm = new FileStream("./Plik.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                //Wyjątek jeśli nie będzie mógł ani otworzyć pliku ani go utworzyć
                Console.WriteLine("Nie można otwożyć 'Plik.txt' do zapisu");
                Console.WriteLine(e.Message);
                return;
            }
            #endregion

            /*
            Na pierwszym miejscu w tablicy zostaje zapisana wartość 1, aby nie 
            odwoływać się do pustej tablicy. W przeciwnym wypadku w obliczeniach
            będzie brane również 0, co spowoduje pojawieniem się artefaktów
            */
            tab[0] = 1;
            //tab[0] = 0;   //test
            Console.Write("Podaj liczbę naturalną do podziału: ");
            number = int.Parse(Console.ReadLine());

                                        //rozpocznij zapis
            //Console.SetOut(writer);   //tylko kiedy chcemy zapisać do pliku
                                        //włącz odliczanie
            Stopwatch timer = Stopwatch.StartNew();
            division(1, number, tab);   //mierzymy czas tylko dla działania funkcji a nie całego programu
                                        //zatrzymaj odliczanie
            timer.Stop();
                                        //wypisanie statystyk
            Console.WriteLine(          
                "\nLiczba wykonanych rekurencji:" + counter + 
                "\nLiczba podziałów: " + counterDiv + 
                "\nW czasie: " + timer.ElapsedMilliseconds + "ms"
                );
                                        //zatrzymaj zapis
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();

            Console.ReadKey();
        }
        /*
        Do funkcji przekaż trzy parametry: position (aktualna pozycja w tabeli), 
        numbPos (liczbę do podziału), tab (tablicę przechowującą dane)
        */
        public static void division(int position, int numbPos, int[] tab)
        {
            counter++;                  //zwiększamy licznik rekurencji
            /*
            W pierwszym kroku sprawdzane jest czy liczba pozostała do podziału równa się 0, jeżeli 
            tak to wyświetlana jest zawartość tablicy (for), przedstawiając odpowiedni podział liczby.
            */
            if (numbPos == 0)
            {
                #region do celów demonstracyjnych można za komentować
                //wypisanie liczba = suma
                //np.: 10 = 1 + 2 + 3 + 4
                Console.Write(number + " = ");
                for (int i = 1; i <= position - 2; i++)
                {
                    Console.Write(tab[i] + " + ");
                }
                Console.Write(tab[position - 1]);
                Console.WriteLine("");
                #endregion

                counterDiv++;
            }
            /*
            W przeciwnym wypadku - wywołaj pętle do szukania kolejnych podziałów
            kolejne wywołania bazują na danych z wcześniejszych iteracji.
            */
            else
            {
                for (int j = tab[position - 1]; j <= numbPos; j++)
                {
                    /*
                    Jeżeli aktualna pozycja jest równa 1, to zapisz wartość licznika j na tej pozycji
                    */
                    if (position == 1)
                    {
                        tab[position] = j;
                        //*1
                        division(position + 1, numbPos - j, tab);
                    }
                    /*
                    W przeciwnym wypadku do zmiennej pomocniczej temp przypisz wartość kolejnej składowej 
                    podziału. Do tablicy (na danej pozycji) przypisz temp
                    */
                    else
                    {
                        int temp = j + 1;
                        tab[position] = temp;
                        //*1
                        division(position + 1, numbPos - temp, tab);
                    }
                    /*
                    *1:
                    Metoda wykonywana jest rekurencyjnie, ze zwiększona wartością aktualnej pozycji oraz 
                    ze zmniejszoną wartością liczby pozostałej do podziału.
                    */

                }
            }
        }
    }
}
