namespace Platform45.MarsRoverChallenge.Rover
{
    public class RoverFactory
    {
        public Rover DeployRover(Plateau plateau)
        {
            return new Rover(plateau);
        }
    }
}