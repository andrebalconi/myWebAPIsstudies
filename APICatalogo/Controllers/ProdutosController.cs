using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }
        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
        }
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"A produto com id={id} não foi encontrado");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível retornar produto com id={id}");
            }
            
        }
        [HttpPost]
        public ActionResult<Produto> Post([FromBody] Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }
        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, [FromBody]Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            //var produto = _uof.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); //only use if id is the primary key;

            if (produto == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return produto;//NoContent();
        }
    }
}
