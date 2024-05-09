namespace Tutorial6.Models;

public class Order
{
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public int Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
}