using Common;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ImpI
{
    public class CalculationService : ICalculationService
    {
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

        public Task<double> Calculate(CalculationRequest request, string operatorHeader)
        {
            if (!_operations.TryGetValue(operatorHeader, out var operation))
            {
                throw new ArgumentException($"Unsupported operation: {operatorHeader}");
            }

            double result = operation(request.Number1, request.Number2);

            return Task.FromResult(result);
        }

        public async Task<List<string>> GetSupportedOperations()
        {
            return await Task.FromResult(_operations.Keys.ToList());
        }
    }
}
