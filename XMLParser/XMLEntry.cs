using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLParser
{
    class XMLEntry
    {
        //element <something> </something> : something
        //attribute category="something"
        //text <something>hello</soething> : hello

        private string _Element;
        private string _Attributes;
        private string _Text;
        private string _XPath;

        public string Element
        {
            get { return _Element; }
            set { _Element = value; }
        }

        public string Attributes
        {
            get { return _Attributes; }
            set { _Attributes = value; }
        }

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        public string XPath
        {
            get { return _XPath; }
            set { _XPath = value; }
        }


        public XMLEntry(string element, IEnumerable<XAttribute> attributes, string text, string xPath)
        {
            _Element = element;
            //_Attribute = attribute;
            _Text = text;
            _XPath = xPath;
            foreach (XAttribute attr in attributes)
            {
                if(attributes.First() == attr) { _Attributes = attr.Name + ":" + attr.Value; }
                else { _Attributes += ", " + attr.Name + ":" + attr.Value; }
            }
        }

        public override string ToString()
        {
            return String.Format("Ele:{0}, Atr{1}, Text{2}, XPath{3}", this.Element, this.Attributes, this.Text, this.XPath);
        }

    }
}
