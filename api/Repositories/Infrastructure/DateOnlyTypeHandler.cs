using System.Data;
using Dapper;

namespace api.Repositories.Infrastructure;

// Dapperに DateOnly の扱い方を教えるための専用通訳クラス
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    // DBに保存する時（DateOnly -> DateTime）のルール
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    // DBから取り出す時（DateTime -> DateOnly）のルール
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}