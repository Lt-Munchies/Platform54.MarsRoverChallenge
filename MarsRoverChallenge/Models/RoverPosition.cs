namespace Platform45.MarsRoverChallenge.Models
{
    public struct RoverPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Heading { get; set; }
        
        public RoverPosition(int x, int y, char heading)
        {
            X = x;
            Y = y;
            Heading = heading;
        }
    }
}