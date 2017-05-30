using Android.App; 
using Android.OS;
using Android.Widget;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App2
{
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/tatras_logo")]
    public class MainActivity : Activity
    {
        TextView initText;
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            initText = FindViewById<TextView>(Resource.Id.initText);
            Connection();
        }

        async void Connection()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            // tools na pripajanie
            ConnectionManager manager = ConnectionManager.GetInstance();

            var connection = Task.Factory.StartNew(()=> {
                //animacne vlakno
                Task.Factory.StartNew(() =>
                {
                    Animation(token);
                }, token);
                //Pripojenie na server
                if (manager.Connect()) {
                    manager.Hello();
                    // ukoncenie animacie
                    tokenSource.Cancel();
                    return true;
                }
                //ukoncenie animacie
                tokenSource.Cancel();
                return false;
            });
            await connection;
            // po pripojeni na server prepni view
            await connection.ContinueWith((x) => {
                if (x.Result == false) {
                    //Error
                    return;
                }
                //prepnutie aktivity
                RunOnUiThread(() => {
                    StartActivity(typeof(LoginActivity1));
                });
            });
        }

        private void Animation(CancellationToken cancel) {
            string[] values = { "Pripajam" , "Pripajam.", "Pripajam..", "Pripajam..."};
            int index = 0;
            while (true) {
                //ukoncenie behu vlakna
                if (cancel.IsCancellationRequested == true) 
                    break;
                // updateovanie textu v hlavnom vlakne
                RunOnUiThread(() => {
                    UpdateText(values[index++ % values.Length]);
                });
                //spanie vlakna
                Thread.Sleep(700);
            }
        }

        private void UpdateText(string text) {
            initText.Text = text;
        }

        private void ConnectionError() {

        }
    }
}

