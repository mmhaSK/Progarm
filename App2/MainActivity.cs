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
            StartActivity(typeof(LoginActivity));
            //Connection();
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
            // po pripojeni na server sparsuj data
            var parse = connection.ContinueWith((isConnected) => {
                if (isConnected.Result == false)
                {
                    RunOnUiThread(() =>
                    {
                        ConnectionError();
                    });
                    return 2;
                }
                return 0; //dummy parsovanie init 
            });
            // po sparsovani prepni view
            await parse.ContinueWith((x) => {
                if (x.Result == 1) { // chyba parsovania nespravne data
                    //uzatvorenie streamu
                    ConnectionManager.GetInstance().Close();
                    //zobrazenie chybovej hlasky
                    RunOnUiThread(() => {
                        ConnectionError();
                    });
                    return;
                }
                else if (x.Result == 2) { // chyba pripojenia 
                    //nic sa nevykona
                    //chybova sprava uz je zobrazena
                    return;
                }
                // korektny vysledok
                //prepnutie aktivity
                RunOnUiThread(() => {
                    StartActivity(typeof(LoginActivity));
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
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Nie je mozne sa pripojit");
            alert.SetMessage("Skontrolujte bod pripojenia");
            alert.SetPositiveButton("Skusit znova", (senderAlert, args) => {
                //znovupripajanie
                Connection();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}

