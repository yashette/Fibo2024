using Microsoft.EntityFrameworkCore;

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
        await using var context = new FibonacciDataContext();
        
        var tasks = new List<Task<FibonacciResult>>();
        foreach (var input in strings)
        {
            var int32 = Convert.ToInt32(input);
            var t_fibo  =  await context.TFibonaccis.Where(t => t.FibInput == int32).FirstOrDefaultAsync();
            if(t_fibo != null)
            {
                var t = Task.Run(() =>
                {
                    return new FibonacciResult(t_fibo.FibInput, (int)t_fibo.FibOutput);
                    
                });
                
                tasks.Add(t);

            }
            else 
            {
                var f = Task.Run(() =>
                {
                    var result = Fibonacci.Run(int32);
                    return new FibonacciResult(int32, result);
                });

                tasks.Add(f);
            }

         /*   var r = Task.Run(() =>
                {
                    var result = Fibonacci.Run(int32);
                    return new FibonacciResult(int32, result);
                }
            );
        
            tasks.Add(r);*/
        }
    
        var results = new List<FibonacciResult>();
        foreach (var task in tasks)
        {
            var r = await task;
           
           var exists = await context.TFibonaccis.AnyAsync(t => t.FibInput == r.input);
           if (!exists)
           {
               context.TFibonaccis.Add(new TFibonacci { FibInput = r.input, FibOutput = r.result });
           }
           
            results.Add(r);
        }
        
        await context.SaveChangesAsync( );
    
        return results;
    }

}