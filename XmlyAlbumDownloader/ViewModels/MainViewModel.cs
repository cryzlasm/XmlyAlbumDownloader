using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using XmlyAlbumDownloader.Common;
using XmlyAlbumDownloader.Models;
using XmlyAlbumDownloader.Services;
using XmlyAlbumDownloaderI.ViewModels;

namespace XmlyAlbumDownloader.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        IAlbum _albumService;

        public MainViewModel()
        {
            _albumService = new AlbumService();
            _albumService.OnDownloaded += Album_OnDownloaded;
        }

        #region Property
        public string WindowTitle
        {
            get
            {
                return AppConfig.AppName + AppConfig.Version;
            }
        }

        /// <summary>
        /// 下载目录
        /// </summary>
        public string DownloadFolder
        {
            get
            {
                return AppConfig.DownloadFolder;
            }
            set { AppConfig.DownloadFolder = value; RaisePropertyChanged(() => DownloadFolder); }
        }

        private string _albumUrl; // "https://www.ximalaya.com/album/20394123";

        public string AlbumUrl
        {
            get { return _albumUrl; }
            set { 
                _albumUrl = value;
                RaisePropertyChanged(() => AlbumUrl);
                SearchCommand.RaiseCanExecuteChanged();
                OpenBrowserCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<TrackViewModel> _tracks = new ObservableCollection<TrackViewModel>();
        public ObservableCollection<TrackViewModel> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                RaisePropertyChanged(() => Tracks);
                DownloadCommand.RaiseCanExecuteChanged();
                
            }
        }

        private string _downloadStatus = "未下载";

        public string DownloadStatus
        {
            get { return _downloadStatus; }
            set
            {
                _downloadStatus = value;
                RaisePropertyChanged(() => DownloadStatus);
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                {
                    OpenFileCommand.RaiseCanExecuteChanged();
                    OpenFileFolderCommand.RaiseCanExecuteChanged();
                });
            }
        }
        #endregion

        #region Command        
        private RelayCommand _searchCommand;

        /// <summary>
        /// 搜索专辑
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(Search, () =>
                    {
                        if (string.IsNullOrWhiteSpace(AlbumUrl))
                            return false;

                        return true;
                    });
                }
                return _searchCommand;
            }
            set { _searchCommand = value; }
        }
        
        private RelayCommand _openBrowserCommand;

        /// <summary>
        /// 在浏览器打开url
        /// </summary>
        public RelayCommand OpenBrowserCommand
        {
            get
            {
                if (_openBrowserCommand == null)
                {
                    _openBrowserCommand = new RelayCommand(OpenBrowser, () =>
                    {
                        if (string.IsNullOrWhiteSpace(AlbumUrl))
                            return false;

                        return true;
                    });
                }
                return _openBrowserCommand;
            }
            set { _openBrowserCommand = value; }
        }

        private RelayCommand _downloadCommand;
        public RelayCommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(Download, () =>
                    {
                        if (this.Tracks == null || this.Tracks.Count == 0)
                                return false;

                        return true;
                    });
                }   
                return _downloadCommand;
            }
            set { _downloadCommand = value; }
        }

        private RelayCommand _openDownloadFolderCommand;

        /// <summary>
        /// 打开下载目录命令
        /// </summary>
        public RelayCommand OpenDownloadFolderCommand
        {
            get
            {
                if (_openDownloadFolderCommand == null)
                {
                    _openDownloadFolderCommand = new RelayCommand(OpenDownloadFolder, () =>
                    {
                        return true;
                    });
                }
                return _openDownloadFolderCommand;
            }
            set { _openDownloadFolderCommand = value; }
        }


        private RelayCommand _setDownloadFolderCommand;

        /// <summary>
        /// 设置下载目录命令
        /// </summary>
        public RelayCommand SetDownloadFolderCommand
        {
            get
            {
                if (_setDownloadFolderCommand == null)
                {
                    _setDownloadFolderCommand = new RelayCommand(SetDownloadFolder, () =>
                    {
                        return true;
                    });
                }
                return _setDownloadFolderCommand;
            }
            set { _setDownloadFolderCommand = value; }
        }

        private RelayCommand<TrackViewModel> openFileFolderCommand;

        /// <summary>
        /// 打开文件目录命令
        /// </summary>
        public RelayCommand<TrackViewModel> OpenFileFolderCommand
        {
            get
            {
                if (openFileFolderCommand == null)
                {
                    openFileFolderCommand = new RelayCommand<TrackViewModel>(OpenFileFolder, (TrackViewModel p) => { return true; });
                }
                return openFileFolderCommand;
            }
            set { openFileFolderCommand = value; }
        }

        private RelayCommand<TrackViewModel> _openFileCommand;

        /// <summary>
        /// 打开文件命令
        /// </summary>
        public RelayCommand<TrackViewModel> OpenFileCommand
        {
            get
            {
                if (_openFileCommand == null)
                {
                    _openFileCommand = new RelayCommand<TrackViewModel>(OpenFile, (TrackViewModel p) =>
                    {
                        //怎么触发？
                        //if (p != null && p.DownloadStatus == 1) return true;
                        //return false;
                        return true;
                    });
                }
                return _openFileCommand;
            }
            set { _openFileCommand = value; }
        }

        private RelayCommand _browseGitHubProjectCommand;

        /// <summary>
        /// 访问github项目命令
        /// </summary>
        public RelayCommand BrowseGitHubProjectCommand
        {
            get
            {
                if (_browseGitHubProjectCommand == null)
                {
                    _browseGitHubProjectCommand = new RelayCommand(BrowseGitHubProject, () => { return true; });
                }
                return _browseGitHubProjectCommand;
            }
            set { _browseGitHubProjectCommand = value; }
        }

      
        private RelayCommand _sendEmailCommand;

        /// <summary>
        /// 访问email命令
        /// </summary>
        public RelayCommand SendEmailCommand
        {
            get
            {
                if (_sendEmailCommand == null)
                {
                    _sendEmailCommand = new RelayCommand(SendEamil, () => { return true; });
                }
                return _sendEmailCommand;
            }
            set { _sendEmailCommand = value; }
        }

        #endregion

        public void Dispose()
        {
            _albumService.OnDownloaded -= Album_OnDownloaded;

            _albumService.StopDownload();
        }

        private async void Search()
        {
            if (!string.IsNullOrWhiteSpace(AlbumUrl))
            {
                //https://www.ximalaya.com/album/20394123
                string albumId = AlbumUrl.Replace("https://www.ximalaya.com/album/", "").Split('/')[0];

                var hasPage = true;
                var pageNum = 1;
                Tracks = new ObservableCollection<TrackViewModel>();

                while (hasPage)
                {
                    var result = await _albumService.GetTracksListAsync(int.Parse(albumId), pageNum);

                    if (result != null)
                    {
                        //为了触发Change,是否有更好的方法？
                        var temp = new ObservableCollection<TrackViewModel>();
                        foreach (var item in Tracks)
                        {
                            temp.Add(item);
                        }

                        foreach (var track in result.Data.Tracks)
                        {
                            temp.Add(new TrackViewModel()
                            {
                                Track = track,
                                IsSelected = true
                            });
                        }
                        
                        Tracks = temp;

                        if (result.Data.TrackTotalCount > (result.Data.Tracks.Count + result.Data.PageSize * (pageNum - 1)))
                        {
                            pageNum++;
                        }
                        else
                        {
                            hasPage = false;
                        }
                    }
                    else
                    {
                        hasPage = false;
                    }
                }

                this.DownloadStatus = "未下载";
            }
        }

        private void OpenBrowser()
        {
            Helper.ProcessStart(AlbumUrl);
        }
        private void Download()
        {
            var selectItems = Tracks
                .Where(e => e.IsEnabled && e.IsSelected)
                .ToList();
            _albumService.DownloadTracks(selectItems);
        }
        private void OpenDownloadFolder()
        {
            Helper.ProcessStart(DownloadFolder + "\\");
        }

        private void SetDownloadFolder()
        {
            var dialog = new CommonOpenFileDialog("下载位置");
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                DownloadFolder = dialog.FileName;
            }
        }
        public void OpenFileFolder(TrackViewModel track)
        {
            Helper.OpenFileFolder(Path.Combine(DownloadFolder, Helper.RemoveInvalidFileNameChars(track.Track.AlbumTitle)));
        }

        private void OpenFile(TrackViewModel track)
        {
            Helper.ProcessStart(Path.Combine(DownloadFolder, Helper.RemoveInvalidFileNameChars(track.Track.AlbumTitle),track.Track.Index + "-" + Helper.RemoveInvalidFileNameChars(track.Track.Title)+ ".m4a"));
        }

        private void BrowseGitHubProject()
        {
            Helper.ProcessStart("https://github.com/ludingsheng/XmlyAlbumDownloader");
        }

        private void SendEamil()
        {
            Helper.ProcessStart("mailto://ludingsheng@hotmail.com");
        }

       
        private void Album_OnDownloaded(TrackViewModel obj)
        {
            var vm = Tracks.SingleOrDefault(e => e == obj);
            if(vm != null)
            {
                vm.IsEnabled = false;
            }

            this.DownloadStatus = $"{obj.Track.AlbumTitle}/{obj.Track.Index}-{obj.Track.Title} 下载完毕!";
        }
    }
}
