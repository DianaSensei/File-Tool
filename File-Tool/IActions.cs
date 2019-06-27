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
    /// <summary>
    /// Class change name with NewCase.
    /// </summary>
    public class NewCaser : IActions , INotifyPropertyChanged
    {
        public IArgs Args { get; set; }

        public string Process(string origin)
        {
            var args = Args as NewCaseArgs;
            int type = args.Case;
            var result = origin;
            switch (type) {
                case 0://To Upper Case
                    result = result.ToUpper();break;
                case 1://To Lower Case
                    result = result.ToLower();break;
                case 2://To First Letter
                    result = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());break;
            }
            return result;
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
            string needle = args.Needle;
            string hammer = args.Hammer;
            var result = origin;
            result = result.Replace(needle, hammer);
            return result;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void ShowUpdateArgDialog(MainWindow mainWindow )
        {
            var screen = new SelectFunctionWindow(Args as ReplaceArgs,mainWindow);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
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
            var args = Args as MoveArgs;
            int type = args.Modes;
            var result = origin;
            string isbn = "", filename = "";
            Match match;
            string regex_I_F = @"(?<isbn>[0-9_-]{1,})[ ](?<filename>[a-zA-Z ,]{1,})";
            string regex_F_I = @"(?<filename>[a-zA-Z ,]{1,})[ ](?<isbn>[0-9_-]{1,})";

            switch (type)
            {
                case 0://IBSN - Filename                   
                    match = Regex.Match(result, regex_I_F);
                    isbn = match.Groups["isbn"].Value;
                    filename = match.Groups["filename"].Value;
                    if (isbn == "" && filename == "")
                    {
                        match = Regex.Match(result, regex_F_I);
                        isbn = match.Groups["isbn"].Value;
                        filename = match.Groups["filename"].Value;
                        result = isbn + " " + filename; break;
                    }
                    result = isbn + " " + filename; break;
                case 1://Filename - ISBN
                    match = Regex.Match(result, regex_F_I);
                    isbn = match.Groups["isbn"].Value;
                    filename = match.Groups["filename"].Value;
                    if (isbn == "" && filename == "")
                    {
                        match = Regex.Match(result, regex_I_F);
                        isbn = match.Groups["isbn"].Value;
                        filename = match.Groups["filename"].Value;
                        result = filename + " " + isbn; break;
                    }
                    result = filename + " " + isbn; break;
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaiseChangeEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void ShowUpdateArgDialog(MainWindow mainWindow)
        {
            var Mode = (Args as MoveArgs).Modes;
            var screen = new SelectFunctionWindow(Args as MoveArgs);
            if (screen.ShowDialog() == true)
            {
                RaiseChangeEvent("Description");
            }
            else
            {
                (Args as MoveArgs).Modes = Mode;
            }
        }

        public string Description
        {
            get
            {
                var args = Args as MoveArgs;
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
            get
            {
                var args = Args as FullnameNormalizeArgs;
                string result = $"Fullname Normalize";
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
            var args = Args as FullnameNormalizeArgs;
            string result = origin;

            var space = ' ';
            string[] temps = result.Split(new[] { space }, StringSplitOptions.RemoveEmptyEntries);
            result = "";

            foreach(var temp in temps)
            {
                var lowerCase = temp.ToLower();
                result += lowerCase[0].ToString().ToUpper() + lowerCase.Substring(1) + " ";
            }
            return result.Trim();
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
}