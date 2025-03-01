﻿using Orders.Domain.Models;

namespace Orders.Application.Specifications.Users;

public class UserByEmailWithOrdersSpec : BaseSpecification<User>
{
    public UserByEmailWithOrdersSpec(string email)
        : base(u => u.Email == email)
    {
        AddInclude(u => u.Orders);
    }
}