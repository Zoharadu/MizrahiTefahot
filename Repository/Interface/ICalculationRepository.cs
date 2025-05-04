
namespace Repository.Interface
{
    public interface ICalculationRepository
    {
        void SaveCalculation(double num1, double num2, string operation, double result);
    }
}
