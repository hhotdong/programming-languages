// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords
// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/versioning-with-the-override-and-new-keywords
using System;

namespace NewAndOverride
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Test1();
            Test2();
        }

        public static void Test1()
        {
            Grandchild grandChild = new Grandchild();
            grandChild          .Foo();
            ((Child)grandChild) .Foo();
            ((Parent)grandChild).Foo();

            Console.WriteLine("-------------");

            grandChild          .Bar();
            ((Child)grandChild) .Bar();
            ((Parent)grandChild).Bar();

            Console.WriteLine("-------------");

            grandChild.Func();

            Console.WriteLine("-------------");

            var child = new Child();
            child.Func();

            /// Output
            // Grandchild Foo
            // Grandchild Foo
            // Grandchild Foo
            // -------------
            // Grandchild Bar
            // Child Bar
            // Parent Bar
            // -------------
            // Parent Func
            // Grandchild Foo
            // Parent Bar
            // -------------
            // Parent Func
            // Child Foo
            // Parent Bar
        }

        public static void Test2()
        {
            Console.WriteLine("-------------");
            Base    baseObj    = new Base();
            Derived derivedObj = new Derived();
            baseObj           .DoWork(1);
            derivedObj        .DoWork(1);
            ((Base)derivedObj).DoWork(1);

            /// Output
            // -------------
            // Base DoWork(int)
            // Derived DoWork(double)
            // Derived DoWork(int)
        }
    }

    public class Parent
    {
        public virtual void Foo()
        {
            Console.WriteLine("Parent Foo");
        }

        public void Bar()
        {
            Console.WriteLine("Parent Bar");
        }

        public void Func()
        {
            Console.WriteLine("Parent Func");
            Foo();
            Bar();
        }
    }

    public class Child : Parent
    {
        public override void Foo()
        {
            Console.WriteLine("Child Foo");
        }

        public new void Bar()
        {
            Console.WriteLine("Child Bar");
        }
    }

    public class Grandchild : Child
    {
        public override void Foo()
        {
            Console.WriteLine("Grandchild Foo");
        }

        public new void Bar()
        {
            Console.WriteLine("Grandchild Bar");
        }
    }

    public class Base
    {
        public virtual void DoWork(int num)
        {
            Console.WriteLine("Base DoWork(int)");
        }
    }

    public class Derived : Base
    {
        public override void DoWork(int num)
        {
            Console.WriteLine("Derived DoWork(int)");
        }

        public void DoWork(double num)
        {
            Console.WriteLine("Derived DoWork(double)");
        }
    }
}
