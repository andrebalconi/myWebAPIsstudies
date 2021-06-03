using ApiCatalogo.Repository;
using GraphQL.Types;

namespace ApiCatalogo.GraphQL
{
    //Mapeamos os campos para uma dada consulta
    //para uma chamada no repositorio (CategoriasRepository)
    public class CategoriaQuery : ObjectGraphType
    {
        //recebe a instância do nosso UnitOfWork que contém 
        //as instãncias dos repositórios
        public CategoriaQuery(IUnitOfWork _context)
        {
            //nosso método vai retornar um objeto Categoria
            Field<CategoriaType>("categoria",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() { Name = "id" }),
                    resolve: context =>
                    {
                        var id = context.GetArgument<int>("id");
                        return _context.CategoriaRepository
                                       .GetById(c => c.CategoriaId == id);
                    });

            //nosso método vai retornar uma lista de objetos categoria
            // aqui resolve vai mapear a requisição do cliente com os dados 
            //da consulta Get definida em CategoriaRepository
            Field<ListGraphType<CategoriaType>>("categorias",
                resolve: context =>
                {
                    return _context.CategoriaRepository.Get();
                });
        }
    }
}
