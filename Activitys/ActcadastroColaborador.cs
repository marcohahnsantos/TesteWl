using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content.PM;
using TESTEWL.Activitys;
using TESTEWL.ClassesGerais;
using Android.Database.Sqlite;
using System.IO;
using Android.Text;
using Java.Lang;
using System;
using Android;

namespace TESTEWL.Activitys
{
    [Activity(Label = "ActcadastroClientes", Theme = "@style/AppThemeSelecionar1", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ActcadastroColaborador : AppCompatActivity
    {
        private string sMessage;
        string[] selecao;
        public ArrayAdapter adapter { get; private set; }
        private SQLiteDatabase sqldTemp;
        private CarreDadosColaboradores item;
        Spinner spinnerconsome;
        BancoDados mdTemp;
        string[] nomes;
        string firstItem = "";
        int Posicao = 0;
        public EditText cnpjcpf { get; set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string sSQLQuery = "";

            SetContentView(Resource.Layout.layoutColaborador);

            SetResult(Result.Canceled);
            //==========================================================//
            //              CRIAR BANCO DE DADOS                        //
            //==========================================================//
            mdTemp = new BancoDados("PRINCIPAL");
            mdTemp.CreateDatabases("PRINCIPAL");
            //==========================================================//


            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "CADASTRO DE COLABORADOR";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            var relogioDigital = FindViewById<DigitalClock>(Resource.Id.digitalClock2);
            relogioDigital.Visibility = Android.Views.ViewStates.Visible;

            Android.Support.V7.Widget.AppCompatImageButton BtnSair = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnSairClientes);
            Android.Support.V7.Widget.AppCompatImageButton BtnSalvar = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnSalvarClientes);
            Android.Support.V7.Widget.AppCompatImageButton BtnExcluir = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnExcluirColaborador);
            TextView Codigo = FindViewById<TextView>(Resource.Id.TxtCodigoColaborador);
            EditText Nome = FindViewById<EditText>(Resource.Id.TxtNomeCliente);


            sSQLQuery = "";
            string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sDB = Path.Combine(sLocation, "PRINCIPAL");
            sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
            bool bIsExists = File.Exists(sDB);
            bool Consome = true;


            spinnerconsome = FindViewById<Spinner>(Resource.Id.spinnerconsome);
            selecao = new string[2];
            selecao[0] = "SIM";
            selecao[1] = "NAO";
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, selecao);
            spinnerconsome.Adapter = adapter;
            spinnerconsome.ItemSelected += (s, e) =>
            {
                firstItem = spinnerconsome.SelectedItem.ToString();
                if (firstItem.Equals(spinnerconsome.SelectedItem.ToString()))
                {
                    Posicao = spinnerconsome.SelectedItemPosition;
                }
                else
                {
                    Toast.MakeText(this, "You have selected: " + e.Parent.GetItemIdAtPosition(e.Position).ToString(),
                        ToastLength.Short).Show();
                }

            };


            CarregaListaColaborador();
            Window.SetSoftInputMode(SoftInput.StateHidden);
            BtnSair.Click += delegate
            {
                Finish();
            };

            BtnSalvar.Click += delegate
            {
                if (Nome.Text == "")
                {
                    Toast.MakeText(this, "Deve ser digitado um nome do colaborador!", ToastLength.Short).Show();
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
                sSQLQuery = $"SELECT _id FROM COLABORADOR WHERE _id='{Codigo.Text}'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                string Data = System.DateTime.Today.ToString();

                if (ValorCursor == 0)
                {
                    string Valores = "'" + Nome.Text + "','" +
                                    spinnerconsome.SelectedItem.ToString() + "','" +
                                    Data + "'";

                    string Campos = "NOME," +
                        "CONSOME_BEBIDA," +
                        "DATA_CADASTRO";

                    sSQLQuery = "INSERT INTO " +
                        "COLABORADOR " + "(" + Campos + ") " +
                        "VALUES(" + Valores + ");";
                    sqldTemp.ExecSQL(sSQLQuery);
                    sMessage = "Record is saved.";
                    Toast.MakeText(this, "REGISTRO GRAVADO COM SUCESSO", ToastLength.Long).Show();
                }
                else
                {
                    string ComandoSql = "update COLABORADOR set " +
                                                   $" NOME='{Nome.Text}'," +
                                                   $" CONSOME_BEBIDA='{ spinnerconsome.SelectedItem.ToString()}'" +
                                                   $" where _id={Codigo.Text}";

                    sqldTemp.ExecSQL(ComandoSql);
                    Toast.MakeText(this, "Atualização realizada com sucesso", ToastLength.Long).Show();
                }
                Nome.Text = "";
                Consome = true;
                CarregaListaColaborador();
            };
            BtnExcluir.Click += delegate
            {
                if (Codigo.Text == "")
                {
                    Toast.MakeText(this, "Deve ser selecionado um colaborador!", ToastLength.Short).Show();
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
                sSQLQuery = $"SELECT _id FROM COLABORADOR WHERE _id={Codigo.Text}";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                string Data = System.DateTime.Today.ToString();

                if (ValorCursor != 0)
                {
                    string ComandoSql = $"DELETE from COLABORADOR  where _id={Codigo.Text}";
                    sqldTemp.ExecSQL(ComandoSql);
                    Toast.MakeText(this, "Registro Excluido com sucesso", ToastLength.Long).Show();
                }
                Nome.Text = "";
                Consome = true;
                CarregaListaColaborador();
            };


            void CarregaListaColaborador()
            {
                Android.Database.ICursor icursorTemp = null;
                sSQLQuery = "SELECT _id,NOME,CONSOME_BEBIDA " +
                 " FROM COLABORADOR ";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                if (ValorCursor > 0)
                {
                    nomes = new string[icursorTemp.Count];

                    for (int i = 0; i < icursorTemp.Count; i++)
                    {
                        icursorTemp.MoveToNext();
                        item = new CarreDadosColaboradores();
                        item.Id = icursorTemp.GetString(0);
                        item.nome = icursorTemp.GetString(1);
                        item.StringConsome =icursorTemp.GetString(2);

                        nomes[i] = item.Id.PadLeft(5, '0') + "   " +
                                   item.StringConsome + "   " +
                                   item.nome.ToString();
                                
                        
                    }
                    ArrayAdapter<System.String> itemsAdapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes);
                    Android.Widget.ListView listview = FindViewById<Android.Widget.ListView>(Resource.Id.ListaClientes);
                    listview = FindViewById<Android.Widget.ListView>(Resource.Id.ListaClientes);

                    listview.Adapter = itemsAdapter;
                    listview.ItemClick += ListView_ItemClick;
                }
                void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
                {
                          Validacoes.ValorItem = nomes[e.Id];
                          sSQLQuery = "SELECT _id," +
                          " NOME, " +
                          " CONSOME_BEBIDA "+
                          " FROM COLABORADOR " +
                          $" WHERE _id={Convert.ToInt16(Validacoes.ValorItem.Substring(0, 6))}";

                         icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);


                         ValorCursor = icursorTemp.Count;
                         if (ValorCursor > 0)
                        {
                            icursorTemp.MoveToNext();
                            Codigo.Text= icursorTemp.GetString(0);
                            Nome.Text = icursorTemp.GetString(1);
                            firstItem= icursorTemp.GetString(2);
                            for (int i = 0; spinnerconsome.Count > i; i++)
                            {
                                spinnerconsome.SetSelection(i);
                                if (firstItem.Equals(spinnerconsome.SelectedItem.ToString()))
                                {
                                    break;
                                }
                            }
                    }
                    }
                }


               

            }
            // Create your application here
        }

        #region [FUNCOES]

    

        public class CarreDadosColaboradores
        {
            public string Id { get; set; }
            public string tipo { get; set; }
            public string nome { get; set; }
            public string Data { get; set; }
            public bool Consome { get; set; }
            public string StringConsome { get; set; }

        }
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    if (item.ItemId == Android.Resource.Id.Home)
        //        Finish();

        //    return base.OnOptionsItemSelected(item);
        //}
        #endregion
    }
