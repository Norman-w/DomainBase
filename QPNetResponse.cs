using System;
using System.IO;
using System.Xml.Serialization;

namespace DomainBase
{
    [Serializable]
    public abstract class NetResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("code")]
        public string ErrCode { get; set; }

        #region 2021年9月12日12:09:01  如果是下载类的request的response
        /// <summary>
        /// 如果请求是需要下载文件的,该字段不为空,直接通过response返回这个文件
        /// </summary>
        public MemoryStream DownloadFileStream { get; set; }

        /// <summary>
        /// 如果请求是下载文件的,该字段不为空,返回的是文件将要保存的文件名
        /// </summary>
        public string DownloadFileName { get; set; }
        #endregion

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("msg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        [XmlElement("sub_code")]
        public string SubErrCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary>
        [XmlElement("sub_msg")]
        public string SubErrMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                //return !string.IsNullOrEmpty(this.ErrCode) || !string.IsNullOrEmpty(this.SubErrCode);
                //2020年4月3日20:40:00下面的代码添加判断 但是这样做容易浪费内存  如果部署正式环境影响性能了  可以替换掉
                return !string.IsNullOrEmpty(this.ErrCode) || !string.IsNullOrEmpty(this.SubErrCode) || !string.IsNullOrEmpty(this.ErrMsg) || !string.IsNullOrEmpty(this.SubErrMsg);
            }
        }
    }
}
