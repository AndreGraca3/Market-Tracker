namespace market_tracker_webapi.Application.Service.Core;

public class CategoryManager(int minCategoryNameLength, int maxCategoryNameLength)
{
    public int MinCategoryNameLength { get; } = minCategoryNameLength;
    public int MaxCategoryNameLength { get; } = maxCategoryNameLength;

    public bool IsValidCategoryName(string name)
    {
        return name.Length >= MinCategoryNameLength && name.Length <= MaxCategoryNameLength;
    }
}
