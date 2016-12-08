﻿namespace DataBrain.Core.Logging
{
    public class Facet
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public Facet() { }

        public Facet(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}