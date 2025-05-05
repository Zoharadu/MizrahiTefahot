using Common;

namespace Service.Interface
{
    public interface ICalculationService
    {
        Task<double> Calculate(CalculationRequest request, string operatorHeader);
        Task<List<string>> GetSupportedOperations();
    }
}
