using DBManager;

namespace ShoppingCart.Models.DatabaseFrameworks
{
    /// <summary>
    /// DB処理を提供するフレームワーククラス。
    /// </summary>
    public class DatabaseFramework(ConnectionManager connectionManager)
    {
        /// <summary>
        /// 読み取り専用の処理を実行する。
        /// 呼び出し側はIReadAction&lt;T&gt; を実装したクラスを渡して処理を記述する。
        /// </summary>
        /// <typeparam name="T">処理の戻り値の型</typeparam>
        /// <param name="action">読み取り処理を提供するクラス</param>
        /// <returns>処理結果</returns>
        /// <exception cref="DatabaseServiceException">処理中に例外が発生した場合</exception>
        public T? Execute<T>(IReadAction<T> action)
        {
            try
            {
                return action.ExecuteQuery();
            }
            catch (InvalidOperationException ex)
            {
                var logger = new ExceptionLogger("logs");
                logger.LogException(ex);
                throw new DatabaseServiceException("データベースの接続でエラーが発生しました。", ex);
            }
            catch (Exception ex)
            {
                var logger = new ExceptionLogger("logs");
                logger.LogException(ex);
                throw new DatabaseServiceException("DB読み取り処理でエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// 書き込み処理をトランザクション内で実行する。
        /// 呼び出し側は IWriteAction&lt;T&gt; を実装したクラスを渡す。
        /// </summary>
        /// <typeparam name="T">処理の戻り値の型</typeparam>
        /// <param name="action">書き込み処理を提供するクラス</param>
        /// <returns>処理結果</returns>
        /// <exception cref="DatabaseServiceException">トランザクション外のエラーが発生した場合</exception>
        public T? Execute<T>(IWriteAction<T> action)
        {
            try
            {
                using var transaction = connectionManager!.BeginTransaction();
                try
                {
                    var result = action.ExecuteNonQuery();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    var logger = new ExceptionLogger("logs");
                    logger.LogException(ex);
                    transaction.Rollback();
                    throw new Exception();
                }
            }
            catch (InvalidOperationException ex)
            {
                var logger = new ExceptionLogger("logs");
                logger.LogException(ex);
                throw new DatabaseServiceException("データベースの接続でエラーが発生しました。", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseServiceException("DB書き込み処理でエラーが発生しました。", ex);
            }
        }
    }
}
