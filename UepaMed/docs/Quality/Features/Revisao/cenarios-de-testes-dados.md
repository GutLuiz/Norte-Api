# Revisão - Análise de Dados - Cenários de testes
# Cenário -> Resultado esperado
- Análise de Dados:
	-> Usuário tenta importar algum arquivo na revisão:
		-> Somente usuário que tem o papel de PROPRIETÁRIO E REVIVOR podem concluir a importação
	-> Usuário tenta detectar duplicados:
		-> Somente usuário que tem o papel de PROPRIETÁRIO deve conseguir detectar duplicadas
	-> Todos os usuários membros da revisão podem navegar pela lista de artigo
	-> Todos os usuários membros da revisão podem abrir o modal de algum artigo
	-> Membros com papel COLABORADOR:
		-> Deve enxergar somente o cabeçalho de lista de artigos
	-> Membros com papel diferente de COLABORADOR:
		-> Deve enxergar todos os cabeçalhos, inclusive o de votação
	-> Cabeçalho de visão geral do progresso:
		-> Deve enxergar todo o histórico de votação INDIVIDUAL do usuário
	-> Membros que participam da votação:
	    -> Só pode iniciar a votação se houver um proprietário definido.
		-> Só pode iniciar uma votação se houver artigos importados.
        -> Só pode iniciar uma votação com revisor envolvido se tiver um avaliador na revisão
		-> Só pode iniciar uma votação com avaliador se tiver um revisor envolvido
		-> O proprietário pode inciar uma votação sozinho
		-> Somente o PROPRIETÁRIO pode começar uma votação
		-> Somente PROPRIETÁRIO e REVISOR podem votar
		-> O voto tem que ser unico para cada artigo
		-> A votação precisar ser cega, pois nenhum usuário vai saber a votação do outro
		-> Com os participantes válidos definidos, o resultado é por maioria simples: 
		vence a opção com mais votos.
		-> A votação acaba quando todos os membros que participam da votação votarem
		-> Sistema deve separar os artigos em conflito
	-> Membros que participam dos artigos em conflito:
		-> Somente o avaliador deve finalizar os artigos em conflito
	-> Lista de artigos apos o final da votação
		-> mostra na lista os artigos incluidos, excluidos ou todos por meio de filtro

