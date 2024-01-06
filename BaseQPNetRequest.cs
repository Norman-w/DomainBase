using System;
using System.Collections.Generic;
using System.Reflection;

namespace DomainBase
{
    /// <summary>
    /// 基础TOP请求类，存放一些通用的请求参数。
    /// </summary>
    public abstract class BaseRequest<T> : IQPNetRequest<T> where T : NetResponse
    {

        /// <summary>
        /// HTTP请求URL参数
        /// </summary>
        internal QPNetDictionary otherParams;
        /// <summary>
        /// HTTP请求头参数
        /// </summary>
        private QPNetDictionary headerParams;
        /// <summary>
        /// 请求目标AppKey
        /// </summary>
        private string targetAppKey;

        /// <summary>
        /// 批量API请求的用户授权码
        /// </summary>
        private string batchApiSession;

        /// <summary>
        /// API在批量调用中的顺序
        /// </summary>
        private int batchApiOrder;

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParams == null)
            {
                this.otherParams = new QPNetDictionary();
            }
            this.otherParams.Add(key, value);
        }
        /// <summary>
        /// 2021年11月30日11:40:28  获取添加的别的参数
        /// </summary>
        /// <returns></returns>
        public QPNetDictionary GetOtherParameters()
        {
            return this.otherParams;
        }

        public void AddHeaderParameter(string key, string value)
        {
            GetHeaderParameters().Add(key, value);
        }

        public IDictionary<string, string> GetHeaderParameters()
        {
            if (this.headerParams == null)
            {
                this.headerParams = new QPNetDictionary();
            }
            return this.headerParams;
        }

        public string GetTargetAppKey()
        {
            return this.targetAppKey;
        }

        public void SetTargetAppKey(string targetAppKey)
        {
            this.targetAppKey = targetAppKey;
        }

        public string GetBatchApiSession()
        {
            return this.batchApiSession;
        }

        public void SetBatchApiSession(string session)
        {
            this.batchApiSession = session;
        }

        public int GetBatchApiOrder()
        {
            return this.batchApiOrder;
        }

        public void SetBatchApiOrder(int order)
        {
            this.batchApiOrder = order;
        }

        public abstract string GetApiName();

        public abstract bool GetIsSessionRequired();

        public abstract void Validate();

        public abstract IDictionary<string, string> GetParameters();


        /// <summary>
        /// 从内存中分配一个此请求对应的Response实例
        /// </summary>
        /// <returns></returns>
        /// 

        #region 我重新拓展的方法
        public T AllocResponse()
        {
            T ret = Activator.CreateInstance(typeof(T)) as T;
            return ret;
        }
        public string GetFieldsAll(Type type)
        {
            System.Text.StringBuilder fields = new System.Text.StringBuilder();
            System.Reflection.PropertyInfo[] infos = type.GetProperties();
            bool hasvalue = false;
            for (int i = 0; i < infos.Length; i++)
            {
                if (hasvalue)
                {
                    fields.Append(",");
                }
                fields.Append(infos[i].Name);
                hasvalue = true;
            }
            return fields.ToString();
        }
        public string GetFieldsWhithoutGeneric(Type type)
        {
            System.Text.StringBuilder fields = new System.Text.StringBuilder();
            System.Reflection.PropertyInfo[] infos = type.GetProperties();
            bool hasvalue = false;
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].PropertyType.IsGenericType)
                {
                    continue;
                }
                if (hasvalue)
                {
                    fields.Append(",");
                }
                fields.Append(infos[i].Name);
                hasvalue = true;
            }
            return fields.ToString();
        }
        public string GetFieldsAllDefault(Type type)
        {
            System.Text.StringBuilder fields = new System.Text.StringBuilder();
            System.Reflection.PropertyInfo[] infos = type.GetProperties();
            bool hasvalue = false;
            for (int i = 0; i < infos.Length; i++)
            {
                if (IsCustomType(infos[i].PropertyType))
                {
                    continue;
                }
                if (hasvalue)
                {
                    fields.Append(",");
                }
                fields.Append(infos[i].Name);
                hasvalue = true;
            }
            return fields.ToString();
        }
        bool IsCustomType(Type type)
        {
            return (type != typeof(object) && Type.GetTypeCode(type) == TypeCode.Object);
        }


        #region 使用反射的方式直接构建一个param的表
        /// <summary>
        /// 获取指定类对象的param的表,字符形式的,根据公共字段,自动的.
        /// </summary>
        /// <param name="whose"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetParametersDicByPublicFieldAuto(object whose)
        {
            QPNetDictionary param = new QPNetDictionary();
            PropertyInfo[] ps = whose.GetType().GetProperties();
            if (param != null)
            {
                foreach (var p in ps)
                {
                    var o = p.GetValue(this, null);
                    param.Add(p.Name.ToLower(), o);
                }
            }
            param.AddAll(this.GetOtherParameters());
            return param;
        }
        #endregion
        #endregion

    }
}
