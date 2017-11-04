using System;

using TextGameTool.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TextGameTool.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
