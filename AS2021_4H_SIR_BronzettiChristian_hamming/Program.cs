using System;
using System.Collections.Generic;
using System.Text;
using Pastel;


namespace AS2021_4H_SIR_BronzettiChristian_hamming
{
    class Program
    {
        //metodo inserimento
        static string Inserisci(string tipoInserimento)
        {
            string input;
            while (true)
            {
                Console.WriteLine($"Inserire la sequenza di bit binaria {tipoInserimento} (0 o 1)");
                input = Console.ReadLine();

                if (ControllaInserimento(input))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK");
                    Console.ResetColor();
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Deve contenere solo 0 o 1");
                    Console.ResetColor();
                }
            }            
            return input;
        }
        
        //controllo inserimento
        static bool ControllaInserimento(string num)
        {
            bool f=true;
            foreach (char s in num)
                if (!s.Equals('1') && !s.Equals('0'))
                     f = false;

            return f;
        }

        static List<int> CalcolaPosizioniParità(List<char> sequenza)
        {
            //proprietà con le posizione sulle quale applicare le parità
            //ho da aggiungere gli underscore nelle posizioni 1;2;4;8; nel caso di una lista è -1
            List<int> posizioneParità = new List<int>();

            for (int i = 0; i < sequenza.Count; i++)
               posizioneParità.Add((int)Math.Pow(2, i)-1);
            
            return posizioneParità;
        }

        /// <summary>
        /// metodo per mettere gli underscore nelle rispettive posizioni richieste
        /// </summary>
        /// <param name="sequenza"></param>
        /// <returns></returns>
        static List<char> AggiungiCaratteriUnderScore(List<char>sequenza, List<int>parity)
        {
            /// <summary>
            /// il for gira per la lunghezza della list
            /// all'interno vi è un foreach che mi va a decidere per quale indice della list aggiungere il trattino
            /// tramite la tabella della parità (vettore), per evitare iterazioni ridondanti breakko il ciclo
            /// </summary>
            /// <param name="sequenza"></param>
            /// <returns></returns>

            for (int i = 0; i < sequenza.Count; i++)
            {
                foreach(int c in parity)
                    if (c == i)
                    {
                        sequenza.Insert(c, '_');
                        break;
                    }
                        
            }

            return sequenza;
        }

        /// <summary>
        /// Metodo che stabilisce la parità (sempre pari)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static bool StabilisciParita(List<char> num, int posizionePartenza)
        {
            
            int k = 0;

            //Parte dalla prima posizione partenza
            //entra in un cliclo che scorre n volte quante la partenza+1
            for (int i = posizionePartenza; i < num.Count; i=i+posizionePartenza+1)
            {
                int j = 0;
                do
                {
                    if (num[i].Equals('1'))
                        k++;

                    i++;
                    j++;
                } while (j<posizionePartenza+1 && i < num.Count); //serve controllo in coda perchè deve fare almeno 1 giro
            }

            if (k % 2 == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Metodo di hamming
        /// </summary>
        /// <param name="sequenza"></param>
        /// <param name="indexPosizione"></param>
        /// <returns></returns>
        static List<char> Hamming(List<char> sequenza, int indexPosizione)
        {
            /// <summary>
            /// Metodo di hamming
            /// </summary>
            /// <param name="sequenza"></param>
            /// è la list coi char
            /// <param name="indexPosizione"></param>
            /// è la posizione sulla quale inserire il bit di parità
            /// <returns></returns>
            //calcolo gli 1 nelle posizioni se è pari metto 0 altrimenti metto 1 per rendere il numero di 1 pari
            bool parità = StabilisciParita(sequenza, indexPosizione);

            if (parità) //se true cioè il numero di bit ad 1 è pari metto uno 0 nel primo posto vuoto
                sequenza = SettaParita(sequenza, '0', indexPosizione);
            else
                sequenza = SettaParita(sequenza, '1', indexPosizione);
 
            return sequenza;
        }

        /// <summary>
        /// Metodo che inserice la parità dove decido io
        /// </summary>
        /// <param name="sequenza">list</param>
        /// <param name="parità">parità da inserire</param>
        /// <param name="indexPosizione">posizione sulla quale inserire la parità</param>
        /// <returns></returns>
        static List<char> SettaParita(List<char> sequenza, char parità, int indexPosizione)
        {
            sequenza.RemoveAt(indexPosizione);
            sequenza.Insert(indexPosizione, parità);
            return sequenza;
        }

        /// <summary>
        /// metodo generale che svolge il lavoro per tenere il main il più pulito possibile
        /// </summary>
        /// <param name="str1">stringaq input</param>
        /// <param name="str2">stringa output</param>
        static string TrovaDifferenze(string str1, string str2)
        {
            
            List<char> sequenza1 = new List<char>();
            sequenza1.AddRange(str1.ToCharArray());

            
            List<char>sequenza2 = new List<char>();
            sequenza2.AddRange(str2.ToCharArray());

            //creo una list nella quale mettere le potenze di 2
            List<int> posizioniParita = CalcolaPosizioniParità(sequenza1);

            //aggiungo gli _
            sequenza1 = AggiungiCaratteriUnderScore(sequenza1, posizioniParita);
            //sequenza2 = AggiungiCaratteriUnderScore(sequenza2, posizioniParita);

            for (int i = 0; i < sequenza2.Count; i++)
            {
                foreach (int p in posizioniParita)
                    if (p == i)
                    {
                        sequenza1 = Hamming(sequenza1, p);
                        //sequenza2 = Hamming(sequenza2, p);
                        break;
                    }
            }

            StringBuilder sb = new StringBuilder();
            int posErrore=0;
            for (int i = 0; i < sequenza1.Count; i++)
            {
                foreach (int p in posizioniParita)
                    if (p == i)
                    {
                        if (sequenza2[i] != sequenza1[i])
                            posErrore = posErrore + p + 1;
                    }
            }

            sb.AppendLine($"Codifica di Hamming della parola in ingresso: {StampaList(sequenza1, posizioniParita)} ");
            sb.AppendLine($"Parola ricevuta: {StampaList(sequenza2, posizioniParita)}");

            if (posErrore == 0)
                sb.AppendLine("Nessun Errore".Pastel("#66FF00")).ToString();
            else
            {
                sb.AppendLine($"Errore trovato al bit {posErrore}".Pastel("#FF0000")
                + $"\nLa stringa corretta è {StampaSequenzaDefinitiva(sequenza1, posErrore)}");
            }
            return sb.ToString();   
        }

        //colorazione del bit corretto
        static string StampaSequenzaDefinitiva(List<char> hamming, int errore)
        {
            StringBuilder sb = new StringBuilder();
            string retVal;
            for (int i = 0; i < hamming.Count; i++)
            {
                if (i == errore - 1)
                {
                    retVal = hamming[i].ToString().Pastel("#00FF00");
                    sb.Append(retVal);
                }
                else
                    sb.Append(hamming[i]);
            }

            return sb.ToString();
        }
        //metodo stampalista ed evidenzia le parità
        static string StampaList(List<char> hamming, List<int>parita)
        {
            StringBuilder sb = new StringBuilder();
            string retVal="";
            int x;
            for (int i = 0; i < hamming.Count; i++)
            {
                
                for(int j =0; j<parita.Count; j++)
                    if (i == parita[j]) 
                    {
                        retVal = hamming[i].ToString().Pastel("#ffff00");
                        break;
                    }
                    else
                        retVal = hamming[i].ToString();

                sb.Append(retVal.ToString());
            }
            
            return sb.ToString();
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Programma che calcola hamming");

            //ho 2 stringhe in input
            //1 è il dato inviato dall'utente
            //l'altra è il dato ricevuto
            //calcolo hamming e se le parità sono uguali non ci sono errori

            Console.WriteLine("Inserire la stringa inviata");
            //stringa 1
            string input = Inserisci("inviata");

            Console.WriteLine("Inserisci la parola ricevuta (comprensiva di codice di Hamming), inserendovi al massimo un errore:");
            //stringa2
            string ricevuta = Inserisci("ricevuta");

            Console.Clear();
            Console.WriteLine($"Confronto tra {input} e {ricevuta}...\n{TrovaDifferenze(input, ricevuta)}");
        }
    }
}
