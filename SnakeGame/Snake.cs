using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Snake : IRenderable
    {
        // List to store the body positions of the snake (head is at index 0)
        private List<Position> _body;

        // Number of growth spurts remaining for the snake
        private int _growthSpurtsRemaining;
        public Snake(Position spawnLocation, int initialSize = 1)
        {
            _body = new List<Position> { spawnLocation };
            _growthSpurtsRemaining = Math.Max(0, initialSize - 1);
            Dead = false;
        }

        // Property that returns whether the snake is dead
        public bool Dead { get; private set; }

        // Property to access the head of the snake (first position in the body list)
        public Position Head => _body[0];

        // Property to get the body positions of the snake (excluding the head)
        private IEnumerable<Position> Body => _body.Skip(1);

        // Method to move the snake in a given direction
        public void Move(Direction direction)
        {
            if (Dead) throw new InvalidOperationException();

            // Determine the new head position based on the direction
            Position newHead = direction switch
            {
                Direction.Up => Head.DownBy(-1),
                Direction.Left => Head.RightBy(-1),
                Direction.Down => Head.DownBy(1),
                Direction.Right => Head.RightBy(1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction.")
            };

            // Check if the new head position collides with the body or is out of bounds
            if (_body.Contains(newHead) || !PositionIsValid(newHead))
            {
                Dead = true;
            }

            // Add the new head to the body list
            _body.Insert(0, newHead);

            // If the snake has growth spurts remaining, decrease it by 1
            if (_growthSpurtsRemaining > 0)
            {
                _growthSpurtsRemaining--;
            }
            else
            {
                // If no growth spurts left, remove the tail (the last body part)
                Position tail = _body[^1];
                _body.RemoveAt(_body.Count - 1);
                ClearCell(tail);
            }
        }

        // Method to increase the snake's growth spurts
        public void Grow()
        {
            if (Dead) throw new InvalidOperationException();

            _growthSpurtsRemaining++;
        }

        // Method to check if a position is within the valid game area (not out of bounds)
        private static bool PositionIsValid(Position position) =>
         position.Top > 1 && position.Top < Console.WindowHeight - 11 &&
         position.Left > 1 && position.Left < Console.WindowWidth - 11;

        // Method to redraw the snake, used when the snake moves
        public void Redraw()
        {
            // Draw the body parts with different symbols: "o" for the body and "0" for the head
            DrawSnakePart(_body[1], "o");
            DrawSnakePart(Head, "0");
        }

        // Method to render the whole snake on the screen
        public void Render()
        {
            DrawSnakePart(Head, "O");
            foreach (var position in Body)
            {
               DrawSnakePart(position, "o");
            }
        }

        // Helper method to draw a single part of the snake (either head or body) at the specified position
        private void DrawSnakePart(Position pos, string symbol)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(pos.Left, pos.Top);
            Console.Write(symbol);
            Console.ResetColor();
        }

        // Method to clear a cell on the screen (used for removing the tail when the snake moves)
        private void ClearCell(Position pos)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(pos.Left, pos.Top);
            Console.Write(" ");
            Console.ResetColor();
        }

    }
}
