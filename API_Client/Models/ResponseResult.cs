using Newtonsoft.Json;

namespace Models
{
    /// <summary>
    /// 响应结果类
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public ResponseResultStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Default(string message = null)
        {
            var result = new ResponseResult
            {
                Status = ResponseResultStatus.Default,
                Message = message
            };

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Success(string message = null)
        {
            var result = new ResponseResult
            {
                Status = ResponseResultStatus.Success,
                Message = message
            };

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="errorMessage">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Error(string errorMessage)
        {
            var result = new ResponseResult
            {
                Status = ResponseResultStatus.Error,
                Message = errorMessage
            };
            return result;
        }

        /// <summary>
        /// 解析返回结果中的状态标志位
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Parse(string message)
        {
            var result = JsonConvert.DeserializeObject<ResponseResult>(message);
            var success = result.Status == ResponseResultStatus.Success;

            return success;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseResult Deserialize(string message)
        {
            var result = JsonConvert.DeserializeObject<ResponseResult>(message);
            return result;
        }
    }

    /// <summary>
    /// 响应消息类
    /// </summary>
    public class ResponseResult<T> : ResponseResult
       // where T //: new()
    {
        /// <summary>
        /// 业务实体
        /// </summary>
        public T Entity
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <returns></returns>
        public static ResponseResult<T> Default()
        {
            var result = new ResponseResult<T>
            {
                Entity = default(T),
                Status = ResponseResultStatus.Default,
                Message = string.Empty
            };
            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="t">结果携带的实体</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Success(T t, string message = null)
        {
            var result = new ResponseResult<T>
            {
                Entity = t,
                Status = ResponseResultStatus.Success,
                Message = message
            };
            return result;
        }

        /// <summary>
        /// Http 响应消息封装类
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        new public static ResponseResult<T> Error(string message = null)
        {
            var result = new ResponseResult<T>
            {
                Entity = default(T),
                Status = ResponseResultStatus.Error,
                Message = message
            };
            return result;
        }

        /// <summary>
        /// Http 响应消息反序列化类
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        new public static ResponseResult<T> Deserialize(string message)
        {
            var response = JsonConvert.DeserializeObject<ResponseResult<T>>(message);
            return response;
        }
    }

    /// <summary>
    /// 结果状态:1-成功; 0-缺省; -1失败
    /// </summary>
    public enum ResponseResultStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        Error = -1,

        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1
    }
}
