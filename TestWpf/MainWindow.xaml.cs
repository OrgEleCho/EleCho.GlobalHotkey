using CommunityToolkit.Mvvm.Input;
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
using EleCho.GlobalHotkey.Windows.Wpf;

namespace TestWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            var x = this.TestCommand;
            InitializeComponent();
        }

        [RelayCommand]
        public void Test(object parameter)
        {
            MessageBox.Show("Fuck you world");
        }

        public static ICommand StaticTestCommand = new RelayCommand(() =>
        {
            MessageBox.Show("Fuck you world");
        });
    }
}
