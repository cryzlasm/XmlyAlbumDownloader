
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using XmlyAlbumDownloader.Models;

namespace XmlyAlbumDownloader.ViewModels
{
    public class TrackViewModel : ViewModelBase
    {
        private Track _track;

        public Track Track
        {
            get { return _track; }
            set
            {
                _track = value;
                RaisePropertyChanged(() => Track);
            }
        }

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }


        /// <summary>
        /// 0 - not start;1 - succeed;-1 - failed;2 - in progress
        /// </summary>
        private int _downloadStatus = 0;

        public int DownloadStatus
        {
            get { return _downloadStatus; }
            set
            {
                _downloadStatus = value;
                RaisePropertyChanged(() => DownloadStatus);
            }
        }

        private string _downloadStatusText;

        public string DownloadStatusText
        {
            get { return _downloadStatusText; }
            set
            {
                _downloadStatusText = value;
                RaisePropertyChanged(() => DownloadStatusText);
            }
        }
    }
}
