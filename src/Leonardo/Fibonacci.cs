using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public record FibonacciResult(int input, int result);

public class Fibonacci
{
    private readonly FibonacciDataContext _context;

    public Fibonacci(FibonacciDataContext context)
    {
        _context = context;
    }
    
    public int Run(int i)
    {
        if (i <= 2) 
            return 1;
        
        return Run(i - 2) + Run(i - 1);
    }
    
    public async Task<List<FibonacciResult>> RunAsync(string[] strings)
    {
       
        var tasks = new List<Task<FibonacciResult>>();
        foreach (var input in strings)
        {
            var int32 = Convert.ToInt32(input);
            var t_fibo  =  await _context.TFibonaccis.Where(t => t.FibInput == int32).FirstOrDefaultAsync();
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
                    var result = Run(int32);
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
           
           var exists = await _context.TFibonaccis.AnyAsync(t => t.FibInput == r.input);
           if (!exists)
           {
               _context.TFibonaccis.Add(new TFibonacci { FibInput = r.input, FibOutput = r.result });
           }
           
            results.Add(r);
        }
        
        await _context.SaveChangesAsync( );
    
        return results;
    }

}