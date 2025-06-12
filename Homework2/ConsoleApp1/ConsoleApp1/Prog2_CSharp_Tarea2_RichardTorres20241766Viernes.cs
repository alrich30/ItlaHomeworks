class DetectorDeParesEImpares
{
    static void Main()
    {
        try
        {
            Console.Write("Ingrese un numero:");
            int numero = int.Parse(Console.ReadLine());

            if (numero % 2 == 0)
            {
                Console.WriteLine("El numero es par");
            }
            else
            {
                Console.WriteLine("El numero es impar");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Ingresaste un valor no numérico.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error inesperado: " + ex.Message);
        }
    }
}
