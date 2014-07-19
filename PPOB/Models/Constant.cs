using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPOB.Models.Constant
{
    public enum EmployeeOBStatus
    {
        未通知= 10,
        OFFER已发 = 20,
        资料待审核 = 30,
        OB结束 = 40,
    }

    public enum Degree
    {
        初中及以下 = 10,
        高中 = 20,
        大专 = 30,
        本科 = 40,
        硕士 = 50,
        博士 = 60,
    }

    public enum HukouType
    {
        本地城镇 = 10,
        外地城镇 = 20,
        本地农村 = 30,
        外地农村 = 40,
        外籍 = 50,
    }

    public enum Marriage
    {
        已婚 = 10,
        未婚 = 20,
        离异 = 30,
    }

    public enum Sex
    {
        男 = 10,
        女 = 20,
    }

    public enum DocumentType
    {
        身份证 = 10,
        护照 = 20,
    }
}