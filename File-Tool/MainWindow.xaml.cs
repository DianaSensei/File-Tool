using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
namespace BatchRename
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Array file
        /// </summary>
        ObservableCollection<File> fileList = new ObservableCollection<File>();
        /// <summary>
        /// Array Folder
        /// </summary>
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();
        /// <summary>
        /// File Info
        /// </summary>
        public class File
        {
            public string FileName { get; set; }
            public string NewFileName { get; set; }
            public string FilePath { get; set; }
            public string NewFilePath { get; set; }
            public string FileError { get; set; }
        }
        /// <summary>
        /// Folder Info
        /// </summary>
        public class Folder : File
        {
            public string FolderName { get; set; }
            public string NewFolderName { get; set; }
            public string FolderPath { get; set; }
            public string NewFolderPath { get; set; }
            public string FolderError { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
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
 
        private void MenuItem_Tab_Top(object sender, RoutedEventArgs e)
        {
            if(_tabcontrolShow.SelectedIndex == 0)
            {
                if (fileList.Count > 0)
                {
                    fileList.Move(FileShow.SelectedIndex, 0);
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
                    fileList.Move(FileShow.SelectedIndex, fileList.Count-1);
                }
            }
            else
            {
                if (folderList.Count > 0)
                {
                    folderList.Move(FolderShow.SelectedIndex, folderList.Count-1);
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
                            fileList.Move(FileShow.SelectedIndex, FileShow.SelectedIndex + 1);
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
                            fileList.Move(FileShow.SelectedIndex, FileShow.SelectedIndex - 1);
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

        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {

            fileList.Add(new File() { FileName = "A", NewFileName = "a1", FilePath = "A", FileError = "DSA" });
            
            fileList.Add(new File() { FileName = "B", NewFileName = "B1", FilePath = "A", FileError = "DSA" });
            
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
            MessageBox.Show("You can do it.","Help");
        }
        /// <summary>
        /// Load Preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Load(object sender, RoutedEventArgs e)
        {
            _comboboxPreset.Items.Add("Default");
            _comboboxPreset.SelectedIndex = 0;
        }
        /// <summary>
        /// Enable NewCase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewCase(object sender, RoutedEventArgs e)
        {
            if (_btnNewCase.IsChecked == true)
            {
                this._stackPanel_NewCase.Children.Add(_comboboxNewCase);
            }
            else
            {
                this._stackPanel_NewCase.Children.Remove(_comboboxNewCase);
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

        private void BtnFun1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFun2(object sender, RoutedEventArgs e)
        {
                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox_Load(sender,new RoutedEventArgs());
            FileShow.ItemsSource = fileList;
            
            this._stackPanel_NewCase.Children.Remove(_comboboxNewCase);
            this._stackPanel_Replace.Children.Remove(_stackPanelReplace);
            this._stackPanel_Move.Children.Remove(_stackPanelMove);
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(FileShow.SelectedIndex != -1 && _tabcontrolShow.SelectedIndex==0)
                fileList.RemoveAt(FileShow.SelectedIndex);
            if (FolderShow.SelectedIndex != -1 && _tabcontrolShow.SelectedIndex == 1)
                folderList.RemoveAt(FolderShow.SelectedIndex);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_tabcontrolShow.SelectedIndex == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    BtnStartBatch(sender, new RoutedEventArgs());
            }
            else
            {
                System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
                openFolderDialog.ShowDialog();
                    BtnStartBatch(sender, new RoutedEventArgs());
            }
        }


        private void BtnNewPreset(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSavePreset(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnFullName(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUnique(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLoadPreset(object sender, RoutedEventArgs e)
        {

        }
    }
}
