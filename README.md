# DealerOnAssignment

This repository contains Cormac D.C.'s solution to the DealerOn interview assignment

## Version Info and Details

This program is a .NET console application that was built with .NET SDK version 6.0.101 on a Windows 10 Operating System

## Assumptions

1. Input & output may be in text files
2. Input is correctly formatted & valid as detailed in the "Example Input" section of the assignment, with no spaces at the end of each line and no empty lines
3. The rover will ignore any directions that cause it to move off of the plateau grid

## Running

1. Clone this repo to your desired directory
2. In a console or terminal, navigate to the directory where this repo is located on your machine
3. A sample input text file has been provided in the /DealerOnAssignment/Inputs/ directory, but feel free to replace this with your own input
4. Enter `dotnet run` if no changes were made to the input file name; if changes were made or if you wish to use a different input file, enter `dotnet run [NewFileName.txt]` (the new file must still be in the Inputs/ directory)
5. The output will be logged as an individual text file in the /DealerOnAssignment/Outputs/ directory

## The Assignment

NASA intends to land robotic rovers on Mars to explore a particularly curious-looking plateau. The rovers must navigate this rectangular plateau in a way so that their on board cameras can get a complete image of the surrounding terrain to send back to Earth.

A simple two-dimensional coordinate grid is mapped to the plateau to aid in rover navigation. Each point on the grid is represented by a pair of numbers X Y which correspond to the number of points East or North, respectively, from the origin. The origin of the grid is represented by 0 0 which corresponds to the southwest corner of the plateau. 0 1 is the point directly north of 0 0, 1 1 is the point immediately east of 0 1, etc. A rover’s current position and heading are represented by a triple X Y Z consisting of its current grid position X Y plus a letter Z corresponding to one of the four cardinal compass points, N E S W. For example, 0 0 N indicates that the rover is in the very southwest corner of the plateau, facing north.

NASA remotely controls rovers via instructions consisting of strings of letters. Possible instruction letters are L, R, and M. L and R instruct the rover to turn 90 degrees left or right, respectively (without moving from its current spot), while M instructs the rover to move forward one grid point along its current heading.

Your task is write an application that takes the test input (instructions from NASA) and provides the expected output
(the feedback from the rovers to NASA). Each rover will move in series, i.e. the next rover will not start moving until
the one preceding it finishes.

### Input

Assume the southwest corner of the grid is 0,0 (the origin). The first line of input establishes the exploration grid bounds by indicating the coordinates corresponding to the northeast corner of the plateau.

Next, each rover is given its instructions in turn. Each rover’s instructions consists of two lines of strings. The first string confirms the rover’s current position and heading. The second string consists of turn / move instructions.

### Example Input

5 5

1 2 N

LMLMLMLMM

3 3 E

MMRMMRMRRM

### Output

Once each rover has received and completely executed its given instructions, it transmits its updated position and heading to NASA.

### Example Output

1 3 N

5 1 E

## Design Explanation, Final Thoughts, & Reflections

This was an extremely enjoyable challenge. I have a lot of familiarity with coding in .NET C# from my experiences at BoomTown ROI, LLC, however this is one of the few times I've had the opportunity to design & implement the architecture of the entire project. Because of this opportunity, I did my best to pull all the stops on good design principles. The application runs via a host object in order to encapsulate the app's resources and lifetime functionality and allow for dependency injection, logging, and configuration. I separated the service & its interface into its own Services/ directory, and created an Xunit project to provide unit tests for the service's public methods with testable business logic. If I had to do it all over or had more time, I would have focused more on test driven development; i.e. writing unit tests *before* I write their corresponding methods. I would also put more emphasis on decreasing the algorithm's time and space complexities, as they both run on the order of O(n^2). I would also look further into the idiomatic nuances of C#, such as possibly using a different data type than Vector2/3 and/or using different methods of vector manipulation and floating point number handling. That being said, I am quite happy with the end product and am extremely grateful to DealerOn for this learning opportunity.
