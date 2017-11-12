using System.Collections.Generic;
using System.Threading.Tasks;
using TextGameTool.Models;
using Windows.Storage;

namespace TextGameTool.Services
{
    public interface IDialogueFileService
    {
        Task<IEnumerable<Dialogue>> GetDialoguesInFolder(StorageFolder folder);
        Task<bool> SaveDialogue(Dialogue dialogue, string fileName);
    }
}
