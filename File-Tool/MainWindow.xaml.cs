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
#endregion

namespace BatchRename
{
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        #region Attributes
        public const string PresetKey = "batchpreset17clc3";
        private int currentItemCount;
        private int totalitem;
        private string presetName="Default";
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

        ObservableCollection<m_File> fileList = new ObservableCollection<m_File>();
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();
        public BindingList<IActions> actions = new BindingList<IActions>();

        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChangeEvent([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int TotalItem {
            get {
                if (_tabcontrolShow.SelectedIndex == 1) return folderList.Count;
                return fileList.Count;
                }
            set
            {
                totalitem = value;
                RaiseChangeEvent("TotalItem");
            }
        }

        public int CurrentItemCount
        {
            get => currentItemCount;
            set
            {
                currentItemCount = value;
                RaiseChangeEvent("CurrentItemCount");
            }
        }

        public string PresetName { get => presetName; set
            {
                presetName = value;
                RaiseChangeEvent("PresetName");
            }
        }
        #region File and Folder Class
        public class m_File : INotifyPropertyChanged
        {
            private string newName;
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            listView.ItemsSource = actions;
            this.DataContext = this;
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

        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {
            _dialoghost.CloseOnClickAway = false;
            _imgCheckProcess.Opacity = 0;
            _dialoghost.IsOpen = true;
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
                    Debug.WriteLine(combinePath(resultPath, tempres, extension));
                    if ((tempres + extension) != victim.FileInfomation.Name)
                    {
                        while (File.Exists(combinePath(resultPath, tempres, extension)))
                        {
                            count++;
                            tempres = result + "(" + count + ")";
                        }
                        if (count > 1) result = tempres;
                    }
                    Debug.WriteLine(victim.FileInfomation.FullName);
                    Debug.WriteLine(combinePath(resultPath, result, extension));
                    File.Move(victim.FileInfomation.FullName, combinePath(resultPath, result, extension));
                    CurrentItemCount++;
                }
                
            }
            else
            {

            }
            _dialoghost.CloseOnClickAway = true;
            _imgCheckProcess.Opacity = 1;
            _processbar.Value = 100;
            fileList.Clear();
            CurrentItemCount = 0;
        }
      
        #region Add & Remove Button
        private void BtnAddFunc(object sender, RoutedEventArgs e)
        {
            SelectFunctionWindow selectfunctionwd = new SelectFunctionWindow(this);
            selectfunctionwd.Show();
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
                    TotalItem++;
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
                    TotalItem++;
                }
                return true;
            }
            return false;
        }
        #endregion

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
            if(openpresetfile.ShowDialog() == true)
            {
                if (!LoadPreset(openpresetfile.FileName))
                {
                    MessageBox.Show("Invalid Preset File!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

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
            sw.WriteLine("#no parameter.");
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

        //Còn Lỗi
        private void BtnUpFunc(object sender, RoutedEventArgs e)
        {
            actions.Add(new NewCaser() { Args = new NewCaseArgs() { Case = 0 } });
            actions.Add(new Replacer() { Args = new ReplaceArgs() { Needle = "a", Hammer = "b" } });
            /*
              int index = listView.SelectedIndex;
            if(index > 0)
            {
                IActions temp = actions[index];
                actions[index] = actions[index-1];
                actions[index-1] = temp;
            }
            */
        }
        //Còn lỗi
        private void BtnDownFunc(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if(index < actions.Count - 1)
            {
                IActions temp = actions[index];
                actions[index] = actions[index+1];
                actions[index + 1] = temp;
            }
        }

        private void BtnEditFunc(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext;
            int index = listView.Items.IndexOf(item);
            var action = listView.Items.GetItemAt(index);
            (action as IActions).ShowUpdateArgDialog();
        }
        private void BtnRemoveFunc(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext;
            int index = listView.Items.IndexOf(item);
            actions.RemoveAt(index);
        }
    }
}
