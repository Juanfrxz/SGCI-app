using System;
using System.Collections.Generic;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PromotionalPlanMenu
    {
        private readonly PromotionalPlanService _service;

        public PromotionalPlanMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PromotionalPlanService(factory.CrearPromoPlanRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE PLANES PROMOCIONALES ===");
                Console.WriteLine("1. Crear Plan Promocional");
                Console.WriteLine("2. Listar Planes Promocionales");
                Console.WriteLine("3. Actualizar Plan Promocional");
                Console.WriteLine("4. Eliminar Plan Promocional");
                Console.WriteLine("0. Volver al Menú Principal");
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
                        CrearPlanPromocional();
                        break;
                    case "2":
                        ListarPlanesPromocionales();
                        break;
                    case "3":
                        ActualizarPlanPromocional();
                        break;
                    case "4":
                        EliminarPlanPromocional();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearPlanPromocional()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR PLAN PROMOCIONAL ===");

            var plan = new PromotionalPlan();

            Console.Write("Nombre del plan: ");
            plan.Nombre = Console.ReadLine();

            Console.Write("ID del técnico: ");
            if (int.TryParse(Console.ReadLine(), out int tecnicoId))
            {
                plan.Tecnico_Id = tecnicoId;
            }

            Console.Write("Descuento (%): ");
            if (double.TryParse(Console.ReadLine(), out double descuento))
            {
                plan.Descuento = descuento;
            }

            Console.Write("Fecha de inicio (YYYY-MM-DD): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime inicio))
            {
                plan.Inicio = inicio;
            }

            Console.Write("Fecha de fin (YYYY-MM-DD): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime fin))
            {
                plan.Fin = fin;
            }

            Console.Write("Datos extra: ");
            plan.Datos_Extra = Console.ReadLine();

            try
            {
                _service.CreatePromotionalPlan(plan);
                Console.WriteLine("\nPlan promocional creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear el plan: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarPlanesPromocionales()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE PLANES PROMOCIONALES ===\n");

            try
            {
                var plans = _service.GetAllPromotionalPlans();
                if (plans.Count == 0)
                {
                    Console.WriteLine("No hay planes promocionales registrados.");
                }
                else
                {
                    foreach (var plan in plans)
                    {
                        Console.WriteLine($"ID: {plan.Id}");
                        Console.WriteLine($"Nombre: {plan.Nombre}");
                        Console.WriteLine($"Descuento: {plan.Descuento}%");
                        Console.WriteLine($"Fecha Inicio: {(plan.Inicio.HasValue ? plan.Inicio.Value.ToShortDateString() : "No especificada")}");
                        Console.WriteLine($"Fecha Fin: {(plan.Fin.HasValue ? plan.Fin.Value.ToShortDateString() : "No especificada")}");
                        Console.WriteLine("----------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al obtener los planes: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarPlanPromocional()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PLAN PROMOCIONAL ===");

            Console.Write("Ingrese el ID del plan a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            var plan = new PromotionalPlan { Id = id };

            Console.Write("Nuevo nombre (dejar en blanco para mantener el actual): ");
            plan.Nombre = Console.ReadLine();

            Console.Write("Nuevo ID del técnico (0 para mantener el actual): ");
            if (int.TryParse(Console.ReadLine(), out int tecnicoId))
            {
                plan.Tecnico_Id = tecnicoId;
            }

            Console.Write("Nuevo descuento (0 para mantener el actual): ");
            if (double.TryParse(Console.ReadLine(), out double descuento))
            {
                plan.Descuento = descuento;
            }

            Console.Write("Nueva fecha de inicio (YYYY-MM-DD, dejar en blanco para mantener la actual): ");
            string? fechaInicio = Console.ReadLine();
            if (!string.IsNullOrEmpty(fechaInicio) && DateTime.TryParse(fechaInicio, out DateTime inicio))
            {
                plan.Inicio = inicio;
            }

            Console.Write("Nueva fecha de fin (YYYY-MM-DD, dejar en blanco para mantener la actual): ");
            string? fechaFin = Console.ReadLine();
            if (!string.IsNullOrEmpty(fechaFin) && DateTime.TryParse(fechaFin, out DateTime fin))
            {
                plan.Fin = fin;
            }

            Console.Write("Nuevos datos extra (dejar en blanco para mantener los actuales): ");
            plan.Datos_Extra = Console.ReadLine();

            try
            {
                _service.UpdatePromotionalPlan(plan);
                Console.WriteLine("\nPlan promocional actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar el plan: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarPlanPromocional()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PLAN PROMOCIONAL ===");

            Console.Write("Ingrese el ID del plan a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.DeletePromotionalPlan(id);
                Console.WriteLine("\nPlan promocional eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar el plan: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 