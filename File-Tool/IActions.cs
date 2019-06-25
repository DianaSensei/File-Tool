using BatchRename;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
}