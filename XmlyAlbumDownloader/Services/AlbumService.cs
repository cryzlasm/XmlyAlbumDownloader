using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XmlyAlbumDownloader.Common;
using XmlyAlbumDownloader.Models;
using XmlyAlbumDownloader.ViewModels;
using XmlyAlbumDownloaderI.ViewModels;

namespace XmlyAlbumDownloader.Services
{
    public class AlbumService: IAlbum
    {
        private readonly string albumUri = "https://www.ximalaya.com/revision/album/v1/getTracksList?albumId={0}&pageNum={1}";
        private readonly string audioUri = "https://www.ximalaya.com/revision/play/v1/audio?id={0}&ptype=1";
        private readonly HttpRequest _httpRequest = new HttpRequest();

        private ConcurrentQueue<TrackViewModel> _downloadQueue = new ConcurrentQueue<TrackViewModel>();
        private List<Track> _searchResult = new List<Track>();

        private bool _stop = false;

        public event Action<TrackViewModel> OnDownloaded;

        public async Task<Album> GetTracksListAsync(int albumId, int pageNum)
        {
            var album = new Album();

            var result = await _httpRequest.GetObjectAsync<Album>(string.Format(albumUri, albumId,pageNum));

            if (result != null)
            {
                album.Data.AlbumId = result.Data.AlbumId;
                album.Data.TrackTotalCount = result.Data.TrackTotalCount;
                album.Data.PageNum = result.Data.PageNum;
                album.Data.PageSize = result.Data.PageSize;
                album.Data.Tracks.AddRange(result.Data.Tracks);
            }                
             
            return album;
        }


        public void DownloadTracks(List<TrackViewModel> tracks)
        {
            Parallel.ForEach<TrackViewModel>(tracks, async item =>
            {
                DownloadTrack(item);

                int sleepTime = 200;
                await Task.Delay(sleepTime);
            }
            );
        }

        private async void DownloadTrack(TrackViewModel track)
        {
            if (_stop) return;

            var file = Path.Combine(AppConfig.DownloadFolder, Helper.RemoveInvalidFileNameChars(track.Track.AlbumTitle),
                                track.Track.Index + "-" + Helper.RemoveInvalidFileNameChars(track.Track.Title) + ".m4a");

            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else if (File.Exists(file))
            {
                track.DownloadStatus = 1;
                track.DownloadStatusText = "下载完毕";
                OnDownloaded?.Invoke(track);
                return;
            }
            track.DownloadStatus = 2;
            track.DownloadStatusText = "下载中";

            var audio = await this.GetAudioAsync(track.Track.TrackId);

            var bytes = await  _httpRequest.GetByteArrayAsync(audio.Src);
            if (bytes == null)
            {
                _downloadQueue.Enqueue(track);
                track.DownloadStatus = -1;
                track.DownloadStatusText = "下载失败";
                Console.WriteLine($"{track.Track.Title} 下载失败");
                return;
            }
            await using var fs = new System.IO.FileStream(file, System.IO.FileMode.CreateNew);
            fs.Write(bytes, 0, bytes.Length);

            track.DownloadStatus = 1;
            track.DownloadStatusText = "下载完毕";
            track.RaisePropertyChanged();

            OnDownloaded?.Invoke(track);
        }
        private async Task<Audio> GetAudioAsync(int trackId)
        {
            var result = await  _httpRequest.GetObjectAsync<AudioData>(string.Format(audioUri, trackId));
            return result.Data;
        }

        public void StopDownload()
        {
            _stop = true;     
        }

        private void Add(TrackViewModel track)
        {
            this._downloadQueue.Enqueue(track);
        }

        private void AddRange(List<TrackViewModel> tracks)
        {
            tracks.ForEach(t => this.Add(t));
        }
    }
}
