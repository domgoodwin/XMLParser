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

        static void Main(string[] args)
        {
            Console.WriteLine("Enter XML: ");
            string rawXML = System.IO.File.ReadAllText(@"C:/examplexml.txt");
            string rawJSON = System.IO.File.ReadAllText(@"C:/jsonexmaple.txt");

            //foreach (XElement xmlElement in xmlElements)
            //{
            //    string output = string.Format("Value:{0}, FirstAtt:{1}, FirstNode:{2}, Name:{3}, Parent:{4}",
            //        xmlElement.Value, xmlElement.FirstAttribute, xmlElement.FirstNode, xmlElement.Name, "test", xmlElement);// xmlElement.Parent);
            //    Console.WriteLine(output);
            //}
            Functions func = new Functions();
            //func.ProcessXML(rawXML);
            List<XMLEntry> json = func.ProcessJSON(rawJSON);
            //Console.WriteLine(rawXML);

            foreach (XMLEntry item in json)
            {
                Console.WriteLine(item.ToString());
            }

            Console.ReadLine();
        }


    }
}
    