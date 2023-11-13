using Newtonsoft.Json;

namespace Theater.SignalR.TerminalChat;

public static class HttpContentSerializerExtensions
{
    /// <summary>
    /// Десериализует ответ через Newtonsoft.Json вместо стандартного
    /// </summary>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> DeserializeContent<T>(this HttpContent content)
    {
        var str = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(str);
    }
}