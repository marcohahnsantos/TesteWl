using System;
using System.Timers;
using System.Collections;
using System.Runtime.InteropServices;
using Android.App;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using System.IO;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content.PM;
using TESTEWL.ClassesGerais;
using Android.Text;
using System.Threading;
using System.Globalization;

namespace TESTEWL.Activitys
{

    [Activity(Label = "ActProdutos", Theme = "@style/AppThemeSelecionar1", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ActGastos : AppCompatActivity
    {

        string[] nomes;
        private string sMessage;
        private bool bDBIsAvailable;
        public ArrayAdapter adapter { get; private set; }
        private SQLiteDatabase sqldTemp;
        private CarregaListaGastos item;
        BancoDados mdTemp;
        public EditText valor { get; set; }
        private const double ValorCheioColaborador = 20;
        private const double ValorMeioColaborador = 10;
        private const double ValorCheioConvidado = 40;
        private const double ValorMeioConvidado = 20;
        protected override void OnCreate(Bundle bundle)
        {
            

            
            base.OnCreate(bundle);

            string sSQLQuery = "";
            
            SetContentView(Resource.Layout.layoutGastos);

            SetResult(Result.Canceled);
            //==========================================================//
            //              CRIAR BANCO DE DADOS                        //
            //==========================================================//
            mdTemp = new BancoDados("PRINCIPAL");
            mdTemp.CreateDatabases("PRINCIPAL");
            //==========================================================//


            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "VETIFICAÇÃO DE GASTOS";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            var relogioDigital = FindViewById<DigitalClock>(Resource.Id.digitalClock2);
            relogioDigital.Visibility = Android.Views.ViewStates.Visible;

            Android.Support.V7.Widget.AppCompatImageButton BtnSair = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnVoltarProduto);
            Android.Support.V7.Widget.AppCompatImageButton BtnSalvar = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnSalvarProduto);
            TextView TotalGastos = FindViewById<TextView>(Resource.Id.txtTotalGasto);
            TextView TotalArrecadado= FindViewById<TextView>(Resource.Id.txtTotalArrecadado); 
            EditText valorcomida = FindViewById<EditText>(Resource.Id.TextValorComida);
            EditText valorbebida = FindViewById<EditText>(Resource.Id.TextValorBebida);
           
            sSQLQuery = "";
            string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sDB = Path.Combine(sLocation, "PRINCIPAL");
            sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
            bool bIsExists = File.Exists(sDB);


            valorcomida.AfterTextChanged += EditTextValorComida_AfterTextChanged;
            valorbebida.AfterTextChanged += EditTextValorBebida_AfterTextChanged;
            nomes = new string[1];
            for (int i = 0; i < 1; i++)
            {
                nomes[i] = " ".ToString();
            }
            CarregaListaGastos();
            Window.SetSoftInputMode(SoftInput.StateHidden);

            BtnSair.Click += delegate
            {
                Finish();
            };

            BtnSalvar.Click += delegate
            {
                if (valorcomida.Text == "")
                {
                    Toast.MakeText(this, "Deve ser digitado o  Valor de Comida!", ToastLength.Short).Show();
                    return;
                }
                if (valorbebida.Text == "")
                {
                    Toast.MakeText(this, "Deve ser digitado o  Valor de Bebida!", ToastLength.Short).Show();
                    return;
                }

                //==============================================================================================//
                //                             VERIFICA SE JÁ EXISTE UM ENDEREÇO GRAVADO                        //
                //==============================================================================================//

                sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                sDB = Path.Combine(sLocation, "PRINCIPAL");
                sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
                bIsExists = File.Exists(sDB);


                Android.Database.ICursor icursorTemp = null;

                sSQLQuery = "";
                sSQLQuery = $"SELECT _id FROM GASTOS WHERE _id=1";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;


                string Data = System.DateTime.Today.ToShortDateString();
                string Hora = System.DateTime.Today.ToShortTimeString();

                if (ValorCursor > 0)
                {
                    string ComandoSql = "update GASTOS set " +
                                $" VALOR={valorcomida.Text.Replace(",",".")}," +
                                $" DATA_CADASTRO='{Data}' " +
                                $" where _id=1";

                    sqldTemp.ExecSQL(ComandoSql);
                    ComandoSql = "update   GASTOS set " +
                                                    $" VALOR={valorbebida.Text.Replace(",",".")}," +
                                                    $" DATA_CADASTRO='{Data}' " +
                                                    $" where _id=2";
                    sqldTemp.ExecSQL(ComandoSql);
                }
                valorcomida.Text = "";
                valorbebida.Text = "";
                CarregaListaGastos();
            };


            void EditTextValorComida_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
            {
                var text = e.Editable.ToString();
                valorcomida.AfterTextChanged -= EditTextValorComida_AfterTextChanged;
                var formatedText = FormatarCampos.ValorValueConverter(text);
                valorcomida.Text = formatedText;
                valorcomida.SetSelection(formatedText.Length);
                valorcomida.AfterTextChanged += EditTextValorComida_AfterTextChanged;
            }
            void EditTextValorBebida_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
            {
                var text = e.Editable.ToString();
                valorbebida.AfterTextChanged -= EditTextValorBebida_AfterTextChanged;
                var formatedText = FormatarCampos.ValorValueConverter(text);
                valorbebida.Text = formatedText;
                valorbebida.SetSelection(formatedText.Length);
                valorbebida.AfterTextChanged += EditTextValorBebida_AfterTextChanged;
            }
            void CarregaListaGastos()
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Android.Database.ICursor icursorTemp = null;

                string sSQLQuery = "SELECT COUNT(_id) " +
                 " FROM COLABORADOR WHERE CONSOME_BEBIDA='SIM'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                double ValorCColaborador=0;
                if (ValorCursor > 0)
                {
                    icursorTemp.MoveToNext();
                    ValorCColaborador = ValorCheioColaborador * Convert.ToInt32(icursorTemp.GetString(0));
                }

                sSQLQuery = "SELECT COUNT(_id) " +
                 " FROM COLABORADOR WHERE CONSOME_BEBIDA='NAO'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                ValorCursor = icursorTemp.Count;
                double ValorMColaborador=0;
                if (ValorCursor > 0)
                {
                    icursorTemp.MoveToNext();
                    ValorMColaborador = ValorMeioColaborador * Convert.ToDouble(icursorTemp.GetString(0));
                }

                sSQLQuery = "SELECT COUNT(_id) " +
                " FROM CONVIDADO WHERE CONSOME_BEBIDA='SIM'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                ValorCursor = icursorTemp.Count;
                double ValorCConvidado=0;
                if (ValorCursor > 0)
                {
                    icursorTemp.MoveToNext();
                    ValorCConvidado = ValorCheioConvidado * Convert.ToDouble(icursorTemp.GetString(0));
                }

                sSQLQuery = "SELECT COUNT(_id) " +
                 " FROM CONVIDADO WHERE CONSOME_BEBIDA='NAO'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                ValorCursor = icursorTemp.Count;
                double ValorMConvidado=0; ;
                if (ValorCursor > 0)
                {
                    icursorTemp.MoveToNext();
                    ValorMConvidado = ValorMeioConvidado * Convert.ToDouble(icursorTemp.GetString(0));
                }
                double TotalArrecadados=Convert.ToDouble(ValorCColaborador+
                                      ValorMColaborador+
                                      ValorCConvidado+
                                      ValorMConvidado);

                TotalArrecadado.Text =  TotalArrecadados.ToString("N2");


                sSQLQuery = "SELECT _id," +
                " TIPO, " +
                " VALOR " +
                 " FROM GASTOS ";


                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);


                ValorCursor = icursorTemp.Count;
                if (ValorCursor > 0)
                {
                    nomes = new string[icursorTemp.Count];
                    double TotalGasto = 0;
                    for (int i = 0; i < icursorTemp.Count; i++)
                    {
                        icursorTemp.MoveToNext();
                        item = new CarregaListaGastos();
                        item.Tipo = icursorTemp.GetString(1);
                        item.Valor = Convert.ToDouble(icursorTemp.GetString(2).Replace(".", ",")).ToString("N2");
                        TotalGasto += Convert.ToDouble(item.Valor);
                        string Linha = "";
                        Linha = item.Tipo.PadRight(25, ' ') +
                        item.Valor ;
                        
                        nomes[i] = Linha;
                    }
                    ArrayAdapter<System.String> itemsAdapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes);



                    Android.Widget.ListView listview = FindViewById<Android.Widget.ListView>(Resource.Id.ListaProdutos);
                    listview = FindViewById<Android.Widget.ListView>(Resource.Id.ListaProdutos);

                    listview.Adapter = itemsAdapter;

                    listview.ItemClick += ListView_ItemClick;
                    TotalGastos.Text = TotalGasto.ToString("N2");
                }

                void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
                {
                    Validacoes.ValorItem = nomes[e.Id];

                    sSQLQuery = "SELECT _id," +
                    " VALOR " +
                    " FROM GASTOS ";
                    icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                    ValorCursor = icursorTemp.Count;
                    if (ValorCursor > 0)
                    {
                        for (int i = 0; ValorCursor > i; i++)
                        {
                            icursorTemp.MoveToNext();
                            if(i==0)
                                valorcomida.Text = Convert.ToDouble(icursorTemp.GetString(1).Replace(".", ",")).ToString("N2");
                            else
                                valorbebida.Text = Convert.ToDouble(icursorTemp.GetString(1).Replace(".", ",")).ToString("N2");
                        }
                    }
                }
            }
        }
        #region [FUNCOES]
        public class CarregaListaGastos
        {
            public string Id { get; set; }
            public string Tipo { get; set; }
            public string Valor { get; set; }
            public string Data { get; set; }
            public string Hora { get; set; }
            private SQLiteDatabase sqldTemp;
            private string sSQLQuery;
            BancoDados mdTemp;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }
        #endregion
    }
}