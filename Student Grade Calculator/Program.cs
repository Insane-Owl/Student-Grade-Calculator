using static System.Console;
class StudentGradeCalculator
{
    static void Main()
    {
        // I wrapped the main loop in a while loop so that you can enter another student's information without having to re-run the script each time.
        while (true)
        {
            DisplayWelcomeMessage();
            string studentName = GetStudentName();
            List<double> grades = GetGrades();
            double average = CalculateAverage(grades);
            char letterGrade = DetermineLetterGrade(average);
            string status = DetermineStatus(average);
            DisplayResults(studentName, average, letterGrade, status);

            Write("Check another? (Y/n) >> ");
            string sentinel = ReadLine();
            if (sentinel != "y" && sentinel != "Y" && sentinel != "")
                break;
            Clear(); // Clear the screen so a student can't accidentally see another's grades.
        }
    }
    private static void DisplayWelcomeMessage()
    {
        WriteLine("=== Student Grade Calculator ===");
    }
    private static string GetStudentName()
    {
        string name;
        while (true)
        {
            Write("Enter Student Name >> ");
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                WriteLine("Name cannot be blank, please re-enter.");
                continue;
            }
            break;
        }
        return name;
    }
    private static List<double> GetGrades()
    {
        List<double> grades = new List<double>(); // First tried an array but forgot those have a static size, lists however are dynamic.
        while (true) // Allows the user to enter as many grades as they want.
        {
            Write("Enter a Score (0-100) or press Enter to finish >> ");
            string input = ReadLine();

            if (string.IsNullOrWhiteSpace(input) && grades.Count > 0) // Enter key breaks out of the loop.
                break;

            double grade;
            if (double.TryParse(input, out grade) && grade >= 0 && grade <= 100)
                grades.Add(grade);
            else
                WriteLine("Invalid score, Must be between 0 and 100. Please re-enter.");
        }
        return grades;
    }
    private static double CalculateAverage(List<double> grades)
    {
        double total = 0;

        foreach (double score in grades)
            total += score;

        double average = total / grades.Count;
        return average;
    }
    private static char DetermineLetterGrade(double grade)
    {
        switch (grade)
        {
            case >= 90:
                return 'A';
            case >= 80:
                return 'B';
            case >= 70:
                return 'C';
            case >= 60:
                return 'D';
            default:
                return 'F';
        }
    }
    private static string DetermineStatus(double grade)
    {
        if (grade >= 60)
            return "Pass";
        return "Fail";
    }
    private static void DisplayResults(string name, double gradeAverage, char gradeLetter, string status)
    {
        WriteLine("-------------------------------------------------------");
        WriteLine($"Student Name: {name}");
        WriteLine($"Final Grade: {gradeAverage:F2}");
        WriteLine($"Letter Grade: {gradeLetter}");
        WriteLine($"Status: {status}");
        WriteLine("-------------------------------------------------------");
    }
}