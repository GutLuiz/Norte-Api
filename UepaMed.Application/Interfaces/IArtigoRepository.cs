using System;
using System.Collections.Generic;
using System.Text;
using UepaMed.Domain.Entities;

namespace UepaMed.Application.Interfaces
{
    public interface IArtigoRepository
    {
        Task AdicionarAsync(Artigo artigo);

        Task<List<Artigo>> ObterPorRevisaoAsync(int revisaoId);
    }
}
