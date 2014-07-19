using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PPOB.Models;
using PPOB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace PPOB.Controllers
{
    public class WXController : Controller
    {
        private PPOBContext db = new PPOBContext();

        private int ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int)span.TotalSeconds;
        }

        private class ArticalItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string PicUrl { get; set; }
            public string Url { get; set; }
        }

        private string TransmitText(JObject jObj, string content, int flag = 0)
        {
            string textTpl = "<xml>" +
            "<ToUserName><![CDATA[{0}]]></ToUserName>" +
            "<FromUserName><![CDATA[{1}]]></FromUserName>" +
            "<CreateTime>{2}</CreateTime>" +
            "<MsgType><![CDATA[text]]></MsgType>" +
            "<Content><![CDATA[{3}]]></Content>" +
            "<FuncFlag>{4}</FuncFlag>" +
            "</xml>";
            string resultStr = String.Format(textTpl, jObj["xml"]["FromUserName"]["#cdata-section"], jObj["xml"]["ToUserName"]["#cdata-section"], ConvertToTimestamp(DateTime.UtcNow).ToString(), content, flag);
            return resultStr;
        }

        private string TransmitNews(JObject jObj, List<ArticalItem> newsArray)
        {

            string itemTpl = "<item>" +
            "<Title><![CDATA[{0}]]></Title>" +
            "<Description><![CDATA[{1}]]></Description>" +
            "<PicUrl><![CDATA[{2}]]></PicUrl>" +
            "<Url><![CDATA[{3}]]></Url>" +
            "</item>";

            string item_str = "";
            foreach (var item in newsArray)
            {
                item_str += String.Format(itemTpl, item.Title, item.Description, item.PicUrl, item.Url);
            }

            string newsTpl = "<xml>" +
            "<ToUserName><![CDATA[{0}]]></ToUserName>" +
            "<FromUserName><![CDATA[{1}]]></FromUserName>" +
            "<CreateTime>{2}</CreateTime>" +
            "<MsgType><![CDATA[news]]></MsgType>" +
            "<Content><![CDATA[]]></Content>" +
            "<ArticleCount>{3}</ArticleCount>" +
            "<Articles>" + item_str + "</Articles>" +
            "</xml>";

            var result = String.Format(newsTpl, jObj["xml"]["FromUserName"]["#cdata-section"], jObj["xml"]["ToUserName"]["#cdata-section"], ConvertToTimestamp(DateTime.UtcNow).ToString(), newsArray.Count());
            return result;
        }

        public string SendSummary(JObject jObj, int employeeId, string title = "")
        {
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("FirstPage", "WX", null, Request.Url.Scheme);
            string resultStr;
            string contentStr;

            var result = db.Employee.Where(a => a.Id == employeeId).SingleOrDefault();
            var cp = db.ClientCity.Where(a => a.ClientId == result.ClientId && ((a.CityId == null && result.CityId == null) || a.CityId == result.CityId)).FirstOrDefault();
            var eDoc = result.EmployeeEnterDocs;
            var docItems = (
                            from b in cp.ClientEnterDocuments
                            join c in eDoc on b.Id equals c.ClientDocumentId into b_c
                            from bc in b_c.DefaultIfEmpty()
                            select new DocItem { Code = b.Id.ToString(), Name = b.Name, WXPath = (bc == null ? null : bc.WXPath) }).ToList();

            if (docItems.Count() > 0)
            {
                string docStr = "";
                foreach (var item in docItems)
                {
                    docStr += item.Code.PadLeft(4, '0') + "     " + (String.IsNullOrWhiteSpace(item.WXPath) ? "[未上传]" : "[已上传]") + "     " + item.Name + "\n";
                }
                List<ArticalItem> als = new List<ArticalItem>{
                                new ArticalItem{ Title=String.IsNullOrWhiteSpace(title) ? "请输入左侧编号上传资料": title + "\n请输入左侧编号继续上传资料", Description = "入职资料:\n" + docStr, Url = url},
                            };
                resultStr = TransmitNews(jObj, als);

            }
            else
            {
                contentStr = "对应入职材料列表尚未配置, 请联系客服!";
                resultStr = TransmitText(jObj, contentStr, 0);
            }
            return resultStr;
        }

        public string SendSalary(JObject jObj, int employeeId, string title = "")
        {
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("FirstPage", "WX", null, Request.Url.Scheme);
            string resultStr;
            string contentStr;

            var result = db.Employee.Where(a => a.Id == employeeId).SingleOrDefault();
            title = result.Name + "您的上月工资为:";
            
            List<ArticalItem> als = new List<ArticalItem>{
                new ArticalItem{ Title=title, Description = "税前工资: 5000\n所得税: 170\n四金个人部分: 1050\n税后工资: 3780", Url = url},
            };
            resultStr = TransmitNews(jObj, als);
            return resultStr;
        }

        private string ReceiveText(JObject jObj)
        {
            int funcFlag = 0;
            string tmpStr = (string)jObj["xml"]["Content"]["#cdata-section"];
            string openId = (string)jObj["xml"]["FromUserName"]["#cdata-section"];
            string contentStr = "";
            string resultStr = "";

            var employee = db.Employee.Where(a => a.OpenId == openId).Single();

            if (tmpStr == "0")
            {
                resultStr = SendSummary(jObj, employee.Id);
            }
            else
            {
                var cp = db.ClientCity.Where(a => a.ClientId == employee.ClientId && ((a.CityId == null && employee.CityId == null) || a.CityId == employee.CityId));
                List<IdName> idNames = (from a in cp
                                        from b in a.ClientEnterDocuments
                                        select new IdName { Id = SqlFunctions.StringConvert((double)b.Id).Trim(), Name = b.Name }).ToList();
                List<IdName> idNames2 = new List<IdName>();
                foreach (var item in idNames)
                {
                    idNames2.Add(new IdName { Id = item.Id.PadLeft(4, '0'), Name = item.Name });
                }

                var tmpIdName = idNames2.Where(a => a.Id == tmpStr).SingleOrDefault();
                if (tmpIdName != null)
                {
                    employee.Code = tmpStr;
                    db.SaveChanges();
                    contentStr = "请上传:" + tmpIdName.Name;
                    resultStr = TransmitText(jObj, contentStr, funcFlag);
                }
                else
                {
                    contentStr = "请输入正确的材料编号,发送0获取编号清单";
                    resultStr = TransmitText(jObj, contentStr, funcFlag);
                }
            }
            return resultStr;
        }

        private void ResponseMsg()
        {
            string textTpl = "<xml>" +
           "<ToUserName><![CDATA[{0}]]></ToUserName>" +
           "<FromUserName><![CDATA[{1}]]></FromUserName>" +
           "<CreateTime>{2}</CreateTime>" +
           "<MsgType><![CDATA[text]]></MsgType>" +
           "<Content><![CDATA[{3}]]></Content>" +
           "<FuncFlag>{4}</FuncFlag>" +
           "</xml>";

            byte[] postStr = Request.BinaryRead(Request.TotalBytes);
            string resultStr = "";
            if (postStr.Length > 0)
            {
                var xml = new XmlDocument();
                xml.LoadXml(System.Text.Encoding.Default.GetString(postStr));
                JObject jObj = JObject.Parse(JsonConvert.SerializeObject(xml));

                string openId = (string)jObj["xml"]["FromUserName"]["#cdata-section"];

                var employee = db.Employee.Where(a => a.OpenId == openId).SingleOrDefault();
                if (employee == null)
                {
                    resultStr = TransmitText(jObj, "请先" + "<a href='" + Url.Action("BindEmployee", "WX", null, Request.Url.Scheme) + "'>" + "点击这里绑定</a>");
                }
                else
                {
                    switch ((string)jObj["xml"]["MsgType"]["#cdata-section"])
                    {
                        case "text":
                            resultStr = ReceiveText(jObj);
                            break;
                        case "image":
                            if (!String.IsNullOrWhiteSpace(employee.Code))
                            {
                                var code2 = Int32.Parse(employee.Code);

                                var employeeEnterDoc = db.EmployeeEnterDoc.Where(a => a.EmployeeId == employee.Id && a.ClientDocumentId == code2).SingleOrDefault();

                                if (employeeEnterDoc == null)
                                {
                                    db.EmployeeEnterDoc.Add(new EmployeeEnterDoc { EmployeeId = employee.Id, ClientDocumentId = code2, WXPath = (string)jObj["xml"]["PicUrl"]["#cdata-section"] });
                                }
                                else
                                {
                                    employeeEnterDoc.WXPath = (string)jObj["xml"]["PicUrl"]["#cdata-section"];
                                }
                                employee.Code = null;
                                try
                                {
                                    db.SaveChanges();
                                    resultStr = SendSummary(jObj, employee.Id, "上传成功!");
                                }
                                catch (Exception e)
                                {
                                    resultStr = TransmitText(jObj, "上传失败!" + e);
                                }
                            }
                            else
                            {
                                resultStr = TransmitText(jObj, "请先输入材料编号!");
                            }
                            break;
                        case "event":
                            resultStr = receiveEvent(jObj, employee);
                            break;
                        default:
                            resultStr = TransmitText(jObj, "未知格式, 请重试!");
                            break;
                    }
                }
            }
            Response.Write(resultStr);
        }

        private string receiveEvent(JObject jObj, Employee employee)
        {
            var result = "";
            switch ((string)jObj["xml"]["Event"]["#cdata-section"])
            {
                case "subscribe":

                    break;
                case "unsubscribe":

                    break;
                case "SCAN":
                    break;
                case "CLICK":
                    switch ((string)jObj["xml"]["EventKey"]["#cdata-section"])
                    {
                        case "V1001_UPLOAD":
                            result = SendSummary(jObj, employee.Id);
                            break;
                        case "V1001_SALARY":
                            result = SendSalary(jObj, employee.Id);
                            break;
                        case "V1001_FEE":
                            result = TransmitText(jObj, "敬请期待...", 0);
                            break;
                        case "V1001_WE":
                            result = TransmitText(jObj, "上海期盛", 0);
                            break;
                        default:
                            break;
                    }
                    break;
                case "LOCATION":
                    break;
                case "VIEW":
                    break;
                default:
                    break;
            }
            return result;
        }

        private bool WeixinBind(string openId)
        {
            return db.Employee.Where(a => a.OpenId == openId).Count() > 0;
        }

        private Employee GetEmployeeFromOpenId(string openId)
        {
            var result = db.Employee.Where(a => a.OpenId == openId).FirstOrDefault();
            return result;
        }

        public void Index()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["echostr"]))
            {
                var echostr = Request.QueryString["echostr"];
                Response.Write(echostr);
                return;
            }
            else
            {
                ResponseMsg();
            }
        }

        public ActionResult BindEmployee()
        {
            if (Session["EmployeeOpenId"] == null)
            {
                //state:2 return to BindEmployee
                return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6ddf4ae7a3e6ba25&redirect_uri=http://116.236.241.90/WX/Oauth2&response_type=code&scope=snsapi_base&state=2#wechat_redirect");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult BindEmployee(BindEmployee model)
        {
            //绑定操作
            string mobile = model.Mobile;
            string code = model.Code;

            bool hasOld = false;
            //删除旧的绑定
            var openId = (string)(Session["EmployeeOpenId"]);
            var resultOld = db.Employee.Where(a => a.OpenId == openId).SingleOrDefault();
            if (resultOld != null)
            {
                hasOld = true;
                resultOld.OpenId = null;
            }
            //end 删除旧的绑定
            //添加新的绑定
            var resultNew = db.Employee.Where(a => a.Mobile == mobile && a.RandomCode == code).SingleOrDefault();
            if (resultNew == null)
            {
                ViewBag.ErrMsg = "未找到对应记录, 请仔细核对您的手机号和验证码后重试!";
                return View(model);
            }
            else
            {
                resultNew.OpenId = (string)Session["EmployeeOpenId"];
                try
                {
                    db.SaveChanges();
                    Session["EmployeeId"] = resultNew.Id;

                    if (hasOld)
                    {
                        return RedirectToAction("FirstPage");
                    }
                    else
                    {
                        return RedirectToAction("FirstPage");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.ErrMsg = e.Message;
                }
                return View(model);
            }
        }

        public ActionResult Oauth2()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx6ddf4ae7a3e6ba25&secret=9fe15887361da26dffb781716b1f6c78&code=" + Request.QueryString["code"] + "&grant_type=authorization_code";
                using (var client = new WebClient())
                {
                    string json = client.DownloadString(url);
                    JObject jObj = JObject.Parse(json);
                    Session["EmployeeOpenId"] = (string)jObj["openid"];
                    if ((string)Request.QueryString["state"] == "1")
                    {
                        return RedirectToAction("FirstPage");
                    }
                    else
                    {
                        return RedirectToAction("BindEmployee");
                    }
                }
            }
            else
            {
                return Content("没有参数Code, 非法操作！");
            }
        }

        public JsonResult SummaryInfo()
        {
            if (Session["EmployeeId"] == null)
            {
                return Json("操作超时, 请返回公众平台首页重新操作!", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int tmpId = (int)Session["EmployeeId"];
                var employee = db.Employee.Where(a => a.Id == tmpId).SingleOrDefault();
                if (employee == null)
                {
                    return Json("未找到对应记录, 请重试!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Name = employee.Name, ClientName = employee.Client.Name }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult FirstPage()
        {
            //ViewBag.CurEmployeeId = 1;
            //return View();
            if (Session["EmployeeOpenId"] == null)
            {
                //state:1 return to FirstPage
                return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6ddf4ae7a3e6ba25&redirect_uri=http://116.236.241.90/WX/Oauth2&response_type=code&scope=snsapi_base&state=1#wechat_redirect");
            }
            var employee = GetEmployeeFromOpenId((string)Session["EmployeeOpenId"]);
            if (employee != null)
            {
                Session["EmployeeId"] = employee.Id;
                ViewBag.CurEmployeeId = employee.Id;
                return View();
            }
            else
            {
                return Redirect("BindEmployee");
            }
        }

        [HttpPost]
        public ActionResult Save()
        {

            // Can process the data any way we want here,
            // e.g., further server-side validation, save to database, etc
            return Json("ok!");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}