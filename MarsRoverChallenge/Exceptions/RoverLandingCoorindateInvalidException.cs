using System;

namespace Platform45.MarsRoverChallenge.Exceptions
{
    public class RoverLandingCoorindateInvalidException : Exception
    {
        public RoverLandingCoorindateInvalidException(string character) : base($"Value:[{character}] is not valid please enter a valid coordinate")
        {
        }
        
        public RoverLandingCoorindateInvalidException(char axis, int maxThreshold) : base($"Coordinate for Axis:[{axis}] may not be more than [{maxThreshold}]")
        {
        }
    }
}