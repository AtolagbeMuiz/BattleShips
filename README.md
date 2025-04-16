###Project Review
This project is a one-sided mini-game called Battleship where the battleships are aligned into a 10x10 grid, where the column are labeled from A-J and rows labeled 1-10.
whereby user enter input in the form of column-row format. for example user input like "A5" means column A row 5. 

Solution summary
The high level approach to solving this propblem was initializing an empty array of Integers filled with default value of zeros. the loop through an in-memory database of ships provided 
in the codebase, then these ships are placed in their respective randomly generated position/cordinate (row,column and orientation). After the postion and coordinate of these ship are determined,
the cells which the ships fall into are depicted with Value 1 to show that the cell is occupied with a ship.
i.e. 0 means cell is empty
     1 means cell has a ship positioned there
As seen below;

![](Battleships/Battleships/Image/screenshot.png)
