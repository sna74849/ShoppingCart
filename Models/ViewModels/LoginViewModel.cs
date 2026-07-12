using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models.ViewModels
{
    /// <summary>
    /// ログイン情報をまとめたViewModel。
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 顧客ID（主キー）。
        /// </summary>
        [Required(ErrorMessage = "IDを入力してください。")]
        public string CustomerId { get; set; } = default!;

        /// <summary>
        /// パスワード。
        /// </summary>
        [Required(ErrorMessage = "パスワードを入力してください。")]
        public string Password { get; set; } = default!;
    }
}