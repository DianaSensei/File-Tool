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

namespace BatchRename
{
    public partial class MainWindow : Window
    {
        #region Attributes
        private int newCaseType;
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
        private string moveString;
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
        private string findString;
        private string replaceString;
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
        // Using a DependencyProperty as the backing store for 
        //IsCheckBoxChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUniqueNameProperty =
            DependencyProperty.Register("IsUniqueName", typeof(bool),
            typeof(MainWindow), new UIPropertyMetadata(false));
        #endregion
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

        ObservableCollection<File> fileList = new ObservableCollection<File>();

        /// <summary>
        /// Array Folder
        /// </summary>
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();
        #region File and Folder Class
        public class File : INotifyPropertyChanged
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
        private void ComboBox_Load(object sender, RoutedEventArgs e)
        {
            _comboboxPreset.Items.Add("Default");
            _comboboxPreset.SelectedIndex = 0;
        }
        /// <summary>
        /// Help -> Show About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Design by Thong", "About");
        }
        /// <summary>
        /// Help -> Help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You can do it.", "Help");
        }
        #endregion

        #region Item Navigation
        private void MenuItem_Tab_Top(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Count > 0)
                {
                    fileList.Remove((File)FileShow.SelectedItem);
                }
            }
            else
            {
                if (folderList.Count > 0)
                {
                    folderList.Move(FolderShow.SelectedIndex, 0);
                }
            }
        }
        private void MenuItem_Tab_Bottom(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Count > 0)
                {
                    //fileList.Move(FileShow.SelectedIndex, fileList.Count-1);
                }
            }
            else
            {
                if (folderList.Count > 0)
                {
                    folderList.Move(FolderShow.SelectedIndex, folderList.Count - 1);
                }
            }
        }
        private void MenuItem_Tab_Down(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Count > 0)
                {
                    if (FileShow.SelectedIndex < fileList.Count)
                    {
                        {
                            //fileList.Move(FileShow.SelectedIndex, FileShow.SelectedIndex + 1);
                        }
                    }
                }
            }
            else
            {
                if (folderList.Count > 0)
                {
                    if (FolderShow.SelectedIndex < folderList.Count)
                    {
                        {
                            folderList.Move(FolderShow.SelectedIndex, FolderShow.SelectedIndex + 1);
                        }
                    }
                }
            }
        }
        private void MenuItem_Tab_Up(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Count > 0)
                {
                    if (FileShow.SelectedIndex != 0)
                    {
                        {
                            // fileList.Move(FileShow.SelectedIndex, FileShow.SelectedIndex - 1);
                        }
                    }
                }
            }
            else
            {
                if (folderList.Count > 0)
                {
                    if (FolderShow.SelectedIndex != 0)
                    {
                        {
                            folderList.Move(FolderShow.SelectedIndex, FolderShow.SelectedIndex - 1);
                        }
                    }
                }
            }
        }
        #endregion

        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {



        }
        /// <summary>
        /// Refresh UI&Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh(object sender, RoutedEventArgs e)
        {
            _comboboxPreset.SelectedIndex = 0;
            fileList.Clear();
            folderList.Clear();
        }

        #region Method
        private void BtnNewCase(object sender, RoutedEventArgs e)
        {
            if (_btnNewCase.IsChecked == true)
            {
                this._stackPanel_NewCase.Children.Add(_comboboxNewCase);
              
            }
            else
            {
                _comboboxNewCase.SelectedIndex = -1;
                this._stackPanel_NewCase.Children.Remove(_comboboxNewCase);
                foreach (var element in fileList)
                    element.NewName = element.FileInfomation.Name;
            }
        }
        private void BtnReplace(object sender, RoutedEventArgs e)
        {
            if (_btnReplace.IsChecked == true)
            {
                this._stackPanel_Replace.Children.Add(_stackPanelReplace);
            }
            else
            {
                this._stackPanel_Replace.Children.Remove(_stackPanelReplace);
            }
        }
        private void BtnMove(object sender, RoutedEventArgs e)
        {
            if (_btnMove.IsChecked == true)
            {
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
        private Boolean LoadFileFromPath(string RootPath)
        {
            DirectoryInfo FolderPath = new DirectoryInfo(RootPath);
            if (FolderPath.Exists)
            {
                FileInfo[] listFileInfo = FolderPath.GetFiles();
                foreach (var file in listFileInfo)
                {
                    fileList.Add(new File() { FileInfomation = file, NewName = file.Name, ErrorStatus = "ChartDonut" });
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

        #region Preset Method
        private void BtnNewPreset(object sender, RoutedEventArgs e)
        {

        }
        private void BtnSavePreset(object sender, RoutedEventArgs e)
        {

        }
        private void BtnLoadPreset(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FileShow.ItemsSource = fileList;
            FolderShow.ItemsSource = folderList;

            ComboBox_Load(sender, new RoutedEventArgs());
            this._stackPanel_NewCase.Children.Remove(_comboboxNewCase);
            this._stackPanel_Replace.Children.Remove(_stackPanelReplace);
            this._stackPanel_Move.Children.Remove(_stackPanelMove);
        }

        private void _comboboxNewCase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboboxNewCase.SelectedIndex == 0)
            {
                foreach (var file in fileList)
                {
                    file.NewName = file.NewName.ToUpper();
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
                    file.NewName = file.NewName.ToLower();
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
                    file.NewName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(file.NewName.ToLower());
                }
                foreach (var folder in folderList)
                {
                    folder.NewName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(folder.NewName.ToLower());
                }

            }
        }
    }
}
