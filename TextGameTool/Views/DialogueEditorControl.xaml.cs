using System;

using TextGameTool.Models;
using TextGameTool.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TextGameTool.Views
{
    public sealed partial class DialogueEditorControl : UserControl
    {
        public DialogueViewModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as DialogueViewModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register(
            nameof(MasterMenuItem),
            typeof(DialogueViewModel),
            typeof(DialogueEditorControl),
            new PropertyMetadata(null, OnMasterMenuItemPropertyChanged)
        );

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DialogueEditorControl;
            var newDialogue = e.NewValue as Dialogue;
            if (newDialogue != null)
            {
                control.MasterMenuItem.Init(newDialogue);
            }
        }

        public DialogueEditorControl()
        {
            InitializeComponent();
        }
    }
}
