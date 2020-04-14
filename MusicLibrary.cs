using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using src;

namespace plmeredith13.CodeLou.FinalProject
{
    class Library
    {
        static string _songRepositoryPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\songs.json";
        static List<Song> SongsList = File.Exists(_songRepositoryPath) ? Read() : new List<Song>();
        private static void Save()
        {
            using (var file = File.CreateText(_songRepositoryPath))
            {
                file.WriteAsync(JsonSerializer.Serialize(SongsList));
            }
        }
        static List<Song> Read()
        {
            return JsonSerializer.Deserialize<List<Song>>(File.ReadAllText(_songRepositoryPath));
        }
        //static List<Song> SongsList = new List<Song>();

        static void Main(string[] args)
        {
            var inputtingSong = true;

            while (inputtingSong)
            {
                DisplayMenu();
                var option = Console.ReadLine();

                switch (int.Parse(option))
                {
                    case 1:
                        InputSong();
                        break;
                    case 2:
                        DisplaySongs();

                        break;
                    case 3:
                        SearchSongs();
                        break;
                    case 4:
                        inputtingSong = false;
                        break;
                }
            }
        }

        private static void DisplaySongs(IEnumerable<Song> Songs)
        {
            if (Songs.Any())
            {
                Console.WriteLine($"Title | Artist |  Album");
                SongsList.ForEach(x =>
                {
                    Console.WriteLine(x.SongDisplay);
                });
            }
            else
            {
                System.Console.WriteLine("No songs found.");
            }
        }

        private static void DisplaySongs() => DisplaySongs(SongsList);

        private static void SearchSongs()
        {
            Console.WriteLine("Search string:");
            var searchString = Console.ReadLine();
            var Songs = SongsList.Where(x => x.SongTitle.ToLower().Contains(searchString.ToLower()) || x.Album.ToLower().Contains(searchString.ToLower()));
            DisplaySongs(Songs);
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Select from the following operations:");
            Console.WriteLine("1: Enter new song");
            Console.WriteLine("2: List all songs");
            Console.WriteLine("3: Search for song by name");
            Console.WriteLine("4: Exit");
        }

        static void InputSong()
        {
            var Song = new Song();
            while (true)
            {
                Console.WriteLine("Enter song title:");
                var SongTitleSuccessful = true;
                var SongTitle = Console.ReadLine();         //var SongTitleSuccessful = int.TryParse(Console.ReadLine(), out var SongTitle);
                if (SongTitleSuccessful)
                {
                    Song.SongTitle = SongTitle;
                    break;
                }
            }
            Console.WriteLine("Enter song artist");
            Song.Artist = Console.ReadLine();
            Console.WriteLine("Enter album name");
            Song.Album = Console.ReadLine();

            SongsList.Add(Song);
            Save();
        }

    }
}