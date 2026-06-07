namespace PROG7311POEAPI.Services
{
    public interface ICurrency
    {
        Task<decimal> ConvertToZAR(string fromCurrency, decimal amount);
    }
}