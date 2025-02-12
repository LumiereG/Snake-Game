using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class SnakeGame : IRenderable
    {
        // The origin position of the game (starting point of the snake)
        private static readonly Position Origin = new Position(2, 2);

        // Lock object to ensure thread-safe direction changes
        private readonly object _directionLock = new object();

        private Direction _currentDirection;
        private Direction _nextDirection;

        private Snake _snake;
        private Apple _apple;
        private int _score = 0;
        private readonly Random _random = new Random();

        public SnakeGame()
        {
            // Initialize the snake, starting at the origin with an initial size of 5
            _snake = new Snake(Origin, initialSize: 5);

            // Generate the first apple for the game
            _apple = CreateApple();
            _currentDirection = Direction.Right;
            _nextDirection = Direction.Right;
            _score = 5;
            Boarders();
            ShowScore();
        }

        // Property that checks if the game is over (if the snake is dead)
        public bool GameOver => _snake.Dead;

        // Method that handles key press events and changes the snake's direction
        public void OnKeyPress(ConsoleKey key)
        {
            Direction newDirection = key switch
            {
                ConsoleKey.W => Direction.Up,
                ConsoleKey.A => Direction.Left,
                ConsoleKey.S => Direction.Down,
                ConsoleKey.D => Direction.Right,
                _ => _nextDirection
            };

            lock (_directionLock)
            {
                // Prevent the snake from turning in the opposite direction (e.g., from right to left)
                if (newDirection != OppositeDirectionTo(_currentDirection))
                {
                    _nextDirection = newDirection;
                }
            }
        }

        // Method that updates the game state for each tick (movement, collision, etc.)
        public bool OnGameTick()
        {
            if (GameOver) throw new InvalidOperationException("Game is over.");

            lock (_directionLock)
            {
                _currentDirection = _nextDirection;
            }

            // Move the snake in the current direction
            _snake.Move(_currentDirection);

            // Check if the snake's head has collided with the apple
            if (_snake.Head.Equals(_apple.Position))
            {
                // Increase score and grow the snake
                _score++;
                ShowScore();
                _snake.Grow();

                // Clear the eaten apple and create a new one
                _apple.ClearCell(_apple.Position);
                _apple = CreateApple();
                return true; // Return true to indicate an apple was eaten
            }
            return false;
        }

        // Helper method to get the opposite direction
        private static Direction OppositeDirectionTo(Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            _ => throw new ArgumentOutOfRangeException()
        };

        // Method to create a new apple at a random position
        private Apple CreateApple()
        {
            int top = _random.Next(2, Console.WindowHeight - 12);
            int left = _random.Next(2, Console.WindowWidth - 12);
            return new Apple(new Position(top, left));
        }

        // Redraw the snake and the apple
        public void Redraw()
        {
            _snake.Redraw();
            _apple.Render();
        }

        // Method to render the whole board
        public void Render()
        {
            Console.Clear();
            Boarders();
            ShowScore();
            _snake.Render();
            _apple.Render();
        }

        // Method to draw the borders of the game (edges of the playing area)
        public void Boarders()
        {
            Console.SetCursorPosition(0, 0);
            for (int x = 1; x < Console.WindowWidth - 10; x++)
            {
                Console.SetCursorPosition(x, 1);
                Console.Write("═");
                Console.SetCursorPosition(x, Console.WindowHeight - 11);
                Console.Write("═");
            }

            for (int y = 1; y < Console.WindowHeight - 10; y++)
            {
                Console.SetCursorPosition(1, y);
                Console.Write("║");
                Console.SetCursorPosition(Console.WindowWidth - 11, y);
                Console.Write("║");
            }

        }

        // Method to display the score at the bottom of the screen
        void ShowScore()
        {
            Console.SetCursorPosition( 0 , Console.WindowHeight - 10);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Score: {_score}");
            Console.ResetColor();
        }

    }
}
