using System.Net.NetworkInformation;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;
using static System.Console;
using System.Collections.Generic;
class StudentGradeCalculator
{
    static void Main()
    {
        while (true)
        {
            DisplayWelcomeMessage();
            string studentType = GetStudentType();

            string fullName = GetStudentName();
            string[] nameParts = fullName.Split(' ');
            string firstName = nameParts[0];
            string lastName = nameParts[^1]; // get last item of list instead of second, in case user entered a middle name


            Student student;

            if (studentType == "U")
                student = new UndergradStudent(firstName, lastName);
            else
                student = new GradStudent(firstName, lastName);

            GetAssignments(student);
            DisplayResults(student);

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

    private static string GetStudentType()
    {
        while (true)
        {
            Write("Enter student type (U for Undergrad, G for Grad) >> ");
            string input = ReadLine().ToUpper().Trim();

            if (input == "U" || input == "G")
            {
                return input;
            }

            WriteLine("Invalid input. Please enter U or G.");
        }
    }

    private static string GetStudentName()
    {
        while (true)
        {
            Write("Enter student First and Last name >> ");
            string name = ReadLine().Trim();

            if (!string.IsNullOrWhiteSpace(name) && name.Contains(" "))
                return name;
            
            WriteLine("You must enter both first and last name separated by a space. Please re-enter.");
        }
    }

    private static void GetAssignments(Student student)
    {
        while (true)
        {
            Write("Enter assignment name (or press Enter to finish) >> ");
            string name = ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(name))
                break;

            Write("Enter a Score (0-100) >> ");
            string input = ReadLine().Trim();

            double score;
            if (double.TryParse(input, out score) && score >= 0 && score <= 100)
            {
                Assignment newAssignment = new Assignment(name, score);
                student.AddAssignment(newAssignment);
            }
            else
            {
                WriteLine("Invalid score, must be between 0 and 100. Please re-enter.");
            }
        }
    }

    private static void DisplayResults(Student student)
    {
        WriteLine("-------------------------------------------------------");
        WriteLine($"Student Name: {student.FirstName + " " + student.LastName}");
        WriteLine();
        WriteLine("Assignments:");

        foreach (Assignment assignment in student.Assignments)
        {
            WriteLine($"- {assignment.Name}: {assignment.Score:F2}");
        }

        WriteLine();
        WriteLine($"Final Grade: {student.CalculateAverage():F2}");
        WriteLine($"Letter Grade: {student.DetermineLetterGrade()}");
        WriteLine($"Status: {student.DetermineStatus()}");
        WriteLine("-------------------------------------------------------");
    }
}

class Assignment
{
    public string Name { get; set; }
    public double Score { get; set; }

    public Assignment(string name, double score)
    {
        Name = name;
        Score = score;
    }
}

class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Assignment> Assignments { get; set; }

    public Student(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Assignments = new List<Assignment>();
    }

    public void AddAssignment(Assignment assignment)
    {
        Assignments.Add(assignment);
    }

    public double CalculateAverage()
    {
        double total = 0;

        foreach (Assignment assignment in Assignments)
            total += assignment.Score;

        double average = total / Assignments.Count;
        return average;
    }

    public char DetermineLetterGrade()
    {
        double average = CalculateAverage();

        switch (average)
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

    public virtual string DetermineStatus()
    {
        return "Unknown";
    }
}

class UndergradStudent : Student
{
    public UndergradStudent(string firstName, string lastName) : base(firstName, lastName) { }

    public override string DetermineStatus()
    {
        if (CalculateAverage() >= 60)
            return "Pass";
        return "Fail";
    }
}

class GradStudent : Student
{
    public GradStudent(string firstName, string lastName) : base(firstName, lastName) { }

    public override string DetermineStatus()
    {
        if (CalculateAverage() >= 80)
            return "Pass";
        return "Fail";
    }
}