using System;
using System.Collections.Generic;
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
    }
    #endregion 
    /// <summary>
    /// Class change name with NewCase.
    /// </summary>
    public class NewCaser : IActions
    {
        public IArgs Args { get; set; }

        public string Description { get; }

        public string Process(string origin)
        {
            var args = Args as NewCaseArgs;
            int type = args.Case;
            var result = origin;
            switch (type) {
                case 1://To Upper Case
                    result = result.ToUpper();break;
                case 2://To Lower Case
                    result = result.ToLower();break;
                case 3://To First Letter
                    result = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());break;
            }
            return result;
        }
    }
}

