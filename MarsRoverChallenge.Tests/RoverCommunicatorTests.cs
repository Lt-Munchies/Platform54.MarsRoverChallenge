using System.Linq;
using Platform45.MarsRoverChallenge;
using Platform45.MarsRoverChallenge.Exceptions;
using Platform45.MarsRoverChallenge.Rover;
using Xunit;

namespace MarsRoverChallenge.Tests
{
    public class RoverCommunicatorTests
    {
        #region Fields

        private readonly RoverCommunicator _roverCommunicator;
        private readonly Plateau _plateau;

        #endregion

        #region Constructor

        public RoverCommunicatorTests()
        {
            var roverFactory = new RoverFactory();
            _plateau = new Plateau();
            _roverCommunicator = new RoverCommunicator(_plateau, roverFactory);
        }

        #endregion

        [Fact]
        public void When_FeedInstruction_With_SetPlateauThresholds_And_InvalidPlateauCoordinates_Expect_PlateauInitializationException()
        {
            //Set Plateau thresholds
            var exception = Assert.Throws<PlateauInitializationException>(() => _roverCommunicator.FeedInstruction("Invalid Coordinate"));
            Assert.Equal($"Plateau coordinate: [Invalid] is invalid. Please enter an integer between 1 and {int.MaxValue}", exception.Message);
        }

        [Fact]
        public void When_FeedInstruction_With_SetPlateauThresholds_And_InvalidPlateauThresolds_Expect_ErrorMessage()
        {
            //Set Plateau thresholds
            var feedback = _roverCommunicator.FeedInstruction("Invalid Format instruction");
            Assert.Equal("Setting plateau threshold requires the following format: [XCoordinate YCoorindate]", feedback);
        }

        [Fact]
        public void When_FeedInstruction_With_LandingInstructions_And_IncorrectHeading_Expect_ErrorMessage()
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            var feedback = _roverCommunicator.FeedInstruction("5 5 F L");
            Assert.Equal("LandingInput must have the following format: [X Y H] (XCoordinate, YCoordinate, Heading respectively)", feedback);
        }

        [Fact]
        public void When_FeedInstruction_With_LandingInstructions_And_IncorrectFormat_Expect_RoverHeadingInvalidException()
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            var exception = Assert.Throws<RoverHeadingInvalidException>(() => _roverCommunicator.FeedInstruction("5 5 F"));
            Assert.Equal("Heading: [F] is invalid. Heading needs be one of the following: [N, E, S, W]", exception.Message);
        }

        [Theory]
        [InlineData("F", "5")]
        [InlineData("5", "F")]
        public void When_FeedInstruction_With_LandingInstructions_And_InvalidCoordinates_Expect_ErrorMessage(string x, string y)
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            var feedback = _roverCommunicator.FeedInstruction($"{x} {y} F");
            Assert.Equal("Value:[F] is not valid please enter a valid coordinate", feedback);
        }

        [Theory]
        [InlineData("5", "6", 'Y')]
        [InlineData("6", "5", 'X')]
        public void When_FeedInstruction_With_LandingInstructions_And_CoordinatesAreMoreThanTheMaxThreshold_Expect_ErrorMessage(string x, string y, char axis)
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            var feedback = _roverCommunicator.FeedInstruction($"{x} {y} F");
            Assert.Equal($"Coordinate for Axis:[{axis}] may not be more than [{5}]", feedback);
        }

        [Fact]
        public void When_FeedInstruction_With_LandingInstructions_And_AStationaryRoverIsInTheWay_Expect_ErrorMessage()
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            _roverCommunicator.FeedInstruction("5 5 E");

            //Feed moving instruction
            _roverCommunicator.FeedInstruction("R");

            //Feed landing instruction
            var feedback = _roverCommunicator.FeedInstruction("5 5 S");
            Assert.Equal("Cannot land rover here. Position is occupied by a different rover", feedback);
        }

        [Theory]
        [InlineData("X")]
        [InlineData("gfeshtrsh")]
        [InlineData("X 54")]
        [InlineData("LMN")]
        [InlineData("LRMX")]
        public void When_FeedInstruction_With_MovingInstructions_And_InvalidFormat_Expect_ErrorMessage(string movingInstructions)
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            _roverCommunicator.FeedInstruction("5 5 E");

            //Feed moving instruction
            var feedback = _roverCommunicator.FeedInstruction(movingInstructions);
            Assert.Equal("Invalid instructions. Moving instructions may only contain the following letters and format: [L, R, M]", feedback);
        }

        [Theory]
        [InlineData("5 5", "5 5 E", "MM")]
        [InlineData("5 5", "5 5 N", "MM")]
        public void When_FeedInstruction_With_InputToMoveAgainstAThreshold_Expect_RoversPositionDoesntChange(string threshold, string startingPosition, string moveInstructions)
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction(threshold);

            //Feed landing instruction
            _roverCommunicator.FeedInstruction(startingPosition);

            //Feed moving instruction
            _roverCommunicator.FeedInstruction(moveInstructions);

            var endPosition = _plateau.GetStationaryRoverPositions().First();

            var endPositionString = $"{endPosition.X} {endPosition.Y} {endPosition.Heading}";
            Assert.Equal(startingPosition, endPositionString);
        }
        
        [Fact]
        public void When_FeedInstruction_With_InputToMoveAgainstAStationaryRover_Expect_RoversPositionDoesntChange()
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("5 5");

            //Feed landing instruction
            _roverCommunicator.FeedInstruction("1 0 N");

            //Feed moving instruction
            _roverCommunicator.FeedInstruction("MRMRMRMR");
            
            //Feed landing instruction
            _roverCommunicator.FeedInstruction("0 0 E");

            //Feed moving instruction
            _roverCommunicator.FeedInstruction("MMM");

            var firstRoverEndPosition = _plateau.GetStationaryRoverPositions().First();
            var firstRoverEndPositionString = $"{firstRoverEndPosition.X} {firstRoverEndPosition.Y} {firstRoverEndPosition.Heading}";
            Assert.Equal("1 0 N", firstRoverEndPositionString);
            
            var secondRoverEndPosition = _plateau.GetStationaryRoverPositions().ElementAt(1);
            var secondRoverEndPositionString = $"{secondRoverEndPosition.X} {secondRoverEndPosition.Y} {secondRoverEndPosition.Heading}";
            Assert.Equal("0 0 E", secondRoverEndPositionString);
        }

        [Fact]
        public void When_FeedInstructions_Expect_AllRoversToBeAtTheExpectedEndPosition()
        {
            //Set Plateau thresholds
            _roverCommunicator.FeedInstruction("2 2");

            //First Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("MMRMM");
            
            //Second Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("MMRM");
            
            //Third Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("MM");
            
            //Fourth Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("MRMM");
            
            //Fifth Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("MRM");
            
            //Sixth Rover
            _roverCommunicator.FeedInstruction("0 0 N");
            _roverCommunicator.FeedInstruction("M");

            var allRoverEndPositions = _plateau.GetStationaryRoverPositions().ToList();

            var firstRoverEndPosition = allRoverEndPositions.First();
            var firstRoverEndPositionString = $"{firstRoverEndPosition.X} {firstRoverEndPosition.Y} {firstRoverEndPosition.Heading}";
            Assert.Equal("2 2 E", firstRoverEndPositionString);
            
            var secondRoverEndPosition = allRoverEndPositions.ElementAt(1);
            var secondRoverEndPositionString = $"{secondRoverEndPosition.X} {secondRoverEndPosition.Y} {secondRoverEndPosition.Heading}";
            Assert.Equal("1 2 E", secondRoverEndPositionString);
            
            var thirdRoverEndPosition = allRoverEndPositions.ElementAt(2);
            var thirdRoverEndPositionString = $"{thirdRoverEndPosition.X} {thirdRoverEndPosition.Y} {thirdRoverEndPosition.Heading}";
            Assert.Equal("0 2 N", thirdRoverEndPositionString);
            
            var fourthRoverEndPosition = allRoverEndPositions.ElementAt(3);
            var fourthRoverEndPositionString = $"{fourthRoverEndPosition.X} {fourthRoverEndPosition.Y} {fourthRoverEndPosition.Heading}";
            Assert.Equal("2 1 E", fourthRoverEndPositionString);
            
            var fifthRoverEndPosition = allRoverEndPositions.ElementAt(4);
            var fifthRoverEndPositionString = $"{fifthRoverEndPosition.X} {fifthRoverEndPosition.Y} {fifthRoverEndPosition.Heading}";
            Assert.Equal("1 1 E", fifthRoverEndPositionString);
            
            var sixthRoverEndPosition = allRoverEndPositions.ElementAt(5);
            var sixthRoverEndPositionString = $"{sixthRoverEndPosition.X} {sixthRoverEndPosition.Y} {sixthRoverEndPosition.Heading}";
            Assert.Equal("0 1 N", sixthRoverEndPositionString);
        }
    }
}