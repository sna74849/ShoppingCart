namespace ShoppingCart.Models.Dao
{
    /// <summary>
    /// DAO（Data Access Object）の基本抽象クラス。
    /// 特定のエンティティ型に対して基本的なCRUD操作を定義します。
    /// </summary>
    /// <typeparam name="Entity">対象となるエンティティの型。</typeparam>
    public interface IBaseEntityDao<Entity> where Entity : class
    {
        /// <summary>
        /// プライマリキーを用いてエンティティを検索します。
        /// </summary>
        /// <param name="pkeys">プライマリキーの値。</param>
        /// <returns>一致するエンティティ。存在しない場合は null。</returns>
        public Entity? Find(params object[] pkeys);

        /// <summary>
        /// 条件に一致するエンティティのListを検索します。
        /// </summary>
        /// <returns>一致するエンティティ。存在しない場合は null。</returns>
        public List<Entity> Find();

        /// <summary>
        /// プライマリキーを用いてエンティティを検索します。
        /// </summary>
        /// <param name="pkeys">プライマリキーの値。</param>
        /// <returns>一致するエンティティのList。存在しない場合は null。</returns>
        public List<Entity> FindBy(params object[] pkeys);

        /// <summary>
        /// 新しいエンティティをデータベースに挿入します。
        /// </summary>
        /// <param name="entity">挿入対象のエンティティ。</param>
        /// <returns>影響を受けた行数。</returns>
        public int Insert(Entity entity);

        /// <summary>
        /// 既存のエンティティを更新します。
        /// </summary>
        /// <param name="entity">更新対象のエンティティ。</param>
        /// <returns>影響を受けた行数。</returns>
        public int Update(Entity entity);

        /// <summary>
        /// エンティティを削除します。
        /// </summary>
        /// <param name="entity">削除対象のエンティティ。</param>
        /// <returns>影響を受けた行数。</returns>
        public int Delete(Entity entity);
    }
}
