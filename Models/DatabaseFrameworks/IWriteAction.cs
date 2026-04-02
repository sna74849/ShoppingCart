namespace ShoppingCart.Models.DatabaseFrameworks
{
    /// <summary>
    /// トランザクション処理を定義するインターフェース。
    /// トランザクション内で実行される。
    /// </summary>
    /// <typeparam name="T">処理結果の型</typeparam>
    public interface IWriteAction<T>
    {

        /// <summary>
        /// トランザクション処理の本体。
        /// </summary>
        /// <returns>処理結果</returns>
        T? ExecuteNonQuery() { throw new NotImplementedException(); }
    }
}
