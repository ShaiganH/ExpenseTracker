using System;

namespace dotnet_project2.Helpers;

public class ExpenseQueryObject
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
