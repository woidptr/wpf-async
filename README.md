# Async in C# WPF

## How to work asynchronously?
To work asynchronously in c# we use `async` and `await` keywords. `async` is used to define an asynchronous function, `await` is used to wait for the task without blocking the main thread.

How to code an async function in C#?
```cs
using System;
using System.Threading.Tasks;

class TestClass {
    public static async Task<string> doAsyncWork() {
        return "Hello, World!"
    }
}
```

And here is how to wait for the async task.
```cs
using System;
using System.Threading.Tasks;

class TestClass {
    public static async Task<string> doAsyncWork(string name) {
        string greetings = await TestClass.getGreetings();

        return string.Format(greetings, name);
    }

    public static async Task<string> getGreetings() {
        return "Hello, {0}!"
    }
}
```