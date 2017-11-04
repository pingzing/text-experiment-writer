using System;

using TextGameTool.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TextGameTool.Views
{
    public sealed partial class TextFilePagePage : Page
    {
        private TextFilePageViewModel ViewModel
        {
            get { return DataContext as TextFilePageViewModel; }
        }

        public TextFilePagePage()
        {
            InitializeComponent();
        }
    }
}
