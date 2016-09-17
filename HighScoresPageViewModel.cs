using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Windows.Storage;

namespace EmotionsGame
{
    public class HighScoresPageViewModel
    {
        public HighScoresPageViewModel()
        {
            Players = new ObservableCollection<HighScorePlayerViewModel>();
        }

        public ObservableCollection<HighScorePlayerViewModel> Players { get; private set; }

        public async Task Initialize()
        {
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(Config.ResultsFolder, CreationCollisionOption.OpenIfExists);
            IReadOnlyList<IStorageFile> files = await folder.GetFilesAsync();
            List<HighScorePlayerViewModel> players = new List<HighScorePlayerViewModel>();
            foreach (StorageFile file in files)
            {
                if (file.FileType == ".jpeg")
                {
                    HighScorePlayerViewModel playerViewModel = await HighScorePlayerViewModel.CreateAsycn(file);
                    if (playerViewModel != null)
                    {
                        players.Add(playerViewModel);
                    }
                }
            }

            IEnumerable<HighScorePlayerViewModel> sortedPlyers = players.OrderBy(i => i.Score).Reverse();
            foreach (HighScorePlayerViewModel player in sortedPlyers)
            {
                Players.Add(player);
            }
        }
    }
}
