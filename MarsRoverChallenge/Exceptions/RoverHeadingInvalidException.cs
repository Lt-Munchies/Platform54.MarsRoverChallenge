using System;

namespace Platform45.MarsRoverChallenge.Exceptions
{
    public class RoverHeadingInvalidException : Exception
    {
        public RoverHeadingInvalidException(string heading) : base($"Heading: [{heading}] is invalid. Heading needs be one of the following: [N, E, S, W]")
        {
        }
    }
}