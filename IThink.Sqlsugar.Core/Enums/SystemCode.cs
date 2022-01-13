/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.ComponentModel;
using System.Linq;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 状态码定义
    /// </summary>
    public enum SystemCode
    {
        #region 登录验证状态码 100开始
        /// <summary>
        /// 用户不存在！
        /// </summary>
        [Description("用户不存在！")]
        [DefaultValue("The account doesn't exist!")]
        NotExist = 100,
        /// <summary>
        /// 用户被删除！
        /// </summary>
        [Description("用户被删除！")]
        [DefaultValue("The account has been deleted!")]
        HasDeleted = 101,
        /// <summary>
        /// 验证码错误，请重试！
        /// </summary>
        [Description("验证码错误，请重试！")]
        [DefaultValue("Incorrect verification code, please try again!")]
        WrongCode = 102,
        /// <summary>
        /// 验证码已失效，请重试！
        /// </summary>
        [Description("验证码已失效，请重试！")]
        [DefaultValue("Invalid verification code , please try again! ")]
        OverdueCode = 103,
        /// <summary>
        /// 用户被锁定！请联系管理员！
        /// </summary>
        [Description("用户被锁定！请联系管理员！")]
        [DefaultValue("The account has been locked!Please contact the administrator!")]
        BeLocked = 104,
        /// <summary>
        /// 此账号已被锁定，是否通过手机短信验证解锁账号！
        /// </summary>
        [Description("此账号已被锁定，是否通过手机短信验证解锁账号！")]
        [DefaultValue("This account has been locked,do you want to unlock the account through SMS verification?")]
        WarnRelieve = 105,
        /// <summary>
        /// 用户被禁用！请联系管理员！
        /// </summary>
        [Description("用户被禁用！请联系管理员！")]
        [DefaultValue("Account disabled!Please contact the administrator!")]
        BeBaned = 106,
        /// <summary>
        /// 用户被禁用！请联系管理员！
        /// </summary>
        [Description("密码错误,请重试！")]
        [DefaultValue("Incorrect password,please try again!")]
        WrongPassword = 107,
        /// <summary>
        /// 此账号用户未绑定手机号码，请联系账号对应管理员或联系客服。
        /// </summary>
        [Description("此账号用户未绑定手机号码，请联系账号对应管理员或联系客服。")]
        [DefaultValue("The user of this account is not bound to the mobile phone number, please contact the account administrator or customer service.")]
        UnBindPhone = 108,
        /// <summary>
        /// 用户不存在或已被删除！
        /// </summary>
        [Description("用户不存在或已被删除！")]
        [DefaultValue("The account doesn't exist or has been deleted!")]
        UserNotExistOrDeleted = 109,
        /// <summary>
        /// 请获取验证码后登录。
        /// </summary>
        [Description("请获取验证码后登录。")]
        [DefaultValue("Please login after obtaining verification code!")]
        PleaseSendVerification = 110,

        /// <summary>
        /// 用户所属企业被禁用！请联系管理员！
        /// </summary>
        [Description("用户所属企业被禁用！请联系管理员！")]
        [DefaultValue("The enterprise of the account is disabled! Please contact the administrator!")]
        UserCompanyBeBaned = 111,

        /// <summary>
        /// 账号已到期，无法登录！
        /// </summary>
        [Description("账号已到期，无法登录")]
        [DefaultValue("Account has expired, unable to log in！")]
        UserHasExpired = 112,
        /// <summary>
        /// 账号未审核或审核不通过！
        /// </summary>
        [Description("账号未审核或审核不通过！")]
        UserUnAudit = 113,

        #endregion

        /// <summary>
        /// 操作成功！
        /// </summary>
        [Description("操作成功！")]
        [DefaultValue("The operation is successful!")]
        Success = 200,

        /// <summary>
        /// 系统异常！请联系管理员！
        /// </summary>
        [Description("系统异常！请联系管理员！")]
        [DefaultValue("System error!Please contact the administrator!")]
        SystemException = 500,

        /// <summary>
        /// 未知的依赖！请联系管理员！
        /// </summary>
        [Description("未知的依赖！请联系管理员！")]
        [DefaultValue("")]
        DependencyError = 501,

        /// <summary>
        /// 未知构造函数！请联系管理员！
        /// </summary>
        [Description("未知构造函数！请联系管理员！")]
        [DefaultValue("")]
        NoConstructorError = 502,

        /// <summary>
        /// 在类型{1}的实例上找不到属性{0}！
        /// </summary>
        [Description("在类型{1}的实例上找不到属性{0}！")]
        [DefaultValue("")]
        TypeNotPropertyError = 503,

        /// <summary>
        /// 类型属性错误
        /// </summary>
        [Description("类型{1}的实例上的属性{0}没有setter。！")]
        [DefaultValue("")]
        TypePropertyNotSetError = 504,

        /// <summary>
        /// 数据库系统异常！请联系管理员！
        /// </summary>
        [Description("数据库系统异常！请联系管理员！")]
        [DefaultValue("Database system error!Please contact the administrator! ")]
        DBException = 600,

        /// <summary>
        /// 数据库操作失败！请刷新重试！
        /// </summary>
        [Description("数据库操作失败！请刷新重试！")]
        [DefaultValue("Database operation failed! Please refresh and try again!")]
        DBOperatorError = 601,

        /// <summary>
        /// 操作文件系统异常！请联系管理员！
        /// </summary>
        [Description("操作文件系统异常！请联系管理员！")]
        [DefaultValue("file system error!Please contact the administrator!")]
        FileException = 700,

        /// <summary>
        /// 授权文件系统异常！请联系管理员！
        /// </summary>
        [Description("授权文件错误！请联系管理员！")]
        [DefaultValue("Authorization file error!Please contact the administrator!")]
        AuthFileException = 700,

        /// <summary>
        /// 网络异常，请检查网络！
        /// </summary>
        [Description("网络异常，请检查网络！")]
        [DefaultValue("Network error,please check the network!")]
        InternetException = 800,

        /// <summary>
        /// 没有操作权限，请联系管理员！
        /// </summary>
        [Description("没有操作权限，请联系管理员！")]
        [DefaultValue("No operation permission, please contact the administrator!")]
        AuthorizationError = 1000,

        /// <summary>
        /// 参数错误！
        /// </summary>
        [Description("参数错误！")]
        [DefaultValue("Parameter error!")]
        ParamEmpty = 1100,

        /// <summary>
        /// {0}不能为空！
        /// </summary>
        [Description("{0}不能为空！")]
        [DefaultValue("{0} can't be empty!")]
        EmptyError = 2000,

        /// <summary>
        /// {0}：{1}已经存在，不能操作!
        /// </summary>
        [Description("{0}:{1}已经存在，不能操作!")]
        [DefaultValue("{0}:{1} already exists, can't operate!")]
        AlreadyExistError = 2001,

        /// <summary>
        /// {0}：{1}已经被使用,不能操作！
        /// </summary>
        [Description("{0}:{1}已经被使用,不能操作！")]
        [DefaultValue("{0}:{1} has been used, can't operate!")]
        AlreadyUsedError = 2002,

        /// <summary>
        /// {0}已经存在，不能操作!
        /// </summary>
        [Description("{0}已经存在，不能操作!")]
        [DefaultValue("{0}:{1} already exist,can't operate!")]
        AlreadyExistErrorOne = 2003,

        /// <summary>
        /// {0}：{1}不存在，不能操作
        /// </summary>
        [Description("{0}:{1}不存在，不能操作！")]
        [DefaultValue("{0}:{1} doesn't exist,can't operate!")]
        NotExistError = 2010,

        /// <summary>
        /// {0}不存在，不能操作
        /// </summary>
        [Description("{0}不存在，不能操作！")]
        [DefaultValue("{0} doesn't exist,can't operate!")]
        NotExistErrorOne = 2011,

        /// <summary>
        /// {0}长度必须小于{1}！
        /// </summary>
        [Description("{0}长度必须小于{1}！")]
        [DefaultValue("{0} length must be less than {1}!")]
        MaxLengthError = 2020,

        /// <summary>
        /// {0}长度必须大于{1}！
        /// </summary>
        [Description("{0}长度必须大于{1}！")]
        [DefaultValue("{0} length must be greater than {1}!")]
        MinLengthError = 2021,

        /// <summary>
        /// {0}长度必须在{1}-{2}之间！
        /// </summary>
        [Description("{0}长度必须在{1}-{2}之间！")]
        [DefaultValue("{0} length must be between {1}-{2}!")]
        BetweenLengthError = 2022,


        /// <summary>
        /// 操作的数据在数据库不存在,不能操作！
        /// </summary>
        [Description("操作的数据在数据库不存在,不能操作！")]
        [DefaultValue("The data does not exist in the database, can not operate!")]
        DBNotExistError = 2030,

        /// <summary>
        /// {0}必须是{1}类型！
        /// </summary>
        [Description("{0}必须是{1}类型！")]
        [DefaultValue("{0} must be of type {1}!")]
        TypeError = 2100,

        /// <summary>
        /// {0}格式错误，正确的格式是{1}！
        /// </summary>
        [Description("{0}格式错误，正确的格式是{1}！")]
        [DefaultValue("{0} format error, the correct format is {1}!")]
        FormatError = 2200,

        /// <summary>
        /// {0}已经被删除，不能操作！
        /// </summary>
        [Description("{0}已经被删除，不能操作！")]
        [DefaultValue("{0} has been deleted, can't operate!")]
        HasBeenDeletedError = 2300,

        /// <summary>
        /// {0}必须小于{1}！
        /// </summary>
        [Description("{0}必须小于{1}！")]
        [DefaultValue("{0} must be less than {1}!")]
        MaxValueError = 2400,

        /// <summary>
        /// {0}必须大于{1}！
        /// </summary>
        [Description("{0}必须大于{1}！")]
        [DefaultValue("{0} must be greater than {1}!")]
        MinValueError = 2401,

        /// <summary>
        /// {0}必须在{1}-{2}之间！
        /// </summary>
        [Description("{0}必须在{1}-{2}之间！")]
        [DefaultValue("{0} must be between {1}-{2}!")]
        BetweenValueError = 2402,

        /// <summary>
        /// 不允许上传空白附件!
        /// </summary>
        [Description("不允许上传空白附件!")]
        [DefaultValue("Upload of blank attachment is not allowed! ")]
        UploadEmptyFileError = 2500,

        /// <summary>
        /// 未知DSL，请检查DSL是否正确!
        /// </summary>
        [Description("未知DSL，请检查DSL是否正确!")]
        [DefaultValue("Unknown DSL, please check the DSL is correct!")]
        UnknowDSLError = 3000,
    }




    /// <summary>
    /// 状态码扩展
    /// </summary>
    public static class StatusCodeExtension
    {
        /// <summary>
        /// 获取StatusCode对应的消息
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="formatValues">消息格式化参数</param>
        /// <returns>返回给使用者的消息</returns>
        public static string Message(this SystemCode statusCode, params object[] formatValues)
        {
            var type = typeof(SystemCode);

            var memInfo = type.GetMember(type.GetEnumName(statusCode));

            // 中文
            var descriptionAttribute = memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

            var message = descriptionAttribute.Description;
            if (formatValues!=null&& formatValues.Length>0)
            {
                message = string.Format(descriptionAttribute.Description, formatValues);
            }
            
            return message;
        }
    }
}
