using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XmlyAlbumDownloader.Models;
using XmlyAlbumDownloader.ViewModels;

namespace XmlyAlbumDownloader.Services
{
    public interface IAlbum
    {
        Task<Album> GetTracksListAsync(int albumId, int pageNum);

        event Action<TrackViewModel> OnDownloaded;

        void DownloadTracks(List<TrackViewModel> tracks);

        void StopDownload();
    }


}
