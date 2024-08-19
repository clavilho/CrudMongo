using CrudMongo.Entidade;
using CrudMongo.Exceptions;
using CrudMongo.Service;
using Microsoft.AspNetCore.Mvc;

namespace CrudMongo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LivrosController : Controller
{
    private readonly LivroService service;

    public LivrosController(LivroService service)
    {
        this.service = service;
    }

    [HttpPost]
    [Route("CadastrarLivro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post(Livro newBook)
    {
        try
        {
            await service.RegistrarLivro(newBook);

            return CreatedAtAction(null, newBook);
        }
        catch (DuplicateException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpGet]
    [Route("BuscarTodosOsLivros")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarTodosOsLivros()
    {
        try
        {
            var livros = await service.BuscarTodosLivros();

            if (livros == null || livros.Count == 0)
                return NotFound("Não foi encontrado nenhum livro");

            return Ok(livros);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet]
    [Route("BuscarLivroPorAutor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarLivrosPorAutor(string nomeAutor)
    {
        try
        {
            var livros = await service.BuscarLivroPorAutor(nomeAutor);

            if (livros == null || livros.Count == 0)
                return NotFound("Não foi encontrado nenhum livro para esse autor");

            return Ok(livros);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


    [HttpDelete]
    [Route("DeletarLivroPorNome")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletarLivroDaBase(string nomeLivro)
    {
        try
        {
            await service.DeletarLivroDaBase(nomeLivro);
            return Ok($"O livro {nomeLivro} foi deletado com sucesso");
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }

}
