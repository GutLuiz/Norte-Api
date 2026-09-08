# Revisão — Requisitos Funcionais

## Visão geral da revisão

	- Todos os usuários participantes da revisão devem visualizar as informações do cadastro da revisão, como título, domínio, tipo, descrição, data de criação e status.

	- Os indicadores e resultados finais da revisão devem permanecer indisponíveis até o encerramento da votação.

	- Todos os participantes devem visualizar a lista de membros da revisão, incluindo nome, papel e status de participação.

	- A lista de membros deve identificar participantes ativos e participantes que saíram da revisão.

	- Somente o Proprietário pode convidar novos membros para a revisão.

	- Convites para entrada na revisão podem ser aceitos antes do início ou após o término da votação. Não deve ser permitido aceitar novos membros durante uma votação ativa.

	- Revisor, Avaliador e Colaborador podem sair da revisão a qualquer momento. A saída deve alterar seu status na lista de membros, sem apagar suas atividades já registradas.

	- Somente o Proprietário pode editar ou excluir a revisão.

## Analise de dados

	- Usuários com papel Proprietário ou Revisor podem importar arquivos de artigos para a revisão.

	- Somente o Proprietário pode resolver artigos duplicados identificados pelo sistema.

	- Todos os participantes da revisão podem visualizar os artigos no cabeçalho `Lista de artigos`.

	- Os artigos devem apresentar informações necessárias para o processo de revisão, seguindo como referência o fluxo do Rayyan.

	- Os cabeçalhos `Visão geral do progresso`, `Votação individual` e `Artigos em conflito` devem estar disponíveis para Proprietário, Revisor e Avaliador.

	- Usuários com papel Colaborador não devem visualizar os cabeçalhos `Visão geral do progresso`, `Votação individual` e `Artigos em conflito`.

	- A visão geral do progresso deve apresentar estatísticas individuais de participação de cada membro.

	- Durante a votação cega, a porcentagem individual de participação de cada membro pode ser exibida, desde que não revele suas decisões de voto.

	- Durante a votação cega, as decisões individuais de cada participante devem permanecer ocultas para os demais membros.

	- Quando todos os participantes ativos e elegíveis concluírem seus votos, a votação deve ser encerrada automaticamente.

	- Após o encerramento da votação, o sistema deve disponibilizar os resultados finais e identificar os artigos em conflito.

## Planilha
	- Os usuários devem poder adicionar informações complementares aos artigos incluídos.
	- Os usuários devem realizar uma nova etapa de votação para definir quais artigos farão parte da revisão final.
	- Os artigos devem ser classificados visualmente por cor:
	  - Verde: artigo incluído na revisão final.
	  - Vermelho: artigo excluído após análise complementar.
	- A classificação por cores e a nova etapa de votação não devem alterar os resultados da votação anterior.