﻿using DIYoutubeDownloader.Internal;
using DIYoutubeDownloader.ViewModels;
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
using System.Windows.Shapes;
using System.Windows.Shell;

namespace DIYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        private IViewModel viewModel { get; set; }

        #region Ctor

        public BaseWindow() : this(null)
        {
        }

        public BaseWindow(ArgumentCollection args)
        {
            InitializeComponent();
            this.Initialize(args);
        }

        #endregion

        #region Events

        private void RNavBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TbeTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void IWindowIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion  
        #region Methods

        #region Initialize
        private void Initialize(ArgumentCollection args)
        {
            try
            {
                ViewModelFactory factory = new ViewModelFactory();
                this.viewModel = factory.CreateViewModel<BaseWindowViewModel>(args);
                this.DataContext = viewModel;

                if (args != null)
                {
                    if (args.Contains(ArgumentCollection.ArgumentType.WindowIcon))
                        iWindowIcon.Source = Utils.ToBitmapImage(args.Get(ArgumentCollection.ArgumentType.WindowIcon));

                }

                //TODO - nie odwoływać się do kokretnych kontrolek tylko do klas implementujacych interfejs
                this.ucYoutubeDownloader.SubscribePropertyChangeNotification(new System.ComponentModel.PropertyChangedEventHandler(this.ChangeListener));
            }
            catch (Exception ex)
            {
                Utils.Logger.Log(EventID.DIYoutubeDownloader.Application.Exception, ex);
            }
        }
        #endregion
        #region ChangeListener
        private void ChangeListener(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && sender is IViewModel)
            {
                IViewModel childViewModel = sender as IViewModel;
                switch (e.PropertyName)
                {
                    case nameof(IWindowPropertyChangeNotifier.TaskBarProgressState):
                    case nameof(IWindowPropertyChangeNotifier.TaskBarProgressValue):
                        viewModel.NotifyPropertyChange(e.PropertyName, childViewModel.GetPropertyValue(e.PropertyName));
                        break;
                    default: break;
                }
            }
        }
        #endregion

        #endregion

    }
}
