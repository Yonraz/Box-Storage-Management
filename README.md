# Box Storage

This DSA course project. The goal is to create a box storage management that will store and retrieve boxes based on width and height.
If no boxes are found at the desired width/height, the program should find the closest available size.
To acheive this functionality I implemented a nested AVL tree data structure.
the first tree's nodes represent the box's width - named WidthNode. each width node holds information about the current node's width, count (amount) and an InnerStorage.
The InnerStorage is the second tree. it's nodes represent height and ultimately the hold the box data itself.
This implementation of nested AVLs was chosen because it's the most efficient - the tree is balanced at all times so each node can be reached in log(n) time, and because it's done in a recursive way, the nearest size can be found by just going back one step without needing to go through the entire data structure everytime.
The UI was built using WPF in the MVVN design pattern.
