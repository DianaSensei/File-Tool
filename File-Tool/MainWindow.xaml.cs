#region lib
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using File_Tool;
#endregion

namespace BatchRename
{
    public partial class MainWindow : Window
    {
        #region Attributes
        public const string PresetKey = "batchpreset17clc3";
        #region NewCase Status
        public bool IsNewCase
        {
            get { return (bool)GetValue(IsNewCaseProperty); }
            set { SetValue(IsNewCaseProperty, value); }
        }
        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNewCaseProperty =
            DependencyProperty.Register("IsNewCase", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
        #region Move Status
        public bool IsMove
        {
            get { return (bool)GetValue(IsMoveProperty); }
            set { SetValue(IsMoveProperty, value); }
        }
        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMoveProperty =
            DependencyProperty.Register("IsMove", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
        #region FullnameNomalize Status
        public bool IsFullnameNomalize
        {
            get { return (bool)GetValue(IsFullnameNomalizeProperty); }
            set { SetValue(IsFullnameNomalizeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFullnameNomalizeProperty =
            DependencyProperty.Register("IsFullnameNomalize", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
        #region Replace Status
        public bool IsReplace
        {
            get { return (bool)GetValue(IsReplaceProperty); }
            set { SetValue(IsReplaceProperty, value); }
        }
        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReplaceProperty =
            DependencyProperty.Register("IsReplace", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
        #region UniqueName Status
        public bool IsUniqueName
        {
            get { return (bool)GetValue(IsUniqueNameProperty); }
            set { SetValue(IsUniqueNameProperty, value); }
        }

        public static string PresetKey1 => PresetKey;

        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUniqueNameProperty =
            DependencyProperty.Register("IsUniqueName", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
        #endregion

        #region Expand Function Item
        ComboBox _comboboxNewCase = null;
        StackPanel _stackPanelMove = null;
        List<StackPanel> _stackPanelReplace = new List<StackPanel>();
        #endregion

        public class ObservableHashSetCollection<T> : ObservableCollection<T>
        {
            public Boolean AddUnique(T item)
            {
                if (Contains(item))
                    return false;
                base.Add(item);
                return true;
            }
        }

        /// <summary>
        /// Array file
        /// </summary>
        ObservableCollection<m_File> fileList = new ObservableCollection<m_File>();
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();
        List<IActions> actions = new List<IActions>();
        #region File and Folder Class
        public class m_File : INotifyPropertyChanged
        {
            private string newName;
            private string error;
            private string errorStatus;
            private FileInfo _FileInfomation;
            public FileInfo FileInfomation
            {
                get => _FileInfomation; set
                {
                    _FileInfomation = value;
                    RaiseChangeEvent();
                }
            }
            public string NewNameShow
            {
                get => newName; set
                {
                    newName = value;
                    RaiseChangeEvent();
                }
            }
            public string Error
            {
                get => error; set
                {
                    error = value;
                    RaiseChangeEvent();
                }
            }
            public string ErrorStatus
            {
                get => errorStatus; set
                {
                    errorStatus = value;
                    RaiseChangeEvent();
                }
            }

            #region PropertyChange
            public event PropertyChangedEventHandler PropertyChanged;
            void RaiseChangeEvent([CallerMemberName]string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
        public class Folder : INotifyPropertyChanged
        {
            private string newName;
            private string error;
            private string errorStatus;
            private DirectoryInfo _FolderInfomation;
            public DirectoryInfo FolderInfomation
            {
                get => _FolderInfomation; set
                {
                    _FolderInfomation = value;
                    RaiseChangeEvent();
                }
            }
            public string NewName
            {
                get => newName; set
                {
                    newName = value;
                    RaiseChangeEvent();
                }
            }
            public string Error
            {
                get => error; set
                {
                    error = value;
                    RaiseChangeEvent();
                }
            }
            public string ErrorStatus
            {
                get => errorStatus; set
                {
                    errorStatus = value;
                    RaiseChangeEvent();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            void RaiseChangeEvent([CallerMemberName]string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = this;
        }

        #region PreLoad Resource
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Design by Thong", "About");
        }
        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You can do it.", "Help");
        }
        #endregion

        #region Menu Function
        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                foreach (var victim in fileList)
                {
                    var resultPath = System.IO.Directory.GetParent(victim.FileInfomation.FullName).FullName;
                    var tempres = System.IO.Path.GetFileNameWithoutExtension(victim.FileInfomation.FullName);
                    var extension = System.IO.Path.GetExtension(victim.FileInfomation.FullName);

                    int count = 1;
                    foreach (var action in actions)
                    {
                        tempres = action.Process(tempres);
                    }
                    var result = tempres;
                    if ((tempres + extension) != victim.FileInfomation.Name)
                    {
                        while (File.Exists(combinePath(resultPath, tempres, extension)))
                        {
                            count++;
                            tempres = result + "(" + count + ")";
                        }
                        if (count > 1) result = tempres;
                    }
                    File.Move(victim.FileInfomation.FullName, combinePath(resultPath, result, extension));
                }
            }
            else
            {

            }

        }
        /// <summary>
        /// Refresh UI&Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion

        #region Method
        private void BtnNewCase(object sender, RoutedEventArgs e)
        {
            if (_btnNewCase.IsChecked == true)
            {
                if (_comboboxNewCase == null)
                {
                    _comboboxNewCase = CreateComboBoxNewCase();
                }
                if (_comboboxNewCase.SelectedIndex == -1)
                {
                    _comboboxNewCase.SelectedIndex = 0;
                }
                this._stackPanel_NewCase.Children.Add(_comboboxNewCase);
            }
            else
            {
                _comboboxNewCase.SelectedIndex = -1;
                if (_comboboxNewCase != null)
                    this._stackPanel_NewCase.Children.Remove(_comboboxNewCase);
                foreach (var element in fileList)
                    element.NewNameShow = element.FileInfomation.Name;
            }
        }

        /// <summary>
        /// Handle Function 
        /// </summary>
        #region Partner Work!!!!
        private void BtnReplace(object sender, RoutedEventArgs e)
        {
            if (_btnReplace.IsChecked == true)
            {
                this._stackPanel_Replace.Children.Add(CreateButtonReplaceSet("+"));
                StackPanel stackpanel = CreateStackPanelReplace();
                _stackPanelReplace.Add(stackpanel);
                InsertBeforeItemStackPanel(this._stackPanel_Replace, stackpanel);

            }
            else
            {
                this._stackPanel_Replace.Children.RemoveAt(this._stackPanel_Replace.Children.Count - 1);
                foreach (var item in _stackPanelReplace)
                {
                    this._stackPanel_Replace.Children.Remove(item);
                }
                _stackPanelReplace.Clear();

            }
        }
        private void BtnMove(object sender, RoutedEventArgs e)
        {
            if (_btnMove.IsChecked == true)
            {
                if (_stackPanelMove == null)
                {
                    _stackPanelMove = CreateStackPanelMove();
                }
                this._stackPanel_Move.Children.Add(_stackPanelMove);

            }
            else
            {
                this._stackPanel_Move.Children.Remove(_stackPanelMove);
            }
        }
        private void BtnFullName(object sender, RoutedEventArgs e)
        {

        }
        private void BtnUnique(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #endregion 

        #region Add & Remove Button
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        LoadFileFromPath(fbd.SelectedPath);
                    }
                }
            }
            else
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        LoadFolderFromPath(fbd.SelectedPath);
                    }
                }
            }
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (FileShow.SelectedIndex != -1 && _tabcontrolShow.SelectedIndex == 0)
                fileList.RemoveAt(FileShow.SelectedIndex);
            if (FolderShow.SelectedIndex != -1 && _tabcontrolShow.SelectedIndex == 1)
                folderList.RemoveAt(FolderShow.SelectedIndex);
        }
        #endregion

        #region Load File & Folder
        private Boolean LoadFileFromPath(string RootPath)
        {
            DirectoryInfo FolderPath = new DirectoryInfo(RootPath);
            if (FolderPath.Exists)
            {
                FileInfo[] listFileInfo = FolderPath.GetFiles();
                foreach (var file in listFileInfo)
                {
                    fileList.Add(new m_File() { FileInfomation = file, NewNameShow = file.Name, ErrorStatus = "ChartDonut" });
                }
                return true;
            }
            return false;
        }
        private Boolean LoadFolderFromPath(string RootPath)
        {
            DirectoryInfo FolderPath = new DirectoryInfo(RootPath);
            if (FolderPath.Exists)
            {
                DirectoryInfo[] listFolderInfo = FolderPath.GetDirectories();
                foreach (var folder in listFolderInfo)
                {
                    folderList.Add(new Folder() { FolderInfomation = folder, NewName = folder.Name, ErrorStatus = "ChartDonut" });
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Preset Method
        private void BtnNewPreset(object sender, RoutedEventArgs e)
        {

        }
        private void BtnSavePreset(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SaveFilePresetDialog = new SaveFileDialog();
            SaveFilePresetDialog.Title = "Save Preset BatchRename";
            SaveFilePresetDialog.Filter = "Text file(*.txt)|*.txt";
            SaveFilePresetDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            SaveFilePresetDialog.DefaultExt = "txt";
            SaveFilePresetDialog.CheckPathExists = true;
            if (SaveFilePresetDialog.ShowDialog() == true)
            {
                SavePreset(SaveFilePresetDialog.FileName);
            }
        }
        private void BtnLoadPreset(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openpresetfile = new OpenFileDialog();

        }
        #endregion


        public void _comboboxNewCase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboboxNewCase.SelectedIndex == 0)
            {
                foreach (var file in fileList)
                {
                    file.NewNameShow = file.NewNameShow.ToUpper();
                }
                foreach (var folder in folderList)
                {
                    folder.NewName = folder.NewName.ToUpper();
                }

            }
            else if (_comboboxNewCase.SelectedIndex == 1)
            {
                foreach (var file in fileList)
                {
                    file.NewNameShow = file.NewNameShow.ToLower();
                }
                foreach (var folder in folderList)
                {
                    folder.NewName = folder.NewName.ToLower();
                }

            }
            else
            {
                foreach (var file in fileList)
                {
                    file.NewNameShow = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(file.NewNameShow.ToLower());
                }
                foreach (var folder in folderList)
                {
                    folder.NewName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(folder.NewName.ToLower());
                }

            }
        }

        #region Create Element UI
        StackPanel CreateStackPanelReplace()
        {
            StackPanel stackpanel = new StackPanel();
            stackpanel.Orientation = Orientation.Vertical;
            TextBox HammerTextBox = CreateTextBoxReplace("Hammer");
            TextBox NeedleTextBox = CreateTextBoxReplace("Needle");

            stackpanel.Children.Add(HammerTextBox);
            stackpanel.Children.Add(NeedleTextBox);
            stackpanel.Children.Add(CreateButtonReplaceSet("x"));
            return stackpanel;
        }
        Button CreateButtonReplaceSet(string command)
        {
            Button button = new Button();
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Content = command;
            button.Width = 200;
            button.Height = 20;
            button.Style = GetStaticResourceStyle("MaterialDesignFlatAccentButton");
            button.Background = Brushes.Transparent;
            if (command == "x")
            {
                button.Background = Brushes.Red;
                button.Foreground = Brushes.Black;
                button.Click += ButtonReplaceRemoveSet_Click;
            }
            else
            {
                button.Background = Brushes.White;
                button.Foreground = Brushes.Red;
                button.Click += ButtonReplaceAddSet_Click;
            }
            return button;
        }

        private void ButtonReplaceAddSet_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackpanel = CreateStackPanelReplace();
            _stackPanelReplace.Add(stackpanel);
            InsertBeforeItemStackPanel(this._stackPanel_Replace, stackpanel);
        }

        private void ButtonReplaceRemoveSet_Click(object sender, RoutedEventArgs e)
        {
            this._stackPanel_Replace.Children.Remove(VisualTreeHelper.GetParent(sender as Control) as UIElement);
            _stackPanelReplace.Remove(VisualTreeHelper.GetParent(sender as Control) as StackPanel);
            if (!_stackPanelReplace.Any())
            {
                IsReplace = false;
                this._stackPanel_Replace.Children.RemoveAt(1);
            }

        }
        private void InsertBeforeItemStackPanel(StackPanel s, StackPanel c)
        {
            s.Children.Insert(s.Children.Count - 1, c);
        }
        StackPanel CreateStackPanelMove()
        {
            StackPanel stackpanel = new StackPanel();
            stackpanel.Orientation = Orientation.Horizontal;
            stackpanel.HorizontalAlignment = HorizontalAlignment.Right;

            RadioButton ISBN_Name = new RadioButton();
            ISBN_Name.Content = "ISBN - Name";
            ISBN_Name.Margin = Margin(ISBN_Name, 0, 0, 15, 0);

            RadioButton Name_ISBN = new RadioButton();
            Name_ISBN.Content = "Name - ISBN";

            ISBN_Name.GroupName = "_radioBtnMove";
            Name_ISBN.GroupName = "_radioBtnMove";

            stackpanel.Children.Add(ISBN_Name);
            stackpanel.Children.Add(new Separator());
            stackpanel.Children.Add(Name_ISBN);

            return stackpanel;
        }
        TextBox CreateTextBoxReplace(string hint)
        {
            TextBox textbox = new TextBox();
            textbox.HorizontalContentAlignment = HorizontalAlignment.Right;
            textbox.VerticalContentAlignment = VerticalAlignment.Center;
            textbox.Height = 35;
            textbox.Width = 240;
            textbox.Margin = Margin(textbox, 5, 0);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textbox, hint);
            MaterialDesignThemes.Wpf.HintAssist.SetForeground(textbox, Brushes.BlueViolet);
            textbox.Style = GetStaticResourceStyle("MaterialDesignFloatingHintTextBox");
            return textbox;
        }
        Style GetStaticResourceStyle(string style)
        {
            return this.FindResource(style) as Style;
        }
        ComboBox CreateComboBoxNewCase()
        {
            ComboBox item = new ComboBox();
            item.Height = 35;
            item.Width = 150;
            item.HorizontalAlignment = HorizontalAlignment.Right;
            item.Margin = Margin(item, 0, 0, 20, 0);
            item.HorizontalContentAlignment = HorizontalAlignment.Center;
            item.VerticalContentAlignment = VerticalAlignment.Center;
            item.SelectionChanged += _comboboxNewCase_SelectionChanged;

            item.Items.Add(CreateComboBoxItemNewCase("To UpperCase"));
            item.Items.Add(CreateComboBoxItemNewCase("To LowerCase"));
            item.Items.Add(CreateComboBoxItemNewCase("To First Upper"));

            return item;
        }
        ComboBoxItem CreateComboBoxItemNewCase(string content)
        {
            ComboBoxItem result = new ComboBoxItem();
            result.Content = content;
            result.HorizontalContentAlignment = HorizontalAlignment.Center;
            return result;
        }
        #endregion

        #region Set Margin
        /// <summary>
        /// Set Margin value for item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns>new Thickness</returns>
        public new Thickness Margin(Control item, int left, int top, int right, int bottom)
        {
            Thickness result = item.Margin;
            result.Left = left;
            result.Top = top;
            result.Right = right;
            result.Bottom = bottom;

            return result;
        }
        /// <summary>
        /// Set Margin value for item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns>new Thickness</returns>
        public new Thickness Margin(Control item, int left, int top)
        {
            Thickness result = item.Margin;
            result.Left = left;
            result.Top = top;

            return result;
        }
        #endregion

        public bool LoadPreset(string path)
        {
            List<string> lines = new List<string>();
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                bool isReadKey = false;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.ElementAt(0) != '#') continue;

                    if (!isReadKey)
                    {
                        if (!CheckValidKeyPreset(line))
                            return false;
                        isReadKey = true;
                        continue;
                    }
                    lines.Add(line);
                }
            }
            return ReadPreset(lines);
        }
        private bool CheckValidKeyPreset(string reg)
        {
            return reg == PresetKey;
        }
        public bool ReadPreset(List<string> listactions)
        {
            if (!listactions.Any())
            {
                return false;
            }
            foreach (var action in listactions)
            {
                string[] element = action.Split(' ');
                switch (element[0])
                {
                    case "NC":
                        {
                            LoadActionNewCase(element);
                        }
                        break;
                    case "FN":
                        {

                        }
                        break;
                    case "M":
                        {

                        }
                        break;
                    case "RP":
                        {

                        }
                        break;
                    case "UN":
                        {

                        }
                        break;

                }
            }
            return true;
        }
        private bool LoadActionNewCase(string[] element)
        {
            NewCaser NewCaseAction = new NewCaser() { Args = new NewCaseArgs() { Case = int.Parse(element[1]) } };
            actions.Add(NewCaseAction);
            return true;
        }

        public bool SavePreset(string path)
        {
            using (var streamWriter = new StreamWriter(path))
            {
                WriteCommentFilePreset(streamWriter);
                foreach (var action in actions)
                {
                    switch (action)
                    {
                        case NewCaser n:
                            {
                                streamWriter.WriteLine("NC" + " " + (n.Args as NewCaseArgs).Case);
                            }
                            break;
                    }
                }
            }
            return true;
        }
        public void WriteCommentFilePreset(StreamWriter sw)
        {
            sw.WriteLine("#Preset File of BatchRename.");
            sw.WriteLine("#Member 1753107 - 1753130 - 1753124.");
            sw.WriteLine("#Please use '#' at headline to write comment.");
            sw.WriteLine("#Syntax: \"KeyAction\" + \" \" + \"parameter1\" + \" \" + \"parameter1\" + '...'");
            sw.WriteLine("#Actions list:");
            sw.WriteLine("#1.NewCase : Key(NC): ");
            sw.WriteLine("#parameter1 = 0: ToUpperCase.");
            sw.WriteLine("#parameter1 = 1: ToLowerCase.");
            sw.WriteLine("#parameter1 = 2: ToFirstLetterCase.");
            sw.WriteLine("#2.Replace : Key(RP): ");
            sw.WriteLine("#parameter1 =  FindWhat");
            sw.WriteLine("#parameter2 =  ReplaceWith");
            sw.WriteLine("#3.Move : Key(RP): ");
            sw.WriteLine("#parameter1 = StartIndex");
            sw.WriteLine("#parameter2 = Length");
            sw.WriteLine("#4.Fullname Normalize : Key(FN): ");
            sw.WriteLine("#no parameter.");
            sw.WriteLine("#5.Unique Name: Key(UN): ");
            sw.WriteLine("#no parameter.\n\n");
        }
        public static string combinePath(string path, string filename, string extension)
        {
            string res = path + "\\" + filename + extension;
            return res;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileShow.ItemsSource = fileList;
            FolderShow.ItemsSource = folderList;
            //this._stackPanel_Replace.Children.Remove(_stackPanelReplace);
        }
    }
}
