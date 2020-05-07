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
            return JsonSerializer.Deserialize<List<Song>>(File.ReadAllText(_songRepositoryPath));   //this throws an exception once the json file hits 1KB and i've spent hours researching it and can't figure out why
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
                        SearchList();
                        break;
                    case 4:
                        SortList();
                        break;
                    case 5:
                        Shuffle(SongsList);
                        break;
                    case 6:
                        RemoveSongs();
                        break;
                    case 7:
                        inputtingSong = false;
                        break;
                }
            }
        }

        private static void DisplaySongs(IEnumerable<Song> Songs)
        {
            if (Songs.Any())
            {
                Console.WriteLine($"\n\"Title\"  -  Artist   (Album) \n");              //displays the full library of songs stored in the json file
                SongsList.ForEach(Song =>
                {
                    Console.WriteLine(Song.SongDisplay + "\n");
                });
            }
            else
            {
                Console.WriteLine("\nNo songs found.");
            }
            Console.WriteLine(" ");
        }

        private static void DisplaySongs() => DisplaySongs(SongsList);

        private static void SearchList()
        {
            SearchMenu();
            var option = Console.ReadLine();

                switch (int.Parse(option))
                {
                    case 1:
                        SearchSongs();
                        break;
                    case 2:
                        SearchArtists();
                        break;
                    case 3:
                        SearchAlbums();
                        break;
                }
        }

        private static void SearchSongs()
        {
            Console.WriteLine("\nSearch for a song: \n");
            var searchString = Console.ReadLine();
            var searchResults = SongsList.Where(Song => Song.SongTitle.ToLower().Contains(searchString.ToLower())).ToList();    //this filters the list correctly but...
            DisplaySongs(searchResults);                                                                                        //...this doesn't display the filtered list for some reason (also spent hours researching this, tried about 15 different ways around it to no avail)
        }

        private static void SearchArtists()
        {
            Console.WriteLine("\nSearch for an artist: \n");
            var searchString = Console.ReadLine();
            var searchResults = SongsList.Where(Song => Song.Artist.ToLower().Contains(searchString.ToLower())).ToList();       //as already mentioned, this filters the list correctly but...
            DisplaySongs(searchResults);                                                                                        //...this STILL doesn't display the filtered list for some reason
        }

        private static void SearchAlbums()
        {
            Console.WriteLine("\nSearch for an album: \n");
            var searchString = Console.ReadLine();
            var searchResults = SongsList.Where(Song => Song.Album.ToLower().Contains(searchString.ToLower())).ToList();        //once again, this filters the list correctly but...
            DisplaySongs(searchResults);                                                                                        //...this STILL doesn't display the filtered list for some reason
        }

        private static void SearchMenu()
        {
            Console.WriteLine("\nHow would you like to search the list?");
            Console.WriteLine("1: Search by song title");
            Console.WriteLine("2: Search by artist");
            Console.WriteLine("3: Search by album");
            Console.WriteLine(" ");
        }

        private static Random random = new Random();
        
        private static void Shuffle<Song>(IList<Song> SongsList)
        {
            Console.WriteLine("\nHere's your randomized songlist! \n");             //shuffles the full library of songs stored in the json file
            int n = SongsList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Song value = SongsList[k];
                SongsList[k] = SongsList[n];
                SongsList[n] = value;
            }
            DisplaySongs();
        }

        private static void SortList()
        {
            SortMenu();
            var option = Console.ReadLine();

                switch (int.Parse(option))
                {
                    case 1:
                        SortByTitle();
                        break;
                    case 2:
                        SortByArtist();
                        break;
                    case 3:
                        SortByAlbum();
                        break;
                }
        }

        private static void SortByTitle()
        {
            SongsList.Sort((x, y) => x.SongTitle.CompareTo(y.SongTitle));           //sorts the full library of songs by song title in alphabetical order
            DisplaySongs();
        }

        private static void SortByArtist()
        {
            SongsList.Sort((x, y) => x.Artist.CompareTo(y.Artist));                 //sorts the full library of songs by artist in alphabetical order
            DisplaySongs();
        }

        private static void SortByAlbum()
        {
            SongsList.Sort((x, y) => x.Album.CompareTo(y.Album));                   //sorts the full library of songs by album in alphabetical order
            DisplaySongs();
        }

        private static void SortMenu()
        {
            Console.WriteLine("\nHow would you like to sort the list?");
            Console.WriteLine("1: Sort by song title");
            Console.WriteLine("2: Sort by artist");
            Console.WriteLine("3: Sort by album");
            Console.WriteLine(" ");
        }

        private static void RemoveSongs()
        {
            bool confirmed = false;
            string Key;
            do
            {
                Console.WriteLine("\nEnter the title of the song you'd like to remove \n");
                Key = Console.ReadLine();
                var Songs = SongsList.Where(x => x.SongTitle.ToLower().Contains(Key.ToLower()));        //once again, this filters the list correctly but...
                DisplaySongs(Songs);                                                                    //...this STILL doesn't display the filtered list for some reason
                
                ConsoleKey response;
                do
                {
                    Console.WriteLine("\nIs this the song you'd like to remove? [y/n] \n");         //this WHOLE method removes a song from the library and saves the file, while also making sure to ask a confirmation y/n question before removing the song
                    response = Console.ReadKey(false).Key;                                          //surprisingly, even though the DisplaySongs(Songs) method doesn't correctly display the filtered list, it WILL delete the correct song from the library
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
            Console.WriteLine("\nEnter song artist \n");                        //adds a song to the library and then saves the file
            Song.Artist = Console.ReadLine();
            Console.WriteLine("\nEnter album name \n");
            Song.Album = Console.ReadLine();
            Console.WriteLine(" ");

            SongsList.Add(Song);
            Save();
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("Select from the following operations:");         //main menu for the console app
            Console.WriteLine("1: Enter new song");
            Console.WriteLine("2: List all songs");
            Console.WriteLine("3: Search for a song");
            Console.WriteLine("4: Sort the song library");
            Console.WriteLine("5: Shuffle the song library");
            Console.WriteLine("6: Remove a song by title");
            Console.WriteLine("7: Exit \n");
        }

    }
}