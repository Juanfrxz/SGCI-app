using System;
using SGCI_app.application.services;
using SGCI_app.infrastructure.Repositories;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class MainMenu
    {
        public MainMenu()
        {
        }

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
                Console.WriteLine("9. Gestión de ARL");
                Console.WriteLine("10. Gestión de Empresas");
                Console.WriteLine("11. Gestion de Productos");
                Console.WriteLine("12. Gestión de Asociaciones Plan-Producto");
                Console.WriteLine("13. Gestión de Tipos de Documentos");
                Console.WriteLine("14. Gestión de Tipo Terceros");
                Console.WriteLine("15. Gestión de Tipo Telefonos");
                Console.WriteLine("16. Gestión de Compras");
                Console.WriteLine("17. Gestión de Ventas");
                Console.WriteLine("18. Gestión de Tipos de Movimiento de Caja");
                Console.WriteLine("19. Gestión de Sesiones de Caja");
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
                    case "9":
                        var arlMenu = new ArlMenu();
                        arlMenu.ShowMenu();
                        break;
                    case "10":
                        var companyMenu = new CompanyMenu();
                        companyMenu.ShowMenu();
                        break;
                    case "11":
                        var productMenu = new ProductMenu();
                        productMenu.ShowMenu();
                        break;
                    case "12":
                        var promoplanMenu = new PromotionalPlanProductMenu();
                        promoplanMenu.ShowMenu();
                        break;
                    case "13":
                        var doctypeMenu = new DocTypeMenu();
                        doctypeMenu.ShowMenu();
                        break;
                    case "14":
                        var tipotercerosMenu = new ThirdPartyTypeMenu();
                        tipotercerosMenu.ShowMenu();
                        break;
                    case "15":
                        var tipotelefonosMenu = new PhoneTypeMenu();
                        tipotelefonosMenu.ShowMenu();
                        break;
                    case "16":
                        var purchaseMenu = new PurchaseMenu();
                        purchaseMenu.ShowMenu();
                        break;
                    case "17":
                        var saleMenu = new SaleMenu();
                        saleMenu.ShowMenu();
                        break;
                    case "18":
                        var cashmovetypeMenu = new CashMovementTypeMenu();
                        cashmovetypeMenu.ShowMenu();
                        break;
                    case "19":
                        var sescajaMenu = new CashSessionMenu();
                        sescajaMenu.ShowMenu();
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