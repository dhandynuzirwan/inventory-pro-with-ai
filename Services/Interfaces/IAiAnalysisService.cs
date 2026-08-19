using System.Threading.Tasks;

namespace InventorySystem.Services;

public interface IAiAnalysisService
{
    Task<string> GenerateAnalysisAsync();
}