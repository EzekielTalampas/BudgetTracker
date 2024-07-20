using Android.Accounts;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BUdjetTraker {
    internal class ExpenseTab {

        string name;
        string user;
        double cost;
        TextView total;
        Context _MainActivity;
        LinearLayout expenseContainer;
        public ExpenseTab(string name, string cost, Context _MainActivity, LinearLayout expenseContainer, string username) {
            this.name = name;
            this.cost = double.Parse(cost);
            this._MainActivity = _MainActivity;
            this.expenseContainer = expenseContainer;
            this.user = username;
        }

        public void AddExpense() {
            // Create a new layout for the expense
            LinearLayout expenseLayout = new LinearLayout(_MainActivity) {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            };

            // Create a TextView for the expense name
            TextView expenseTextView = new TextView(_MainActivity) {
                Text = $"{name}: ₱ {cost.ToString("F2")}",
                TextSize = 18,
                LayoutParameters = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1)
            };

            // Create a Button to add amount
            Button increaseButton = new Button(_MainActivity) {
                Text = "+",
                LayoutParameters = new LinearLayout.LayoutParams(100, LinearLayout.LayoutParams.WrapContent, 0)
            };

            // Create a Button to subtract amount
            Button decreaseButton = new Button(_MainActivity) {
                Text = "-",
                LayoutParameters = new LinearLayout.LayoutParams(100, LinearLayout.LayoutParams.WrapContent, 0)
            };


            // Set up click event for the increase button
            increaseButton.Click += (s, args) =>
            {
                ShowAmountDialog("Add Amount", (addedAmount) =>
                {
                    cost += addedAmount;
                    total.Text = (double.Parse(total.Text) + addedAmount).ToString();
                    new XampProperties($"update.php?username={user}&category={name}&cost={cost}").CreateResponse();
                    expenseTextView.Text = $"{name}: ₱ {cost:0.00}";
                    //code here to save
                });
            };

            // Set up click event for the decrease button
            decreaseButton.Click += (s, args) =>
            {
                ShowAmountDialog("Subtract Amount", (subtractedAmount) =>
                {
                    total.Text = (double.Parse(total.Text) - cost).ToString(); 
                    cost = Math.Max(0, cost - subtractedAmount);
                    total.Text = (double.Parse(total.Text) + cost).ToString(); 
                    new XampProperties($"update.php?username={user}&category={name}&cost={cost}").CreateResponse();
                    expenseTextView.Text = $"{name}: ₱ {cost:0.00}";
                    //code here to save
                });
            };

            // Add the TextView and Buttons to the expense layout
            expenseLayout.AddView(expenseTextView);
            expenseLayout.AddView(decreaseButton);
            expenseLayout.AddView(increaseButton);

            // Add the expense layout to the container
            expenseContainer.AddView(expenseLayout);
            
        }
        
        private void ShowAmountDialog(string title, Action<double> onAmountEntered) {
            // Create an EditText to input the amount
            EditText input = new EditText(_MainActivity) {
                InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal
            };

            // Create and show the AlertDialog
            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(_MainActivity);
            dialog.SetTitle(title);
            dialog.SetView(input);
            dialog.SetPositiveButton("OK", (sender, e) => {
                string amountText = input.Text;
                if (double.TryParse(amountText, out double amount)) {
                    onAmountEntered(amount);
                } else {
                    Toast.MakeText(_MainActivity, "Invalid amount entered.", ToastLength.Short).Show();
                }
            });
            dialog.SetNegativeButton("Cancel", (sender, e) => { });
            dialog.Show();
        }

        public void SetTotalCost(TextView total) {
            this.total = total;
        }
    }
}