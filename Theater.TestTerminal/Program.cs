using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
//using Theater.Clients;

namespace Theater.TestTerminal;

class TestAttribute : Attribute
{
    public string TestProp { get; set; }
}

[Test(TestProp = "ttt")]
[Serializable]
class TestKey 
{
    public int TestProp { get; init; }

    public override int GetHashCode()
    {
        return TestProp;
    }

    public override bool Equals(object obj)
    {
        Console.WriteLine("3123");
        if (ReferenceEquals(obj, this)) return true;
        if (ReferenceEquals(obj, null)) return false;
        if (ReferenceEquals(this, null)) return false;
        if (this.GetType() != obj.GetType()) return false;

        return ((TestKey)obj).TestProp == TestProp;
    }


}

interface IInterface
{
    public void Tetet();
    public void Tetetf();
}


class TestInherit : TestKey
{
    public override bool Equals(object obj)
    {

        return base.Equals(obj);
    }
}

internal sealed class Program
{
    private static string test = "123";

    private static int[] TwoSum(int[] nums, int target)
    {
        if (nums is null || nums.Length < 2)
            return null;

        var numIndices = new Dictionary<int, int>();
   
        for (var i = 0; i < nums.Length; i++)
        {
            var complement = target - nums[i];

            if (numIndices.ContainsKey(complement))
                return new[] { numIndices[complement], i };

            if (!numIndices.ContainsKey(nums[i]))
                numIndices.Add(key: nums[i], value: i);
        }

        return null;
    }

    public static bool IsArrayEmpty(string[] array)
    {
        return array?.All(string.IsNullOrWhiteSpace) ?? true;
    }

    private static async Task Main(string[] args)
    {
        var isEmpty = IsArrayEmpty(null);
        isEmpty = IsArrayEmpty(new []{(string)null});
        isEmpty = IsArrayEmpty(new []{""});
        isEmpty = IsArrayEmpty(new []{"321"});
        var twoSum = TwoSum(nums: new[] { 11, 4, 100, 2,  5, 7, }, target: 18);

        // TODO: Fix Nuget packages for Docker
        var serviceCollection = new ServiceCollection();

        //ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        //var userAccountClient = serviceProvider.GetRequiredService<IUserAccountClient>();

        //await TestUserAccountMethods(userAccountClient);

        var tt = new List<TestKey> { new TestKey { TestProp = 1 } };

        var ttg = new List<TestKey> { new TestKey { TestProp = 1 } };
        var hfds = new ConcurrentDictionary<int, int>();

      
        var testKey = new TestKey();
        var testCastCastToObject = (object)testKey;
        testKey = (TestKey)testCastCastToObject;

        var testObj = new object();
        //testKey = (TestKey)testObj;

        lock (new object())
        {
            //await PrintLenght();
        }

        await PrintLenght().ConfigureAwait(false);

        test = "rfd";

    }
    public static async Task PrintLenght()
    {
        try
        {
            await Task.Delay(1_000);
            await GetPageAsync("http://ya.ru");
            Console.WriteLine("Content length: {0}", 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static async Task GetPageAsync(string url)
    {
        return;
        throw new NotImplementedException();
        var client = new HttpClient();
        var content = await client.GetStringAsync(url);
    }

    private static float GetNumber() => 15.0f;

    //private static void ConfigureServices(IServiceCollection services)
    //{
    //    services.AddTheaterClients(baseAddress: "http://localhost:5000/");
    //}

    //private static async Task TestUserAccountMethods(IUserAccountClient userAccountClient)
    //{
    //    var user = await userAccountClient.GetUserById(Guid.Parse("e1f83d38-56a7-435b-94bd-fe891ed0f03a"));

    //    Console.WriteLine(new string('-', 20));
    //}
}