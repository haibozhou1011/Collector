namespace Collector.Models
{
    public struct ParseProductPatterns
    {
        public static string ProductNamePattern = "(?<=<h1 class=\"product-name\" itemprop=\"name\">).*?(?=</h1>)";
        public static string ProductJsnPattern = @"(?<=var skuProducts=).*?(?=;\s*var skuAttrIds=)";
        public static string ProductImageJsonPattern = "(?<=window.runParams.imageBigViewURL=).*?(?=;)";
        public static string ProductCurrencyPattern = "(?<=window.runParams.currencyCode=\").*?(?=\";)";
        public static string ProductColorPattern =
            "(?<=<a data-role=\"sku\" data-sku-id=\"{0}\" id=\"sku-1-{0}\" title=\").*?(?=\")";
        public static string ProductSizePattern =
            "(?<=<a data-role=\"sku\" data-sku-id=\"{0}\" id=\"sku-2-{0}\" href=\"javascript:void\\(0\\)\"\\s+><span>).*?(?=</)";
    }
}