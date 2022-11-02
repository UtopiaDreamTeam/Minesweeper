using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Minesweeper.Core.Helpers;
using Minesweeper.Core.Interfaces;
using Minesweeper.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace Minesweeper.Uno.Views
{
    public sealed partial class GamePage : Page,IRunOnMainThread,IAskForName
    {
        private readonly GamePageViewModel viewModel;
        public GamePage()
        {
            InitializeComponent();
            viewModel = new GamePageViewModel(this,this);
            viewModel.PropertyChanged += OnPropertyChanged;
            DataContext = viewModel;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.Cells))
                Resize();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }

        private void Resize()
        {
            double minSize = Math.Min(grid.ActualHeight / viewModel.Settings.GameDifficulty.Rows, grid.ActualWidth / viewModel.Settings.GameDifficulty.Rows);
            minSize = Math.Max(0, minSize);

            for (int i = 0; i < viewModel.Cells.Length; i++)
            {
                viewModel.Cells[i].Height = minSize;
                viewModel.Cells[i].Width = minSize;
                var (x, y) = CellHelper.GetPosFromIndex(i, viewModel.Settings.GameDifficulty.Columns);
                viewModel.Cells[i].Margin = (minSize * x, minSize * y, 0, 0);
            }
        }

        private void CloseGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        private async void About_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                Title = "About",
                Content = new AboutPage(),
            };
            await dialog.ShowAsync();
        }
        private async void Help_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                Title = "Help",
                Content = new HelpPage(),
            };
            await dialog.ShowAsync();
        }
        private async void CustomSettings_Click(object sender, RoutedEventArgs e)
        {
            CustomFieldPage customFieldPage = new CustomFieldPage(viewModel);
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Close",
                Title = "Custom game settings",
                Content = customFieldPage,
            };
            var result=await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
                return;
            var newField = customFieldPage.ViewModel.GameDifficulty;
            viewModel.Settings.GameDifficulty = newField;
            viewModel.RestartCommand.Execute(null);
        }

        public void Run(Action action)
        {
            DispatcherQueue.TryEnqueue(() => { action(); });
        }

        public async Task<string> AskForName()
        {
            TextBox textBox = new TextBox();
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                Title = "Input your name",
                Content = textBox,
            };
            await dialog.ShowAsync();
            return textBox.Text;
        }
        private async void BestTime_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                Title = "Best Times",
                Content = new ScorePage(viewModel.Settings.HighScores),
            };
            await dialog.ShowAsync();
        }
    }
}
