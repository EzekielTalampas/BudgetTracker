using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Text.Json;
using System;
using static Java.Util.Jar.Attributes;

namespace BUdjetTraker {
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {
        private TextView totalAmountTextView;
        private Button homebtn, sumbtn, addButton;
        private EditText expenseNameEditText;
        private LinearLayout expenseContainer;

        Intent i;
        String username;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);                                                         
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.homepage);

            totalAmountTextView = FindViewById<TextView>(Resource.Id.totalAmountTextView);
            homebtn = FindViewById<Button>(Resource.Id.homeButton);
            addButton = FindViewById<Button>(Resource.Id.addButton);
            expenseNameEditText = FindViewById<EditText>(Resource.Id.expenseNameEditText);
            expenseContainer = FindViewById<LinearLayout>(Resource.Id.expenseContainer);

            username = Intent.GetStringExtra("username");

            //clearout
            expenseContainer.RemoveAllViews();

            //load user's data
            String results = new XampProperties("load.php?username=" + username).CreateResponse();
            using JsonDocument info = JsonDocument.Parse(results);
            JsonElement root = info.RootElement;
            double totalCost = 0;
            if (root.ValueKind == JsonValueKind.Array) {
                for (int i = 0; i < root.GetArrayLength(); i++) {
                    double cost = double.Parse(root[i].GetProperty("cost").ToString());
                    totalCost += cost;
                    ExpenseTab _tab = new ExpenseTab(root[i].GetProperty("category").ToString(), cost.ToString(), this, expenseContainer, username);
                    _tab.AddExpense();
                    _tab.SetTotalCost(totalAmountTextView);
                }
            }
            totalAmountTextView.Text = totalCost.ToString();

            //sumbtn.Click += gotoSum;
            addButton.Click += (s, e) => {
                string category = expenseNameEditText.Text;

                if (string.IsNullOrWhiteSpace(category)) {
                    Toast.MakeText(this, "Name must be filled out!", ToastLength.Long).Show();
                    return;
                }

                String results = new XampProperties("add.php?username=" + username + "&category=" + category + "&cost=0").CreateResponse();
                if (results.Contains("accept")) {
                    Toast.MakeText(this, "Added!", ToastLength.Long).Show();
                    ExpenseTab _tab = new ExpenseTab(category, "0.00", this, expenseContainer, username);
                    _tab.AddExpense();
                    _tab.SetTotalCost(totalAmountTextView);
                } else {
                    Toast.MakeText(this, "Failed!", ToastLength.Long).Show();
                }
            };

            homebtn.Click += (s, e) => Finish();
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
