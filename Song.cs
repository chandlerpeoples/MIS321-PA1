namespace MIS321_PA1
{
    public class Song
    {
        public string SongID {get; set;}
        public string SongTitle {get; set;}
        public string DateAdded {get; set;}

        public int CompareTo(Song compareSong) //compareto method for sorting by DateAdded
        {
            return this.DateAdded.CompareTo(compareSong.DateAdded);
        }
    }
}