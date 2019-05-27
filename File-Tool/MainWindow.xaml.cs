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
        ObservableCollection<File> fileList = new ObservableCollection<File>();
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();
        public class File
        {
            public string FileName { get; set; }
            public string NewFileName { get; set; }
            public string FilePath { get; set; }
            public string NewFilePath { get; set; }
            public string FileError { get; set; }
        }
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
        private void MenuItem_Tab_AddFile(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Tab_AddFolder(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Tab_Top(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Tab_Bottom(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Tab_Down(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Tab_Up(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStartBatch(object sender, RoutedEventArgs e)
        {

            fileList.Add(new File() { FileName = "A", NewFileName = "a1", FilePath = "A", FileError = "DSA" });
        }

        private void BtnRefresh(object sender, RoutedEventArgs e)
        {
            _comboboxPreset.SelectedIndex = 0;
            fileList.Clear();
            folderList.Clear();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Design by Thong", "About");
        }
        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You can do it.","Help");
        }
        
        private void ComboBox_Load(object sender, RoutedEventArgs e)
        {
            _comboboxPreset.Items.Add("Default");
            _comboboxPreset.SelectedIndex = 0;
        }
        
        private void BtnNewCase(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnReplace(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckTog(object sender, RoutedEventArgs e)
        {

        }

        private void UnCheckTog(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMove(object sender, RoutedEventArgs e)
        {

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
                
            }
        }


        private void BtnNewPreset(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSavePreset(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
