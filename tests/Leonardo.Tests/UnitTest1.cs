namespace Leonardo.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var results = await Fibonacci.RunAsync(new string[] { "1", "2", "3" });
        Assert.Equal(1, results[0].input);

    }
}