using System;
using System.Collections.Generic;
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

        }

        private void BtnRefresh(object sender, RoutedEventArgs e)
        {
           
            _comboboxPreset.SelectedIndex = 0;
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
            _comboboxPreset.Items.Add("Preset 01");
            _comboboxPreset.Items.Add("Preset 02");
            _comboboxPreset.Items.Add("Preset 03");
            _comboboxPreset.SelectedIndex = 0;
        }
        
        private void BtnNewCase(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Case");
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
        }

        private void _btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
