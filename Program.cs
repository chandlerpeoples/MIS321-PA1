using System;
using System.IO;
using System.Collections.Generic;
using System.Linq; // for delete method (Single method)

namespace MIS321_PA1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Song> songList = new List<Song>();
            GetAllSongs(songList);
            Welcome();
            DisplayMenu();
            int userChoice = GetUserChoice();
            MenuRoute(userChoice, songList);
        }
        static void Welcome() //displays welcome message
        {
            Console.Write("Welcome to Big Al's Gameday Playlist! Here you can view all of Big Al's songs he likes to listen to on gameday, and even add your own!\n");
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }
        static void DisplayMenu() //menu
        {
            Console.WriteLine("\nEnter the number for your corresponding choice.\n1. View All Songs\n2. Add a Song\n3. Delete a Song\n4. Exit Application");
        }
        static int GetUserChoice() // returns user choice for menu
        {
            return int.Parse(Console.ReadLine());
        }
        static void MenuRoute(int userChoice, List<Song> songList) //menu route
        {
            if(userChoice == 4)
            {
                Exit();
            }

            while(userChoice != 4)
            {
                switch (userChoice)
                {
                    case 1:
                        ViewAllSongs(songList);
                        break;
                    case 2:
                        AddSong(songList);
                        break;
                    case 3:
                        DeleteSong(songList);
                        break;
                    default:
                    BadUserChoice(userChoice);
                    break;            
                }
                DisplayMenu(); //update read
                userChoice = GetUserChoice();
                if(userChoice == 4)
                {
                    Exit();
                }
            }
        }
        static void Exit() //closes program
        {
            Environment.Exit(0);
        }
        static void BadUserChoice(int userChoice) //error message
        {
            Console.WriteLine($"\n{userChoice} is not a valid choice.");
        }
        static void ViewAllSongs(List<Song> songList) //method that displays all songs
        {
            StreamReader inFile = null;

            try
            {
                inFile = new StreamReader("songs.txt");
                string line = inFile.ReadLine();

                if(line == null)
                {
                    inFile.Close();     //this try-catch just displays a message if the song file is empty
                    int zero = 0;       //im sure there is an easier way to do this but this is how i did it
                    int temp = 100 / zero;     // in this line im just forcing an error message to catch
                } 
                else
                {
                    inFile.Close();
                }
            }
            catch(DivideByZeroException) 
            {
                Console.WriteLine("\nThere are currently no songs on the playlist. Add one!");
                return;
            }

            Console.WriteLine("");
            songList.Sort(delegate (Song x, Song y) //sorting by most recently added
            {
                return y.DateAdded.CompareTo(x.DateAdded); 
            });

            foreach (Song song in songList)
            {
                Console.WriteLine($"SONG ID: {song.SongID}\tTITLE: {song.SongTitle}\tADDED ON: {song.DateAdded}");   
            }
        }
        static void AddSong(List<Song> songList) //method to add a song to list
        {
            Console.WriteLine("\nAre you sure you want to add a song? Enter yes or no.");
            string response = Console.ReadLine().ToUpper();
            while(response != "YES") //error handling
            {
                if(response == "NO")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid response. Please enter yes or no.");
                    response = Console.ReadLine().ToUpper();
                }
            }

            Console.WriteLine("\nWhat is the title of the song you wish to add?");
            string temp = GenerateRandomID();
            string temp2 = Console.ReadLine();  //object properties
            string temp3 = DateTime.Now.ToString();

            songList.Add(new Song() {SongID = temp, SongTitle = temp2, DateAdded = temp3});
            
            // songList.Sort(delegate (Song x, Song y)
            // {
            //     return y.DateAdded.CompareTo(x.DateAdded);
            // });
            SaveAllSongs(songList);
        }
        static void DeleteSong(List<Song> songList) //method to delete a song from list
        {
            Console.WriteLine("\nAre you sure you want to delete a song? Enter yes or no.");
            string response = Console.ReadLine().ToUpper();
            while(response != "YES") //errror handling
            {
                if(response == "NO")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid response. Please enter yes or no.");
                    response = Console.ReadLine().ToUpper();
                }
            }

            Console.WriteLine("\nWhat is the ID of the song you wish to delete?");
            string deletedID = Console.ReadLine();
            var removedItem = new Song();   //this is an index method which matches the ID entered with
            try                             //an ID connected to an object in the list and deletes the object
            {
                removedItem = songList.Single(temp => temp.SongID == deletedID);
                songList.Remove(removedItem);
            }
            catch(InvalidOperationException) //catches if the ID entered does not exist as a property in the list
            {
                Console.WriteLine($"\nDelete failed. {deletedID} is not an existing song ID.");
                return;
            }
            
            Console.WriteLine("\nDelete successful.");
            SaveAllSongs(songList);
        }
        static void GetAllSongs(List<Song> songList) //gathers data from text file and populates list
        {
            StreamReader inFile = null;
            try                                         //try catch to make sure "songs.txt" exists
            {
                inFile = new StreamReader("songs.txt");
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Something went wrong with your file. Please try again. \n" + e);
            }
            
            string line = inFile.ReadLine();

            while(line != null)
            {
                string[] tempArray = line.Split('#'); 
                Song song = new Song(){SongID = tempArray[0], SongTitle = tempArray[1], DateAdded = tempArray[2]};
                songList.Add(song);
                line = inFile.ReadLine();
            }

            inFile.Close();
        }
        static string GenerateRandomID()    //method to generate a "random unique" ID
        {                                   
            Random temp = new Random();
            return temp.Next(0, 100000).ToString("D6");
        }

        static void SaveAllSongs(List<Song> songList) //prints all song data to txt file
        {
            StreamWriter outFile = new StreamWriter("songs.txt");

            foreach (Song song in songList)
            {
                outFile.WriteLine($"{song.SongID}#{song.SongTitle}#{song.DateAdded}");
            }

            outFile.Close();
        }
    }
}
