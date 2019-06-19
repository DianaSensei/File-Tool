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
        /*
        private string needle;
        private string hammer;
        private int startIndex;
        private int length;
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
        */
        public NewCaseArgs newCaseArgs;
        public ReplaceArgs replaceArgs;

        List<Container> containers = new List<Container>();

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisChangedEvent(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public SelectFunctionWindow()
        {
            InitializeComponent();
        }
        public SelectFunctionWindow(NewCaseArgs args )
        {
            InitializeComponent();
            newCaseArgs = args;
            _comboboxSelect.SelectedIndex = 1;
            _comboboxSelect.IsEnabled = false;
        }
        public SelectFunctionWindow(ReplaceArgs args)
        {
            InitializeComponent();
            replaceArgs = args;
            _comboboxSelect.SelectedIndex = 0;
            _comboboxSelect.IsEnabled = false;
        }
        private void BtnAddFunction_Click(object sender, RoutedEventArgs e)
        {


            DialogResult = true;
            Close();
        }

        private void BtnCancelAddFunction_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            if(cb.SelectedIndex == 1)
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
            if (newCaseArgs != null)
                DataContext = newCaseArgs;
            else if (replaceArgs != null)
                DataContext = replaceArgs;
            RemoveStackPanel();
        }
        private void RemoveStackPanel()
        {
            if(this._stackpanelParent.Children.Contains(_stackpanelParameterReplace))
                this._stackpanelParent.Children.Remove(_stackpanelParameterReplace);
            if (this._stackpanelParent.Children.Contains(_stackpanelParameterNewCase))
                this._stackpanelParent.Children.Remove(_stackpanelParameterNewCase);
            if (this._stackpanelParent.Children.Contains(_stackpanelParameterMove))
                this._stackpanelParent.Children.Remove(_stackpanelParameterMove);
        }

        private StackPanel CreateStackPanel(MaterialDesignThemes.Wpf.PackIconKind kind, string content)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
            icon.Kind = kind;
            icon.Height = 24;icon.Width = 24; icon.VerticalAlignment = VerticalAlignment.Center;

            Label label = new Label();
            label.Content = content;
            label.FontWeight = FontWeights.DemiBold;
            label.FontSize = 15; label.Height = 35; label.VerticalAlignment = VerticalAlignment.Center;

            stackPanel.Children.Add(icon);
            stackPanel.Children.Add(label);
            return stackPanel;
        }

        private void _comboboxSelect_Loaded(object sender, RoutedEventArgs e)
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
    }
}
