using System.Diagnostics;

namespace Platform45.MarsRoverChallenge
{
    public class RoverFactory
    {
        public Rover DeployRover(Plateau plateau)
        {
            return new Rover(plateau);
        }
    }
}