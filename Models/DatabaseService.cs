using DBManager;

namespace ShoppingCart.Models
{
    /// <summary>
    /// データベース操作を提供するサービスクラス。
    /// 読み取り処理はラムダ式で実装し、
    /// 書き込み処理はクラス（インターフェース実装）で実装する。
    /// </summary>
    public class DatabaseService(ConnectionManager connectionManager)
    {
        /// <summary>
        /// 読み取り専用の処理を実行する。
        /// 呼び出し側はラムダ式を渡して処理を記述する。
        /// </summary>
        /// <typeparam name="T">処理の戻り値の型</typeparam>
        /// <param name="action">読み取り処理を行うラムダ式</param>
        /// <returns>処理結果</returns>
        /// <exception cref="DatabaseServiceException">処理中に例外が発生した場合</exception>
        public T? Read<T>(Func<T> action)
        {
            try
            {
                return action();
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
        /// <exception cref="InvalidOperationException">トランザクション内でエラーが発生した場合</exception>
        public T? Write<T>(IWriteAction<T> action)
        {
            try
            {
                using var transaction = connectionManager!.BeginTransaction();
                try
                {
                    var result = action.Execute();
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
            catch (Exception ex)
            {
                throw new DatabaseServiceException("DB書き込み処理でエラーが発生しました。", ex);
            }
        }
    }

    /// <summary>
    /// 書き込み処理を定義するインターフェース。
    /// トランザクション内で実行される。
    /// </summary>
    /// <typeparam name="T">処理結果の型</typeparam>
    public interface IWriteAction<T>
    {
        /// <summary>
        /// 書き込み処理の本体。
        /// </summary>
        /// <returns>処理結果</returns>
        T? Execute();
    }

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
