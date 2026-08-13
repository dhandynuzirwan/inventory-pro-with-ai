using Google.GenAI;
using InventorySystem.Models;

namespace InventorySystem.Services;

public class AiAnalysisService
{
    private readonly DashboardService _dashboardService;
    private readonly ProductService _productService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiAnalysisService> _logger;

    public AiAnalysisService(
        DashboardService dashboardService,
        ProductService productService,
        IConfiguration configuration,
        ILogger<AiAnalysisService> logger)
    {
        _dashboardService = dashboardService;
        _productService = productService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateAnalysisAsync()
    {
        try
        {
            // Gather data
            var totalProducts = await _dashboardService.GetTotalProductsAsync();
            var lowStockCount = await _dashboardService.GetLowStockCountAsync();
            var totalCategories = await _dashboardService.GetTotalCategoriesAsync();
            var totalValue = await _dashboardService.GetTotalInventoryValueAsync();
            var stockByCategory = await _dashboardService.GetStockByCategoryAsync();
            var lowStockProducts = await _productService.GetLowStockProductsAsync();
            var recentTransactions = await _dashboardService.GetRecentTransactionsAsync(20);

            // Build context prompt
            var prompt = BuildPrompt(totalProducts, lowStockCount, totalCategories, totalValue,
                stockByCategory, lowStockProducts, recentTransactions);

            // Call Gemini API
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return GenerateFallbackAnalysis(totalProducts, lowStockCount, totalCategories, totalValue,
                    lowStockProducts);
            }

            var client = new Client(apiKey: apiKey);
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.0-flash",
                contents: prompt
            );

            var result = response.Candidates?[0]?.Content?.Parts?[0]?.Text;
            return result ?? "Tidak dapat menghasilkan analisis saat ini.";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Gagal memanggil Gemini API, menggunakan fallback analysis");
            // Fallback to rule-based
            var totalProducts = await _dashboardService.GetTotalProductsAsync();
            var lowStockCount = await _dashboardService.GetLowStockCountAsync();
            var totalCategories = await _dashboardService.GetTotalCategoriesAsync();
            var totalValue = await _dashboardService.GetTotalInventoryValueAsync();
            var lowStockProducts = await _productService.GetLowStockProductsAsync();

            return GenerateFallbackAnalysis(totalProducts, lowStockCount, totalCategories, totalValue,
                lowStockProducts);
        }
    }

    private string BuildPrompt(int totalProducts, int lowStockCount, int totalCategories,
        decimal totalValue, List<CategoryStockData> stockByCategory,
        List<Product> lowStockProducts, List<StockTransaction> recentTransactions)
    {
        var categoryInfo = string.Join("\n", stockByCategory.Select(c =>
            $"  - {c.CategoryName}: {c.TotalStock} unit ({c.ProductCount} produk)"));

        var lowStockInfo = string.Join("\n", lowStockProducts.Select(p =>
            $"  - {p.Name} (SKU: {p.SKU}): stok {p.Stock}, minimum {p.MinimumStock}, harga Rp {p.Price:N0}"));

        var txIn = recentTransactions.Where(t => t.Type == TransactionType.In).Sum(t => t.Quantity);
        var txOut = recentTransactions.Where(t => t.Type == TransactionType.Out).Sum(t => t.Quantity);

        return $"""
            Kamu adalah asisten analisis inventory yang cerdas. Berikan ringkasan analisis dalam Bahasa Indonesia berdasarkan data berikut:

            DATA INVENTORY:
            - Total produk: {totalProducts}
            - Total kategori: {totalCategories}
            - Barang stok rendah: {lowStockCount}
            - Nilai total inventory: Rp {totalValue:N0}

            STOK PER KATEGORI:
            {categoryInfo}

            BARANG STOK RENDAH (di bawah minimum):
            {lowStockInfo}

            TRANSAKSI TERAKHIR:
            - Total barang masuk: {txIn} unit
            - Total barang keluar: {txOut} unit

            INSTRUKSI:
            1. Buat ringkasan singkat kondisi inventory (2-3 kalimat)
            2. Highlight barang yang paling kritis perlu restok (sebutkan nama dan angkanya)
            3. Berikan 2-3 rekomendasi aksi yang harus dilakukan
            4. Gunakan emoji untuk memperjelas poin
            5. Format output dalam plain text (bukan markdown), singkat dan padat, maksimal 200 kata
            """;
    }

    private string GenerateFallbackAnalysis(int totalProducts, int lowStockCount,
        int totalCategories, decimal totalValue, List<Product> lowStockProducts)
    {
        var lines = new List<string>
        {
            $"📦 Inventory saat ini memiliki {totalProducts} produk di {totalCategories} kategori dengan nilai total Rp {totalValue:N0}."
        };

        if (lowStockCount > 0)
        {
            lines.Add($"\n⚠️ Perhatian: {lowStockCount} barang berada di bawah stok minimum dan perlu segera di-restok:");
            foreach (var p in lowStockProducts.Take(3))
            {
                var ratio = p.MinimumStock > 0 ? (p.Stock * 100 / p.MinimumStock) : 0;
                lines.Add($"  • {p.Name} — stok {p.Stock}/{p.MinimumStock} ({ratio}%)");
            }
            if (lowStockProducts.Count > 3)
                lines.Add($"  • ...dan {lowStockProducts.Count - 3} barang lainnya");
        }
        else
        {
            lines.Add("\n✅ Semua barang memiliki stok yang cukup. Tidak ada yang perlu di-restok saat ini.");
        }

        lines.Add("\n💡 Rekomendasi: Prioritaskan restok barang dengan rasio stok/minimum terendah.");

        return string.Join("\n", lines);
    }
}
