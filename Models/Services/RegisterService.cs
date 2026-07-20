using ShoppingCart.Models.Actions;
using ShoppingCart.Models.Daos;
using ShoppingCart.Models.DatabaseFrameworks;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Entities;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.Services
{
    /// <summary>
    /// 注文登録画面（配送先入力）に関するユースケースを提供するサービスクラス。
    /// </summary>
    /// <remarks>
    /// 本クラスは配送先情報の取得、およびカート内商品の在庫確認処理を担当する。
    /// 各処理は Action クラスに委譲し、トランザクション管理は <see cref="DatabaseFramework"/> に一任する。
    ///
    /// 【責務】
    /// ・ユースケース単位での処理の組み立て
    /// ・Action の生成および実行
    ///
    /// 【前提】
    /// ・各DAOはDIコンテナにより注入されていること
    /// </remarks>
    public class RegisterService(DatabaseFramework dbFramework, DestinationDao destinationDao, ItemSalesStockDao itemSalesStockDao)
    {
        /// <summary>
        /// 顧客IDを指定して配送先一覧を取得する。
        /// </summary>
        /// <param name="customerId">顧客ID（null・空文字不可）</param>
        /// <returns>配送先情報の一覧。該当データが存在しない場合は <c>null</c></returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><see cref="DestinationReadAction"/> を生成</item>
        /// <item><see cref="DatabaseFramework"/> を介して実行</item>
        /// </list>
        ///
        /// 【用途】
        /// ・注文登録画面での配送先選択
        ///
        /// 【設計方針】
        /// ・データ未存在は例外ではなく null で表現する
        /// </remarks>
        public List<DestinationEntity>? GetDestinationList(string customerId)
        {
            return dbFramework.Execute(new DestinationReadAction(customerId, destinationDao));
        }

        /// <summary>
        /// カート内商品の在庫・販売情報を取得し、全商品が在庫数を満たしているかを判定する。
        /// </summary>
        /// <param name="cartItemVmList">カート内の商品一覧（null不可）</param>
        /// <param name="itemSalesStockDtoList">
        /// 出力パラメータ。<paramref name="cartItemVmList"/> に対応する商品の在庫・販売情報一覧。
        /// </param>
        /// <returns>
        /// カート内の全商品が要求数量以上の在庫を保持している場合は <c>true</c>、
        /// 1件でも在庫数を下回る商品がある場合は <c>false</c>
        /// </returns>
        /// <remarks>
        /// 本メソッドは以下の処理を行う。
        /// <list type="number">
        /// <item><paramref name="cartItemVmList"/> の各商品について <see cref="ItemSalesStockReadAction"/> を生成・実行</item>
        /// <item>取得した在庫・販売情報を <paramref name="itemSalesStockDtoList"/> に格納</item>
        /// <item>各商品の在庫数と要求数量を比較し、在庫充足を判定</item>
        /// </list>
        ///
        /// 【用途】
        /// ・注文確定前のカート内商品の在庫確認
        ///
        /// 【設計方針】
        /// ・商品自体が存在しない場合（マスタ未登録等）は例外をスローする
        /// ・在庫数が不足している場合は例外にせず、戻り値 <c>false</c> で表現する
        /// </remarks>
        /// <exception cref="Exception">
        /// <paramref name="cartItemVmList"/> 内のJANコードに該当する商品が存在しない場合
        /// </exception>
        public bool TryGetItems(List<CartItemViewModel> cartItemVmList, out List<ItemSalesStockDto> itemSalesStockDtoList)
        {
            var isAllInStock = true;
            itemSalesStockDtoList = [];
            foreach (var cartItemVm in cartItemVmList)
            {
                var result = dbFramework.Execute(new ItemSalesStockReadAction(cartItemVm.JanCd, itemSalesStockDao));
                if (result == null)
                {
                    throw new Exception($"商品コード {cartItemVm.JanCd} の商品が見つかりません。");
                }
                else
                {
                    itemSalesStockDtoList.Add(result);
                }

                if (result.Qty < cartItemVm.Qty)
                {
                    isAllInStock = false;
                }
            }
            return isAllInStock;
        }
    }
}