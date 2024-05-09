namespace Tutorial6.Models.DTOs;

using System;

using System;

public class ProductWarehouseReq
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public int IdOrder { get; set; }
    public int Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

