using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    // Enum to represent the four possible directions the snake can move in
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // Interface to enforce a Render method on any class that needs to render something on the screen
    interface IRenderable
    {
        void Render();

    }

    // Readonly struct representing a position in a 2D grid (console screen)
    readonly struct Position
    {
        public Position(int top, int left)
        {
            Top = top; // Vertical position (row)
            Left = left; // Horizontal position (column)
        }
        public int Top { get; }
        public int Left { get; }

        // Method to create a new position by moving to the right by 'n' steps
        public Position RightBy(int n) => new Position(Top, Left + n);

        // Method to create a new position by moving downward by 'n' steps
        public Position DownBy(int n) => new Position(Top + n, Left);
    }


}
