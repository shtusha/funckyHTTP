using System;

namespace FunckyApp.Core
{
    public class Fragment
    {
        private string _inflatedText;

        public Fragment(string original, string inflated = null)
        {
            if(original == null) {throw new ArgumentNullException("original");}

            OriginalText = original;
            _inflatedText = inflated;


            //If first letter of original string is uppercase so should be first letter of indlated text.
            if (!string.IsNullOrEmpty(inflated))
            {
                if (char.IsUpper(OriginalText[0]))
                {
                    _inflatedText = _inflatedText.Substring(0,1).ToUpper() + _inflatedText.Substring(1, _inflatedText.Length -1);
                } 
            }
        }


        public string OriginalText { get; private set; }

        public string InflatedText
        {
            get
            {
                return _inflatedText ?? OriginalText;
            }
        }

        public bool IsInflated { get { return _inflatedText != null; } }
        
    }
}