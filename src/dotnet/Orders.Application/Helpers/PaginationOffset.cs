﻿namespace Orders.Application.Helpers;

public static class PaginationOffset
{
    public static (int, int) Calculate(int page, int index)
    {
        return (page * (index - 1), page);
    }
}