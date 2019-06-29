using BatchRename;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace File_Tool
{
    #region Action xử lí 
    public interface IActions
    {
        IArgs Args { get; set; }
        /// <summary>
        /// Hàm xử lí chuỗi
        /// </summary>
        /// <param name="origin">Chuỗi gốc</param>
        /// <returns>Kết quả sau khi xử lí</returns>
        string Process(string origin);
        /// <summary>
        ///  Mô tả về hành động
        /// </summary>
        /// <returns></returns>
        string Description { get; }
        void ShowUpdateArgDialog(MainWindow mainWindow);
    }
    #endregion 
    public class NewCaser : IActions , INotifyPropertyChanged
    {
        public IArgs Args { get; set; }
        public string Process(string origin)
        {
            var args = Args as NewCaseArgs;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            int type = args.Case;
            var result = filename;
            switch (type) {
                case 0://To Upper Case
                    result = result.ToUpper();break;
                case 1://To Lower Case
                    result = result.ToLower();break;
                case 2://To First Letter
                    result = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());break;
            }
            return PathHandler.combinePath(path,result,extension);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var Case = (Args as NewCaseArgs).Case;
            var screen = new SelectFunctionWindow(Args as NewCaseArgs,mainWindow);
            if(screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
            else
            {
                (Args as NewCaseArgs).Case = Case;
            }
        }
        public string Description
        {
            get
            {
                var args = Args as NewCaseArgs;
                var type = args.Case;
                string types="";
                if (type == 0) types = "To UpperCase";
                if (type == 1) types = "To LowerCase";
                if (type == 2) types = "To LetterCase ";
                var result = $"NewCase with \"{types}\"";
                return result;
            }
        }
    }
    public class Replacer : IActions, INotifyPropertyChanged
    {
        public IArgs Args { get; set; }
        public string Process(string origin)
        {
            var args = Args as ReplaceArgs;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            string needle = args.Needle;
            string hammer = args.Hammer;
            var result = filename;
            result = result.Replace(needle, hammer);
            return PathHandler.combinePath(path,result,extension);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateArgDialog(MainWindow mainWindow )
        {
            var Needle = (Args as ReplaceArgs).Needle;
            var Hammer = (Args as ReplaceArgs).Hammer;
            var screen = new SelectFunctionWindow(Args as ReplaceArgs,mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
            else
            {
                (Args as ReplaceArgs).Needle = Needle;
                (Args as ReplaceArgs).Hammer = Hammer;
            }
        }
        public string Description
        {
            get
            {
                var args = Args as ReplaceArgs;
                string needle = args.Needle;
                string hammer = args.Hammer;
                string result = $"Replace \"{needle}\" by \"{hammer}\"";
                return result;
            }
        }
    }
    public class ISBN : IActions, INotifyPropertyChanged
    {
        public IArgs Args { get; set; }
        public string Process(string origin)
        {
            var args = Args as ISBNArgs;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            int type = args.Modes;
            var result = filename;
            string g_isbn = "", g_filename = "";
            string regex_I_F = @"(?<isbn>[0-9_-]{1,})[ ](?<filename>[a-zA-Z ,]{1,})";
            string regex_F_I = @"(?<filename>[a-zA-Z ,]{1,})[ ](?<isbn>[0-9_-]{1,})";

            switch (type)
            {
                case 0://IBSN - Filename
                    if(Regex.IsMatch(result,regex_F_I)){
                        Match match = Regex.Match(result, regex_F_I);
                        g_isbn = match.Groups["isbn"].Value;
                        g_filename = match.Groups["filename"].Value;
                        result = g_isbn + " " + g_filename; break;
                    }
                    break;
                case 1://Filename - ISBN
                    if(Regex.IsMatch(result, regex_I_F))
                    {
                        Match match = Regex.Match(result, regex_I_F);
                        g_isbn = match.Groups["isbn"].Value;
                        g_filename = match.Groups["filename"].Value;
                        result = g_filename + " " + g_isbn; break;
                    }
                    break;
            }
            return PathHandler.combinePath(path,result,extension);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var Mode = (Args as ISBNArgs).Modes;
            var screen = new SelectFunctionWindow(Args as ISBNArgs, mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
            else
            {
                (Args as ISBNArgs).Modes = Mode;
            }
        }

        public string Description
        {
            get
            {
                var args = Args as ISBNArgs;
                var mode = args.Modes;
                string types = "";
                if (mode == 0) types = "IBSN - Filename";
                if (mode == 1) types = "Filename - IBSN";
                var result = $"\"{types}\"";
                return result;
            }
        }
    }
    public class FullNameNormalize : IActions, INotifyPropertyChanged
    {
        public IArgs Args { get; set; }

        public string Description
        {
            get => "Fullname Normalize";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public string Process(string origin)
        {
            var args = Args as FullnameNormalizeArgs;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            string result = filename;

            var space = ' ';
            string[] temps = result.Split(new[] { space }, StringSplitOptions.RemoveEmptyEntries);
            result = "";

            foreach(var temp in temps)
            {
                var lowerCase = temp.ToLower();
                result += lowerCase[0].ToString().ToUpper() + lowerCase.Substring(1) + " ";
            }
            return PathHandler.combinePath(path,result.Trim(),extension);
        }
        
        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var screen = new SelectFunctionWindow(Args as FullnameNormalizeArgs, mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class ExtensionChanger : IActions, INotifyPropertyChanged
    {
        public IArgs Args { get; set; }
        public string Description
        {
            get
            {
                var args = Args as ExtensionArgs;
                args.oldExt = args.oldExt.Replace(" ", "");
                args.newExt = args.newExt.Replace(" ", "");
                if (args.oldExt[0] != '.') args.oldExt = '.' + args.oldExt;
                if (args.newExt[0] != '.') args.newExt = '.' + args.newExt;
                string result = $"Change Extension \"{args.oldExt}\" to \"{args.newExt}\"";
                return result;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public string Process(string origin)
        {
            var args = Args as ExtensionArgs;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            args.oldExt = args.oldExt.Replace(" ", "");
            args.newExt = args.newExt.Replace(" ", "");

            if (args.oldExt[0] != '.') args.oldExt = '.' + args.oldExt;
            if (args.newExt[0] != '.') args.newExt = '.' + args.newExt;
            string newExt = extension;
            if (extension == args.oldExt)
                newExt = args.newExt;
            return PathHandler.combinePath(path, filename, newExt);
        }
        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var oldExt = (Args as ExtensionArgs).oldExt;
            var newExt = (Args as ExtensionArgs).newExt;
            var screen = new SelectFunctionWindow(Args as ExtensionArgs, mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
            else
            {
                (Args as ExtensionArgs).oldExt = oldExt;
                (Args as ExtensionArgs).newExt = newExt;
            }
        }
    }
    public class UniqueName : IActions, INotifyPropertyChanged
    {
        public IArgs Args { get; set; }

        public string Description
        {
            get => "Unique Name";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public string Process(string origin)
        {
            var args = Args as UniqueName;
            var path = PathHandler.getPath(origin);
            var filename = PathHandler.getFileName(origin);
            var extension = PathHandler.getExtension(origin);

            var result = System.Guid.NewGuid().ToString();
            return PathHandler.combinePath(path, result, extension);
        }

        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var screen = new SelectFunctionWindow(Args as UniqueNameArgs, mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
        }
    }
    public class PathHandler
    {
        public static string combinePath(string path, string filename, string extension)
        {
            string res = path + "\\" + filename + extension;
            return res;
        }
        public static string getPath(string fullPath)
        {
            var m_path = System.IO.Directory.GetParent(fullPath).FullName;
            return m_path;
        }
        public static string getFileName(string fullPath)
        {
            var m_filename = System.IO.Path.GetFileNameWithoutExtension(fullPath);
            return m_filename;
        }
        public static string getExtension(string fullPath)
        {
            var m_ext = System.IO.Path.GetExtension(fullPath);
            return m_ext;
        }
    }
}