using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace XMLParser
{
    class Program
    {
        static List<XMLEntry> entries = new List<XMLEntry>();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter XML: ");
            string rawXML = System.IO.File.ReadAllText(@"C:/examplexml.txt");

            XDocument doc = XDocument.Parse(rawXML);

            IEnumerable<XElement> xmlElements = (doc.Root.Descendants());

            //foreach (XElement xmlElement in xmlElements)
            //{
            //    string output = string.Format("Value:{0}, FirstAtt:{1}, FirstNode:{2}, Name:{3}, Parent:{4}",
            //        xmlElement.Value, xmlElement.FirstAttribute, xmlElement.FirstNode, xmlElement.Name, "test", xmlElement);// xmlElement.Parent);
            //    Console.WriteLine(output);
            //}
            ProcessNodes(xmlElements);
            //Console.WriteLine(rawXML);

            foreach (XMLEntry item in entries)
            {
                Console.WriteLine(item.ToString());
            }

            Console.ReadLine();
        }

        static private void ProcessNodes(IEnumerable<XElement> elements)
        {
            foreach (XElement item in elements)
            {
                ProcessNode(item);
            }
        }

        static private void ProcessNode(XElement xmlElement)
        {
            if (xmlElement.HasElements)
            {
                //ProcessNodes(xmlElement.Descendants());
            }
            else
            {
                XMLEntry entry = new XMLEntry(
                    xmlElement.Name.ToString(), xmlElement.Attributes(),
                    xmlElement.Value, GetAbsoluteXPath(xmlElement));
                entries.Add(entry);
            }
        }

        public static string GetAbsoluteXPath(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Func<XElement, string> relativeXPath = e =>
            {
                int index = IndexPosition(e);
                string name = e.Name.LocalName;

                // If the element is the root, no index is required

                return (index == -1) ? "/" + name : string.Format
                (
                    "/{0}[{1}]",
                    name,
                    index.ToString()
                );
            };

            var ancestors = from e in element.Ancestors()
                            select relativeXPath(e);

            return string.Concat(ancestors.Reverse().ToArray()) +
                   relativeXPath(element);
        }

        public static int IndexPosition(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Parent == null)
            {
                return -1;
            }

            int i = 1; // Indexes for nodes start at 1, not 0

            foreach (var sibling in element.Parent.Elements(element.Name))
            {
                if (sibling == element)
                {
                    return i;
                }

                i++;
            }

            throw new InvalidOperationException
                ("element has been removed from its parent.");
        }
    }
}
    