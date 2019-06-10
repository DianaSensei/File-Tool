using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Tool
{
    public interface IActions
    {
        IArgs IArg { get; set; }

        string Process(string origin);  
    }
    public class Replacer : IActions
    {
        public IArgs IArg { get; set; }

        public string Process(string origin)
        {
            string result = origin;
            return result.Replace("Needle","Hammer");
        }
    }
}
