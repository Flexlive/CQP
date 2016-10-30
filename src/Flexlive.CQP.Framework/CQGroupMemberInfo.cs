using System;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 群成员信息。
    /// </summary>
    public class CQGroupMemberInfo
    {
        
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        public CQGroupMemberInfo()
        {
            this.RefreshDate = DateTime.MinValue;
            this.QQNumber = 0;
            this.HasBadRecord = false;
            this.LevelName = String.Empty;
            this.Area = String.Empty;
            this.Authority = "成员";
            this.JoinTime = DateTime.MinValue;
            this.GroupCard = String.Empty;
            this.QQName = String.Empty;
            this.Age = 0;
            this.GroupNumber = 0;
            this.Gender = "保密";
            this.CanModifyVestCard = false;
            this.GroupHonor = String.Empty;
            this.HonorExpirationTimes = 0;
            this.LastSpeakingTime = DateTime.MinValue;
        }

        /// <summary>
        /// 获取群号。
        /// </summary>
        public long GroupNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 获取群号。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：GroupNumber")]
        public long 群号
        {
            get
            {
                return this.GroupNumber;
            }
            set
            {
                this.GroupNumber = value;
            }
        }

        /// <summary>
        /// 获取QQ号码
        /// </summary>
        public long QQNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 获取昵称。
        /// </summary>
        public string QQName
        {
            get;
            set;
        }

         /// <summary>
        /// 获取昵称。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：QQName")]
        public string 昵称
        {
            get
            {
                return this.QQName;
            }
            set
            {
                this.QQName = value;
            }
        }

        /// <summary>
        /// 获取群名称。
        /// </summary>
        public string GroupCard
        {
            get;
            set;
        }

        /// <summary>
        /// 获取群名称。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：GroupCard")]
        public string 名片
        {
            get
            {
                return this.GroupCard;
            }
            set
            {
                this.GroupCard = value;
            }
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        public string Gender
        {
            get;
            set;
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：Gender")]
        public string 性别
        {
            get
            {
                return this.Gender;
            }
            set
            {
                this.Gender = value;
            }
        }

        /// <summary>
        /// 获取年龄。
        /// </summary>
        public int Age
        {
            get;
            set;
        }

        /// <summary>
        /// 获取年龄。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：Age")]
        public int 年龄
        {
            get
            {
                return this.Age;
            }
            set
            {
                this.Age = value;
            }
        }

        /// <summary>
        /// 获取加群时间。
        /// </summary>
        public DateTime JoinTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取加群时间。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：JoinTime")]
        public DateTime 加群时间
        {
            get
            {
                return this.JoinTime;
            }
            set
            {
                this.JoinTime = value;
            }
        }

        /// <summary>
        /// 获取最后发言。
        /// </summary>
        public DateTime LastSpeakingTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取最后发言。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：LastSpeakingTime")]
        public DateTime 最后发言
        {
            get
            {
                return this.LastSpeakingTime;
            }
            set
            {
                this.LastSpeakingTime = value;
            }
        }

        /// <summary>
        /// 获取管理权限 成员；管理员；群主。
        /// </summary>
        public string Authority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取管理权限 成员；管理员；群主。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：Authority")]
        public string 管理权限
        {
            get
            {
                return this.Authority;
            }
            set
            {
                this.Authority = value;
            }
        }

        /// <summary>
        /// 获取专属头衔。
        /// </summary>
        public string GroupHonor
        {
            get;
            set;
        }

        /// <summary>
        /// 获取专属头衔。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：GroupHonor")]
        public string 专属头衔
        {
            get
            {
                return this.GroupHonor;
            }
            set
            {
                this.GroupHonor = value;
            }
        }

        /// <summary>
        /// 获取地区。
        /// </summary>
        public string Area
        {
            get;
            set;
        }

        /// <summary>
        /// 获取地区。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：Area")]
        public string 地区
        {
            get
            {
                return this.Area;
            }
            set
            {
                this.Area = value;
            }
        }

        /// <summary>
        /// 获取等级名称。
        /// </summary>
        public string LevelName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取等级名称。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：LevelName")]
        public string 等级名称
        {
            get
            {
                return this.LevelName;
            }
            set
            {
                this.LevelName = value;
            }
        }

        /// <summary>
        /// 获取不良记录成员。
        /// </summary>
        public bool HasBadRecord
        {
            get;
            set;
        }

        /// <summary>
        /// 获取不良记录成员。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：HasBadRecord")]
        public bool 不良记录成员
        {
            get
            {
                return this.HasBadRecord;
            }
            set
            {
                this.HasBadRecord = value;
            }
        }

        /// <summary>
        /// 获取允许修改名片。
        /// </summary>
        public bool CanModifyVestCard
        {
            get;
            set;
        }

        /// <summary>
        /// 获取允许修改名片。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：CanModifyVestCard")]
        public bool 允许修改名片
        {
            get
            {
                return this.CanModifyVestCard;
            }
            set
            {
                this.CanModifyVestCard = value;
            }
        }

        /// <summary>
        /// 获取专属头衔过期时间(以秒为单位）。
        /// </summary>
        public int HonorExpirationTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 获取专属头衔过期时间(以秒为单位）。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：HonorExpirationTimes")]
        public int 专属头衔过期时间
        {
            get
            {
                return this.HonorExpirationTimes;
            }
            set
            {
                this.HonorExpirationTimes = value;
            }
        }

        /// <summary>
        /// 数据刷新时间。
        /// </summary>
        internal DateTime RefreshDate
        {
            get;
            set;
        }
    }
}
