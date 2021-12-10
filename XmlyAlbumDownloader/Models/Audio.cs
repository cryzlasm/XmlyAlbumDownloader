using System;
using System.Collections.Generic;
using System.Text;

namespace XmlyAlbumDownloader.Models
{
    //"trackId": 191719702,
    //"canPlay": true,
    //"isPaid": false,
    //"hasBuy": true,
    //"src": "https://aod.cos.tx.xmcdn.com/group61/M05/35/D2/wKgMZl0Q3CKBv2AOABtZvJegPQg552.m4a",
    //"albumIsSample": false,
    //"sampleDuration": 0,
    //"isBaiduMusic": false,
    //"firstPlayStatus": true,
    //"isVipFree": false,
    //"isXimiAhead": false,
    //"isAlbumTimeLimited": false,
    //"ximiVipFreeType": 0
    public class Audio
    {
        public int TrackId { get; set; }

        public string Src { get; set; }
    }

    public class AudioData
    {
        public Audio Data { get; set; }
    }
}
