namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public enum ProductUnit
{
    Units,
    Kilograms,
    Grams,
    Liters,
    Milliliters
}

public static class ProductUnitExtensions
{
    public static string GetUnitName(this ProductUnit unit)
    {
        return unit switch
        {
            ProductUnit.Units => "unidades",
            ProductUnit.Kilograms => "kilogramas",
            ProductUnit.Grams => "gramas",
            ProductUnit.Liters => "litros",
            ProductUnit.Milliliters => "millilitros",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    public static string GetBaseUnitName(this ProductUnit unit)
    {
        return unit switch
        {
            ProductUnit.Units => "uni",
            ProductUnit.Kilograms => "kg",
            ProductUnit.Grams => "gr",
            ProductUnit.Liters => "L",
            ProductUnit.Milliliters => "ml",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    public static ProductUnit GetUnitFromName(this string name)
    {
        return name switch
        {
            "unidades" => ProductUnit.Units,
            "kilogramas" => ProductUnit.Kilograms,
            "gramas" => ProductUnit.Grams,
            "litros" => ProductUnit.Liters,
            "millilitros" => ProductUnit.Milliliters,
            _ => throw new ArgumentException($"Invalid argument: {name}")
        };
    }
}