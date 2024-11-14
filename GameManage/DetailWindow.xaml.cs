using GameManage.DAL.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameManage
{
    public partial class DetailWindow : Window
    {
        private readonly Game _game;

        public DetailWindow(Game game)
        {
            InitializeComponent();
            _game = game;
            DisplayGameDetails();
        }

        private void DisplayGameDetails()
        {
            // Set game details text
            GameNameText.Text = _game.Name;
            PublisherText.Text = "Publisher: " + _game.Publisher;
            ReleaseDateText.Text = "Release Date: " + _game.ReleaseDate.ToShortDateString();
            DescriptionText.Text = "Description: " + _game.Description;

            // Set front cover image
            var frontCoverImage = GetImageFromBase64(_game.FrontCover);
            if (frontCoverImage != null)
            {
                FrontCoverImage.Source = frontCoverImage;
            }
            else
            {
                // Set a placeholder image or color
                FrontCoverImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder.png"));
            }

            // Set banner image
            var bannerImage = GetImageFromBase64(_game.Banner);
            if (bannerImage != null)
            {
                BannerImage.Source = bannerImage;
            }
            else
            {
                // Set a placeholder image or color for the banner
                BannerImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder.png"));
            }

            // Attach event handlers for the buttons
            PlayButton.Click += PlayButton_Click;
            BrowseButton.Click += BrowseButton_Click;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Log the exe path
            Console.WriteLine("ExePath: " + _game.ExePath);

            // Trim quotes around the path if they exist
            string exePath = _game.ExePath.Trim('"');

            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
            else
            {
                MessageBox.Show("Executable file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Log the directory path
            string exePath = _game.ExePath.Trim('"'); // Remove any extra quotes around the path
            string directoryPath = Path.GetDirectoryName(exePath);

            Console.WriteLine("Directory Path: " + directoryPath);

            if (Directory.Exists(directoryPath))
            {
                Process.Start("explorer.exe", directoryPath); // Open the folder where the game executable is located
            }
            else
            {
                MessageBox.Show("Game file location not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private ImageSource GetImageFromBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var stream = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting base64 to image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
