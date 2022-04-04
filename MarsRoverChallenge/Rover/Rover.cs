using System;
using System.Linq;
using Platform45.MarsRoverChallenge.Exceptions;
using Platform45.MarsRoverChallenge.Models;

namespace Platform45.MarsRoverChallenge.Rover
{
    public class Rover
    {
        #region Fields

        private readonly Plateau _plateau;
        private RoverPosition _roverPosition;

        #endregion

        #region Properties

        public bool HasLanded { get; private set; }

        #endregion

        #region Constructor

        public Rover(Plateau plateau)
        {
            _plateau = plateau;
        }

        #endregion

        private static int ValidateRoverCoordinate(string stringCoordinate, int maxThreshold, char axis)
        {
            if (int.TryParse(stringCoordinate, out var coordinate) is false)
                throw new RoverLandingCoorindateInvalidException(stringCoordinate);

            if (coordinate > maxThreshold)
                throw new RoverLandingCoorindateInvalidException(axis, maxThreshold);

            return coordinate;
        }

        private static char ValidateRoverHeading(string heading)
        {
            var validCharacters = new[] {"N", "E", "S", "W"};

            if (validCharacters.Contains(heading.ToUpper()) is false)
                throw new RoverHeadingInvalidException(heading);

            return Convert.ToChar(heading.ToUpper());
        }

        public string InterpretRoverMoveInstructions(string rawInstructions)
        {
            var instructions = rawInstructions.ToUpper().ToCharArray();

            var validCharacters = new[] {'L', 'R', 'M'};

            if (instructions.Any(p => char.IsNumber(p) || validCharacters.Contains(p) is false))
                return "Invalid instructions. Moving instructions may only contain the following letters and format: [L, R, M]";

            foreach (var instruction in instructions)
            {
                if (instruction is 'L' or 'R')
                {
                    TurnRover(instruction);
                }
                else
                {
                    MoveRover();
                }
            }

            return string.Empty;
        }

        public string SetRoverLandingPosition(string instruction)
        {
            var instructions = instruction.Split(' ');
            if (instructions.Length != 3)
            {
                return "LandingInput must have the following format: [X Y H] (XCoordinate, YCoordinate, Heading respectively)";
            }

            try
            {
                var xCoordinate = ValidateRoverCoordinate(instructions[0], _plateau.ThresholdX, 'X');
                var yCoordinate = ValidateRoverCoordinate(instructions[1], _plateau.ThresholdY, 'Y');
                var heading = ValidateRoverHeading(instructions[2]);

                if (_plateau.GetStationaryRoverPositions().Any(p => p.X == xCoordinate && p.Y == yCoordinate))
                    return "Cannot land rover here. Position is occupied by a different rover";

                _roverPosition = new RoverPosition(xCoordinate, yCoordinate, heading);
            }
            catch (RoverLandingCoorindateInvalidException e)
            {
                return e.Message;
            }

            HasLanded = true;
            return string.Empty;
        }

        private void MoveRover()
        {
            var stationaryRovers = _plateau.GetStationaryRoverPositions();

            switch (_roverPosition.Heading)
            {
                case 'N':
                {
                    var potentialPositon = $"{_roverPosition.X} {_roverPosition.Y + 1}";

                    if (stationaryRovers.Select(p => $"{p.X} {p.Y}").Any(p => p == potentialPositon) is false && _roverPosition.Y + 1 <= _plateau.ThresholdY)
                        _roverPosition.Y++;
                    break;
                }
                case 'E':
                {
                    var potentialPositon = $"{_roverPosition.X + 1} {_roverPosition.Y}";

                    if (stationaryRovers.Select(p => $"{p.X} {p.Y}").Any(p => p == potentialPositon) is false && _roverPosition.X + 1 <= _plateau.ThresholdX)
                        _roverPosition.X++;
                    break;
                }
                case 'S':
                {
                    var potentialPositon = $"{_roverPosition.X} {_roverPosition.Y - 1}";

                    if (_roverPosition.Y - 1 >= 0 && stationaryRovers.Select(p => $"{p.X} {p.Y}").Any(p => p == potentialPositon) is false)
                        _roverPosition.Y--;
                    break;
                }
                case 'W':
                {
                    var potentialPositon = $"{_roverPosition.X - 1} {_roverPosition.Y}";

                    if (_roverPosition.X - 1 >= 0 && stationaryRovers.Select(p => $"{p.X} {p.Y}").Any(p => p == potentialPositon) is false)
                        _roverPosition.X--;
                    break;
                }
            }
        }

        private void TurnRover(char turnInstruction)
        {
            _roverPosition.Heading = turnInstruction switch
            {
                'L' => _roverPosition.Heading switch
                {
                    'N' => 'W',
                    'W' => 'S',
                    'S' => 'E',
                    'E' => 'N',
                    _ => _roverPosition.Heading
                },
                'R' => _roverPosition.Heading switch
                {
                    'N' => 'E',
                    'E' => 'S',
                    'S' => 'W',
                    'W' => 'N',
                    _ => _roverPosition.Heading
                },
                _ => _roverPosition.Heading
            };
        }

        public RoverPosition GetCurrentPosition()
        {
            return _roverPosition;
        }
    }
}