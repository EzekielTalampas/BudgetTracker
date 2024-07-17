using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace BUdjetTraker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText foodEditText, transportationEditText, schoolEditText, entertainmentEditText, savingsEditText;
        private TextView totalAmountTextView;
        private Button foodbtn, transpobtn, schoolbtn, entertainbtn, savingsbtn, homebtn, sumbtn;

        Intent i;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.homepage);

            
            foodEditText = FindViewById<EditText>(Resource.Id.foodEditText);
            transportationEditText = FindViewById<EditText>(Resource.Id.transportationEditText);
            schoolEditText = FindViewById<EditText>(Resource.Id.schoolEditText);
            entertainmentEditText = FindViewById<EditText>(Resource.Id.entertainmentEditText);
            savingsEditText = FindViewById<EditText>(Resource.Id.savingsEditText);
            totalAmountTextView = FindViewById<TextView>(Resource.Id.totalAmountTextView);

            
            foodbtn = FindViewById<Button>(Resource.Id.foodButton);
            transpobtn = FindViewById<Button>(Resource.Id.transportationButton);
            schoolbtn = FindViewById<Button>(Resource.Id.schoolButton);
            entertainbtn = FindViewById<Button>(Resource.Id.entertainmentButton);
            savingsbtn = FindViewById<Button>(Resource.Id.savingsButton);
            homebtn = FindViewById<Button>(Resource.Id.homeButton);
            sumbtn = FindViewById<Button>(Resource.Id.sumButton);


            sumbtn.Click += gotoSum;

            
        }

        public void gotoSum(object sender, EventArgs e)
        {
            i = new Intent(this, typeof(Summary));
            StartActivity(i);
        }

        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
