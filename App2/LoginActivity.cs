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

namespace App2
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextView _pass;
        TextView _user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            base.SetContentView(Resource.Layout.Login);
            //nastavenie akcie
            Button singin = FindViewById<Button>(Resource.Id.sigin);
            singin.Click += delegate
            {
                SignIn();
            };
            //nastavenie udajov
            _pass = FindViewById<Button>(Resource.Id.pswd);
            _user = FindViewById<Button>(Resource.Id.usrname);
        }

        private void SignIn() {

        }
    }
}