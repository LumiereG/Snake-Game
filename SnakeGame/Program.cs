using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Media;
using static System.Net.Mime.MediaTypeNames;

namespace SnakeGame;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.CursorVisible = false;

        // Set the initial tick rate of the game (100 ms per tick)
        TimeSpan tickRate = TimeSpan.FromMilliseconds(100);
        SnakeGame snakeGame = new SnakeGame();

        // Create a CancellationTokenSource to manage task cancellation
        CancellationTokenSource cts = new();

        // Lock object to synchronize access to shared resources in multi-threading
        object gameLock = new();

        // Load the sound for the game over event (optional, will be null if not found)
        SoundPlayer? player = LoadGameOverSound();

        // Start a task to monitor key presses for controlling the snake
        Task monitorKeyPresses = MonitorKeyPresses(snakeGame, cts.Token);
        

        while (!snakeGame.GameOver)
        {
            // If the snake eats an apple, increase the game speed by reducing the tick rate
            if (snakeGame.OnGameTick())
            {
                tickRate = TimeSpan.FromMilliseconds(Math.Max(30, tickRate.TotalMilliseconds - 10));
            }
            snakeGame.Redraw();
            await Task.Delay(tickRate);
        }

        // Cancel the monitoring tasks as the game is over
        cts.Cancel();

        // Play the game over sound, if it was successfully loaded
        player?.Play();

        await ShowGameOverAnimation(snakeGame);
        DisplayGameOverScreen();
        Console.Clear();
        await monitorKeyPresses;
    }

    // Method to load the "game over" sound from an embedded resource
    private static SoundPlayer? LoadGameOverSound()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("SnakeGame.SnakeGame.Sound.wav");
            return stream != null ? new SoundPlayer(stream) : null;
        }
        catch
        {
            return null;
        }
    }

    // Method to continuously monitor for key presses to control the snake
    private static async Task MonitorKeyPresses(SnakeGame snakeGame, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // Check if a key is pressed
            if (Console.KeyAvailable)
            {
                // Get the key press and pass it to the snake game for processing
                var key = Console.ReadKey(intercept: true).Key;
                snakeGame.OnKeyPress(key);
            }
            await Task.Delay(10);
        }
    }

    // Show a brief animation after the game ends by clearing the screen and rendering the game state
    private static async Task ShowGameOverAnimation(SnakeGame snakeGame)
    {
        for (int i = 0; i < 3; i++)
        {
            Console.Clear();
            await Task.Delay(300);
            snakeGame.Render();
            await Task.Delay(300);
        }
    }

    // Display the "Game Over" message and prompt the user to exit
    private static void DisplayGameOverScreen()
    {
        Console.Clear();
        string gameOverText = "GAME OVER";
        Console.SetCursorPosition((Console.WindowWidth - gameOverText.Length) / 2, Console.WindowHeight / 3);
        Console.WriteLine(gameOverText);
        Console.SetCursorPosition((Console.WindowWidth - 20) / 2, Console.WindowHeight / 2);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
