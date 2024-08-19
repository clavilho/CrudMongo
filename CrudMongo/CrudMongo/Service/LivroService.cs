using CrudMongo.Entidade;
using CrudMongo.Exceptions;
using MongoDB.Driver;

namespace CrudMongo.Service;

public class LivroService
{
    private readonly IMongoCollection<Livro> _livrosCollection;

    public LivroService()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("BookStore");
        _livrosCollection = mongoDatabase.GetCollection<Livro>("Books");
    }

    public async Task RegistrarLivro(Livro newBook)
    {
        try
        {
            var livroCadastro = await _livrosCollection.FindAsync(x => x.Nome == newBook.Nome);

            if (livroCadastro.Any())
            {
                throw new DuplicateException("O livro que esta tentando cadastrar ja existe na base de dados");
            }

            await _livrosCollection.InsertOneAsync(newBook);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<Livro>> BuscarTodosLivros()
    {
        try
        {
            var livros = await _livrosCollection.Find(Builders<Livro>.Filter.Empty).ToListAsync();

            return livros;
        }
        catch (Exception ec)
        {

            throw;
        }

    }

    public async Task<List<Livro>> BuscarLivroPorAutor(string nomeAutor)
    {
        try
        {
            var livros = await _livrosCollection.Find(x => x.Autor == nomeAutor).ToListAsync();

            return livros;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task DeletarLivroDaBase(string nomeLivro)
    {
        try
        {
            var livroCadastro = await _livrosCollection.Find(x => x.Nome == nomeLivro).AnyAsync();

            if (!livroCadastro)
            {
                throw new NotFoundException("Esse livro que deseja excluir não existe na base");
            }

            var resultado = await _livrosCollection.DeleteManyAsync(x => x.Nome == nomeLivro);

            if (resultado.DeletedCount == 0)
            {
                throw new Exception("O livro não pôde ser excluído, verifique se os critérios estão corretos.");
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
}
