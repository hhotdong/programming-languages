# Asynchronous programming

## In C#...
- Supports language-level asynchronous programming model, TAP(Task-based Asynchronous Pattern), which makes easier to write asynchronous code. TAP is one of several ways of writing asynchronous code in C#.

## When needed?
- I/O-bound work: Requesting data from a network, accessing a database, or reading and writing to a file system.
- CPU-bound work: Performing an expensive calculation.

## How to use?
- I/O-bound work: Await an operation that returns a Task or Task<T> inside of an async method.
- CPU-bound work: Await an operation that is started on a background thread with the Task.Run method.

## async keyword
- Turns a method into an async method, which allows you to use the await keyword in its body.
- On the C# side of things, the compiler transforms your code into a state machine that keeps track of things like yielding execution when an await is reached and resuming execution when a background job has finished.

## await keyword
- Yields control to the caller of the method that performed await and suspends the calling method until the awaited task is complete.
- When used with Task.Run() method, that work is done on background thread.
- Can only be used inside an async method.

## Return value
- Task, for an async method that performs an operation but returns no value. That is, a call to the method returns a Task, but when the Task is completed, any await expression that's awaiting the Task evaluates to void.
- Task<TResult>, for an async method that returns a value.
- void, for an event handler. It's  generally discouraged for code other than event handlers because callers cannot await those methods and must implement a different mechanism to report successful completion or error conditions.
- Any type that has an accessible GetAwaiter method. The object returned by the GetAwaiter method must implement the System.Runtime.CompilerServices.ICriticalNotifyCompletion interface.
- IAsyncEnumerable<T>, for an async method that returns an async stream.
- Consider using ValueTask where possible in order to avoid the extra allocations. The System.Threading.Tasks.ValueTask<TResult> type is one such implementation, which is a lightweight implementation of a generalized task-returning value. It is available by adding the NuGet package System.Threading.Tasks.Extensions.
- Async method can't declare any in, ref or out parameters, nor can it have a reference return value, but it can call methods that have such parameters.
- The Result property of Task<TResult> is a blocking property. If you try to access it before its task is finished, the thread that's currently active is blocked until the task completes and the value is available. In most cases, you should access the value by using await instead of accessing the property directly.
- The caller of a void-returning async method can't catch exceptions thrown from the method. Such unhandled exceptions are likely to cause your application to fail. If a method that returns a Task or Task<TResult> throws an exception, the exception is stored in the returned task. The exception is rethrown when the task is awaited. Make sure that any async method that can produce an exception has a return type of Task or Task<TResult> and that calls to the method are awaited.

## Note
- Check if CPU-bound work is costly enough compared with the overhead of context switches when multithreading.
- Because LINQ uses deferred (lazy) execution, async calls won't happen immediately as they do in a foreach loop unless you force the generated sequence to iterate with a call to .ToList() or .ToArray().

# References
- https://learn.microsoft.com/en-us/dotnet/csharp/async
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async?source=recommendations
- https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/async-return-types
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-7.0/task-types
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/general#asyncmethodbuilder-attribute
- https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/start-multiple-async-tasks-and-process-them-as-they-complete?source=recommendations&pivots=dotnet-7-0

# Read more
- https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
- https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/task-asynchronous-programming-model
- https://devblogs.microsoft.com/dotnet/configureawait-faq/
- https://en.wikipedia.org/wiki/Referential_transparency
- https://en.wikipedia.org/wiki/Futures_and_promises
- https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
