using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;

namespace ShoppingCart.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty((string?)TempData["customerId"]))
                {
                    //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                    return RedirectToAction("Login", "Account");
                } 
                else
                {
                    using (new ConnectionManager("Shopping"))
                    {
                        object[] pkeys = { (string)TempData["customerId"]! };
                        ViewBag.Destinations = new DestinationDao().FindBy(pkeys);
                    }
                    // ログアウト後ヒストリーバックによりキャッシュで再表示しないように再リクエストをレスポンスに設定
                    HttpContext.Response.Headers.Add("Cache-Control", "private, no-cache, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0");
                    HttpContext.Response.Headers.Add("Pragma", "no-cache");
                    HttpContext.Response.Headers.Add("Expires", "-1");
                    return View(HttpContext.Session.GetObject<List<CartItemDto>>("cart"));
                }
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.StackTrace;
                ViewData["message"] = e.Message;

                return View("Error");
            }
        }

        [HttpPost("/register/orders{destinationNo}")]
        public IActionResult Create([FromRoute] int destinationNo, [Bind] List<OrderScheduledDeliveryDto> orderScheduledDeliveriesDto)
        {
            try
            {
                using (var trn = new ConnectionManager("Shopping").BeginTransaction())
                {
                    var cartItemDtoList = HttpContext.Session.GetObject<List<CartItemDto>>("cart") ?? throw new Exception();
                    //注文番号を生成
                    var dateTime = DateTime.Now;
                    var orderCd = dateTime.Month.ToString() + dateTime.Day.ToString() + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString();
                    orderCd = orderCd.PadLeft(11, '0');
                    if (string.IsNullOrEmpty((string?)TempData["customerId"]))
                    {
                        //View("../Account/Login")で指定するとブラウザに"Home"のパスが残る。
                        return RedirectToAction("Login", "Account");
                    }

                    try
                    {
                        new OrderHeaderDao().Insert(new OrderHeaderEntity
                        {
                            OrderCd = orderCd,
                            DestinationNo = destinationNo
                        });
                        int itemCnt = 0;
                        foreach (CartItemDto cartItemDto in cartItemDtoList)
                        {
                            StockDao stockDao = new();
                            object[] pkeys = { cartItemDto.item.JanCd, cartItemDto.InCartQty };
                            List<StockEntity> stockEtyList = stockDao.FindBy(pkeys);
                            if (stockEtyList.Count < cartItemDto.InCartQty)
                            {
                                throw new OrderException("在庫が不足しています。");
                            }
                            else if (stockEtyList.Count == 0)
                            {
                                throw new OrderException("在庫がありません。");
                            }
                            int seqNo = 1;
                            foreach (var stockEty in stockEtyList)
                            {
                                new OrderDetailDao().Insert(new OrderDetailEntity
                                {
                                    OrderCd = orderCd,
                                    SalesCd = new SalesDao().Find(cartItemDto.item.JanCd)!.SalesCd,
                                    SeqNo = seqNo++,
                                    ScheduledDeliveryAt
                                        = orderScheduledDeliveriesDto[itemCnt].ScheduledDeliveryIs ? orderScheduledDeliveriesDto[itemCnt].ScheduledDeliveryAt : null,
                                    ShippedAt = null
                                });
                                stockEty.OrderCd = orderCd;
                                new StockDao().Update(stockEty);
                            }
                            itemCnt += 1;
                        }
                        //cartItemDtoList.Remove(cartItemDtoList.Find(it => it.item.JanCd == cartItemDto.item.JanCd) ?? throw new Exception());
                        HttpContext.Session.Clear();
                        TempData.Remove("count");
                        trn.Commit();
                        return View("Order", new OrderDao().Find(orderCd));
                    }
                    catch (Exception e)
                    {
                        /* 
                            ロールバックするにはTrnが必要なためtry-catchを２重にする。
                         */
                        trn.Rollback();
                        throw new OrderException("注文処理が失敗しました。", e);
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["stackTrace"] = e.ToString();
                ViewData["message"] = e.Message;
                return View("Error");
            }
        }
    }
}
public class OrderException : Exception
{
    public OrderException(string message, Exception? innerException = null)
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
