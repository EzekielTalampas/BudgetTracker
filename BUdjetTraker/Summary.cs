using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BUdjetTraker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Summary : AppCompatActivity
    {
        private Button homebtn, resetbtn;
        Intent i;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Summary);


            homebtn = FindViewById<Button>(Resource.Id.homeButton);
            resetbtn = FindViewById<Button>(Resource.Id.resetbutton);



            homebtn.Click += gotoHome;

        }
        public void gotoHome(object sender, EventArgs e)
        {
            i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}