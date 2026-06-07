namespace Prog7311PoeTwo.Services
{
    public interface ICurrency
    {
        Task<decimal> ConvertToZAR(string fromCurrency, decimal amount);
    }
}