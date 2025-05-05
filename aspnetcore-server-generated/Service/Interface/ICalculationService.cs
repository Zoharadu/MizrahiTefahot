using Common;

namespace Service.Interface
{
    // Defines the contract for arithmetic calculation operations
    public interface ICalculationService
    {
        // Performs a calculation based on the given request and operator
        Task<double> Calculate(CalculationRequest request, string operatorHeader);

        /// Returns a list of supported arithmetic operations
        Task<List<string>> GetSupportedOperations();
    }
}
