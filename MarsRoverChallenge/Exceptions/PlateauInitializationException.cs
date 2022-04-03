using System;

namespace Platform45.MarsRoverChallenge.Exceptions
{
    public class PlateauInitializationException : Exception
    {
        public PlateauInitializationException(string coordinate) : base($"Plateau coordinate: [{coordinate}] is invalid. Please enter an integer between 1 and {int.MaxValue}")
        {
        }
    }
}