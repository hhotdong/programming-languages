// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings
using System;

// sorting or comparing strings isn't always a culture-sensitive operation.
// For example, strings that are used internally by an application typically should be handled identically across all cultures.
// When culturally independent string data, such as XML tags, HTML tags, user names, file paths, and the names of system objects,
// are interpreted as if they were culture-sensitive, application code can be subject to subtle bugs, poor performance, and, in some cases,
// security issues.



namespace String
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Use overloads that explicitly specify the string comparison rules for string operations.
            // Typically, this involves calling a method overload that has a parameter of type StringComparison.


            Console.WriteLine("Hello World!");
        }
    }
}
