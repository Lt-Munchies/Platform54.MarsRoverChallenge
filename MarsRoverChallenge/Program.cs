using System;
using Platform45.MarsRoverChallenge.Exceptions;

namespace Platform45.MarsRoverChallenge
{
    class Program
    {
        #region Fields

        private static Plateau _plateau;

        #endregion

        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to [Rover Deployment Manager v.1.0.0-alpha01]");

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Enter plateau thresholds for X and Y respectively: ");

                        Console.WriteLine("X Threshold: ");
                        var xThreshold = Console.ReadLine();

                        Console.WriteLine("Y Threshold: ");
                        var yThreshold = Console.ReadLine();

                        _plateau = new Plateau();
                        _plateau.SetPlateauSize(xThreshold, yThreshold);
                    }
                    catch (PlateauInitializationException e)
                    {
                        Console.Clear();
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"Plateau X and Y thresholds set to ({_plateau.ThresholdX},{_plateau.ThresholdY})");
                    break;
                }

                while (true)
                {
                    var rover = new Rover.Rover(_plateau);

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter line of rover landing instructions");
                        var landingInstruction = Console.ReadLine();

                        var landingInstructionFeedback = rover.SetRoverLandingPosition(landingInstruction);

                        if (string.IsNullOrWhiteSpace(landingInstructionFeedback) is false)
                        {
                            Console.WriteLine(landingInstructionFeedback);
                            Console.WriteLine("Press 'Enter' to try again");
                            Console.ReadLine();
                            continue;
                        }
                        
                        break;
                    }
                    
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Landing instructions received. Rover has landed");
                        Console.WriteLine("Please enter line of rover moving instructions");
                        var movingInstruction = Console.ReadLine();

                        var moveInstructionsFeedback = rover.InterpretRoverMoveInstructions(movingInstruction);

                        if (string.IsNullOrWhiteSpace(moveInstructionsFeedback) is false)
                        {
                            Console.WriteLine(moveInstructionsFeedback);
                            Console.WriteLine("Press 'Enter' to try again");
                            Console.ReadLine();
                            continue;
                        }
                        
                        break;
                    }
                    
                    _plateau.AddRoverPosition(rover.GetCurrentPosition());

                    var hasMoreInstructions = false;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Moving instructions received. Rover has moved");
                        Console.WriteLine("Enter 'YES' if you would like to deploy another rover. Enter 'NO' if you do not");
                        var hasMoreInstructionsRaw = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(hasMoreInstructionsRaw))
                            continue;

                        if (hasMoreInstructionsRaw.ToUpper() == "YES")
                        {
                            hasMoreInstructions = true;
                            break;
                        }

                        if (hasMoreInstructionsRaw.ToUpper() == "NO")
                        {
                            break;
                        }
                    }

                    if (hasMoreInstructions is false)
                        break;
                }

                Console.Clear();
                Console.WriteLine("All final positions of all Rovers:");
                foreach (var stationaryRoverPosition in _plateau.GetStationaryRoverPositions())
                {
                    Console.WriteLine($"{stationaryRoverPosition.X} {stationaryRoverPosition.Y} {stationaryRoverPosition.Heading}");
                }

                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();

                var playAgain = false;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("All rovers deployed and moved. Thank you for using our program");
                    Console.WriteLine("Enter 'YES' to play again. Enter 'NO' to exit");
                    var playAgainInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(playAgainInput))
                        continue;

                    if (playAgainInput.ToUpper() == "YES")
                    {
                        playAgain = true;
                        break;
                    }

                    if (playAgainInput.ToUpper() == "NO")
                    {
                        break;
                    }

                    Console.Clear();
                }

                if (playAgain is false)
                    break;
            }

            Console.Clear();
            Console.WriteLine("GoodBye");
        }
    }
}