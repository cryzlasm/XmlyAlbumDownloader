using System;
using System.Collections.Generic;
using System.Text;

namespace XmlyAlbumDownloader.Models
{
    public class Album
    {
        public Album()
        {
            this.Data = new AlbumData();
            this.Data.Tracks = new List<Track>();
            this.Data.Audios = new List<Audio>();
        }
        public AlbumData Data;
    }

    //  "currentUid": 83328107,
    //  "albumId": 20394123,
    //  "trackTotalCount": 180,
    //  "sort": 0,  
    //  "pageNum": 1,
    //  "pageSize": 30,
    //  "superior": [],
    //  "lastPlayTrackId": 0
    public class AlbumData
    {
        public int AlbumId { get; set; }

        public int TrackTotalCount { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public List<Track> Tracks { get; set; }

        public List<Audio> Audios { get; set; }

    }
}
