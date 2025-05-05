using Common;
using Service.Interface;

namespace Service.ImpI
{
    // Service responsible for performing arithmetic operations
    public class CalculationService : ICalculationService
    {
        // Initializes supported arithmetic operations
        private static Dictionary<string, Func<double, double, double>> InitializeOperations() =>
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "add", (a, b) => a + b },
                { "subtract", (a, b) => a - b },
                { "multiply", (a, b) => a * b },
                { "divide", (a, b) =>
                    {
                        if (b == 0) throw new DivideByZeroException();
                        return a / b;
                    }
                }
            };

        private readonly Dictionary<string, Func<double, double, double>> _operations = InitializeOperations();

        // Performs the requested arithmetic operation
        public Task<double> Calculate(CalculationRequest request, string operatorHeader)
        {
            if (!_operations.TryGetValue(operatorHeader, out var operation))
                throw new ArgumentException($"Unsupported operation: {operatorHeader}");

            double result = operation(request.Number1, request.Number2);
            return Task.FromResult(result);
        }

        // Returns a list of supported operation names.
        public Task<List<string>> GetSupportedOperations()
        {
            return Task.FromResult(_operations.Keys.ToList());
        }
    }
}
