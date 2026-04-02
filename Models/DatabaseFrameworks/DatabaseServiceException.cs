namespace ShoppingCart.Models.DatabaseFrameworks
{
    /// <summary>
    /// データベースサービスの例外クラス。
    /// </summary>
    public class DatabaseServiceException : Exception
    {
        public DatabaseServiceException(string message, Exception? innerException = null)
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
