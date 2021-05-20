﻿using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
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
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("==========Get Produtos===============");
                var categorias = _context.CategoriaRepository.GetCategoriasProdutos().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do Banco de Dados");
            }
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _context.CategoriaRepository.Get().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }

        [HttpGet("{id}", Name="ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
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
        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _context.CategoriaRepository.Add(categoria);
                _context.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar uma nova Categoria");
            }
            
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Update(categoria);
                _context.Commit();
                return Ok($"A categoria com id={id} foi atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a Categoria");
            }
            
        }
        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            //var produto = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            try
            {
                var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id); //only use if id is the primary key;

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _context.CategoriaRepository.Delete(categoria);
                _context.Commit();

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
