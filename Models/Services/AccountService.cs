using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Entities;

namespace ShoppingCart.Models.Services
{
    /// <summary>
    /// アカウントに関するユースケースを提供するサービスクラス。
    /// </summary>
    /// <remarks>
    /// 本クラスは顧客の認証（ログイン）処理を担当する。
    /// 処理そのものは Action クラスに委譲し、トランザクション管理は <see cref="DatabaseFramework"/> に一任する。
    ///
    /// 【責務】
    /// ・ユースケース単位での処理の組み立て
    /// ・Action の生成および実行
    ///
    /// 【前提】
    /// ・各DAOはDIコンテナにより注入されていること
    /// </remarks>
    public class AccountService(DatabaseFramework dbFramework, CustomerDao dao)
    {
        /// <summary>
        /// 顧客IDとパスワードを指定してログイン認証を行う。
        /// </summary>
        /// <param name="customerId">顧客ID（null・空文字不可）</param>
        /// <param name="password">パスワード（null・空文字不可）</param>
        /// <returns>認証に成功した場合は顧客情報。該当データが存在しない場合、またはパスワードが一致しない場合は <c>null</c></returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="CustomerReadAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行</item>
        /// </list>
        ///
        /// 【用途】
        /// ・ログイン画面での認証処理
        ///
        /// 【設計方針】
        /// ・認証失敗（顧客未存在・パスワード不一致）は例外ではなく null で表現する
        /// </remarks>
        public CustomerEntity? Login(string customerId, string password)
        {
            return dbFramework.Execute(new CustomerReadAction(customerId, password, dao));
        }
    }
}