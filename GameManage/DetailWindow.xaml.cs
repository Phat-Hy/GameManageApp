using GameManage.DAL.Entities;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameManage
{
    public partial class DetailWindow : Window
    {
        public DetailWindow(Game game)
        {
            InitializeComponent();
            LoadGameDetails(game);
        }

        private void LoadGameDetails(Game game)
        {
            GameTitle.Text = game.Name;
            GamePublisher.Text = $"Publisher: {game.Publisher}";
            GameReleaseDate.Text = $"Release Date: {game.ReleaseDate.ToShortDateString()}";
            GameDescription.Text = game.Description;

            BannerImage.Source = new BitmapImage(new Uri($"data:image/png;base64,{game.Banner}", UriKind.RelativeOrAbsolute));
            FrontCoverImage.Source = new BitmapImage(new Uri($"data:image/png;base64,{game.FrontCover}", UriKind.RelativeOrAbsolute));
        }
    }
}
