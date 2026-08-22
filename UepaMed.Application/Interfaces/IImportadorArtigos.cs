using System;
using System.Collections.Generic;
using System.Text;


namespace UepaMed.Application.Interfaces
{
    public interface IImportadorArtigos
    {
        bool Suporta(string extensao);

        Task<List<Artigo>> ImportarAsync(Stream arquivo);
    }
}