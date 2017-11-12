using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextGameTool.Models;
using TextGameTool.Services;

namespace TextGameTool.ViewModels
{
    public class DialogueViewModel : ViewModelBase
    {
        private readonly IDialogueFileService _dialogueFileService;

        private Dialogue _backingDialogue;

        private ICommand _saveDialogueCommand;
        public ICommand SaveDialogueCommand
        {
            get { return _saveDialogueCommand ?? (_saveDialogueCommand = new RelayCommand(SaveDialogue)); }
        }

        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set => Set(ref _titleText, value);
        }

        private string _bodyText;
        public string BodyText
        {
            get => _bodyText;
            set
            {
                Set(ref _bodyText, value);
                // todo: don't update bodypreview unless this has valid formatting
                RaisePropertyChanged(nameof(BodyPreview));
            }
        }
        
        public string BodyPreview
        {
            get
            {
                // todo: parse and format the body text
                return BodyText;
            }
        }

        public DialogueViewModel(IDialogueFileService dialogueFileService)
        {
            _dialogueFileService = dialogueFileService;
        }

        public DialogueViewModel Init(Dialogue dialogue)
        {
            _backingDialogue = dialogue;
            TitleText = dialogue.Title;
            BodyText = dialogue.Body;
            return this;
        }

        private async void SaveDialogue()
        {
            await _dialogueFileService.SaveDialogue(new Dialogue
            {
                Title = TitleText,
                Body = BodyText,
                DirectedNeighborRelativePaths = _backingDialogue.DirectedNeighborRelativePaths,
                RelativeDialoguePath = _backingDialogue.RelativeDialoguePath,
                UndirectedNeighborRelativePaths = _backingDialogue.UndirectedNeighborRelativePaths
            },
            TitleText);
        }
    }
}           

