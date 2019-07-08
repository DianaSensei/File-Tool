﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Tool
{
    public interface IArgs{    }
    public class ReplaceArgs : IArgs
    {
        public string Needle { get; set; }
        public string Hammer { get; set; }

    }
    public class ISBNArgs : IArgs
    {
        public int Modes { get; set; }
    }
    public class NewCaseArgs : IArgs {
        public int Case { get; set; }
    }
    public class FullnameNormalizeArgs : IArgs {
    }
    public class UniqueNameArgs : IArgs {
    }
    public class ExtensionArgs : IArgs
    {
        public string oldExt { get; set; }
        public string newExt { get; set; }
    }
}