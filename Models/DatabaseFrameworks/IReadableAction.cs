namespace ShoppingCart.Models.DatabaseFrameworks
{
    /// <summary>
    /// 問い合わせ処理を定義するインターフェース。
    /// </summary>
    /// <typeparam name="T">処理結果の型</typeparam>
    public interface IReadAction<T>
    {

        /// <summary>
        /// 問い合わせ処理の本体。
        /// </summary>
        /// <returns>処理結果</returns>
        T? ExecuteQuery() { throw new NotImplementedException(); }
    }
}
