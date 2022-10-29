using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minesweeper.Core.Models
{
    public class Settings : ObservableObject
    {
        private static string FilePath => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            , "settings.mine");

        [JsonIgnore]
        private bool marks = true;
        public bool Marks
        {
            get => marks;
            set
            {
                SetProperty(ref marks, value);
            }
        }
        [JsonIgnore]
        private GameDifficulty gameDifficulty = GameDifficulty.MeduimDifficulty;
        public GameDifficulty GameDifficulty
        {
            get => gameDifficulty;
            set
            {
                SetProperty(ref gameDifficulty, value);
            }
        }
        [JsonIgnore]
        private Dictionary<GameDifficulty.Difficulty, (string, int)> highScores =
            new Dictionary<GameDifficulty.Difficulty, (string, int)>() {
                { GameDifficulty.Difficulty.Easy,("Annonymous",999)},
                { GameDifficulty.Difficulty.Meduim,("Annonymous",999)},
                { GameDifficulty.Difficulty.Hard,("Annonymous",999)}
            };
        public Dictionary<GameDifficulty.Difficulty, (string, int)> HighScores { 
            get=>highScores; 
            set=>SetProperty(ref highScores,value); 
        }
        public async Task Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions() { IncludeFields = true });
            await File.WriteAllTextAsync(FilePath, json);
        }
        public static Settings Load()
        {
            if (!File.Exists(FilePath))
                return new Settings();
            var json = File.ReadAllText(FilePath);
            var settings = JsonSerializer.Deserialize<Settings>(json, new JsonSerializerOptions() { IncludeFields = true });
            if (settings != null)
                return settings;
            else return new Settings();
        }
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            await Save();
        }
    }
}
