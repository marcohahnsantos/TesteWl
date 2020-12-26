using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TESTEWL.ClassesGerais
{
    class BancoDados
    {
        /// <summary>
        /// SQLiteDatabase object sqldTemp to handle SQLiteDatabase.
        /// </summary>
        private SQLiteDatabase sqldTemp;
        /// <summary>
        /// The sSQLquery for query handling.
        /// </summary>
        private string sSQLQuery;
        /// <summary>
        /// The sMessage to hold message.
        /// </summary>
        private string sMessage;
        /// <summary>
        /// The bDBIsAvailable for database is available or not.
        /// </summary>
        private bool bDBIsAvailable;
        /// <summary>
        /// Initializes a new instance of the <see cref="MyDatabaseDemo.MyDatabase"/> class.
        /// </summary>
        /// 


        public BancoDados()
        {
            sMessage = "";
            bDBIsAvailable = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MyDatabaseDemo.MyDatabase"/> class.
        /// </summary>
        /// <param name='sDatabaseName'>
        /// Pass your database name.
        /// </param>
        public BancoDados(string sDatabaseName)
        {
            try
            {
                sMessage = "";
                bDBIsAvailable = false;
                //  CreateDatabaseVeiculoEmpresa(sDatabaseName);

            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MyDatabaseDemo.MyDatabase"/> database available.
        /// </summary>
        /// <value>
        /// <c>true</c> if database available; otherwise, <c>false</c>.
        /// </value>
        public bool DatabaseAvailable
        {
            get { return bDBIsAvailable; }
            set { bDBIsAvailable = value; }
        }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return sMessage; }
            set { sMessage = value; }
        }
        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name='sDatabaseName'>
        /// Pass database name.
        /// </param>

        public async void CreateDatabases(string sDatabaseName)
        {
            
            try
            {
               

                sMessage = "";
                string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sDB = Path.Combine(sLocation, "PRINCIPAL");
                sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
                bool bIsExists = File.Exists(sDB);

                //sSQLQuery = "DELETE FROM COLABORADOR ";
                //sqldTemp.ExecSQL(sSQLQuery);
                //bDBIsAvailable = true;


                sSQLQuery = "CREATE TABLE IF NOT EXISTS " +
                       "COLABORADOR " +
                       "(_id INTEGER PRIMARY KEY," +
                       "NOME VARCHAR," +
                       "CONSOME_BEBIDA BOLEAN," +
                       "DATA_CADASTRO DATETIME);";
                sqldTemp.ExecSQL(sSQLQuery);
                bDBIsAvailable = true;

               
            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
            try
            {
                sMessage = "";
                string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sDB = Path.Combine(sLocation, "PRINCIPAL");
                sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
                bool bIsExists = File.Exists(sDB);

                //sSQLQuery = "DROP TABLE CONVIDADO ";
                //sqldTemp.ExecSQL(sSQLQuery);
                //bDBIsAvailable = true;

                sSQLQuery = "CREATE TABLE IF NOT EXISTS " +
                       "CONVIDADO " +
                       "(_id INTEGER PRIMARY KEY," +
                       "ID_COLABORADOR INTEGER," +
                       "NOME VARCHAR," +
                       "CONSOME_BEBIDA VARCHAR," +
                        "DATA_CADASTRO VARCHAR);";
                sqldTemp.ExecSQL(sSQLQuery);
                bDBIsAvailable = true;
            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }

            try
            {
                sMessage = "";
                string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sDB = Path.Combine(sLocation, "PRINCIPAL");
                sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
                bool bIsExists = File.Exists(sDB);

                //sSQLQuery = "DROP TABLE CONVIDADO ";
                //sqldTemp.ExecSQL(sSQLQuery);
                //bDBIsAvailable = true;

                sSQLQuery = "CREATE TABLE IF NOT EXISTS " +
                       "GASTOS " +
                       "(_id INTEGER PRIMARY KEY," +
                       "TIPO VARCHAR," +
                       "VALOR NUMERIC(18,2),"+
                        "DATA_CADASTRO VARCHAR);";
                sqldTemp.ExecSQL(sSQLQuery);
                bDBIsAvailable = true;

                Android.Database.ICursor icursorTemp = null;
                sSQLQuery = "SELECT  _id " +
                        " FROM GASTOS WHERE _id=1";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                if (ValorCursor == 0)
                {
                    string Valores = $"1,'COMIDA',0,'{System.DateTime.Today.ToString()}'";
                                 
                    string Campos = "_id," +
                        "TIPO," +
                        "VALOR," +
                        "DATA_CADASTRO";

                    sSQLQuery = "INSERT INTO " +
                        "GASTOS " + "(" + Campos + ") " +
                        "VALUES(" + Valores + ");";
                    sqldTemp.ExecSQL(sSQLQuery);
                    sMessage = "Record is saved.";
                }
                sSQLQuery = "SELECT  _id " +
                        " FROM GASTOS WHERE _id=2";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                ValorCursor = icursorTemp.Count;
                if (ValorCursor == 0)
                {
                    string Valores = $"2,'BEBIDA',0,'{System.DateTime.Today.ToString()}'";

                    string Campos = "_id," +
                        "TIPO," +
                        "VALOR," +
                        "DATA_CADASTRO";

                    sSQLQuery = "INSERT INTO " +
                        "GASTOS " + "(" + Campos + ") " +
                        "VALUES(" + Valores + ");";
                    sqldTemp.ExecSQL(sSQLQuery);
                    sMessage = "Record is saved.";
                }

            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }


        /// <summary>
        /// Adds the record.
        /// </summary>
        /// <param name='sName'>
        /// Pass name.
        /// </param>
        /// <param name='iAge'>
        /// Pass age.
        /// </param>
        /// <param name='sCountry'>
        /// Pass country.
        /// </param>

        public void FinalizaDatabases(string sDatabaseName)
        {
            sqldTemp.Close();
        }
    
        public void UpdateRecord(string Tabela, string Where, string Valores)
        {
            try
            {
                sSQLQuery = "UPDATE " +
                    Tabela + " SET " + Valores + " " +
                    Where;
                sqldTemp.ExecSQL(sSQLQuery);
                sMessage = "Record is saved.";
            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }

        public void AddRecordComando(string Comando)
        {
            try
            {
                sSQLQuery = Comando;
                sqldTemp.ExecSQL(sSQLQuery);
                sMessage = "Record is saved.";

            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }

        public void Delete1Record(int iId, string Tabela)
        {
            try
            {
                sSQLQuery = "DELETE FROM " + Tabela + " WHERE _id=" + iId + "";

                sqldTemp.ExecSQL(sSQLQuery);
                sMessage = "Record is deleted: " + iId;
            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }

        
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="MyDatabaseDemo.MyDatabase"/> is reclaimed by garbage collection.
        /// </summary>
        ~BancoDados()
        {
            try
            {
                sMessage = "";
                bDBIsAvailable = false;
                sqldTemp.Close();
            }
            catch (SQLiteException ex)
            {
                sMessage = ex.Message;
            }
        }

    }
}