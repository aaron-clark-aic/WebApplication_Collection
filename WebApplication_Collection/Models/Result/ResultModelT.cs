using System.Collections.Generic;
using WebApplicationV.Utils;

namespace WebApplicationV.Models
{
    public class ResultModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResultState State { get; set; }
        
        private string desc;
        /// <summary>
        /// 状态码描述
        /// </summary>
        public string Desc
        {
            get
            {
                if (desc != null)
                {
                    return desc;
                }
                else
                {
                    return State.FormatState();
                }
            }
            set
            {
                desc = value;
            }
        }

        public string pinState { get; set; }

        public ResultModel() { }
        public ResultModel(bool success)
            : this(success, ResultState.UNKNOWN_ERR) { }
        /// <summary>
        /// 成功请直接调用不包含参数ResultState的构造函数
        /// </summary>
        /// <param name="success"></param>
        /// <param name="err"></param>
        public ResultModel(bool success, ResultState err)
        {
            this.State = success ? ResultState.SUCCESS : err;
        }

    }

    public class ResultModel<T> : ResultModel
    {
        public T Data { get; set; }

        public ResultModel() { }
        public ResultModel(bool success, T data) : base(success)
        {
            this.Data = data;
        }
        public ResultModel(bool success, ResultState err, T data)
            : base(success, err)
        {
            this.Data = data;
        }
    }

    public class ResultModelWithLimit<T> : ResultModel<IEnumerable<T>>
    {
        /// <summary>
        /// (设置列表)用于指示条目上限
        /// </summary>
        public int Limit { get; set; }
    }

    public class ResultModelWithRange<T> : ResultModel<IEnumerable<T>>
    {
        /// <summary>
        /// (查询分页)用于返回总条数
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// (查询分页)用于返回第一条索引
        /// </summary>
        public int? StartIndex { get; set; }

        /// <summary>
        /// (查询分页)用于返回最后一条索引
        /// </summary>
        public int? EndIndex { get; set; }
    }
}