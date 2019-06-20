using BatchRename;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace File_Tool
{
    public partial class SelectFunctionWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Mode of Window (true:Select Function, false:Edit Function)
        /// </summary>
        public bool Mode = true;

        #region Attributes
        private int cases;
        private string needle = "";
        private string hammer = "";
        private int startIndex;
        private int length;
        public int Case
        {
            get => cases;
            set
            {
                cases = value;
                RaisChangedEvent("Case");
            }
        }
        public string Needle
        {
            get => needle;
            set
            {
                needle = value;
                RaisChangedEvent("Needle");
            }
        }
        public string Hammer
        {
            get => hammer;
            set
            {
                hammer = value;
                RaisChangedEvent("Hammer");
            }
        }
        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
                RaisChangedEvent("StartIndex");
            }
        }
        public int Length
        {
            get => length;
            set
            {
                length = value;
                RaisChangedEvent("Length");
            }
        }
        #endregion

        #region Args DataContext
        public NewCaseArgs newCaseArgs;
        public ReplaceArgs replaceArgs;
        private MainWindow mainWindow;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisChangedEvent(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        /// <summary>
        /// _comboboxSelect Index: 0 = Replace
        ///                        1 = NewCase
        ///                        2 = Move
        ///                        3 = Fullname Normalize
        ///                        4 = Unique Name 
        /// </summary>
        #region Constructor With Parameter
        public SelectFunctionWindow()
        {
            InitializeComponent();
        }
        public SelectFunctionWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
        public SelectFunctionWindow(NewCaseArgs args)
        {
            InitializeComponent();
            newCaseArgs = args;
            ChangesContentShowButtonFunction("OK");
            Mode = false;
            _comboboxSelect.SelectedIndex = 1;
            _comboboxSelect.IsEnabled = false;
        }
        public SelectFunctionWindow(ReplaceArgs args)
        {
            InitializeComponent();
            replaceArgs = args;
            ChangesContentShowButtonFunction("OK");
            Mode = false;
            _comboboxSelect.SelectedIndex = 0;
            _comboboxSelect.IsEnabled = false;
        }
        #endregion

        private void BtnAddFunction_Click(object sender, RoutedEventArgs e)
        {
            if (_comboboxSelect.SelectedIndex == 0)
            {
                if (!Needle.Any() || !Hammer.Any()) return;
                mainWindow.actions.Add(new Replacer() { Args = new ReplaceArgs() { Needle = this.Needle, Hammer = this.Hammer } });
            }
        }
        private void BtnEditFunction_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void BtnCancelFunction_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboboxSelect_Loaded(object sender, RoutedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterReplace);
                return;
            }
            if (cb.SelectedIndex == 1)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterNewCase);
                return;
            }
            if (cb.SelectedIndex == 2)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterMove);
                return;
            }
            else RemoveStackPanel();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterReplace);
                return;
            }
            if (cb.SelectedIndex == 1)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterNewCase);
                return;
            }
            if (cb.SelectedIndex == 2)
            {
                RemoveStackPanel();
                this._stackpanelParent.Children.Add(_stackpanelParameterMove);
                return;
            }
            else RemoveStackPanel();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
                DataContext = this;
            else if (newCaseArgs != null)
                DataContext = newCaseArgs;
            else if (replaceArgs != null)
                DataContext = replaceArgs;
            RemoveStackPanel();
        }
        private void RemoveStackPanel()
        {
            if (this._stackpanelParent.Children.Contains(_stackpanelParameterReplace))
                this._stackpanelParent.Children.Remove(_stackpanelParameterReplace);
            if (this._stackpanelParent.Children.Contains(_stackpanelParameterNewCase))
                this._stackpanelParent.Children.Remove(_stackpanelParameterNewCase);
            if (this._stackpanelParent.Children.Contains(_stackpanelParameterMove))
                this._stackpanelParent.Children.Remove(_stackpanelParameterMove);
        }
        private void BtnFunction_Click(object sender, RoutedEventArgs e)
        {
            if (Mode)  BtnAddFunction_Click(sender, e);
            else       BtnEditFunction_Click(sender, e);

        }
        public void ChangesContentShowButtonFunction(string str)
        {
            _btnFunction.Content = str;
        }
    }
}
