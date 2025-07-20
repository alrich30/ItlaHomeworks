// Contacto.cs
public class Contacto
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string Direccion { get; set; }
    public string Edad { get; set; }
    public bool EsFavorito { get; set; }

    public void Mostrar(int indice)
    {
        Console.WriteLine($"Contacto #{indice}");
        Console.WriteLine($"Nombre: {Nombre}");
        Console.WriteLine($"Apellido: {Apellido}");
        Console.WriteLine($"Teléfono: {Telefono}");
        Console.WriteLine($"Correo: {Correo}");
        Console.WriteLine($"Dirección: {Direccion}");
        Console.WriteLine($"Edad: {Edad}");
        Console.WriteLine($"Favorito: {(EsFavorito ? "Sí" : "No")}");
        Console.WriteLine("-----------------------------");
    }
}

// Agenda.cs
public class Agenda
{
    private List<Contacto> contactos = new List<Contacto>();

    public void AgregarContacto()
    {
        Console.Clear();
        Console.WriteLine("=== Agregar nuevo contacto ===");

        Contacto nuevo = new Contacto();

        Console.Write("Nombre: "); nuevo.Nombre = Console.ReadLine();
        Console.Write("Apellido: "); nuevo.Apellido = Console.ReadLine();
        Console.Write("Teléfono: "); nuevo.Telefono = Console.ReadLine();
        Console.Write("Correo electrónico: "); nuevo.Correo = Console.ReadLine();
        Console.Write("Dirección física: "); nuevo.Direccion = Console.ReadLine();
        Console.Write("Edad: "); nuevo.Edad = Console.ReadLine();
        Console.Write("¿Es favorito? (si/no): ");
        nuevo.EsFavorito = Console.ReadLine()?.ToLower() == "si";

        contactos.Add(nuevo);
        Console.WriteLine("\nContacto agregado correctamente.");
    }

    public void ListarContactos()
    {
        Console.Clear();
        Console.WriteLine("=== Lista de contactos ===\n");

        if (contactos.Count == 0)
            Console.WriteLine("No hay contactos registrados.");
        else
            for (int i = 0; i < contactos.Count; i++)
                contactos[i].Mostrar(i + 1);
    }

    public void EditarContacto()
    {
        Console.Clear();
        Console.WriteLine("Editar contacto\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos para editar.");
            return;
        }

        for (int i = 0; i < contactos.Count; i++)
            Console.WriteLine($"{i + 1}. {contactos[i].Nombre} {contactos[i].Apellido}");

        Console.Write("\nSeleccione el número del contacto que desea editar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= contactos.Count)
        {
            Contacto contacto = contactos[indice - 1];

            Console.WriteLine("\nPresiona ENTER para mantener el valor actual.\n");

            Console.Write($"Nombre ({contacto.Nombre}): ");
            string entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Nombre = entrada;

            Console.Write($"Apellido ({contacto.Apellido}): "); entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Apellido = entrada;

            Console.Write($"Teléfono ({contacto.Telefono}): "); entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Telefono = entrada;

            Console.Write($"Correo ({contacto.Correo}): "); entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Correo = entrada;

            Console.Write($"Dirección ({contacto.Direccion}): "); entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Direccion = entrada;

            Console.Write($"Edad ({contacto.Edad}): "); entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.Edad = entrada;

            Console.Write($"¿Favorito? sí/no ({(contacto.EsFavorito ? "sí" : "no")}): ");
            entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada)) contacto.EsFavorito = entrada.ToLower() == "si";

            Console.WriteLine("\nContacto actualizado correctamente.");
        }
        else Console.WriteLine("Opción inválida.");
    }

    public void EliminarContacto()
    {
        Console.Clear();
        Console.WriteLine("Eliminar contacto\n");

        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos para eliminar.");
            return;
        }

        for (int i = 0; i < contactos.Count; i++)
            Console.WriteLine($"{i + 1}. {contactos[i].Nombre} {contactos[i].Apellido}");

        Console.Write("\nSeleccione el número del contacto que desea eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice >= 1 && indice <= contactos.Count)
        {
            Contacto seleccionado = contactos[indice - 1];
            Console.Write($"\n¿Estás seguro de eliminar a {seleccionado.Nombre} {seleccionado.Apellido}? (s/n): ");
            string confirmacion = Console.ReadLine()?.ToLower();

            if (confirmacion == "s")
            {
                contactos.RemoveAt(indice - 1);
                Console.WriteLine("\nContacto eliminado correctamente.");
            }
            else Console.WriteLine("\nEliminación cancelada.");
        }
        else Console.WriteLine("Opción inválida.");
    }

    public void BuscarContacto()
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
            string nombre = contactos[i].Nombre.ToLower();
            string apellido = contactos[i].Apellido.ToLower();

            if (nombre.Contains(termino) || apellido.Contains(termino))
            {
                contactos[i].Mostrar(i + 1);
                encontrado = true;
            }
        }

        if (!encontrado)
            Console.WriteLine("\nNo se encontraron coincidencias.");
    }
}

// Program.cs
class Program
{
    static void Main()
    {
        Agenda agenda = new Agenda();
        bool salir = false;

        while (!salir)
        {
            Console.Clear();
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
                case "1": agenda.AgregarContacto(); break;
                case "2": agenda.EditarContacto(); break;
                case "3": agenda.EliminarContacto(); break;
                case "4": agenda.BuscarContacto(); break;
                case "5": agenda.ListarContactos(); break;
                case "6": salir = true; break;
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
}
