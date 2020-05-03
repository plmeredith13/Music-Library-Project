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

        static void Main(string[] args)
        {
            Console.Clear();
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
                        RemoveSongs();
                        break;
                    case 5:
                        inputtingSong = false;
                        break;
                }
            }
        }

        private static void DisplaySongs(IEnumerable<Song> Songs)
        {
            if (Songs.Any())
            {
                Console.WriteLine($"\n\"Title\"  -  Artist   (Album) \n");
                SongsList.ForEach(x =>
                {
                    Console.WriteLine(x.SongDisplay + "\n");
                });
            }
            else
            {
                Console.WriteLine("\nNo songs found.");
            }
            Console.WriteLine(" ");
        }

        private static void DisplaySongs() => DisplaySongs(SongsList);

        private static void SearchSongs()
        {
            Console.WriteLine("\nSearch for a song: \n");
            var searchString = Console.ReadLine();
            var searchResults = SongsList.Where(x => x.SongTitle.ToLower().Contains(searchString.ToLower()));
            DisplaySongs(searchResults);
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("Select from the following operations:");
            Console.WriteLine("1: Enter new song");
            Console.WriteLine("2: List all songs");
            Console.WriteLine("3: Search for song by title");
            Console.WriteLine("4: Remove a song by title");
            Console.WriteLine("5: Exit \n");
        }

        private static void RemoveSongs()
        {
            bool confirmed = false;
            string Key;
            do
            {
                Console.WriteLine("\nEnter the title of the song you'd like to remove \n");
                Key = Console.ReadLine();
                var Songs = SongsList.Where(x => x.SongTitle.ToLower().Contains(Key.ToLower()));
                DisplaySongs(Songs);
                
                ConsoleKey response;
                do
                {
                    Console.WriteLine("\nIs this the song you'd like to remove? [y/n] \n");
                    response = Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);
                confirmed = response == ConsoleKey.Y;
            } while (!confirmed);
            
            if (confirmed)
                {
                    SongsList.RemoveAll(x => x.SongTitle.ToLower().Contains(Key.ToLower()));
                    Console.WriteLine("\nYou have successfully deleted the song from the music library! \n");

                    Save();
                }
        }

        static void InputSong()
        {
            Console.WriteLine(" ");
            var Song = new Song();
            Console.WriteLine("Enter song title \n");
            Song.SongTitle = Console.ReadLine();
            Console.WriteLine("\nEnter song artist \n");
            Song.Artist = Console.ReadLine();
            Console.WriteLine("\nEnter album name \n");
            Song.Album = Console.ReadLine();
            Console.WriteLine(" ");

            SongsList.Add(Song);
            Save();
        }

    }
}