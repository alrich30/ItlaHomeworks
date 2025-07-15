using System;

class Program

{

    static List<string[]> contactos = new List<string[]>();


    static void Main()
    {
        bool salir = false;

        while (!salir)
        {
            Console.Clear(); // Limpia la consola en cada iteración

            Console.WriteLine("===== AGENDA PERSONAL =====");
            Console.WriteLine("1. Agregar contacto");
            Console.WriteLine("2. Editar contacto");
            Console.WriteLine("3. Eliminar contacto");
            Console.WriteLine("4. Buscar contacto");
            Console.WriteLine("5. Listar todos los contactos");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarContacto();
                    break;

                case "2":
                    EditarContacto(contactos);
                    break;

                case "3":
                    EliminarContacto(contactos);
                    break;

                case "4":
                    BuscarContacto(contactos);
                    break;

                case "5":
                    ListarContactos();
                    break;

                case "6":
                    salir = true;
                    break;

                default:
                    Console.WriteLine("Opción no válida. Presione una tecla para continuar.");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        Console.WriteLine("\nGracias por usar la agenda. ¡Hasta luego!");
    }

    static void AgregarContacto()
    {
        Console.Clear();
        Console.WriteLine("=== Agregar nuevo contacto ===");

        string[] nuevo = new string[7];

        Console.Write("Nombre: ");
        nuevo[0] = Console.ReadLine();

        Console.Write("Apellido: ");
        nuevo[1] = Console.ReadLine();

        Console.Write("Teléfono: ");
        nuevo[2] = Console.ReadLine();

        Console.Write("Correo electrónico: ");
        nuevo[3] = Console.ReadLine();

        Console.Write("Dirección física: ");
        nuevo[4] = Console.ReadLine();

        Console.Write("Edad: ");
        nuevo[5] = Console.ReadLine();

        Console.Write("¿Es favorito? (si/no): ");
        nuevo[6] = Console.ReadLine()?.ToLower() == "si" ? "Sí" : "No";

        contactos.Add(nuevo);

        Console.WriteLine("\nContacto agregado correctamente.");
    }

    static void ListarContactos()
    {
        Console.Clear();
        Console.WriteLine("=== Lista de contactos ===\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos registrados.");
        }
        else
        {
            for (int i = 0; i < contactos.Count; i++)
            {
                string[] c = contactos[i];
                Console.WriteLine($"Contacto #{i + 1}");
                Console.WriteLine($"Nombre: {c[0]}");
                Console.WriteLine($"Apellido: {c[1]}");
                Console.WriteLine($"Teléfono: {c[2]}");
                Console.WriteLine($"Correo: {c[3]}");
                Console.WriteLine($"Dirección: {c[4]}");
                Console.WriteLine($"Edad: {c[5]}");
                Console.WriteLine($"Favorito: {c[6]}");
                Console.WriteLine("-----------------------------");
            }
        }
    }
    static void EditarContacto(List<string[]> contactos)
    {
        Console.Clear();
        Console.WriteLine("Editar contacto\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos para editar.");
            return;
        }

        // Mostrar la lista de contactos numerada
        for (int i = 0; i < contactos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {contactos[i][0]} {contactos[i][1]}");
        }

        Console.Write("\nSeleccione el número del contacto que desea editar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= contactos.Count)
        {
            string[] contacto = contactos[indice - 1];

            Console.WriteLine("\nPresiona ENTER para mantener el valor actual.\n");

            Console.Write($"Nombre ({contacto[0]}): ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) contacto[0] = nombre;

            Console.Write($"Apellido ({contacto[1]}): ");
            string apellido = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(apellido)) contacto[1] = apellido;

            Console.Write($"Teléfono ({contacto[2]}): ");
            string telefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefono)) contacto[2] = telefono;

            Console.Write($"Email ({contacto[3]}): ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) contacto[3] = email;

            Console.Write($"Dirección ({contacto[4]}): ");
            string direccion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(direccion)) contacto[4] = direccion;

            Console.Write($"Edad ({contacto[5]}): ");
            string edad = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(edad)) contacto[5] = edad;

            Console.Write($"¿Favorito? sí/no ({contacto[6]}): ");
            string favorito = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(favorito)) contacto[6] = favorito.ToLower();

            Console.WriteLine("\nContacto actualizado correctamente.");
        }
        else
        {
            Console.WriteLine("Opción inválida.");
        }
    }

    static void EliminarContacto(List<string[]> contactos)
    {
        Console.Clear();
        Console.WriteLine("Eliminar contacto\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos para eliminar.");
            return;
        }

        // Mostrar contactos con índice
        for (int i = 0; i < contactos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {contactos[i][0]} {contactos[i][1]}");
        }

        Console.Write("\nSeleccione el número del contacto que desea eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= contactos.Count)
        {
            string[] seleccionado = contactos[indice - 1];
            Console.Write($"\n¿Estás seguro de que deseas eliminar a {seleccionado[0]} {seleccionado[1]}? (s/n): ");
            string confirmacion = Console.ReadLine()?.ToLower();

            if (confirmacion == "s")
            {
                contactos.RemoveAt(indice - 1);
                Console.WriteLine("\nContacto eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("\nEliminación cancelada.");
            }
        }
        else
        {
            Console.WriteLine("Opción inválida.");
        }
    }

    static void BuscarContacto(List<string[]> contactos)
    {
        Console.Clear();
        Console.WriteLine("Buscar contacto\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos registrados.");
            return;
        }

        Console.Write("Ingrese el nombre o apellido a buscar: ");
        string termino = Console.ReadLine()?.ToLower();

        bool encontrado = false;

        for (int i = 0; i < contactos.Count; i++)
        {
            string nombre = contactos[i][0].ToLower();
            string apellido = contactos[i][1].ToLower();

            if (nombre.Contains(termino) || apellido.Contains(termino))
            {
                string[] c = contactos[i];
                Console.WriteLine($"\nResultado #{i + 1}");
                Console.WriteLine($"Nombre: {c[0]}");
                Console.WriteLine($"Apellido: {c[1]}");
                Console.WriteLine($"Teléfono: {c[2]}");
                Console.WriteLine($"Correo: {c[3]}");
                Console.WriteLine($"Dirección: {c[4]}");
                Console.WriteLine($"Edad: {c[5]}");
                Console.WriteLine($"Favorito: {c[6]}");
                Console.WriteLine("-----------------------------");
                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("\nNo se encontraron coincidencias.");
        }
    }


}