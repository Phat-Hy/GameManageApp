using GameManage.BLL.Services;
using GameManage.DAL.Entities;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameManage
{
    public partial class MainWindow : Window
    {
        private readonly GameService _gameService;

        public MainWindow()
        {
            InitializeComponent();
            _gameService = new GameService();
            LoadGames();
        }

        private void LoadGames()
        {
            GameWrapPanel.Children.Clear();
            var games = _gameService.GetAllGames();

            foreach (var game in games)
            {
                Debug.WriteLine($"Game: {game.Name}");

                // Create a border for each game
                var gameBorder = new Border
                {
                    Width = 200,
                    Height = 280,
                    Margin = new Thickness(10),
                    BorderBrush = Brushes.Black,  // Adding a visible border
                    BorderThickness = new Thickness(2),
                    Tag = game
                };

                // Load and set the background image
                var frontCoverImage = GetImageFromBase64(game.FrontCover);
                if (frontCoverImage != null)
                {
                    gameBorder.Background = new ImageBrush
                    {
                        ImageSource = frontCoverImage,
                        Stretch = Stretch.UniformToFill  // Ensures image scales properly
                    };
                }
                else
                {
                    // Optionally, you can set a placeholder background here
                    gameBorder.Background = new SolidColorBrush(Colors.Gray);  // Placeholder if image fails
                }

                // Create overlay for play button and options
                var overlayGrid = CreateOverlay(game);
                gameBorder.Child = overlayGrid;

                // Add MouseEnter and MouseLeave event handlers to the gameBorder
                gameBorder.MouseEnter += (s, e) =>
                {
                    overlayGrid.Visibility = Visibility.Visible;  // Show overlay when mouse enters the game border
                };

                gameBorder.MouseLeave += (s, e) =>
                {
                    overlayGrid.Visibility = Visibility.Collapsed;  // Hide overlay when mouse leaves the game border
                };

                // Add the game border to the wrap panel
                GameWrapPanel.Children.Add(gameBorder);
            }

            // Force the layout to update
            GameWrapPanel.InvalidateVisual();
            GameWrapPanel.UpdateLayout();
        }



        private ImageSource GetImageFromBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                Debug.WriteLine("Base64 string is null or empty.");
                return null;
            }

            try
            {
                Debug.WriteLine($"Base64 string length: {base64String.Length}");

                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var stream = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;  // Ensures the image is fully loaded before being used
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error converting base64 to image: {ex.Message}");
                return null;
            }
        }



        private Grid CreateOverlay(Game game)
        {
            var overlayGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)), // Semi-transparent overlay
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Visibility = Visibility.Collapsed // Initially hidden
            };

            // Play button
            var playButton = new Button
            {
                Content = "▶",
                FontSize = 30,
                Width = 60,
                Height = 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent
            };
            playButton.Click += (s, e) => LaunchGame(game.ExePath);

            // Options Menu
            var optionsMenu = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };
            var editButton = new Button { Content = "Edit" };
            var deleteButton = new Button { Content = "Delete" };

            editButton.Click += (s, e) => EditGame(game);
            deleteButton.Click += (s, e) => DeleteGame(game);

            optionsMenu.Children.Add(editButton);
            optionsMenu.Children.Add(deleteButton);

            // Add buttons to the overlay
            overlayGrid.Children.Add(playButton);
            overlayGrid.Children.Add(optionsMenu);

            // Open the DetailWindow when the game border is clicked
            overlayGrid.MouseDown += (s, e) =>
            {
                var detailWindow = new DetailWindow(game);
                detailWindow.ShowDialog();
            };

            return overlayGrid;
        }


        private void LaunchGame(string exePath)
        {
            Process.Start(exePath);
        }

        private void EditGame(Game game)
        {
            Game selected = game as Game;
            MessageBox.Show("Game Choosen: " + selected.Name);
            AddUpdateWindow addUpdateWindow = new AddUpdateWindow();
            addUpdateWindow.EditedOne = selected;
            addUpdateWindow.ShowDialog();
            LoadGames();
        }

        private void DeleteGame(Game game)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this game?", "Delete Game", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _gameService.DeleteGame(game.Id);
                LoadGames();  // Refresh the game list
            }
        }

        private void AddNewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var addUpdateWindow = new AddUpdateWindow();
            addUpdateWindow.ShowDialog();
            LoadGames(); // Refresh game list after adding a new game
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameSearchBox.Text.Trim();
            string publisher = PublisherSearchBox.Text.Trim();

            var filteredGames = _gameService.SearchByNameOrPublisher(name, publisher);

            GameWrapPanel.Children.Clear();
            foreach (var game in filteredGames)
            {
                Debug.WriteLine($"Game: {game.Name}");

                var gameBorder = new Border
                {
                    Width = 200,
                    Height = 280,
                    Margin = new Thickness(10),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2),
                    Tag = game
                };

                var frontCoverImage = GetImageFromBase64(game.FrontCover);
                if (frontCoverImage != null)
                {
                    gameBorder.Background = new ImageBrush
                    {
                        ImageSource = frontCoverImage,
                        Stretch = Stretch.UniformToFill
                    };
                }
                else
                {
                    gameBorder.Background = new SolidColorBrush(Colors.Gray);
                }

                var overlayGrid = CreateOverlay(game);
                gameBorder.Child = overlayGrid;

                gameBorder.MouseEnter += (s, e) => overlayGrid.Visibility = Visibility.Visible;
                gameBorder.MouseLeave += (s, e) => overlayGrid.Visibility = Visibility.Collapsed;

                GameWrapPanel.Children.Add(gameBorder);
            }

            GameWrapPanel.InvalidateVisual();
            GameWrapPanel.UpdateLayout();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortOrder = selectedItem.Tag.ToString();

                // Sort the games based on the selected option
                var sortedGames = _gameService.SortGames(sortOrder);

                // Update the displayed games
                GameWrapPanel.Children.Clear();
                foreach (var game in sortedGames)
                {
                    AddGameToWrapPanel(game);
                }
            }
        }
        private void AddGameToWrapPanel(Game game)
        {
            // Create a border for each game
            var gameBorder = new Border
            {
                Width = 200,
                Height = 280,
                Margin = new Thickness(10),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Tag = game
            };

            var frontCoverImage = GetImageFromBase64(game.FrontCover);
            if (frontCoverImage != null)
            {
                gameBorder.Background = new ImageBrush
                {
                    ImageSource = frontCoverImage,
                    Stretch = Stretch.UniformToFill  // Ensures image scales properly
                };
            }
            else
            {
                gameBorder.Background = new SolidColorBrush(Colors.Gray);
            }

            var overlayGrid = CreateOverlay(game);
            gameBorder.Child = overlayGrid;

            gameBorder.MouseEnter += (s, e) =>
            {
                overlayGrid.Visibility = Visibility.Visible;  // Show overlay when mouse enters the game border
            };

            gameBorder.MouseLeave += (s, e) =>
            {
                overlayGrid.Visibility = Visibility.Collapsed;  // Hide overlay when mouse leaves the game border
            };

            GameWrapPanel.Children.Add(gameBorder);
        }

    }
}
