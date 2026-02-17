using Microsoft.AspNetCore.SignalR;

public class ProductHub : Hub
{
    public static IHubContext<ProductHub>? Context { get; set; }

    public static void NotifyProductsChanged()
    {
        Context?.Clients.All.SendAsync("productsChanged");
    }
}
