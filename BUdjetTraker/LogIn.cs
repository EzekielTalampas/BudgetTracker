using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Icu.Text.IDNA;

namespace BUdjetTraker {
    [Activity(Label = "Budjet Tracker", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LogIn : AppCompatActivity {
        private Button logInbtn, createbtn;
        private EditText username, password;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.logIn);

            logInbtn = FindViewById<Button>(Resource.Id.login_button);
            createbtn = FindViewById<Button>(Resource.Id.create_account_button);
            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);

            logInbtn.Click += (s, e) => login("login", new string[] { "Successful Login!", "Unsuccessful Login!" });
            createbtn.Click += (s, e) => login("create", new string[] { "Account Created!", "Account not Created!" });
        }

        public void login(string cmd, string[] message) {
            string[] txtField = { username.Text, password.Text };
            if (txtField.Any(item => string.IsNullOrWhiteSpace(item))) {
                Toast.MakeText(this, "All fields must be filled out!", ToastLength.Long).Show();
                return;
            }

            string request = new XampProperties($"{cmd}.php?username=" + username.Text + "&password=" + password.Text).CreateResponse();
            if (request.Contains("accept")) {
                Toast.MakeText(this, message[0], ToastLength.Long).Show();
                Intent i = new Intent(this, typeof(MainActivity));
                i.PutExtra("username", username.Text);
                StartActivity(i);
            } else {
                Toast.MakeText(this, message[1], ToastLength.Long).Show();
            }

        }
    }
}