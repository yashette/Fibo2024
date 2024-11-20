namespace Leonardo;

public record FibonacciResult(int input, int result);

public static class Fibonacci
{
    public static int Run(int i)
    {
        if (i <= 2) 
            return 1;
        
        return Run(i - 2) + Run(i - 1);
    }
    
    public static async Task<List<FibonacciResult>> RunAsync(string[] strings)
    {
        var tasks = new List<Task<FibonacciResult>>();
        foreach (var input in strings)
        {
            var int32 = Convert.ToInt32(input);

            var r = Task.Run(() =>
                {
                    var result = Fibonacci.Run(int32);
                    return new FibonacciResult(int32, result);
                }
            );
        
            tasks.Add(r);
        }
    
        var results = new List<FibonacciResult>();
        foreach (var task in tasks)
        {
            var r = await task;
            results.Add(r);
        }
    
        return results;
    }

}