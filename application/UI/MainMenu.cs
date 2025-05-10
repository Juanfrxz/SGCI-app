using System;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class MainMenu
    {
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Gestión de Países");
                Console.WriteLine("2. Gestión de Regiones");
                Console.WriteLine("3. Gestión de Ciudades");
                Console.WriteLine("4. Gestión de Clientes");
                Console.WriteLine("5. Gestión de Empleados");
                Console.WriteLine("6. Gestión de Proveedores");
                Console.WriteLine("7. Gestión de Planes Promocionales");
                Console.WriteLine("8. Gestión de EPS");
                Console.WriteLine("0. Salir");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1":
                        var countryMenu = new CountryMenu();
                        countryMenu.ShowMenu();
                        break;
                    case "2":
                        var regionMenu = new RegionMenu();
                        regionMenu.ShowMenu();
                        break;
                    case "3":
                        var cityMenu = new CityMenu();
                        cityMenu.ShowMenu();
                        break;
                    case "4":
                        var dtoClientMenu = new ClientMenu();
                        dtoClientMenu.ShowMenu();
                        break;
                    case "5":
                        var employeeMenu = new EmployeeMenu();
                        employeeMenu.ShowMenu();
                        break;
                    case "6":
                        var providerMenu = new ProviderMenu();
                        providerMenu.ShowMenu();
                        break;
                    case "7":
                        var promotionalPlanMenu = new PromotionalPlanMenu();
                        promotionalPlanMenu.ShowMenu();
                        break;
                    case "8":
                        var epsMenu = new EpsMenu();
                        epsMenu.ShowMenu();
                        break;
                    case "0":
                        Console.WriteLine("¡Hasta pronto!");
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
} 