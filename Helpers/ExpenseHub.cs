using System;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_project2.Helpers;

public class ExpenseHub : Hub
{
    public async Task NotifyExpenseAdded()
    {
        await Clients.All.SendAsync("ExpenseAdded");
    }

}
