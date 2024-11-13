using GameManage.BLL.Services;
using GameManage.DAL.Entities;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace GameManage
{
    public partial class AddUpdateWindow : Window
    {
        private readonly GameService _gameService;

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
                FrontCoverFilePathTextBlock.Text = _frontCoverImagePath;  // Show file path
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
                BannerImageFilePathTextBlock.Text = _bannerImagePath;  // Show file path
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

                var newGame = new Game
                {
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
    }
}
