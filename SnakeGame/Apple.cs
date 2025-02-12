using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Apple : IRenderable
    {
        public Apple(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(Position.Left, Position.Top);
            Console.Write("*");
            Console.ResetColor();
        }

        public void ClearCell(Position pos)
        {
            Console.BackgroundColor = ConsoleColor.Black; // Устанавливаем цвет фона
            Console.SetCursorPosition(pos.Left, pos.Top);
            Console.Write(" "); // Стираем хвост
            Console.ResetColor(); // Сбрасываем цвет
        }
    }
}
