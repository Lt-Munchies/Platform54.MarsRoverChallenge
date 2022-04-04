#nullable enable
namespace Platform45.MarsRoverChallenge.Rover
{
    public class RoverCommunicator
    {
        #region Fields

        private readonly Plateau _plateau;
        private readonly RoverFactory _roverFactory;
        private Rover? _roverInProgress;

        #endregion

        #region Constructor

        public RoverCommunicator(Plateau plateau, RoverFactory roverFactory)
        {
            _plateau = plateau;
            _roverFactory = roverFactory;
        }

        #endregion

        public string FeedInstruction(string lineInstruction)
        {
            if (_plateau.ThresholdX == 0 || _plateau.ThresholdY == 0)
            {
                var plateauThresholds = lineInstruction.Split();

                if (plateauThresholds.Length != 2)
                    return "Setting plateau threshold requires the following format: [XCoordinate YCoorindate]";

                _plateau.SetPlateauSize(plateauThresholds[0], plateauThresholds[1]);
            }

            _roverInProgress ??= _roverFactory.DeployRover(_plateau);

            if (_roverInProgress.HasLanded is false)
            {
                return _roverInProgress.SetRoverLandingPosition(lineInstruction);
            }

            var moveInstructionsFeedback = _roverInProgress.InterpretRoverMoveInstructions(lineInstruction);

            if (string.IsNullOrWhiteSpace(moveInstructionsFeedback) is false)
                return moveInstructionsFeedback;

            _plateau.AddRoverPosition(_roverInProgress.GetCurrentPosition());
            _roverInProgress = null;
            return string.Empty;
        }
    }
}