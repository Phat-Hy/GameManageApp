using GameManage.BLL.Services;
using GameManage.DAL.Entities;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GameManage
{
    public partial class AddUpdateWindow : Window
    {
        private readonly GameService _gameService;
        public Game EditedOne { get; set; }

        public AddUpdateWindow()
        {
            InitializeComponent();
            _gameService = new GameService();
        }

        // Store the paths of the selected images
        private string _frontCoverImagePath;
        private string _bannerImagePath;

        private void ChooseFrontCoverImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _frontCoverImagePath = openFileDialog.FileName;
                FrontCoverFilePathTextBlock.Text = _frontCoverImagePath;
            }
        }

        private void ChooseBannerImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _bannerImagePath = openFileDialog.FileName;
                BannerImageFilePathTextBlock.Text = _bannerImagePath;
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Convert images to Base64 if selected
                string frontCoverBase64 = string.IsNullOrEmpty(_frontCoverImagePath) ? string.Empty : ConvertImageToBase64(_frontCoverImagePath);
                string bannerImageBase64 = string.IsNullOrEmpty(_bannerImagePath) ? string.Empty : ConvertImageToBase64(_bannerImagePath);

                var game = new Game
                {
                    Id = EditedOne?.Id ?? 0,  // If EditedOne is not null, use its Id for updates
                    Name = GameNameTextBox.Text,
                    Publisher = PublisherTextBox.Text,
                    ReleaseDate = ReleaseDatePicker.SelectedDate ?? DateTime.Now,
                    Description = DescriptionTextBox.Text,
                    ExePath = ExePathTextBox.Text,
                    FrontCover = frontCoverBase64,
                    Banner = bannerImageBase64
                };

                _gameService.AddGame(newGame);
                MessageBox.Show("Game added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditedOne != null)
            {
                GameNameTextBox.Text = EditedOne.Name;
                PublisherTextBox.Text = EditedOne.Publisher;
                ReleaseDatePicker.SelectedDate = EditedOne.ReleaseDate;
                DescriptionTextBox.Text = EditedOne.Description;
                ExePathTextBox.Text = EditedOne.ExePath;

                // Set the front cover image
                if (!string.IsNullOrEmpty(EditedOne.FrontCover))
                {
                    FrontCoverPreview.Source = GetImageFromBase64(EditedOne.FrontCover);
                }

                // Set the banner image
                if (!string.IsNullOrEmpty(EditedOne.Banner))
                {
                    BannerImagePreview.Source = GetImageFromBase64(EditedOne.Banner);
                }
            }
            // Set text fields and other properties
            return;

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
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

    }
}
