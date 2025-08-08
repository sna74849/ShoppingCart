namespace ShoppingCart.Models.Dao
{
    /// <summary>
    /// DAO（Data Access Object）の基本抽象クラス。
    /// 特定のDTO型に対して基本的なRead操作を定義します。
    /// </summary>
    /// <typeparam name="Dto">対象となるDTOの型。</typeparam>
    public interface IBaseDtoDao<Dto> where Dto : class
    {
        /// <summary>
        /// プライマリキーを用いてDTOを検索します。
        /// </summary>
        /// <param name="pkeys">プライマリキーの値。</param>
        /// <returns>一致するDTO。存在しない場合は null。</returns>
        public Dto? Find(params object[] pkeys);

        /// <summary>
        /// 条件に一致するDTOのListを検索します。
        /// </summary>
        /// <returns>一致するDTOのList。存在しない場合は null。</returns>
        public List<Dto> Find();

        /// <summary>
        /// プライマリキーを用いてDTOを検索します。
        /// </summary>
        /// <param name="pkeys">プライマリキーの値。</param>
        /// <returns>一致するDTOのList。存在しない場合は null。</returns>
        public List<Dto> FindBy(params object[] pkeys);
    }
}
