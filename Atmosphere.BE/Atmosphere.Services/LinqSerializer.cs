using System.Linq.Expressions;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Remote.Linq;
using Remote.Linq.Text.Json;

namespace Atmosphere.Services;

public class LinqSerializer<TDelegate> : IBsonSerializer<Expression<TDelegate>>
{
    private static JsonSerializerOptions jsonSerializerOptions =
        new JsonSerializerOptions().ConfigureRemoteLinq();
    public Type ValueType => typeof(Expression<TDelegate>);

    public Expression<TDelegate> Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args
    )
    {
        var bsonReader = context.Reader;
        if (bsonReader.CurrentBsonType == BsonType.Null)
        {
            bsonReader.ReadNull();
            return null;
        }

        var binaryData = bsonReader.ReadBinaryData();
        var remote = JsonSerializer.Deserialize<Remote.Linq.Expressions.LambdaExpression>(
            binaryData.Bytes,
            jsonSerializerOptions
        );

        return (Expression<TDelegate>)remote.ToLinqExpression();
    }

    public void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        Expression<TDelegate> value
    )
    {
        var bsonWriter = context.Writer;
        if (value is null)
        {
            bsonWriter.WriteNull();
            return;
        }

        var remote = value.ToRemoteLinqExpression();
        bsonWriter.WriteBinaryData(
            JsonSerializer.SerializeToUtf8Bytes(remote, jsonSerializerOptions)
        );
    }

    public void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        object value
    )
    {
        Serialize(context, args, (Expression<TDelegate>)value);
    }

    object IBsonSerializer.Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args
    )
    {
        return Deserialize(context, args);
    }
}
