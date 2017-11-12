using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using Microsoft.Toolkit.Uwp.UI.Controls;

using TextGameTool.Models;
using TextGameTool.Services;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;
using Autofac;

namespace TextGameTool.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        private readonly IDialogueFileService _dialogueService;

        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(OpenFolder);
                }
                return _openFolderCommand;
            }
        }

        private ICommand _createNewCommand;
        public ICommand CreateNewCommand
        {
            get
            {
                if (_createNewCommand == null)
                {
                    _createNewCommand = new RelayCommand(CreateNew);
                }
                return _createNewCommand;
            }
        }

        private Dialogue _selected;
        public Dialogue Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public AdvancedCollectionView DialogueList { get; set; }

        private ObservableCollection<DialogueViewModel> _backingDialogueList = new ObservableCollection<DialogueViewModel>();        

        public MasterDetailViewModel(IDialogueFileService dialogueService)
        {
            _dialogueService = dialogueService;
            DialogueList = new AdvancedCollectionView(_backingDialogueList);
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            DialogueList.Clear();            

            if (viewState == MasterDetailsViewState.Both)
            {
                
            }
        }

        private async void OpenFolder()
        {
            FolderPicker picker = new FolderPicker();
            picker.FileTypeFilter.Add(".dlg");
            picker.FileTypeFilter.Add(".json");
            try
            {
                StorageFolder pickedFolder = await picker.PickSingleFolderAsync();
                if (pickedFolder == null)
                {
                    return;
                }

                IEnumerable<Dialogue> dialogues = await _dialogueService.GetDialoguesInFolder(pickedFolder);
                using (DialogueList.DeferRefresh())
                {
                    foreach(Dialogue dialogue in dialogues)
                    {
                        using (ILifetimeScope dialogueVmScope = ViewModelLocator.Container.BeginLifetimeScope())
                        {
                            _backingDialogueList.Add(dialogueVmScope.Resolve<DialogueViewModel>()
                                .Init(dialogue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to open folder: {ex.Message}. Stack trace: {ex.StackTrace}");
            }
        }

        private void CreateNew()
        {
            using (ILifetimeScope dialogueVmScope = ViewModelLocator.Container.BeginLifetimeScope())
            {
                _backingDialogueList.Add(dialogueVmScope.Resolve<DialogueViewModel>()
                    .Init(new Dialogue { Title = "New Dialogue" }));
            }
        }
    }
}
