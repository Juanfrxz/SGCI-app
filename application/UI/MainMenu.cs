using System;
using SGCI_app.application.services;
using SGCI_app.infrastructure.Repositories;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class MainMenu : BaseMenu
    {
        private readonly string[] menuOptions = new string[]
        {
            "Gestión de Países",
            "Gestión de Regiones",
            "Gestión de Ciudades",
            "Gestión de Clientes",
            "Gestión de Empleados",
            "Gestión de Proveedores",
            "Gestión de Planes Promocionales",
            "Gestión de EPS",
            "Gestión de ARL",
            "Gestión de Empresas",
            "Gestión de Productos",
            "Gestión de Asociaciones Plan-Producto",
            "Gestión de Tipos de Documentos",
            "Gestión de Tipo Terceros",
            "Gestión de Tipo Telefonos",
            "Gestión de Compras",
            "Gestión de Ventas",
            "Gestión de Tipos de Movimiento de Caja",
            "Gestión de Sesiones de Caja",
            "Gestión de Movimientos de Caja"
        };

        private const int OptionsPerPage = 10;
        private const int ColumnsPerPage = 2;
        private const int OptionsPerColumn = 5;

        public MainMenu()
        {
        }

        private void DisplayMenuPage(int page)
        {
            ShowHeader("MENÚ PRINCIPAL");
            
            int startIndex = page * OptionsPerPage;
            int endIndex = Math.Min(startIndex + OptionsPerPage, menuOptions.Length);

            // Calculate column width based on the longest option
            int columnWidth = menuOptions.Max(opt => opt.Length) + 5;

            for (int i = 0; i < OptionsPerColumn; i++)
            {
                int leftIndex = startIndex + i;
                int rightIndex = startIndex + i + OptionsPerColumn;

                string leftOption = leftIndex < endIndex ? $"{leftIndex + 1,2}. {menuOptions[leftIndex]}" : "";
                string rightOption = rightIndex < endIndex ? $"{rightIndex + 1,2}. {menuOptions[rightIndex]}" : "";

                Console.Write(leftOption.PadRight(columnWidth));
                if (!string.IsNullOrEmpty(rightOption))
                {
                    Console.WriteLine(rightOption);
                }
                else
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\n0.  Salir");
            if (menuOptions.Length > OptionsPerPage)
            {
                Console.WriteLine($"N.  Siguiente página");
                Console.WriteLine($"P.  Página anterior");
            }
            
            DrawSeparator();
            Console.Write("\nSeleccione una opción: ");
        }

        public override void ShowMenu()
        {
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(menuOptions.Length / (double)OptionsPerPage);

            while (true)
            {
                DisplayMenuPage(currentPage);

                string? input = Console.ReadLine()?.ToUpper();
                
                if (string.IsNullOrEmpty(input))
                {
                    ShowErrorMessage("Por favor, ingrese una opción válida.");
                    continue;
                }

                if (input == "N" && currentPage < totalPages - 1)
                {
                    currentPage++;
                    continue;
                }
                else if (input == "P" && currentPage > 0)
                {
                    currentPage--;
                    continue;
                }

                if (!int.TryParse(input, out int option))
                {
                    ShowErrorMessage("Opción no válida.");
                    continue;
                }

                if (option == 0)
                {
                    ShowSuccessMessage("¡Gracias por usar SGCI! ¡Hasta pronto!");
                    return;
                }

                if (option < 1 || option > menuOptions.Length)
                {
                    ShowErrorMessage("Opción no válida.");
                    continue;
                }

                // Execute the selected option
                ExecuteOption(option);
            }
        }

        private void ExecuteOption(int option)
        {
            switch (option)
            {
                case 1:
                    var countryMenu = new CountryMenu();
                    countryMenu.ShowMenu();
                    break;
                case 2:
                    var regionMenu = new RegionMenu();
                    regionMenu.ShowMenu();
                    break;
                case 3:
                    var cityMenu = new CityMenu();
                    cityMenu.ShowMenu();
                    break;
                case 4:
                    var dtoClientMenu = new ClientMenu();
                    dtoClientMenu.ShowMenu();
                    break;
                case 5:
                    var employeeMenu = new EmployeeMenu();
                    employeeMenu.ShowMenu();
                    break;
                case 6:
                    var providerMenu = new ProviderMenu();
                    providerMenu.ShowMenu();
                    break;
                case 7:
                    var promotionalPlanMenu = new PromotionalPlanMenu();
                    promotionalPlanMenu.ShowMenu();
                    break;
                case 8:
                    var epsMenu = new EpsMenu();
                    epsMenu.ShowMenu();
                    break;
                case 9:
                    var arlMenu = new ArlMenu();
                    arlMenu.ShowMenu();
                    break;
                case 10:
                    var companyMenu = new CompanyMenu();
                    companyMenu.ShowMenu();
                    break;
                case 11:
                    var productMenu = new ProductMenu();
                    productMenu.ShowMenu();
                    break;
                case 12:
                    var promoplanMenu = new PromotionalPlanProductMenu();
                    promoplanMenu.ShowMenu();
                    break;
                case 13:
                    var doctypeMenu = new DocTypeMenu();
                    doctypeMenu.ShowMenu();
                    break;
                case 14:
                    var tipotercerosMenu = new ThirdPartyTypeMenu();
                    tipotercerosMenu.ShowMenu();
                    break;
                case 15:
                    var tipotelefonosMenu = new PhoneTypeMenu();
                    tipotelefonosMenu.ShowMenu();
                    break;
                case 16:
                    var purchaseMenu = new PurchaseMenu();
                    purchaseMenu.ShowMenu();
                    break;
                case 17:
                    var saleMenu = new SaleMenu();
                    saleMenu.ShowMenu();
                    break;
                case 18:
                    var cashmovetypeMenu = new CashMovementTypeMenu();
                    cashmovetypeMenu.ShowMenu();
                    break;
                case 19:
                    var sescajaMenu = new CashSessionMenu();
                    sescajaMenu.ShowMenu();
                    break;
                case 20:
                    var movcajaMenu = new CashMovementMenu();
                    movcajaMenu.ShowMenu();
                    break;
            }
        }
    }
} 