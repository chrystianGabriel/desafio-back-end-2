using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.Entities
{
    public class Playlist : Entity
    {
        private readonly IList<Track> _tracks;
        public IReadOnlyCollection<Track> Tracks => _tracks.ToArray();
        
        [JsonIgnore]
        public string Genre { get; private set;}
        public Playlist(string genre) 
        {
            Genre = genre;
            _tracks = new List<Track>();
        }
        public void AddTrack(Track track)
        {   
            this._tracks.Add(track);
        }
    }
}