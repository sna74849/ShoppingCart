using DynamicDll.Db;
using System;
using System.Data.SqlClient;
using ShoppingCart.Models.Dto;

namespace ShoppingCart.Models.Dao
{
    public class ItemDao : BaseDao<ItemDto> {

        public override int Delete(ItemDto entity) {
            throw new NotImplementedException();
        }

        public override ItemDto Find(params object[] pkeys) {
            ItemDto itemDto = new ItemDto();

            string query = @"SELECT s.jan_cd, s.item_cd, s.price ,j.item_nm, p.file_path,";
            query += "(SELECT COUNT(jan_cd) FROM t_stock WHERE jan_cd = s.jan_cd AND order_cd IS NULL) AS amount ";
            query += "FROM m_salse_item s ";
            query += "INNER JOIN m_jancode j ON s.jan_cd = j.jan_cd ";
            query += "INNER JOIN m_jancode_photo p ON s.jan_cd = p.jan_cd ";
            query += "WHERE del_flag = '0' and item_cd = @itemCd";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Transaction = trn;
                cmd.Parameters.Add(new SqlParameter("@itemCd", System.Data.SqlDbType.Char)).Value = pkeys[0];
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    reader.Read();
                    itemDto.JanCd = GetString(reader["jan_cd"]);
                    itemDto.ItemCd = GetString(reader["item_cd"]);
                    itemDto.ItemNm = GetString(reader["item_nm"]);
                    itemDto.FilePath = GetString(reader["file_path"]);
                    itemDto.Price = GetInt(reader["price"]) ?? 0;
                    itemDto.Amount = GetInt(reader["amount"]) ?? 0;
                }
            }
            return itemDto;
        }
        public override int Insert(ItemDto entity) {
            throw new NotImplementedException();
        }
        public override int Update(ItemDto entity) {
            throw new NotImplementedException();
        }
        public List<ItemDto> FindAll() {
            List<ItemDto> itemDtos = new List<ItemDto>();

            string query = "SELECT s.jan_cd, s.item_cd, s.price ,j.item_nm, p.file_path,";
            query += "(SELECT COUNT(jan_cd) FROM t_stock WHERE jan_cd = s.jan_cd AND order_cd IS NULL) AS amount ";
            query += "FROM m_salse_item s INNER JOIN m_jancode j ";
            query += "ON s.jan_cd = j.jan_cd INNER JOIN m_jancode_photo p ";
            query += "ON s.jan_cd = p.jan_cd ";
            query += "WHERE del_flag = '0'";

            using (SqlCommand cmd = new SqlCommand(query, con)) {
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        ItemDto itemDto = new ItemDto();
                        itemDto.JanCd = GetString(reader["jan_cd"]);
                        itemDto.ItemCd = GetString(reader["item_cd"]);
                        itemDto.ItemNm = GetString(reader["item_nm"]);
                        itemDto.FilePath = GetString(reader["file_path"]);
                        itemDto.Price = GetInt(reader["price"]) ?? 0;
                        itemDto.Amount = GetInt(reader["amount"]) ?? 0;
                        itemDtos.Add(itemDto);
                    }
                }
            }
            return itemDtos;
        }
    }
}
