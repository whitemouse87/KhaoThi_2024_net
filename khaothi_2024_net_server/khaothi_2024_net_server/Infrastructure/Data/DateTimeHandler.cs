using Dapper;
using System.Data;

namespace khaothi_2024_net_server.Infrastructure.TypeHandlers
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override DateTime Parse(object value)
        {
            return value == null ? DateTime.MinValue : (DateTime)value;
        }

        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = value;
        }
    }
}