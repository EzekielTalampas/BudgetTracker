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
        private TextView totalAmountTextView;
        private Button homebtn, sumbtn, addButton;
        private EditText expenseNameEditText;
        private LinearLayout expenseContainer;

        Intent i;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.homepage);

            totalAmountTextView = FindViewById<TextView>(Resource.Id.totalAmountTextView);
            homebtn = FindViewById<Button>(Resource.Id.homeButton);
            sumbtn = FindViewById<Button>(Resource.Id.sumButton);
            addButton = FindViewById<Button>(Resource.Id.addButton);
            expenseNameEditText = FindViewById<EditText>(Resource.Id.expenseNameEditText);
            expenseContainer = FindViewById<LinearLayout>(Resource.Id.expenseContainer);

            sumbtn.Click += gotoSum;
            addButton.Click += AddExpense;

            LoadExpenses();
        }

        public void gotoSum(object sender, EventArgs e)
        {
            i = new Intent(this, typeof(Summary));
            StartActivity(i);
        }

        public void AddExpense(object sender, EventArgs e)
        {
            string expenseName = expenseNameEditText.Text;
            if (!string.IsNullOrWhiteSpace(expenseName))
            {
                // Create a new layout for the expense
                LinearLayout expenseLayout = new LinearLayout(this)
                {
                    Orientation = Orientation.Horizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
                };

                // Create a TextView for the expense name
                TextView expenseTextView = new TextView(this)
                {
                    Text = $"{expenseName}: ₱ 0.00",
                    TextSize = 18,
                    LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
                };

                // Create a Button to add amount
                Button increaseButton = new Button(this)
                {
                    Text = "+",
                    LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
                };

                // Create a Button to subtract amount
                Button decreaseButton = new Button(this)
                {
                    Text = "-",
                    LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
                };

                // Initialize the amount
                decimal amount = 0.00m;

                // Set up click event for the increase button
                increaseButton.Click += (s, args) =>
                {
                    ShowAmountDialog("Add Amount", (addedAmount) =>
                    {
                        amount += addedAmount;
                        expenseTextView.Text = $"{expenseName}: ₱ {amount:0.00}";
                        SaveExpense(expenseName, amount);
                    });
                };

                // Set up click event for the decrease button
                decreaseButton.Click += (s, args) =>
                {
                    ShowAmountDialog("Subtract Amount", (subtractedAmount) =>
                    {
                        amount = Math.Max(0, amount - subtractedAmount);
                        expenseTextView.Text = $"{expenseName}: ₱ {amount:0.00}";
                        SaveExpense(expenseName, amount);
                    });
                };

                // Add the TextView and Buttons to the expense layout
                expenseLayout.AddView(expenseTextView);
                expenseLayout.AddView(decreaseButton);
                expenseLayout.AddView(increaseButton);

                // Add the expense layout to the container
                expenseContainer.AddView(expenseLayout);

                // Clear the expense name input field
                expenseNameEditText.Text = string.Empty;
            }
            else
            {
                Toast.MakeText(this, "Please enter an expense name.", ToastLength.Short).Show();
            }
        }

        private void ShowAmountDialog(string title, Action<decimal> onAmountEntered)
        {
            // Create an EditText to input the amount
            EditText input = new EditText(this)
            {
                InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal
            };

            // Create and show the AlertDialog
            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            dialog.SetTitle(title);
            dialog.SetView(input);
            dialog.SetPositiveButton("OK", (sender, e) =>
            {
                string amountText = input.Text;
                if (decimal.TryParse(amountText, out decimal amount))
                {
                    onAmountEntered(amount);
                }
                else
                {
                    Toast.MakeText(this, "Invalid amount entered.", ToastLength.Short).Show();
                }
            });
            dialog.SetNegativeButton("Cancel", (sender, e) => { });
            dialog.Show();
        }

        private void SaveExpense(string name, decimal amount)
        {
            var prefs = GetSharedPreferences("Expenses", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString(name, amount.ToString());
            editor.Apply();
        }

        private void LoadExpenses()
        {
            var prefs = GetSharedPreferences("Expenses", FileCreationMode.Private);
            var allExpenses = prefs.All;
            foreach (var expense in allExpenses)
            {
                string name = expense.Key;
                string amountStr = expense.Value.ToString();
                if (decimal.TryParse(amountStr, out decimal amount))
                {
                    AddExpenseToLayout(name, amount);
                }
            }
        }

        private void AddExpenseToLayout(string name, decimal amount)
        {
            // Create a new layout for the expense
            LinearLayout expenseLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            };

            // Create a TextView for the expense name
            TextView expenseTextView = new TextView(this)
            {
                Text = $"{name}: ₱ {amount:0.00}",
                TextSize = 18,
                LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
            };

            // Create a Button to add amount
            Button increaseButton = new Button(this)
            {
                Text = "+",
                LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
            };

            // Create a Button to subtract amount
            Button decreaseButton = new Button(this)
            {
                Text = "-",
                LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
            };

            // Set up click event for the increase button
            increaseButton.Click += (s, args) =>
            {
                ShowAmountDialog("Add Amount", (addedAmount) =>
                {
                    amount += addedAmount;
                    expenseTextView.Text = $"{name}: ₱ {amount:0.00}";
                    SaveExpense(name, amount);
                });
            };

            // Set up click event for the decrease button
            decreaseButton.Click += (s, args) =>
            {
                ShowAmountDialog("Subtract Amount", (subtractedAmount) =>
                {
                    amount = Math.Max(0, amount - subtractedAmount);
                    expenseTextView.Text = $"{name}: ₱ {amount:0.00}";
                    SaveExpense(name, amount);
                });
            };

            // Add the TextView and Buttons to the expense layout
            expenseLayout.AddView(expenseTextView);
            expenseLayout.AddView(decreaseButton);
            expenseLayout.AddView(increaseButton);

            // Add the expense layout to the container
            expenseContainer.AddView(expenseLayout);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
