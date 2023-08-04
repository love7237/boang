using Microsoft.Extensions.Options;
using System;

/* 
 * 雪花Id算法：https://www.cnblogs.com/sunyuliang/p/12161416.html
 * 
 * 0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
 * 
 *  1位符号标识，用于Java兼容，始终为0
 * 41位时间序列，精确到毫秒，存储的是时间戳差值(当前时间戳 - 起始时间戳)，可以使用69年，计算公式：(1L << 41) / (1000L * 60 * 60 * 24 * 365)
 * 10位机器标识，包括5位centerId和5位workerId，支持分布式部署在1024个节点
 * 12位计数序列，是毫秒时间序列内的自增计数，支持每个节点每毫秒产生4096个序号
 * 
 * 加起来64位，对应long型
 */

namespace T.Utility.Snowflake
{
    /// <summary>
    /// 雪花Id辅助类
    /// </summary>
    public class SnowflakeHelper
    {
        /// <summary>
        /// 起始时间戳(2021-01-01 00:00:00 距unix初始时间的毫秒数)
        /// </summary>
        private const long _initialTimestamp = 1609430400000L;

        /// <summary>
        /// 数据中心Id位数
        /// </summary>
        private const int _centerIdBits = 5;

        /// <summary>
        /// 数据节点Id位数
        /// </summary>
        private const int _workerIdBits = 5;

        /// <summary>
        /// 数据中心Id取值范围[0,2^_centerIdBits]
        /// </summary>
        private const long _maxCenterId = -1L ^ (-1L << _centerIdBits);

        /// <summary>
        /// 数据节点Id取值范围[0,2^_clientIdBits]
        /// </summary>
        private const long _maxWorkerId = -1L ^ (-1L << _workerIdBits);

        /// <summary>
        /// 毫秒内计数序列位数
        /// </summary>
        private const int _sequenceBits = 12;

        /// <summary>
        /// 数据中心Id左移位数
        /// </summary>
        private const int _centerIdShift = _sequenceBits + _workerIdBits;

        /// <summary>
        /// 节点Id左移位数
        /// </summary>
        private const int _workerIdShift = _sequenceBits;

        /// <summary>
        /// 时间戳左移位数
        /// </summary>
        private const int _timestampLeftShift = _sequenceBits + _workerIdBits + _centerIdBits;

        /// <summary>
        /// 毫秒内计数序列掩码(0b111111111111=0xfff=4095)
        /// </summary>
        private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);

        /// <summary>
        /// 数据中心Id
        /// </summary>
        private long _centerId { get; set; } = 0;

        /// <summary>
        /// 数据节点Id
        /// </summary>
        private long _workerId { get; set; } = 0;

        /// <summary>
        /// 毫秒内计数
        /// </summary>
        private long _sequence { get; set; } = 0;

        /// <summary>
        /// 上次生成Id的时间戳
        /// </summary>
        private long _lastSequenceTimestamp { get; set; } = 0;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _o = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings"></param>
        public SnowflakeHelper(SnowflakeSettings settings)
        {
            if (settings.CenterId < 0 || settings.CenterId > _maxCenterId)
            {
                throw new ArgumentOutOfRangeException(nameof(settings));
            }
            if (settings.WorkerId < 0 || settings.WorkerId > _maxWorkerId)
            {
                throw new ArgumentOutOfRangeException(nameof(settings));
            }

            this._centerId = settings.CenterId;
            this._workerId = settings.WorkerId;
        }

        /// <summary>
        /// 根据前次生成的Id修正起始时间戳
        /// </summary>
        /// <param name="id"></param>
        public void FixLastTimestamp(long id)
        {
            var timestamp = id >> _timestampLeftShift;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp + _initialTimestamp);

            this._lastSequenceTimestamp = dateTimeOffset.ToUnixTimeMilliseconds() + 1;
        }

        /// <summary>
        /// 生成一个雪花
        /// </summary>
        /// <returns></returns>
        public Flake Next()
        {
            lock (_o)
            {
                var currentTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (currentTimestamp > _lastSequenceTimestamp)
                {
                    //时间戳增长,毫秒内序列重置
                    _sequence = 0L;
                }
                else if (currentTimestamp == _lastSequenceTimestamp)
                {
                    //时间戳不变,毫秒内序列递增
                    _sequence = (_sequence + 1) & _sequenceMask;

                    //毫秒内序列溢出
                    if (_sequence == 0)
                    {
                        //阻塞到下一个毫秒,获得新的时间戳
                        currentTimestamp = GetNextTimestamp(_lastSequenceTimestamp);
                    }
                }
                else
                {
                    //时间戳变小,系统时钟被回拨
                    _sequence = (_sequence + 1) & _sequenceMask;

                    if (_sequence > 0)
                    {
                        //毫秒内序列递增,则停留在最后一次时间戳，等待系统时间追赶
                        currentTimestamp = _lastSequenceTimestamp;
                    }
                    else
                    {
                        //毫秒内序列溢出,则进位到下一个毫秒
                        currentTimestamp = _lastSequenceTimestamp + 1;
                    }
                }

                //更新上次生成时间
                _lastSequenceTimestamp = currentTimestamp;

                var flake = new Flake()
                {
                    Id = (currentTimestamp - _initialTimestamp << _timestampLeftShift) | (_centerId << _centerIdShift) | (_workerId << _workerIdShift) | _sequence,
                    CenterId = this._centerId,
                    WorkerId = this._workerId,
                    DateTime = DateTimeOffset.FromUnixTimeMilliseconds(currentTimestamp).LocalDateTime,
                    Sequence = this._sequence
                };

                return flake;
            }
        }

        /// <summary>
        /// 以阻塞的方式获取下一个时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private long GetNextTimestamp(long lastTimestamp)
        {
            long currentTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            while (currentTimestamp <= lastTimestamp)
            {
                currentTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            return currentTimestamp;
        }

        /// <summary>
        /// 解析Id的时间信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DateTime AnalyzeDateTime(long id)
        {
            var timestamp = id >> _timestampLeftShift;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp + _initialTimestamp);

            return dateTimeOffset.LocalDateTime;
        }

        /// <summary>
        /// 解析Id信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Flake Analyze(long id)
        {
            var flake = new Flake() { Id = id };

            var timestamp = id >> _timestampLeftShift;
            flake.DateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp + _initialTimestamp).LocalDateTime;

            var centerId = (id ^ (timestamp << _timestampLeftShift)) >> _centerIdShift;
            flake.CenterId = centerId;

            var clientId = (id ^ ((timestamp << _timestampLeftShift) | (centerId << _centerIdShift))) >> _workerIdShift;
            flake.WorkerId = clientId;

            var sequence = id & _sequenceMask;
            flake.Sequence = sequence;

            return flake;
        }

        /// <summary>
        /// 计算日期所在的时间区间的Id最小最大值
        /// </summary>
        /// <param name="date">本地时间</param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public (long MinimumId, long MaximumId) GetIdentiferRange(DateTime date, TimeInterval interval)
        {
            long minTimestamp = 0L;
            long maxTimestamp = 0L;

            switch (interval)
            {
                case TimeInterval.Year:
                    minTimestamp = new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    maxTimestamp = new DateTimeOffset(date.Year, 12, 31, 23, 59, 59, 999, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    break;

                case TimeInterval.Month:
                    minTimestamp = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    maxTimestamp = new DateTimeOffset(date.Year, date.Month, date.AddMonths(1).AddDays(-date.Day).Day, 23, 59, 59, 999, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    break;

                case TimeInterval.Day:
                    minTimestamp = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    maxTimestamp = new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, 999, TimeSpan.Zero).ToUnixTimeMilliseconds();
                    break;
            }

            long offset = (long)TimeZoneInfo.Local.BaseUtcOffset.TotalMilliseconds;

            long minimumId = (minTimestamp - _initialTimestamp - offset << _timestampLeftShift) | (_centerId << _centerIdShift) | (_workerId << _workerIdShift) | 0;
            long maximumId = (maxTimestamp - _initialTimestamp - offset << _timestampLeftShift) | (_centerId << _centerIdShift) | (_workerId << _workerIdShift) | -1L ^ (-1L << _sequenceBits);

            return (minimumId, maximumId);
        }

        /// <summary>
        /// 计算时间区间的Id取值
        /// </summary>
        /// <param name="minDateTime"></param>
        /// <param name="maxDateTime"></param>
        /// <returns></returns>
        public (long MinimumId, long MaximumId) GetIdentiferRange(DateTime minDateTime, DateTime maxDateTime)
        {
            long minTimestamp = new DateTimeOffset(minDateTime).ToUnixTimeMilliseconds();
            long maxTimestamp = new DateTimeOffset(maxDateTime).ToUnixTimeMilliseconds();

            long offset = (long)TimeZoneInfo.Local.BaseUtcOffset.TotalMilliseconds;

            long minimumId = (minTimestamp - _initialTimestamp - offset << _timestampLeftShift) | (_centerId << _centerIdShift) | (_workerId << _workerIdShift) | 0;
            long maximumId = (maxTimestamp - _initialTimestamp - offset << _timestampLeftShift) | (_centerId << _centerIdShift) | (_workerId << _workerIdShift) | -1L ^ (-1L << _sequenceBits);

            return (minimumId, maximumId);
        }
    }

    /// <summary>
    /// 时间区间
    /// </summary>
    public enum TimeInterval
    {
        /// <summary>
        /// 年
        /// </summary>
        Year,

        /// <summary>
        /// 月
        /// </summary>
        Month,

        /// <summary>
        /// 日
        /// </summary>
        Day,
    }
}
