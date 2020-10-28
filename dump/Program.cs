using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dump
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person p = null;
            // NullPointerException will happen in next line
            Console.WriteLine("{0}-{1}", p.ID, p.Name);
            Console.ReadKey();
        }
    }
}
