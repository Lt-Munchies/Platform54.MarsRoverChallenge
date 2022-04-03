using System.Collections.Generic;
using Platform45.MarsRoverChallenge.Exceptions;

namespace Platform45.MarsRoverChallenge
{
    /// <summary>
    /// Represents a plateau used for the landing of Rovers
    /// </summary>
    public class Plateau
    {
        #region Fields

        public int ThresholdX { get; }
        public int ThresholdY { get; }

        private readonly List<RoverPosition> _stationaryRoverPositions;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a Plateau with an X and Y coordinate representing the width and length of the plateau
        /// </summary>
        /// <param name="thresholdX">Max grid width represented as x coordinate</param>
        /// <param name="thresholdY">Max grid length represented as y coordinate</param>
        public Plateau(string thresholdX, string thresholdY)
        {
            ThresholdX = ValidatePlateauCoordinate(thresholdX);
            ThresholdY = ValidatePlateauCoordinate(thresholdY);
            _stationaryRoverPositions = new List<RoverPosition>();
        }

        #endregion

        public void AddRoverPosition(RoverPosition roverPosition)
        {
            _stationaryRoverPositions.Add(roverPosition);
        }

        public IEnumerable<RoverPosition> GetStationaryRoverPositions()
        {
            return _stationaryRoverPositions;
        }

        private static int ValidatePlateauCoordinate(string coordinateString)
        {
            if (int.TryParse(coordinateString, out var coordinate) is false)
                throw new PlateauInitializationException(coordinateString);

            if (coordinate <= 0)
                throw new PlateauInitializationException(coordinateString);

            return coordinate;
        }
    }
}