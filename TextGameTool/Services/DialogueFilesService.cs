using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TextGameTool.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace TextGameTool.Services
{
    public class DialogueFilesService : IDialogueFileService
    {
        public const string DialogueFileExtension = "dlg";
        private Dictionary<string, StorageFile> _knownFiles = new Dictionary<string, StorageFile>();
        private StorageFolder _activeFolder;

        public async Task<IEnumerable<Dialogue>> GetDialoguesInFolder(StorageFolder folder)
        {
            _activeFolder = folder;

            IEnumerable<StorageFile> dialogueFiles = (await folder.GetFilesAsync())
                .Where(x => x.FileType == $".{DialogueFileExtension}");

            List<Dialogue> dialogueList = new List<Dialogue>();
            foreach (var file in dialogueFiles)
            {
                _knownFiles.AddOrUpdate(file.DisplayName, file);
                string fileContents = await FileIO.ReadTextAsync(file);
                Dialogue deserializedDialogue = JsonConvert.DeserializeObject<Dialogue>(fileContents);
                dialogueList.Add(deserializedDialogue);
            }
            return dialogueList;
        }

        /// <summary>
        /// Attempt to serialize the given Dialogue to JSON, and save it with the given filename. If the application hasn't seen a file with this name before,
        /// it will ask the user where to save it.
        /// </summary>
        /// <param name="dialogue">The Dialogue file to save.</param>
        /// <param name="fileName">File's desired name.</param>
        /// <returns>True if saving succeeded, false otherwise.</returns>
        public async Task<bool> SaveDialogue(Dialogue dialogue, string fileName)
        {
            string dialogueAsJson = JsonConvert.SerializeObject(dialogue);

            string[] splitString = fileName.Split('.');
            string displayName = splitString.Length > 1
                ? splitString
                    .Take(splitString.Length - 1)
                    .Aggregate((accumulated, extra) => accumulated + extra)
                : splitString[0];

            if (_knownFiles.TryGetValue(displayName, out StorageFile file))
            {
                try
                {
                    await FileIO.WriteTextAsync(file, dialogueAsJson);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Found file in known list. Failed to update-save. Message: {ex.Message}. Stack trace: {ex.StackTrace}");
                    return false;
                }
            }
            else
            {
                FileSavePicker picker = new FileSavePicker
                {
                    DefaultFileExtension = $".{DialogueFileExtension}",
                    SuggestedFileName = fileName,                    
                };
                picker.FileTypeChoices.Add("Dialogue File", new[] { ".dlg" });
                try
                {
                    StorageFile resultingFile = await picker.PickSaveFileAsync();
                    if (resultingFile != null)
                    {
                        _knownFiles.AddOrUpdate(displayName, resultingFile);
                        await FileIO.WriteTextAsync(resultingFile, dialogueAsJson);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Did not find file in known list. Failed to new-save. Message: {ex.Message}. Stack trace: {ex.StackTrace}");
                    return false;
                }
            }
        }
    }
}
