using System;
using System.Drawing;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.Collections;
using Android.Bluetooth;
using System.Collections.Generic;
using Android.OS;
using Xamarin.Forms;
using System.Text;
using System.Threading.Tasks;

namespace TESTEWL
{
    public class Validacoes
    {

       


        protected static int valorCampoInt;
        public static int ValorCampoInt
        {
            get { return valorCampoInt; }
            set { valorCampoInt = value; }
        }
        protected static string valorItem;
        public static string ValorItem
        {
            get { return valorItem; }
            set { valorItem = value; }
        }

        //========================================================
        public static string VerificaSenha(string Senha)
        {
            string dec = string.Empty;
            for (int i = 0; i < Senha.Length; i++)
            {

                string cDec = ConverteCaractere(((byte)Senha[i]).ToString());
                if (cDec.Length < 3)
                    cDec = cDec.PadLeft(3, '0');

                dec += cDec;
            }
            return dec;
        }
        public static string RetornaValorCampo(string ValorCampo, int ValorNrCaracTotal)
        {
            int ValorNrCaractereCampo = ValorCampo.Length;
            string retorno = " ";
            var i = ValorCampo.Split(' ');
            int i1 = 1;

            if (i.Count() > 1)
                i1 = i.Count() + 2;

            if (ValorNrCaracTotal < ValorNrCaractereCampo)
            {
                retorno = ValorCampo.Substring(0, ValorNrCaracTotal).PadRight(ValorNrCaracTotal, ' ');
            }
            else
            {
                if (ValorNrCaractereCampo <= 10)
                    retorno = ValorCampo.Substring(0, ValorNrCaractereCampo).PadRight(ValorNrCaracTotal + (6 + (i1)), ' ');
                else if (ValorNrCaractereCampo <= 14)
                    retorno = ValorCampo.Substring(0, ValorNrCaractereCampo).PadRight(ValorNrCaracTotal + 8, ' ');
                else
                    retorno = ValorCampo.Substring(0, ValorNrCaractereCampo).PadRight(ValorNrCaracTotal + 2, ' ');

            }
            return retorno;
        }


        public static string ConverteCaractere(string caractere)
        {
            string retorno = RetornoClipCaractere(int.Parse(caractere));
            return retorno;
        }

        public static string RetornoClipCaractere(int Caractere)
        {
            string caractere;
            char c;
            if (Convert.ToInt16(Caractere) <= 50)
            {
                caractere = (Convert.ToInt16(Caractere) + 4).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }
            if (Convert.ToInt16(Caractere) > 50 && Convert.ToInt16(Caractere) <= 60)
            {
                caractere = (Convert.ToInt16(Caractere) + 7).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }
            if (Convert.ToInt16(Caractere) > 61 && Convert.ToInt16(Caractere) <= 80)
            {
                caractere = (Convert.ToInt16(Caractere) + 2).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }
            if (Convert.ToInt16(Caractere) == 80)
            {
                caractere = (Convert.ToInt16(67)).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }

            if (Convert.ToInt16(Caractere) > 81 && Convert.ToInt16(Caractere) <= 100)
            {
                caractere = (Convert.ToInt16(Caractere) + 3).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }
            if (Convert.ToInt16(Caractere) > 101 && Convert.ToInt16(Caractere) <= 255)
            {
                caractere = (Convert.ToInt16(Caractere) - 11).ToString();
                c = Convert.ToChar(int.Parse(caractere));
                return c.ToString();
            }

            return "";
        }
        public static bool RetornaCaractereNumericoValido(string c)
        {
            if (c.ToString() != "0" &&
                              c.ToString() != "1" &&
                              c.ToString() != "2" &&
                              c.ToString() != "3" &&
                              c.ToString() != "4" &&
                              c.ToString() != "5" &&
                              c.ToString() != "6" &&
                              c.ToString() != "7" &&
                              c.ToString() != "8" &&
                              c.ToString() != "9")
            {
                return false;
            }
            return true;
        }

  

        
        
        public static int VerificaEspacoBrancoValorEsquerda(string ValorValor)
        {
            if(ValorValor.Length==4)
                 return 15;
            if (ValorValor.Length == 5)
                return 10;


            return 15;
        }
       
        
       

        

        public static System.Byte ConverteCaracterehex(string caractere)
        {
            Byte retorno = 0X30;
            if (caractere == "0")
                retorno = 0X30;
            if (caractere == "1")
                retorno = 0X31;
            if (caractere == "2")
                retorno = 0X32;
            if (caractere == "3")
                retorno = 0X33;
            if (caractere == "4")
                retorno = 0X34;
            if (caractere == "5")
                retorno = 0X35;
            if (caractere == "6")
                retorno = 0X36;
            if (caractere == "7")
                retorno = 0X37;
            if (caractere == "8")
                retorno = 0X38;
            if (caractere == "9")
                retorno = 0X39;
            if (caractere == "Ç")
                retorno = 0X80;
            if (caractere == "ç")
                retorno = 0X87;
            if (caractere == "Ã")
                retorno = 0X8e;
            if (caractere == "ã")
                retorno = 0X84;
            if (caractere == "Ô")
                retorno = 0X99;
            if (caractere == "ô")
                retorno = 0X93;
            if (caractere == "ê")
                retorno = 0X88;
            if (caractere == "ê")
                retorno = 0X88;
            if (caractere == "Ê")
                retorno = 0XD3;
            return retorno;
        }

   



        public static void ZeraPlafaforma(BluetoothSocket Socket,
                                              int NrPlataforma)
        {

            bool ValidaPeso = false;
        

            int Contador = 0;
             
                Contador = Contador + 1;
                //=================================+
                //  PRIMEIRO 05 CARACTERES FIXO   //
                //================================//
                //           >00A1                //
                //================================//
                byte[] buffer = new byte[21];
                buffer[0] = 0x3E;
                buffer[1] = 0x30;
                buffer[2] = 0x30;
                buffer[3] = 0x41;
                buffer[4] = 0x31;
                //==================================
                //   06 CARACTERE FIXO            //
                //================================//
                //           >                    //
                //================================//
                buffer[5] = 0x3E;
                //==================================
                //   7 E 8                        //
                //================================//
                //   ID PLATAFORMA = NRPLATAFORMA //
                //================================//
                buffer[6] = 0x30;
                if(NrPlataforma==1)
                    buffer[7] =0x31;
                if (NrPlataforma == 2)
                    buffer[7] = 0x32;
                if (NrPlataforma == 3)
                    buffer[7] = 0x33;
                if (NrPlataforma == 4)
                    buffer[7] = 0x34;
                if (NrPlataforma == 5)
                    buffer[7] = 0x35;
                if (NrPlataforma == 6)
                    buffer[7] = 0x36;
            //==================================
            //   9 E 10  CARACTERES FIXOS     //
            //================================//
            //   OPERACAO 61                  //
            //================================//
            buffer[8] = 0x36;
            buffer[9] = 0x31; ;
            //==================================
            //   10 A 16  SEQUENCIACOMANDOS   //
            //================================//
            //  “FF3F0100    FF3F0100         //
            //================================//
            buffer[10] = 0x46;
            buffer[11] = 0x46;
            buffer[12] = 0x33;
            buffer[13] = 0x46;
            buffer[14] = 0x30;
            buffer[15] = 0x31;
            buffer[16] = 0x30;
            buffer[17] = 0x30;
            buffer[18] = 0x30;
            buffer[19] = 0x30;
            buffer[20] = 0x0D;


          
                var output = Socket.OutputStream;
                var readput = Socket.InputStream;
                output.Write(buffer, 0, 21);
            
        }

        public static void CalibraPeso(BluetoothSocket Socket,
                                              int NrPlataforma,
                                              string Peso)
        {

            bool ValidaPeso = false;


            int Contador = 0;

            Contador = Contador + 1;
            //=================================+
            //  PRIMEIRO 05 CARACTERES FIXO   //
            //================================//
            //        >FF5F00003203           //
            //================================//
            byte[] buffer = new byte[30];
            buffer[0] = 0x3E;
            buffer[1] = 0x46;
            buffer[2] = 0x46;
            buffer[3] = 0x35;
            buffer[4] = 0x46;
            buffer[5] = 0x30;
            buffer[6] = 0x30;
            buffer[7] = 0x30;
            buffer[8] = 0x30;
            buffer[9] = 0x33;
            buffer[10] = 0x32;
            buffer[11] = 0x30;
            buffer[12] = 0x33;
            buffer[13] = 0x30;

            //==================================
            //   7 E 8                        //
            //================================//
            //   ID PLATAFORMA = NRPLATAFORMA //
            //================================//
            if (NrPlataforma == 1)
                buffer[14] = 0x31;
            if (NrPlataforma == 2)
                buffer[14] = 0x32;
            if (NrPlataforma == 3)
                buffer[14] = 0x33;
            if (NrPlataforma == 4)
                buffer[14] = 0x34;
            if (NrPlataforma == 5)
                buffer[14] = 0x35;
            if (NrPlataforma == 6)
                buffer[14] = 0x36;
            //==================================
            //  15 A 20  CARACTERES FIXOS     //
            //================================//
            //   COMANDO AAAAAA               //
            //================================//
            buffer[15] = 0x41;
            buffer[16] = 0x41;
            buffer[17] = 0x41;
            buffer[18] = 0x41;
            buffer[19] = 0x41;
            buffer[20] = 0x41;

            //==================================
            //   10 A 16  SEQUENCIACOMANDOS   //
            //================================//
            //  “FF3F0100    FF3F0100         //
            //================================//
            buffer[10] = 0x46;
            buffer[11] = 0x46;
            buffer[12] = 0x33;
            buffer[13] = 0x46;
            buffer[14] = 0x30;
            buffer[15] = 0x31;
            buffer[16] = 0x30;
            buffer[17] = 0x30;
            buffer[18] = 0x30;
            buffer[19] = 0x30;
            buffer[20] = 0x0D;

            byte caractere;
            for (int i=0;i<=6;i++)
            {
                if(Peso.Substring(i,1)=="0")
                    buffer[20+i] = 0x30;
                if (Peso.Substring(i, 1) == "1")
                    buffer[20 + i] = 0x31;
                if (Peso.Substring(i, 1) == "2")
                    buffer[20 + i] = 0x32;
                if (Peso.Substring(i, 1) == "3")
                    buffer[20 + i] = 0x33;
                if (Peso.Substring(i, 1) == "4")
                    buffer[20 + i] = 0x34;
                if (Peso.Substring(i, 1) == "5")
                    buffer[20 + i] = 0x35;
                if (Peso.Substring(i, 1) == "6")
                    buffer[20 + i] = 0x36;
                if (Peso.Substring(i, 1) == "7")
                    buffer[20 + i] = 0x37;
                if (Peso.Substring(i, 1) == "8")
                    buffer[20 + i] = 0x38;
                if (Peso.Substring(i, 1) == "9")
                    buffer[20 + i] = 0x39;
            }


            var output = Socket.OutputStream;
            var readput = Socket.InputStream;
            output.Write(buffer, 0, 21);

        }



    }
}