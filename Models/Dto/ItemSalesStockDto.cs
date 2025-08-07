namespace ShoppingCart.Models.Dto
{
    /// <summary>
    /// 商品と在庫情報をまとめたDTOクラス。未注文・未枝番の在庫も含めて商品情報を一覧表示する用途で使用します。
    /// </summary>
    public class ItemSalesStockDto
    {
        /// <summary>
        /// pathのディレクトリパス
        /// </summary>
        const string DIRECTORY_IMG = "/img/";

        /// <summary>
        /// 商品のJANコード（バーコードの識別コード）
        /// </summary>
        public string JanCd { get; set; } = default!;

        /// <summary>
        /// 商品の単価
        /// </summary>
        public int Price { get; set; } = default!;

        /// <summary>
        /// 商品名
        /// </summary>
        public string ItemNm { get; set; } = default!;

        /// <summary>
        /// 商品画像のファイル名
        /// </summary>
        public string FileNm { get; set; } = default!;

        /// <summary>
        /// 商品画像のファイル名（フルパス）
        /// </summary>
        public string FilePath
        {
            get { return DIRECTORY_IMG + FileNm; }
        }

        /// <summary>
        /// 在庫数。条件を満たす在庫がない場合は 0。
        /// </summary>
        public int Qty { get; set; } = default!;

    }
}

