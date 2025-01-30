using Dapper;
using System.Data;

namespace khaothi_2024_net_server.Infrastructure.Data
{
    public class NullableDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
    {
        public override DateTime? Parse(object value)
        {
            return value == DBNull.Value ? null : (DateTime?)value;
        }

        public override void SetValue(IDbDataParameter parameter, DateTime? value)
        {
            parameter.Value = value.HasValue ? (object)value.Value : DBNull.Value;
        }
    }
}
