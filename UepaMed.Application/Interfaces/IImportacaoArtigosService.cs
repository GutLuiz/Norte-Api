using System;
using System.Collections.Generic;
using System.Text;

namespace UepaMed.Application.Interfaces
{
    public interface IImportacaoArtigosService
    {
        Task ImportarAsync(int revisaoId, Stream arquivo, string nomeArquivo);
    }
}
