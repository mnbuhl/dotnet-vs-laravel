﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Models;

public class Order : BaseEntity
{
    public long Total { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public User? User { get; set; }

    [ForeignKey(nameof(BillingAddress))]
    public Guid BillingAddressId { get; set; }

    public Address? BillingAddress { get; set; }

    [ForeignKey(nameof(ShippingDetails))]
    public Guid ShippingDetailsId { get; set; }

    public ShippingDetails? ShippingDetails { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    public void CalculateTotal()
    {
        if (OrderLines.Count <= 0)
        {
            return;
        }

        foreach (var line in OrderLines)
        {
            Total += line.Price * (100 - line.Discount) / 100 * line.Quantity;
        }
    }
}