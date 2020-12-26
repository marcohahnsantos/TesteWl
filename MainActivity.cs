using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Timers;
using Android.Database.Sqlite;
using Android.Content.PM;
using Android.Content;
using Android.Bluetooth;
using System;
using Android.Widget;
using Android.Views;
using Acr.UserDialogs;

namespace TESTEWL
{
    [Activity(Label = "TESTE WL",Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        Timer timer = null;
       
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.activity_main);
     
            timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            // StartScan();
         
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Dispose();
            Finish();
            StartActivity(typeof(ActPrincipal));

        }
    }
}