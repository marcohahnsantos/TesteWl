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
    public class ActCadastroConvidados : AppCompatActivity
    {
        private string sMessage;
        string[] selecao;
        string[] selecaocolaborador;
        string[] selecaocolaboradorvalor;
        public ArrayAdapter adapter { get; private set; }
        private SQLiteDatabase sqldTemp;
        private CarreDadosComvidados item;
        Spinner spinnercolaborador;
        Spinner spinnerconsome;
        BancoDados mdTemp;
        string[] nomes;
        string[] nomesColaborador;
        string firstItemColaborador = "";
        int PosicaoColaborador = 0;
        string firstItem = "";
        
        int Posicao = 0;
        public EditText cnpjcpf { get; set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string sSQLQuery = "";

            SetContentView(Resource.Layout.layoutConvidado);

            SetResult(Result.Canceled);
            //==========================================================//
            //              CRIAR BANCO DE DADOS                        //
            //==========================================================//
            mdTemp = new BancoDados("PRINCIPAL");
            mdTemp.CreateDatabases("PRINCIPAL");
            //==========================================================//


            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "CADASTRO DE CONVIDADOS";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            var relogioDigital = FindViewById<DigitalClock>(Resource.Id.digitalClock2);
            relogioDigital.Visibility = Android.Views.ViewStates.Visible;

            Android.Support.V7.Widget.AppCompatImageButton BtnSair = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnSairConvidado);
            Android.Support.V7.Widget.AppCompatImageButton BtnSalvar = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnSalvarConvidado);
            Android.Support.V7.Widget.AppCompatImageButton BtnExcluir = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.BtnExcluirConvidado);
            TextView Codigo = FindViewById<TextView>(Resource.Id.TxtCodigoColaborador);
            EditText Nome = FindViewById<EditText>(Resource.Id.TxtNomeConvidado);






            sSQLQuery = "";
            string sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sDB = Path.Combine(sLocation, "PRINCIPAL");
            sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
            bool bIsExists = File.Exists(sDB);
            bool Consome = true;

            spinnercolaborador = FindViewById<Spinner>(Resource.Id.spinnercolaborador);
            //==============================================================================================//
            //                             VERIFICA COLABORADORES                                            //
            //==============================================================================================//
            sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            sDB = Path.Combine(sLocation, "PRINCIPAL");
            sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
            bIsExists = File.Exists(sDB);
            Android.Database.ICursor icursorTemp = null;
            sSQLQuery = $"SELECT _id,NOME FROM COLABORADOR ";
            icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
            int ValorCursor = icursorTemp.Count;
            selecaocolaborador = new string[ValorCursor];
            selecaocolaboradorvalor = new string[ValorCursor];

            for (int i = 0; i < icursorTemp.Count; i++)
            {
                icursorTemp.MoveToNext();
                item = new CarreDadosComvidados();
                selecaocolaboradorvalor[i]= icursorTemp.GetString(0);
                selecaocolaborador[i] = icursorTemp.GetString(1);
            }

              
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, selecaocolaborador);
            spinnercolaborador.Adapter = adapter;
            spinnercolaborador.ItemSelected += (s, e) =>
            {
                firstItemColaborador = spinnercolaborador.SelectedItem.ToString();
                if (firstItemColaborador.Equals(spinnercolaborador.SelectedItem.ToString()))
                {
                    PosicaoColaborador = spinnercolaborador.SelectedItemPosition;
                    Validacoes.ValorCampoInt =Convert.ToInt32(selecaocolaboradorvalor[PosicaoColaborador]);
                }
                else
                {
                    Toast.MakeText(this, "You have selected: " + e.Parent.GetItemIdAtPosition(e.Position).ToString(),
                        ToastLength.Short).Show();
                }
            };
          



            spinnerconsome = FindViewById<Spinner>(Resource.Id.spinnerconsomeconvidado);
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
                    Toast.MakeText(this, "Deve ser digitado um nome de Convidado !", ToastLength.Short).Show();
                    return;
                }
                //===================================================================//
                //          VERIFICA SE O COLABORADOR JA TEM 01 CONVIDADO            //
                //===================================================================//
                Android.Database.ICursor icursorTemp = null;
                sSQLQuery = $"SELECT _id FROM CONVIDADO WHERE " +
                            $" ID_COLABORADOR={Validacoes.ValorCampoInt} "+
                            $" AND CONSOME_BEBIDA='{spinnerconsome.SelectedItem.ToString()}'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                if (ValorCursor > 0)
                {
                    Toast.MakeText(this, "Só é permitido 01 convidado !", ToastLength.Short).Show();
                    return;
                }

                //==============================================================================================//
                //                             VERIFICA SE JÁ EXISTE UM CONVIDADO GRAVADO                        //
                //==============================================================================================//
                sLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                sDB = Path.Combine(sLocation, "PRINCIPAL");
                sqldTemp = SQLiteDatabase.OpenOrCreateDatabase(sDB, null);
                bIsExists = File.Exists(sDB);
               
                sSQLQuery = $"SELECT _id FROM CONVIDADO WHERE _id='{Codigo.Text}'";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                ValorCursor = icursorTemp.Count;
                string Data = System.DateTime.Today.ToString();
               
                



                if (ValorCursor == 0)
                {
                

                    string Valores = "'" + Nome.Text + "'," +
                                    Validacoes.ValorCampoInt+",'"+
                                    spinnerconsome.SelectedItem.ToString() + "','" +
                                    Data + "'";

                    string Campos = "NOME," +
                                    "ID_COLABORADOR,"+
                                    "CONSOME_BEBIDA," +
                                    "DATA_CADASTRO";

                    sSQLQuery = "INSERT INTO " +
                        "CONVIDADO " + "(" + Campos + ") " +
                        "VALUES(" + Valores + ");";
                    sqldTemp.ExecSQL(sSQLQuery);
                    sMessage = "Record is saved.";
                    Toast.MakeText(this, "REGISTRO GRAVADO COM SUCESSO", ToastLength.Long).Show();
                }
                else
                {
                    string ComandoSql = "update CONVIDADO set " +
                                                   $" NOME='{Nome.Text}'," +
                                                   $" CONSOME_BEBIDA='{ spinnerconsome.SelectedItem.ToString()}'" +
                                                   $" where _id={Codigo.Text}";

                    sqldTemp.ExecSQL(ComandoSql);
                    Toast.MakeText(this, "Atualização realizada com sucesso", ToastLength.Long).Show();
                }
                Codigo.Text = "";
                Nome.Text = "";
                spinnerconsome.SetSelection(0);
                spinnercolaborador.SetSelection(0);
                CarregaListaColaborador();
                CarregaListaColaborador();
            };
            BtnExcluir.Click += delegate
            {
                if (Codigo.Text == "")
                {
                    Toast.MakeText(this, "Deve ser selecionado um convidado!", ToastLength.Short).Show();
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
                sSQLQuery = $"SELECT _id FROM convidado WHERE _id={Codigo.Text}";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                string Data = System.DateTime.Today.ToString();

                if (ValorCursor != 0)
                {
                    string ComandoSql = $"DELETE from CONVIDADO  where _id={Codigo.Text}";
                    sqldTemp.ExecSQL(ComandoSql);
                    Toast.MakeText(this, "Registro Excluido com sucesso", ToastLength.Long).Show();
                }
                Codigo.Text = "";  
                Nome.Text = "";
                spinnerconsome.SetSelection(0);
                spinnercolaborador.SetSelection(0);
                CarregaListaColaborador();
            };


            void CarregaListaColaborador()
            {
                Android.Database.ICursor icursorTemp = null;
                sSQLQuery = "SELECT _id,NOME,CONSOME_BEBIDA " +
                 " FROM CONVIDADO ";
                icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);
                int ValorCursor = icursorTemp.Count;
                if (ValorCursor > 0)
                {
                    nomes = new string[icursorTemp.Count];
                    nomes = new string[icursorTemp.Count];

                    for (int i = 0; i < icursorTemp.Count; i++)
                    {
                        icursorTemp.MoveToNext();
                        item = new CarreDadosComvidados();
                        item.Id = icursorTemp.GetString(0);
                        item.nome = icursorTemp.GetString(1);
                        item.StringConsome = icursorTemp.GetString(2);

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
                    sSQLQuery = "SELECT T1._id," +
                    " T1.NOME, " +
                    " T1.CONSOME_BEBIDA, " +
                    " T2.NOME " +
                    " FROM CONVIDADO T1" +
                    " LEFT JOIN COLABORADOR T2 ON" +
                    " T2._id=T1.ID_COLABORADOR "+
                    $" WHERE T1._id={Convert.ToInt16(Validacoes.ValorItem.Substring(0, 6))}";

                    icursorTemp = sqldTemp.RawQuery(sSQLQuery, null);

                    ValorCursor = icursorTemp.Count;
                    if (ValorCursor > 0)
                    {
                        icursorTemp.MoveToNext();
                        Codigo.Text = icursorTemp.GetString(0);
                        Nome.Text = icursorTemp.GetString(1);
                        firstItem = icursorTemp.GetString(2);
                        for (int i = 0; spinnerconsome.Count > i; i++)
                        {
                            spinnerconsome.SetSelection(i);
                            if (firstItem.Equals(spinnerconsome.SelectedItem.ToString()))
                            {
                                break;
                            }
                        }
                        firstItemColaborador = icursorTemp.GetString(3);
                        for (int i = 0; spinnercolaborador.Count > i; i++)
                        {
                            spinnercolaborador.SetSelection(i);
                            if (firstItemColaborador.Equals(spinnercolaborador.SelectedItem.ToString()))
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



    public class CarreDadosComvidados
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
