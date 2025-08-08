using Microsoft.Data.SqlClient;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ShoppingCart.Models
{
    /// <summary>
    /// データベース接続とトランザクション管理を行うクラス。
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        /// <summary>
        /// 内部で使用する共有SQL接続。
        /// </summary>
        internal static SqlConnection? _con;

        /// <summary>
        /// 現在のトランザクションマネージャ。
        /// </summary>
        internal static ITransactionManager? _trn;

        /// <summary>
        /// 指定された接続文字列で新しい <see cref="ConnectionManager"/> を初期化し、接続をオープンします。
        /// </summary>
        /// <param name="connectionStringName">SQL Serverへの接続文字列。</param>
        /// <exception cref="InvalidOperationException">接続文字列が未設定の場合。</exception>
        public ConnectionManager(string connectionStringName)
        {
            var connectionString 
                = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString 
                ?? 
                throw new InvalidOperationException("Connection must be set.");
            _con = new SqlConnection(connectionString);
            _con.Open();
        }

        /// <summary>
        /// 新しいトランザクションを開始します。
        /// </summary>
        /// <returns>開始されたトランザクションを管理する <see cref="ITransactionManager"/> のインスタンス。</returns>
        public ITransactionManager BeginTransaction()
        {
            _trn = new TransactionManager(_con!.BeginTransaction());
            return _trn;
        }

        /// <summary>
        /// 接続を破棄します。
        /// </summary>
        public void Dispose()
        {
            _con?.Dispose();
        }

        /// <summary>
        /// SQLトランザクションの管理を行う内部クラス。
        /// </summary>
        private class TransactionManager : ITransactionManager
        {
            /// <summary>
            /// 内部SQLトランザクション。
            /// </summary>
            public SqlTransaction trn;

            /// <summary>
            /// トランザクションを指定して新しい <see cref="TransactionManager"/> を初期化します。
            /// </summary>
            /// <param name="_sqlTransaction">制御対象の <see cref="SqlTransaction"/>。</param>
            public TransactionManager(SqlTransaction _sqlTransaction)
            {
                this.trn = _sqlTransaction;
            }

            /// <inheritdoc/>
            public void Commit()
            {
                trn?.Commit();
            }

            /// <inheritdoc/>
            public void Rollback()
            {
                trn?.Rollback();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                trn?.Dispose();
            }

            /// <inheritdoc/>
            public SqlTransaction GetSqlTransaction()
            {
                return trn;
            }
        }
    }

    /// <summary>
    /// トランザクション操作のインターフェース。
    /// </summary>
    public interface ITransactionManager : IDisposable
    {

        /// <summary>
        /// 内部の <see cref="SqlTransaction"/> を取得します。
        /// </summary>
        /// <returns>制御対象の <see cref="SqlTransaction"/>。</returns>
        SqlTransaction GetSqlTransaction();

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        void Commit();

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        void Rollback();

        /// <summary>
        /// トランザクションを破棄します。
        /// </summary>
        new void Dispose();
    }

    /// <summary>
    /// SQLコマンドを構築するためのビルダークラス。
    /// チェーン構文によりSQL文とパラメータの設定が可能です。
    /// </summary>
    internal class SqlCommandBuilder
    {
        /// <summary>
        /// SQLコマンドを表す <see cref="SqlCommand"/> のインスタンス。
        /// </summary>
        private readonly SqlCommand _com = new();

        /// <summary>
        /// <see cref="SqlCommandBuilder"/> の新しいインスタンスを初期化します。
        /// 内部で <see cref="ConnectionManager._con"/> および <see cref="ConnectionManager._trn"/> を使用します。
        /// </summary>
        public SqlCommandBuilder()
        {
            /// <summary>
            /// DB接続
            /// </summary>
            _com.Connection = ConnectionManager._con;
            /// <summary>
            /// トランザクション
            ///</summary>
            _com.Transaction = ConnectionManager._trn?.GetSqlTransaction();
        }

        /// <summary>
        /// 実行するSQL文を設定します。
        /// </summary>
        /// <param name="commandText">SQLのコマンド文字列。</param>
        /// <returns>このインスタンス。</returns>
        public SqlCommandBuilder WithCommandText(string commandText)
        {
            _com.CommandText = commandText;
            return this;
        }

        /// <summary>
        /// パラメータをコマンドに追加します。
        /// </summary>
        /// <param name="name">パラメータ名。</param>
        /// <param name="value">パラメータの値（null の場合は DBNull）。</param>
        /// <returns>このインスタンス。</returns>
        public SqlCommandBuilder AddParameter(string name, object? value)
        {
            _com.Parameters.AddWithValue(name, value ?? DBNull.Value);
            return this;
        }

        /// <summary>
        /// 構築した <see cref="SqlCommand"/> を取得します。
        /// 接続とコマンドテキストが設定されていない場合は例外をスローします。
        /// </summary>
        /// <returns>構築された <see cref="SqlCommand"/>。</returns>
        /// <exception cref="InvalidOperationException">接続またはコマンドテキストが未設定の場合。</exception>
        public SqlCommand Build()
        {
            if (_com.Connection == null)
                throw new InvalidOperationException("Connection must be set.");
            if (string.IsNullOrWhiteSpace(_com.CommandText))
                throw new InvalidOperationException("CommandText must be set.");
            return _com;
        }

        /// <summary>
        /// 使用している <see cref="SqlCommand"/> を破棄します。
        /// </summary>
        public void Dispose()
        {
            _com.Dispose();
        }
    }

    /// <summary>
    /// SqlDataReaderは値がobject型になるため適切な
    /// 型に型変換するための拡張クラス
    /// </summary>
    public static class SqlDataReaderExtensions
    {

        // SqlDataReaderに列があるか確認
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string? GetNullableString(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        public static int? GetNullableInt(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
        }

        public static long? GetNullableLong(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetInt64(ordinal);
        }

        public static bool? GetNullableBool(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
        }
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
        }
        public static decimal? GetNullableDecimal(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
        }


        public static string GetString(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        public static int GetInt(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }

        public static long GetLong(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0L : reader.GetInt64(ordinal);
        }

        public static bool GetBool(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? false : reader.GetBoolean(ordinal);
        }
        public static DateTime GetDateTime(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? DateTime.Now : reader.GetDateTime(ordinal);
        }
        public static decimal GetDecimal(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0m : reader.GetDecimal(ordinal);
        }
    }
}
