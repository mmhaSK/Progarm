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
    interface IConnection
    {

        bool Connect();
        void WriteData(string data);
        string ReadData();
        void Hello();
    }
}