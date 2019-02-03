using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Input;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YoutubeDownloader downloader { get; }
        private YoutubeMedia downloadingMedia { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();

            this.downloader = new YoutubeDownloader();
        }

        private void InitializeControls()
        {
            //TODO Create Own style
            SfSkinManager.ApplyStylesOnApplication = true;
            SfSkinManager.SetVisualStyle(this, VisualStyles.Office2013DarkGray);
        }

        private void TbeUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                SfTextBoxExt tbeUrl = sender as SfTextBoxExt;
                this.downloadingMedia = this.downloader.GetMediaInfo(tbeUrl?.Text);
                this.BindData(this.downloadingMedia);
            }
            catch(Exception ex)
            {
                //TODO Log
                this.downloadingMedia = null;
            }
            finally
            {
                ddbaDownloadMedia.IsEnabled = this.downloadingMedia != null;
            }
        }

        private void BindData(YoutubeMedia toBind)
        {
            baThumbnail.LargeIcon = Utils.ToBitmapImage(toBind?.Thumbnail ?? ResourceImage128.YouTube);
            tbeTitleAuthor.Text = toBind?.ToString() ?? null;
            mtbDuration.Value = (toBind?.Duration ?? new TimeSpan()).ToString("c");
            rMediaRating.Value = toBind?.AverageRatings ?? 0.0;
            baMediaLikes.Label = toBind?.LikesCount.ToString() ?? 0.ToString();
            baMediaDislikes.Label = toBind?.DislikesCount.ToString() ?? 0.ToString();

            //ddbaDownloadMedia.
        }

        private void DropDownMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.downloader.Download(this.downloadingMedia.MediaId, "Test");
        }
    }
}
