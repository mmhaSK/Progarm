using Android.App;
using Android.Widget;
using Android.OS;
using Java.Lang;
using System;
using System.Threading.Tasks;

namespace App2
{
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/tatras_logo")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.button1);
            TextView text = FindViewById<TextView>(Resource.Id.initText);
            button.Click += delegate
            {
                text.Text = "asdad";
            };
            DoWorkAsync();

        }


        async void DoWorkAsync()
        {
            await Task.Delay(5000);
            StartActivity(typeof(LoginActivity1));
        }
    }
}

