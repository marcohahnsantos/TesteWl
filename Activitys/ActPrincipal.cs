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

using Acr.UserDialogs;
using Android;
using Android.Bluetooth;
using Java.Lang;

namespace TESTEWL
{
    [Activity(Label = "ActPrincipal", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ActPrincipal : AppCompatActivity
    {

        public SQLiteDatabase sqldTemporario;
        /// <summary>
        /// The sSQLquery for query handling.
        /// </summary>

        /// <summary>
        /// The sMessage to hold message.
        /// </summary>
        private string sMessage;
        private bool bDBIsAvailable;
        BancoDados mdTemp;
        public SQLiteDatabase sqldTemp;
        Android.Database.ICursor icursorTemp = null;
        string sSQLQuery = "";
        BluetoothSocket[] socket = new BluetoothSocket[1];

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutprincipal);
            //==========================================================//
            //              CRIAR BANCO DE DADOS                        //
            //==========================================================//
            mdTemp = new BancoDados("PRINCIPAL");
            mdTemp.CreateDatabases("PRINCIPAL");
            //==========================================================//
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "SISTEMA CONTROLE DE CHURRASCO";
            var relogioDigital = FindViewById<DigitalClock>(Resource.Id.digitalClock1);
            relogioDigital.Visibility = Android.Views.ViewStates.Visible;
            Android.Support.V7.Widget.AppCompatButton BtnColaborador = FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.BtnColaboradorCad);
            Android.Support.V7.Widget.AppCompatButton BtnConvidado = FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.BtnConvidadoCad);
            Android.Support.V7.Widget.AppCompatButton BtnGastos = FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.BtnGastos);
            Android.Support.V7.Widget.AppCompatButton BtnSair = FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.BtnSair);
         

            if ((int)Build.VERSION.SdkInt < 23)
            {
                // return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[]
                    { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
         
            BtnSair.Click += delegate
            {
                BtnSair.Enabled = false;
                this.FinishAffinity();
                //Finish();
            };
            BtnColaborador.Click += delegate
            {
                StartActivity(typeof(ActcadastroColaborador));
               // this.FinishAffinity();
                //Finish();
            };
            BtnConvidado.Click += delegate
            {
                StartActivity(typeof(ActCadastroConvidados));
            };
            BtnGastos.Click += delegate
            {
               StartActivity(typeof(ActGastos));
            };
        }
        public async void Aguarde()
        {

            using (UserDialogs.Instance.Loading("Aguarde...", null, null, true, MaskType.Black))
            {
                await System.Threading.Tasks.Task.Delay(9000);
            }

        }
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
            
        //   // MenuInflater.Inflate(Resource.Menu.home, menu);
        //   // return base.OnCreateOptionsMenu(menu);
        //}
       
    }
}