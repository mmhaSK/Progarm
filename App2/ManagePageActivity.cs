using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace App2
{
    [Activity(Label = "ManagePageActivity")]
    public class ManagePageActivity : Activity
    {
        private TextView errorMessage;
        private ToggleButton window;
        private ToggleButton curtains;
        private bool inProgress = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ManagePage);
            errorMessage = FindViewById<TextView>(Resource.Id.errorMessage);
            window = FindViewById<ToggleButton>(Resource.Id.windowState);
            curtains = FindViewById<ToggleButton>(Resource.Id.curtainsState);
            window.Click += (o, e) => {
                if (inProgress)
                    return;
                if(window.Checked)
                    SendRequest("O=1");
                else
                    SendRequest("O=0");
            };
            curtains.Click += (o, e) => {
                if (inProgress)
                    return;
                if (curtains.Checked)
                    SendRequest("R=1");
                else
                    SendRequest("R=0");
            };
        }
        
        private async void SendRequest(string request) {
            inProgress = true;
            ConnectionManager manager = ConnectionManager.GetInstance();
            await Task.Factory.StartNew(() =>
            {
                if (!manager.isOpen()) {
                    RunOnUiThread(() =>
                    {
                        ConnectionError();
                    });
                    return;
                }
                string parsed = Parser.makeRequest(request);
                manager.WriteData(parsed);
                string respond = manager.ReadData();
                if (respond == null) {
                    manager.Close();
                    RunOnUiThread(() =>
                    {
                        ConnectionError();
                    });
                    return;
                }
                UpdateStates(Parser.Parse(respond));
                inProgress = false;
            });
        }

        private void UpdateStates(State s) {
            if (s == null)
            {
                UpdateError("Neplatna odpoved.");
            }
            RunOnUiThread(() =>
            {
                window.Checked = s.okna;
                curtains.Checked = s.rolety;
            });
        }

        private void UpdateError(String message) {
            RunOnUiThread(() =>
            {
                errorMessage.Text = message;
            });
        }

        private void ConnectionError()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Nie je mozne sa pripojit");
            alert.SetMessage("Skontrolujte bod pripojenia");
            alert.SetPositiveButton("Skusit znova", (senderAlert, args) => {
                    StartActivity(typeof(MainActivity));
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}