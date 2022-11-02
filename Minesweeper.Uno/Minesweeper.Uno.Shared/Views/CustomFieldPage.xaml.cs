using Microsoft.UI.Xaml.Controls;
using Minesweeper.Core.ViewModels;

// To learn more about Uno, the Uno project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Minesweeper.Uno.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomFieldPage : Page
    {
        public CustomFieldPageViewModel ViewModel { get; }
        public CustomFieldPage(GamePageViewModel gameViewModel)
        {
            this.InitializeComponent();

            ViewModel = new CustomFieldPageViewModel(gameViewModel);
            DataContext=ViewModel;
        }
    }
}
