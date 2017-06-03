using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;

namespace XMLParser
{
    class Functions
    {
        public List<XMLEntry> entries = new List<XMLEntry>();

        public List<XMLEntry> ProcessXML(string rawXML)
        {
            entries.Clear();
            XDocument doc = XDocument.Parse(rawXML);
            IEnumerable<XElement> xmlElements = (doc.Root.Descendants());
            ProcessNodes(xmlElements);
            return entries;
        }

        public List<XMLEntry> ProcessJSON(string rawJSON)
        {
            entries.Clear();
            byte[] bytes = Encoding.ASCII.GetBytes(rawJSON);
            using (var stream = new MemoryStream(bytes))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                var xml = XDocument.Load(jsonReader);
                IEnumerable<XElement> elements = (xml.Root.Descendants());
                ProcessNodes(elements);
                return entries;
            }
        }

        private void ProcessNodes(IEnumerable<XElement> elements)
        {
            foreach (XElement item in elements)
            {
                ProcessNode(item);
            }
        }

        private void ProcessNode(XElement xmlElement)
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

        private string GetAbsoluteXPath(XElement element)
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

        private int IndexPosition(XElement element)
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
