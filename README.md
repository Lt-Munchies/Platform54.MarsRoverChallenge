# Platform54.MarsRoverChallenge

## Mars Rover technical Challenge

- The problem below requires some kind of input. You are free to implement any mechanism for feeding input into your solution (for example, using hard coded data within a unit test). You should provide sufficient evidence that your solution is complete by, as a minimum, indicating that it works correctly against the supplied test data.

- We highly recommend using a unit testing framework such as JUnit or NUnit. Even if you have not used it before, it is simple to learn and incredibly useful.

- The code you write should be of production quality, and most importantly, it should be code you are proud of.

### MARS ROVERS

A squad of robotic rovers are to be landed by NASA on a plateau on Mars.

This plateau, which is curiously rectangular, must be navigated by the rovers so that their on board cameras can get a complete view of the surrounding terrain to send back to Earth.
A rover's position is represented by a combination of an x and y co-ordinates and a letter representing one of the four cardinal compass points. The plateau is divided up into a grid to simplify navigation. An example position might be 0, 0, N, which means the rover is in the bottom left corner and facing North.
In order to control a rover, NASA sends a simple string of letters. The possible letters are 'L', 'R' and 'M'. 'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its current spot.
'M' means move forward one grid point, and maintain the same heading.
Assume that the square directly North from (x, y) is (x, y+1).

```
Input:

The first line of input is the upper-right coordinates of the plateau, the lower-left coordinates are assumed to be 0,0.
The rest of the input is information pertaining to the rovers that have been deployed. Each rover has two lines of input. The first line gives the rover's position, and the second line is a series of instructions telling the rover how to explore the plateau.
The position is made up of two integers and a letter separated by spaces, corresponding to the x and y co-ordinates and the rover's orientation.
Each rover will be finished sequentially, which means that the second rover won't start to move until the first one has finished moving.

Output:

The output for each rover should be its final co-ordinates and heading.

```
```
Test Input:
5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM

Expected Output:
1 3 N
5 1 E
```

### Assumptions:
1. Reaching the edge of the plateau only prohibits movement of the rover and does not cause a rover to fall off the plateau
2. Once a rover has reached its final destination it stays there for the duration of the mission until restart
3. When a rover is directed into the path of a stationary rover it simply ignores the instruction and attempts to continue with the rest of the instructions
4. When a rover attempts to land on an occupied space on the plateau (because of a stationary rover) it will fail to land gracefully and crash
5. Instructions will be sent in 1 line at a time
6. 1 line will contain the full set of instructions meant for that line and not partial instructions
7. Lines of instructions will always be ordered and come in the correct order namely 1. Rover Position 2. Instructions to move respectively
8. Nasa can send communication to the rover after it has landed and has not pre-programmed the rover with the set of instructions

### Installation:
App is a console application

The console application does not take in exact structure of the test input

Simply publish solution to the desired location and run the exe file

### Unit tests

Unit tests are written to take in the exact expected input structure

Refer to unit tests for a more exact examination of expected functionality
