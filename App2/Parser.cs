using System.Linq;

namespace App2
{
    class Parser
    {
        /*
         * 1 - otvorene
         *<I><O>1</O><R>0</R></I> Init 
         *<C><O>1</O></C> // command- otvori okno 
         **/
        /// <summary>
        /// Sparsuje string na objekt triedy State.
        /// V pripade zleho vstupu vyvola vyminku
        /// </summary>
        private static string init = "I,O,R";


        public static State Parse(string input)
        {
            if (input.Trim().Length == 0)
                return null;
            State state = new State();
            bool correct = Validate(state, "", input);
            init = "I,O,R";
            return correct ? state : null;
        }

        private static bool Validate(State state, string last_tag, string input)
        {
            if (input.Trim().Length == 0)
            {
                return true;
            }
            if (input.IndexOf("<") < 0)
            {
                if (state != null && last_tag == "<R>")
                {
                    state.rolety = input.Equals("1");
                }
                else if (state != null && last_tag == "<O>")
                {
                    state.okna = input.Equals("1");
                }
                else
                {
                    return false;
                }
                return true;
            }
            string tag = input.Substring(0, 3);
            if (!init.Contains(tag[1]))
            {
                return false;
            }
            else
            {
                init = init.Replace(tag[1], ' ');
                string end_tag = "</" + tag[1] + ">";
                int to = input.IndexOf(end_tag);
                if (to < 0)
                {
                    return false;
                }
                if (!Validate(state, tag, input.Substring(tag.Length, to - end_tag.Length + 1)))
                    return false;
                return Validate(state, last_tag, input.Substring(to + end_tag.Length, input.Length - to - end_tag.Length));
            }
        }
        /// <summary>
        /// Tvar vstupu argumentu <paramref name="input"/> O=1 | R=1
        ///
        /// </summary>
        /// <returns></returns>
        public static string makeRequest(string input) {
            string[] arr = input.Split(new char[] { '=' });
            return "<C><"+arr[0]+">"+arr[1]+"</" + arr[0] + "></C>";
        }

    }
}