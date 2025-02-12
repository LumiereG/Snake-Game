using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    // Apple class representing an apple object in the game
    class Apple : IRenderable
    {
        public Apple(Position position)
        {
            Position = position;
        }

        // Property to get the current position of the apple
        public Position Position { get; }

        // Method to render the apple on the console
        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(Position.Left, Position.Top);
            Console.Write("*");
            Console.ResetColor();
        }

        // Method to clear a specific cell on the console (e.g., when the apple is eaten)
        public void ClearCell(Position pos)
        {
            Console.BackgroundColor = ConsoleColor.Black; 
            Console.SetCursorPosition(pos.Left, pos.Top);
            Console.Write(" "); 
            Console.ResetColor(); 
        }
    }
}
