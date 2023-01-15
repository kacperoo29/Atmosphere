using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serialize.Linq.Serializers;

namespace Atmosphere.Services;

public class LinqSerializer<TDelegate> : IBsonSerializer<Expression<TDelegate>>
{
    public Type ValueType => typeof(Expression<TDelegate>);

    public Expression<TDelegate> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;
        if (bsonReader.CurrentBsonType == BsonType.Null)
        {
            bsonReader.ReadNull();
            return null;
        }

        var jsonString = bsonReader.ReadString();
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var expression = serializer.DeserializeText(jsonString);

        return (Expression<TDelegate>)expression;
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Expression<TDelegate> value)
    {
        var bsonWriter = context.Writer;
        if (value is null)
        {
            bsonWriter.WriteNull();
            return;
        }

        var serializer = new ExpressionSerializer(new JsonSerializer());
        var jsonString = serializer.SerializeText(value);
        bsonWriter.WriteString(jsonString);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        Serialize(context, args, (Expression<TDelegate>)value);
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }
}