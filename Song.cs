using System;
namespace src
{
    public class Song
    {
        public string SongTitle { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string SongDisplay => $"{SongTitle} | {Artist} | {Album} ";
    }
}