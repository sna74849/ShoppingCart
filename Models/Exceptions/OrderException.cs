namespace ShoppingCart.Models.Exceptions
{
    /// <summary>
    /// 注文エラーを表す例外クラス。
    /// </summary>
    public class OrderException(string message, Exception? innerException = null) : Exception(message, innerException)
    {
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
