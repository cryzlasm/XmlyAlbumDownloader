using System;
using System.Collections.Generic;
using System.Text;

namespace XmlyAlbumDownloader.Models
{
    //"index": 1,
    //"trackId": 151906072,
    //"isPaid": false,
    //"tag": 0,
    //"title": "二丫声律启蒙 | 一东 1 | 云对雨 雪对风",
    //"playCount": 5431220,
    //"showLikeBtn": true,
    //"isLike": false,
    //"showShareBtn": true,
    //"showCommentBtn": true,
    //"showForwardBtn": true,
    //"createDateFormat": "2年前",
    //"url": "/sound/151906072",
    //"duration": 432,
    //"isVideo": false,
    //"isVipFirst": false,
    //"breakSecond": 0,
    //"length": 432,
    //"albumId": 20394123,
    //"albumTitle": "声律启蒙 | 笠翁对韵 | 儿童国学",
    //"albumCoverPath": "group52/M05/F8/44/wKgLe1wstNWwo6gIACT3BkDSVkg800.jpg",
    //"anchorId": 4778601,
    //"anchorName": "二丫讲故事"
    public class Track
    {
        public int Index { get; set; }

        public int TrackId { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public string AlbumTitle { get; set; }

        public string AnchorId { get; set; }

        public string AnchorName { get; set; }
    }
}
