using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Threading;

using WPF.Common.Factories;
using WPF.Common.Interfaces;
using Logger = WPF.Common.Logger.Logger;
using ArgumentCollection = WPF.Common.Common.ArgumentCollection;
using WPFUtils = WPF.Common.Common.Utils;

using DesktopDashboard.Resources;
using DesktopDashboard.Internals;
using DesktopDashboard.Common;
using DesktopDashboard.ViewModels;
using EventID = DesktopDashboard.Internals.EventID.DesktopDashboard;

namespace DesktopDashboard
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IViewModel viewModel;
        
     

        public MainWindow()
        {
            InitializeComponent();
            Initialize(null);
            try
            {
                Logger.Log(EventID.Application.Start);

                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
                Left = SystemParameters.PrimaryScreenWidth - Width;
                
            }
            catch(Exception ex)
            {

            }
        }


        #region Methods

        #region Initialize
        private void Initialize(ArgumentCollection args)
        {
            try
            {
                if (args == null)
                    args = new ArgumentCollection();
                args.Set(ArgumentCollection.ArgumentType.DockingManager, dmMainWindow);
                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<MainWindowViewModel>(args);
                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Logger.Log(EventID.Application.Exception, ex);
            }
        }
        #endregion

        #endregion
        

        private static System.Windows.Media.Color ConvertColorType(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private static ImageSourceConverter ImageSourceConverter = new ImageSourceConverter();
        private static System.Windows.Media.ImageSource ConvertImage(System.Drawing.Bitmap image)
        {
            if (image == null)
                return null;
            return (ImageSource)ImageSourceConverter.ConvertFrom(image);
        }
        
               
        private void baToolBarCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void baToolBarTopMost_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
            if (this.Topmost)
                baToolBarTopMost.SmallIcon = WPFUtils.ToBitmapImage(ResourceImage48.LockOpen);
            else
                baToolBarTopMost.SmallIcon = WPFUtils.ToBitmapImage(ResourceImage48.Lock);
        }
        
        
    }
}
