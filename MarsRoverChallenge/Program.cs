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

                        _plateau = new Plateau(xThreshold, yThreshold);
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
                    var rover = new Rover(_plateau);
                    var lineNumber = 1;
                    while (lineNumber < 3)
                    {
                        Console.Clear();
                        Console.WriteLine($"Please enter line [{lineNumber}] of instructions");
                        var landingInput = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(landingInput))
                            continue;

                        var feedback = rover.FeedInstruction(landingInput);

                        if (string.IsNullOrWhiteSpace(feedback) is false)
                        {
                            Console.WriteLine(feedback);
                            Console.WriteLine("Press 'ENTER' to try again");
                            Console.ReadLine();
                            continue;
                        }
                        
                        lineNumber++;
                    }
                    
                    _plateau.AddRoverPosition(rover.GetCurrentPosition());

                    var hasMoreInstructions = false;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter 'YES' if you have more instructions. Enter 'NO' if you do not");
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
                    Console.WriteLine("All rovers deployed. Thank you for using our program");
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