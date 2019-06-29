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
using System.Diagnostics;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

#endregion

namespace BatchRename
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Attributes
        public string regex_check_filename_index = @"(?<filename>[\(,\),_,\-,.,\w+]{1,})(?<indextag>\((?<index>\d+)\)$)";
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        public const string PresetKey = "batchpreset17clc3";
        private int totalitem;
        private string presetName = "Default";
        private string process = "";
        //  Color mode
        private string foregroundColor = "Black";
        private string backgroundColor = "#f5f5f5";
        private string backgroundfuncColor = "White";
        private string shadowColor = "LightGray";
        private string backgroundtabitemColor = "#f5f5f5";
        #endregion

        #region List manager
        BindingList<m_File> fileList = new BindingList<m_File>();
        BindingList<m_Folder> folderList = new BindingList<m_Folder>();
        public BindingList<IActions> actions = new BindingList<IActions>();
        #endregion

        #region PropertyChanged value
        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChangeEvent([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int TotalItem
        {
            get
            {
                if (_tabcontrolShow.SelectedIndex == 1) return folderList.Count;
                return fileList.Count;
            }
            set
            {
                totalitem = value;
                RaiseChangeEvent("TotalItem");
            }
        }
        public string Process { get => process;
            set
            {
                process = value;
                RaiseChangeEvent("Process");
            }
        }
        public string PresetName
        {
            get => presetName; set
            {
                presetName = value;
                RaiseChangeEvent("PresetName");
            }
        }
        public string ForegroundColor
        {
            get => foregroundColor;
            set
            {
                foregroundColor = value;
                RaiseChangeEvent("ForegroundColor");
            }
        }
        public string BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                RaiseChangeEvent("BackgroundColor");
            }
        }
        public string BackgroundfuncColor
        {
            get => backgroundfuncColor;
            set
            {
                backgroundfuncColor = value;
                RaiseChangeEvent("BackgroundfuncColor");
            }
        }
        public string ShadowColor
        {
            get => shadowColor;
            set
            {
                shadowColor = value;
                RaiseChangeEvent("ShadowColor");
            }
        }
        public string BackgroundtabitemColor
        {
            get => backgroundtabitemColor;
            set
            {
                backgroundtabitemColor = value;
                RaiseChangeEvent("BackgroundtabitemColor");
            }
        }
        #endregion

        #region File and Folder Class
        public class m_File : INotifyPropertyChanged
        {
            private string newName;
            private string errorStatus;
            private string filename;
            private string fullpath;
            private string errordetail;
            private string errorColor;
            public string FileName
            {
                get => filename; set
                {
                    filename = value;
                    RaiseChangeEvent();
                }
            }
            public string FullPath
            {
                get => fullpath; set
                {
                    fullpath = value;
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
            public string ErrorStatus
            {
                get => errorStatus; set
                {
                    errorStatus = value;
                    RaiseChangeEvent();
                }
            }
            public string ErrorDetail
            {
                get => errordetail;
                set
                {
                    errordetail = value;
                    RaiseChangeEvent("ErrorDetail");
                }
            }
            public string ErrorColor { get => errorColor;set
                {
                    errorColor = value;
                    RaiseChangeEvent("ErrorColor");
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
        public class m_Folder : INotifyPropertyChanged
        {
            private string newName;
            private string errorStatus;
            private string foldername;
            private string fullpath;
            private string errordetail;
            private string errorColor;
            public string FolderName
            {
                get => foldername; set
                {
                    foldername = value;
                    RaiseChangeEvent();
                }
            }
            public string FullPath
            {
                get => fullpath; set
                {
                    fullpath = value;
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
            public string ErrorStatus
            {
                get => errorStatus; set
                {
                    errorStatus = value;
                    RaiseChangeEvent();
                }
            }
            public string ErrorDetail
            {
                get => errordetail;
                set
                {
                    errordetail = value;
                    RaiseChangeEvent("ErrorDetail");
                }
            }
            public string ErrorColor
            {
                get => errorColor; set
                {
                    errorColor = value;
                    RaiseChangeEvent("ErrorColor");
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            listView.ItemsSource = actions;
            this.DataContext = this;
        }

        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Any() && actions.Any())
                {
                    ShowCompleteDialog();
                    foreach (var victim in fileList)
                    {
                        HandleFile(victim);
                    }
                    CloseCompleteDialog();
                    fileList.Clear();
                }
            }
            else
            {
                if (folderList.Any() && actions.Any())
                {
                    ShowCompleteDialog();
                    foreach (var victim in folderList)
                    {
                        HandleFolder(victim);
                    }
                    CloseCompleteDialog();
                    folderList.Clear();
                }
            }
        }
        private void HandleFile(m_File victim)
        {
            if (victim.FileName == victim.NewName)
            {
                if (!victim.FileName.Equals(victim.NewName))
                    if(File.Exists(victim.FullPath))
                        File.Move(victim.FullPath, PathHandler.getPath(victim.FullPath) + "\\" + victim.NewName);
                return;
            }
            var path = PathHandler.getPath(victim.FullPath);
            var oldname = victim.FileName;

            var newname = PathHandler.getFileName(victim.NewName);
            var newext = PathHandler.getExtension(victim.NewName);

            int count;
            var result = newname;
            var temp = result;
            if (Regex.IsMatch(result, regex_check_filename_index))
            {
                Console.WriteLine("Matching!");
                Match match = Regex.Match(result, regex_check_filename_index);
                count = int.Parse(match.Groups["index"].Value);
                Console.WriteLine(count);
                result = match.Groups["filename"].Value;
                Console.WriteLine("Filename: " + result);
                temp = result;
            }
            else count = 1;
            while (File.Exists(PathHandler.combinePath(path, result, newext)))
            {
                if (_checkboxSkipfile.IsChecked == true)
                    return;
                result = $"{temp}({++count})";
            }
            Debug.WriteLine(victim.FullPath);
            if(File.Exists(victim.FullPath))
            {
                string str = PathHandler.combinePath(path, result, newext);
                Process = $"Rename: {victim.FullPath} to {str}";
                File.Move(victim.FullPath, str);
            }
        }
        private void HandleFolder(m_Folder victim)
        {
            if (victim.FolderName == victim.NewName)
            {
                if (!victim.FolderName.Equals(victim.NewName))
                {
                    if(Directory.Exists(victim.FullPath))
                        Directory.Move(victim.FullPath, PathHandler.getPath(victim.FullPath) + "\\" + victim.NewName);
                }
                return;
            }
            var path = PathHandler.getPath(victim.FullPath);
            var oldname = victim.FolderName;
            var newname = victim.NewName;

            int count;
            var result = newname;
            var temp = result;
            if (Regex.IsMatch(result, regex_check_filename_index))
            {
                Console.WriteLine("Matching!");
                Match match = Regex.Match(result, regex_check_filename_index);
                count = int.Parse(match.Groups["index"].Value);
                Console.WriteLine(count);
                result = match.Groups["filename"].Value;
                Console.WriteLine("Filename: " + result);
                temp = result;
            }
            else count = 1;
            while (Directory.Exists(PathHandler.combinePath(path, result, "")))
            {
                if (_checkboxSkipfile.IsChecked == true)
                    return;
                result = $"{temp}({++count})";
            }
            if(Directory.Exists(victim.FullPath))
            {
                string str = PathHandler.combinePath(path, result, "");
                Process = $"Rename: {victim.FullPath} to {str}";
                Directory.Move(victim.FullPath, PathHandler.combinePath(path, result, ""));
            }
        }
        #region Add & Remove Button
        private void BtnAddFunc(object sender, RoutedEventArgs e)
        {
            SelectFunctionWindow selectfunctionwd = new SelectFunctionWindow(this);
            selectfunctionwd.ShowDialog();
            if (_tabcontrolShow.SelectedIndex == 0)
                UpdateNewName("file");
            else UpdateNewName("folder");
        }
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
                for (int i = FileShow.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Debug.WriteLine("Remove item: " + (FileShow.SelectedItems[i] as m_File).FileName);
                    fileList.Remove(FileShow.SelectedItems[i] as m_File);
                }
            else if (FolderShow.SelectedIndex != -1 && _tabcontrolShow.SelectedIndex == 1)
                for (int i = FolderShow.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Debug.WriteLine("Remove item: " + (FolderShow.SelectedItems[i] as m_Folder).FolderName);
                    folderList.Remove(FolderShow.SelectedItems[i] as m_Folder);
                }
        }
        #endregion

        #region Load File & Folder
        private void LoadFileFromPath(string RootPath)
        {
            DirectoryInfo FolderPath = new DirectoryInfo(RootPath);
            if (FolderPath.Exists)
            {
                FileInfo[] listFileInfo = FolderPath.GetFiles();
                foreach (var file in listFileInfo)
                {
                    var m_file = new m_File() { FileName = file.Name, FullPath = file.FullName, NewName = file.Name, ErrorStatus = "ChartDonut" };
                    if (CheckExitsInFileList(m_file)) continue;
                    fileList.Add(m_file);
                    UpdateNewName("file");
                    TotalItem++;
                }
            }
        }
        private void LoadFolderFromPath(string RootPath)
        {
            DirectoryInfo FolderPath = new DirectoryInfo(RootPath);
            if (FolderPath.Exists)
            {
                DirectoryInfo[] listFolderInfo = FolderPath.GetDirectories();
                foreach (var folder in listFolderInfo)
                {
                    var m_foler = new m_Folder() { FolderName = folder.Name, FullPath = folder.FullName, NewName = folder.Name, ErrorStatus = "ChartDonut" };
                    if (CheckExitsInFolderList(m_foler)) continue;
                    folderList.Add(m_foler);
                    UpdateNewName("folder");
                    TotalItem++;
                }
            }
        }
        #endregion

        #region Other
        private bool CheckExitsInFileList(m_File item)
        {
            foreach (var items in fileList)
                if (items.FullPath == item.FullPath)
                    return true;
            return false;
        }
        private bool CheckExitsInFolderList(m_Folder item)
        {
            foreach (var items in folderList)
                if (items.FullPath == item.FullPath)
                    return true;
            return false;
        }
        private void UpdateNewName(string Mode)
        {
            if (Mode == "file")
            {
                foreach (var victim in fileList)
                {
                    string newName = victim.FullPath;
                    foreach (var action in actions)
                    {
                        newName = action.Process(newName);
                    }
                    victim.NewName = PathHandler.getFileName(newName) + PathHandler.getExtension(newName);
                    if(File.Exists(PathHandler.getPath(victim.FullPath) + "\\" + victim.NewName))
                    {
                        victim.ErrorDetail = "File has been exist!";
                        victim.ErrorStatus = "Error";
                        victim.ErrorColor = "Red";
                    }
                    else
                    {
                        victim.ErrorDetail = "OK";
                        victim.ErrorStatus = "CheckCircle";
                        victim.ErrorColor = "Green";
                    }
                }
            }
            else
            {
                foreach (var victim in folderList)
                {
                    string newName = victim.FullPath;
                    foreach (var action in actions)
                    {
                        newName = action.Process(newName);
                    }
                    victim.NewName = PathHandler.getFileName(newName) + PathHandler.getExtension(newName);
                    if (Directory.Exists(PathHandler.getPath(victim.FullPath) + "\\" + victim.NewName))
                    {
                        victim.ErrorDetail = "Folder has been exist!";
                        victim.ErrorStatus = "Error";
                        victim.ErrorColor = "Red";
                    }
                    else
                    {
                        victim.ErrorDetail = "OK";
                        victim.ErrorStatus = "CheckCircle";
                        victim.ErrorColor = "Green";
                    }
                }
            }
        }
        private void ShowCompleteDialog()
        {
            _dialoghost.CloseOnClickAway = false;
            _imgCheckProcess.Opacity = 0;
            _dialoghost.IsOpen = true;
        }
        private void CloseCompleteDialog()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        #endregion

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _dialoghost.CloseOnClickAway = true;
                _imgCheckProcess.Opacity = 1;
                _processbar.Value = 100;
                timer.Stop();
            });
        }
        #region Preset Method
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
            openpresetfile.Title = "Open Preset BatchRename";
            openpresetfile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openpresetfile.ShowDialog() == true)
            {
                if (!LoadPreset(openpresetfile.FileName))
                {
                    MessageBox.Show("Invalid Preset File!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        #endregion
        #region Preset region
        public bool LoadPreset(string path)
        {
            List<string> lines = new List<string>();
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                bool isReadKey = false;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.ElementAt(0) == '#') continue;

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
            PresetName = System.IO.Path.GetFileNameWithoutExtension(path);
            return ReadPreset(lines);
        }
        private bool CheckValidKeyPreset(string reg)
        {
            Debug.WriteLine("Check: " + reg + PresetKey);
            return reg == PresetKey;
        }
        public bool ReadPreset(List<string> listactions)
        {
            foreach (var action in listactions)
            {
                string[] element = action.Split(' ');
                switch (element[0])
                {
                    case "NC":
                        {
                            actions.Add(new NewCaser() { Args = new NewCaseArgs() { Case = int.Parse(element[1]) } });
                        }
                        break;
                    case "FN":
                        {
                            actions.Add(new FullNameNormalize() { Args = new FullnameNormalizeArgs() });
                        }
                        break;
                    case "ISBN":
                        {
                            actions.Add(new ISBN() { Args = new ISBNArgs() { Modes = int.Parse(element[1]) } });
                        }
                        break;
                    case "RP":
                        {
                            actions.Add(new Replacer() { Args = new ReplaceArgs() { Needle = element[1], Hammer = element[2] } });
                        }
                        break;
                    case "UN":
                        {
                            actions.Add(new UniqueName() { Args = new UniqueNameArgs() });
                        }
                        break;
                    case "EC":
                        {
                            actions.Add(new ExtensionChanger() { Args = new ExtensionArgs() { oldExt = element[1], newExt = element[2] } });
                        }
                        break;
                }

            }
            UpdateNewName("file");
            UpdateNewName("folder");
            return true;
        }
        public bool SavePreset(string path)
        {
            using (var streamWriter = new StreamWriter(path))
            {
                WriteCommentFilePreset(streamWriter);
                streamWriter.WriteLine(PresetKey);
                foreach (var action in actions)
                {
                    switch (action)
                    {
                        case NewCaser n:
                            {
                                streamWriter.WriteLine("NC" + " " + (n.Args as NewCaseArgs).Case);
                            }
                            break;
                        case Replacer r:
                            {
                                streamWriter.WriteLine("RP" + " " + (r.Args as ReplaceArgs).Needle + " " + (r.Args as ReplaceArgs).Hammer);
                            }
                            break;
                        case ISBN i:
                            {
                                streamWriter.WriteLine("ISBN" + " " + (i.Args as ISBNArgs).Modes);

                            }
                            break;
                        case FullNameNormalize f:
                            {
                                streamWriter.WriteLine("FN");
                            }
                            break;
                        case ExtensionChanger e:
                            {
                                streamWriter.WriteLine("EC" + " " + (e.Args as ExtensionArgs).oldExt + " " + (e.Args as ExtensionArgs).newExt);

                            }
                            break;
                        case UniqueName u:
                            {
                                streamWriter.WriteLine("UN");
                            }
                            break;
                    }
                }
            }
            return true;
        }
        public void WriteCommentFilePreset(StreamWriter sw)
        {
            sw.WriteLine("#Preset File of BatchRename.\t Member 1753107 - 1753130 - 1753124.");
            sw.WriteLine("#Please use '#' at headline to write comment.");
            sw.WriteLine("#Syntax: \"KeyAction\" + \" \" + \"parameter1\" + \" \" + \"parameter1\" + '...'");
            sw.WriteLine("#Actions list:");
            sw.WriteLine("#1.NewCase : Key(NC): ");
            sw.WriteLine("#parameter1 = 0: ToUpperCase.\t 1: ToLowerCase.\t 2: ToFirstLetterCase.");
            sw.WriteLine("#2.Replace : Key(RP): ");
            sw.WriteLine("#parameter1 =  FindWhat\t parameter2 =  ReplaceWith");
            sw.WriteLine("#3.Move : Key(ISBN): ");
            sw.WriteLine("#parameter1 = StartIndex\t parameter2 = Length");
            sw.WriteLine("#4.Fullname Normalize : Key(FN): ");
            sw.WriteLine("#5.Unique Name: Key(UN): ");
            sw.WriteLine("#6.Change Extension : Key(EC): ");
            sw.WriteLine("#parameter1 =  from\t parameter2 = to");
        }
        #endregion

        #region Function navigate
        private void BtnUpFunc(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if(index > 0)
            {
                var temp = actions[index];
                actions.RemoveAt(index);
                actions.Insert(index - 1, temp);
                UpdateNewName("file");
                UpdateNewName("folder");
            }
        }
        private void BtnDownFunc(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if (index < actions.Count - 1)
            {
                var temp = actions[index];
                actions.RemoveAt(index);
                actions.Insert(index + 1,temp);
                UpdateNewName("file");
                UpdateNewName("folder");
            }
        }
        private void BtnEditFunc(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext;
            int index = listView.Items.IndexOf(item);
            var action = listView.Items.GetItemAt(index);
            (action as IActions).ShowUpdateArgDialog(this);
            if (_tabcontrolShow.SelectedIndex == 0)
                UpdateNewName("file");
            else UpdateNewName("folder");
        }
        private void BtnRemoveFunc(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext;
            int index = listView.Items.IndexOf(item);
            actions.RemoveAt(index);
            if (_tabcontrolShow.SelectedIndex == 0)
                UpdateNewName("file");
            else UpdateNewName("folder");
        }
        #endregion

        private void BtnColorMode_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if (button.IsChecked == false)
            {
                ForegroundColor = "Black";
                BackgroundColor = "#f5f5f5";
                BackgroundfuncColor = "White";
                ShadowColor = "LightGray";
                BackgroundtabitemColor = "#f5f5f5";
            }
            else
            {
                ForegroundColor = "White";
                BackgroundColor = "#20242a";
                BackgroundfuncColor = "#323741";
                ShadowColor = "LightGray";
                BackgroundtabitemColor = "#424751";
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileShow.ItemsSource = fileList;
            FolderShow.ItemsSource = folderList;
            //this._stackPanel_Replace.Children.Remove(_stackPanelReplace);
        }
    }
}