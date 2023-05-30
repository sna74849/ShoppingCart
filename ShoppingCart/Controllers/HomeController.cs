using DynamicDll.Db;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Dao;
using ShoppingCart.Models.Dto;
using ShoppingCart.Models.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.Json;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }
        public IActionResult Index() {
            return View("Index");
        }

        public IActionResult Login() { 
            return View(); 
        }
        public IActionResult Logout() {
            TempData.Remove("customerId");
            HttpContext.Session.Clear();

            return View();
        }
        public IActionResult LoginCheck(string customerId, string password) {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(password)) {
                ViewData["errorMessage"] = "IDとパスワードを入力してください。";
                return View("Login");
            }
            using (TranMng mng = TranMng.BeginTransaction("Shopping")) {
                CustomerDao customerDao = new CustomerDao();
                CustomerEntity customerEntity = customerDao.Find(customerId,password);
                if(customerEntity == null) {
                    ViewData["errorMessage"] = "IDかパスワードが間違っています。";
                    return View("Login");
                } else {
                    TempData.Add("customerId", customerEntity.CustomerId);
                }
            }
            return View("Index");
        }
        public IActionResult Menu() {
            return View();
        }

        public IActionResult Items() {
            List<ItemDto> itemDtos = new List<ItemDto>();
            using (TranMng mng = TranMng.BeginTransaction("Shopping")) {
                ItemDao itemDao = new ItemDao();
                itemDtos = itemDao.FindAll();
            }
            return View(itemDtos);
        }
        public IActionResult AddCart(string itemCd, int qty) {
            List<ItemDto> itemDtos
                = HttpContext.Session.GetObject<List<ItemDto>>("cart") ?? new List<ItemDto>();
            ItemDto itemDto = new ItemDto();
            using (TranMng mng = TranMng.BeginTransaction("Shopping")) {
                ItemDao itemDao = new ItemDao();
                itemDto = itemDao.Find(itemCd);
            }
            
            if (itemDtos.Find(it => it.ItemCd == itemCd) == null) {
                itemDto.Qty = qty;
                itemDtos.Add(itemDto);
            } else {
                itemDtos.Find(it => it.ItemCd == itemCd).Qty = qty;
            }
            
            HttpContext.Session.SetObject<List<ItemDto>>("cart", itemDtos);
            
            return RedirectToAction("Items"); 
        }
        public IActionResult Cart() {
            return View(HttpContext.Session.GetObject<List<ItemDto>>("cart"));
        }
        public IActionResult ChangeCart(string itemCd, int qty) {
            List<ItemDto> itemDtos
                = HttpContext.Session.GetObject<List<ItemDto>>("cart");
 
            itemDtos.Find(it => it.ItemCd == itemCd).Qty = qty;

            HttpContext.Session.SetObject<List<ItemDto>>("cart", itemDtos);
            return RedirectToAction("Cart");
        }
        public IActionResult DeleteCart(string itemCd) {
            List<ItemDto> itemDtos
                = HttpContext.Session.GetObject<List<ItemDto>>("cart");

            itemDtos.Remove(itemDtos.Find(it => it.ItemCd == itemCd));
                
            HttpContext.Session.SetObject<List<ItemDto>>("cart", itemDtos);
            return RedirectToAction("Cart");
        }

        public IActionResult Register() {
            if(string.IsNullOrEmpty((string?)TempData["customerId"])) {
                return View("Login");
            }

            List<DestinationEntity> destinationEntities = new List<DestinationEntity>();
            using (TranMng mng = TranMng.BeginTransaction("Shopping")) {
                DestinationDao destinationDao = new DestinationDao();
                destinationEntities = destinationDao.FindAll((string?)TempData["customerId"]??"");
                ViewBag.Destinations = destinationEntities;
            }
            // ログアウト後ヒストリーバックによりキャッシュで再表示しないように再リクエストをレスポンスに設定
            HttpContext.Response.Headers.Add("Cache-Control", "private, no-cache, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0");
            HttpContext.Response.Headers.Add("Pragma", "no-cache");
            HttpContext.Response.Headers.Add("Expires", "-1");
            return View(HttpContext.Session.GetObject<List<ItemDto>>("cart"));
        }

        public IActionResult Order(string? destinationNo) {

            using (TranMng mng = TranMng.BeginTransaction("Shopping")) {
                List<ItemDto> itemDtos = HttpContext.Session.GetObject<List<ItemDto>>("cart");

                //注文番号を生成
                DateTime dateTime = DateTime.Now;
                string orderCd = dateTime.Month.ToString() + dateTime.Day.ToString() + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString();  
                orderCd = orderCd.PadLeft(11, '0');

                //枝番
                int branchNo = 0;

                foreach (ItemDto itemDto in itemDtos) {
                    StockEntity stockEntity = new StockEntity();
                    stockEntity.JanCd = itemDto.JanCd;
                    stockEntity.OrderCd = orderCd;
                    stockEntity.BranchNo = ++branchNo;
                    
                    OrderEntity orderEntity = new OrderEntity();
                    orderEntity.OrderCd = orderCd;
                    orderEntity.BranchNo = branchNo;
                    orderEntity.CustomerId = "account1@emtech.com";
                    orderEntity.ItemCd = itemDto.ItemCd;

                    DeliveryEntity deliveryEntity = new DeliveryEntity();
                    deliveryEntity.OrderCd = orderCd;
                    deliveryEntity.BranchNo = branchNo;
                    deliveryEntity.DestinationNo = int.Parse(destinationNo);
                    try {
                        new OrderDao().Insert(orderEntity);
                        for (int j = 0; j < itemDto.Qty; j++) {
                            new StockDao().Update(stockEntity);
                        }
                        new DeliveryDao().Insert(deliveryEntity);
                        mng.Commit();
                    } catch (Exception e) {
                        mng.Rollback();
                        ViewData["stackTrace"] = e.StackTrace;
                        return View("Error");
                    }
                }
                return View(new OrderHistoryDao().FindAll(orderCd));
           }
  
        }
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
    /// <summary>
    /// セッション拡張クラス
    /// </summary>
    public static class SessionExtensions {
        // セッションにオブジェクトを書き込む
        public static void SetObject<T>(this ISession session, string key, T obj) {
            var json = JsonSerializer.Serialize(obj);
            session.SetString(key, json);
        }
        // セッションからオブジェクトを読み込む
        public static T GetObject<T>(this ISession session, string key) {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json)
            ? default(T)
            : JsonSerializer.Deserialize<T>(json);
        }
    }
}