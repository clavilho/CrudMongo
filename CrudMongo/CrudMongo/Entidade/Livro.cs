using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CrudMongo.Entidade;

public class Livro
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string? Id { get; set; }
    public string Nome { get; set; }
    public string Autor { get; set; }
    public int NumeroPaginas { get; set; }
}
