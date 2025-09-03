namespace ShoppingCart.Models.Exceptions
{
    /// <summary>
    /// 在庫エラーを表す例外クラス。
    /// </summary>
    public class StockException : Exception
    {
        public StockException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
        public override string ToString()
        {
            var baseStr = base.ToString();

            if (InnerException != null)
            {
                return $"{baseStr}\n\n{InnerException.StackTrace}";
            }
            return baseStr;
        }
    }
}
