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
using System.Collections;

namespace App2
{
    class AccountManager
    {
        /// <summary>
        /// Tabulka obsahuje vsetky mena a hesla registrovanych uzovatelov
        /// </summary>
        private Hashtable hashtable = new Hashtable();

        public void AddAccount(string username, string passHash) {
            hashtable[username] = passHash;
        }
        /// <summary>
        /// Metoda zmeni heslo uzivatela.
        /// </summary>
        /// <param name="username"> Uzivatelske meno ktoremu sa ma zmenit heslo</param>
        /// <param name="passHash"> Nove heslo uzivatela upravene hash funkciou</param>
        public void ChangePassword(string username, string passHash) {
            if (hashtable.Contains(username)) {
                hashtable[username] = passHash;
            }
        }
        /// <summary>
        /// Metoda zisti ci je uzivatel zadany parametrami zaregistovany
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passHash"></param>
        /// <returns> true ak je uzivatel zaregistrovany inak false</returns>
        public bool IsRegistered(string username, string passHash) {
            string hash = (string) hashtable[username];
            return (hash != null && hash.Equals(passHash));
        }
        
    }
}