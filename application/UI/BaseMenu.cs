using System;
using Figgle;

namespace SGCI_app.application.UI
{
    public abstract class BaseMenu
    {
        protected BaseMenu(bool showIntro)
        {
            if (showIntro)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(FiggleFonts.Standard.Render("SGCI"));
                Console.ResetColor();
                Console.WriteLine("\nBienvenido al Sistema de Gestión Comercial Integral");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        // Este será el constructor por defecto: llama al anterior con showIntro = true
        protected BaseMenu() : this(true) { }


        protected void ShowHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(FiggleFonts.Standard.Render("SGCI"));
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"=== {title} ===");
            Console.ResetColor();
            Console.WriteLine();
        }

        protected void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        protected void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {message}");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        protected void ShowInfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\nℹ {message}");
            Console.ResetColor();
        }

        protected string GetValidatedInput(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(input) && !allowEmpty)
                {
                    ShowErrorMessage("Este campo no puede estar vacío.");
                    continue;
                }
                
                return input ?? string.Empty;
            }
        }

        protected int GetValidatedIntInput(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= minValue && result <= maxValue)
                {
                    return result;
                }
                ShowErrorMessage($"Por favor, ingrese un número válido entre {minValue} y {maxValue}.");
            }
        }

        protected void DrawSeparator(char character = '-', int length = 50)
        {
            Console.WriteLine(new string(character, length));
        }

        public abstract void ShowMenu();
    }
} 