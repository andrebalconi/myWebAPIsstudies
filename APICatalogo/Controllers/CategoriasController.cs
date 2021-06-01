using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork contexto, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _context = contexto;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("==========Get Produtos===============");
                var categorias = await _context.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do Banco de Dados");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriaParameters)
        {
            var categorias = await _context.CategoriaRepository.GetCategorias(categoriaParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }

        [HttpGet("{id}", Name="ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria == null)
                {
                    _logger.LogInformation($"==========Get Error on id={id}===============");
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do Banco de Dados");
            }
            
        }
        /// <summary>
        /// Add a New Category
        /// </summary>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _context.CategoriaRepository.Add(categoria);
                await _context.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar uma nova Categoria");
            }
            
        }
        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Update(categoria);
                await _context.Commit();
                return Ok($"A categoria com id={id} foi atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a Categoria");
            }
            
        }
        /// <summary>
        /// Delete a Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            //var produto = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            try
            {
                var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id); //only use if id is the primary key;

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _context.CategoriaRepository.Delete(categoria);
                await _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir a Categoria de id={id}");
            }
            
        }

    }
}
