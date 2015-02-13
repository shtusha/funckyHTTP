using System;

namespace FunckyApp.Models
{
    public class Link
    {
        public Link(){} //need one for serialization.

        public Link(string rel, string href)
        {
            Rel = rel;
            Href = href;
        }

        public string Rel { get; set; }
        public string Href { get; set; }
    }
}