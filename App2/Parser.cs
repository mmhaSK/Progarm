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
    class Parser
    {
        /*
         * 1 - otvorene
         *<I><O>1</O><R>0</R></I> Init 
         *<C><O>1</O></C> // otvori okno 
         **/
        public bool IsValid(string input) {
            string[] arr = input.Split(new char[] { '>', '<' });
            int l = 0, r = arr.Length - 1;
            while (l < r) {
                string f = arr[l++];
                string s = arr[r--];
                if (s.Equals("/" + f))
                    return false;
            }
            return true;
        }

        public void Parse() {

        }
    }
}