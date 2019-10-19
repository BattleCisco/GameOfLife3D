# Conway's Game Of Life - Now in 3D
![CGOL3DGIF](https://user-images.githubusercontent.com/26437052/67147383-cbd00500-f294-11e9-8c70-c7821f251a06.gif)

## Explaination
I wanted to make Conway's game of life, but in 3D. I went about it using Scriptable Objects and a 3D array for it.

## Functionality
* The game rule is described by a ScriptableObject, allowing you to quickly swap one simulation for another.
* You can add a CellAttribute ScriptableObject to a cell to override the rules for it to make it invulnerable.

## Conclusion
You need alot of compute power to run a larger version, as the 3 dimensions cubes the amount of cells. But regardless I am satisfied with how it turned out.

## Possible Improvements
* Switch from 3D array to a longer 1D array.
* Use compute Shader to faster compute the states.

## Links

[Candidates for the Game of Life in Three Dimensions](https://wpmedia.wolfram.com/uploads/sites/13/2018/02/01-3-1.pdf)
