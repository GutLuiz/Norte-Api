using System;
using System.Collections.Generic;
using System.Text;
using UepaMed.Domain.Entities.Artigos;


namespace UepaMed.Application.Interfaces.Artigos
{
    public interface IImportadorArtigos
    {
        bool Suporta(string extensao);

        Task<List<Artigo>> ImportarAsync(Stream arquivo);
    }
}