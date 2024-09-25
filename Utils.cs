namespace LiaLista;

public static class Utils
{
    public static string GetString(string message, string errorMessage = "Invalid input, try again.")
    {
        Console.Clear();
        do
        {
            Console.Write(message);
            string input = Console.ReadLine().ToLower().Trim();

            if (string.IsNullOrWhiteSpace(input) || input.Length < 1)
            {
                Console.Clear();
                Console.WriteLine(errorMessage);
                continue;
            }

            return $"{char.ToUpper(input[0])}{input[1..]}";

        } while (true);
    }

    public static int GetInt(
            string message,
            string errorMessage = "Invalid input, try again.",
            int min = int.MinValue,
            int max = int.MaxValue
            )
    {
        Console.Clear();
        do
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out int num) &&
                    num <= max && num >= min)
            {
                return num;
            }

            Console.Clear();
            Console.WriteLine(errorMessage);
        } while (true);
    }
}