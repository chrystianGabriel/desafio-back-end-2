using DesafioConexaLabs.Entities.Enum;
using System.Collections.Generic;

namespace DesafioConexaLabs.Entities
{
    public class Playlist
    {
        public List<Track> SongsList { get; private set; }
        public EPlaylistCategory Category { get; private set; }

        public Playlist(EPlaylistCategory category, List<Track> songsList = null)
        {
            this.SongsList = songsList ?? new List<Track>();
            this.Category = category;
        }
    }
}
