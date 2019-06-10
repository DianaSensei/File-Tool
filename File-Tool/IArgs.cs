using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Tool
{
    public interface IArgs
    {
    }
    public class ReplaceArgs : IArgs
    {
        string Needle;
        string Hammer;

    }
    public class RemoveArgs : IArgs {
        int StartIndex;
        int Count;
    }

    public class NewCaseArgs : IArgs {
        int Case;
    }
    public class FullnameNormalizeArgs : IArgs {

    }
    public class UniqueNameArgs : IArgs {}
}
