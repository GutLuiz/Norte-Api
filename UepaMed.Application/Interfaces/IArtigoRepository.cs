using System;
using System.Collections.Generic;
using System.Text;

namespace UepaMed.Application.Interfaces
{
    public interface IArtigoRepository
    {
        Task AdicionarAsync(Artigo artigo);

        Task<List<Artigo>> ObterPorRevisaoAsync(int revisaoId);
    }
}
