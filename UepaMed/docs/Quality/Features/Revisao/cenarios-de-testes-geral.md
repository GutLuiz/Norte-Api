# Revisão - Visão Geral - Cenários de Testes
# CENARIO -> RESULTADO ESPERADO
- VISÃO GERAL:
	- INFORMAÇÕES DA REVISÃO:
		-> Usuário abre uma revisão específica -> deve ser aprensentado o 
		visão geral com informações da revisão, resumo dos dados e membros da revisão
		-> informações da revisão -> Somente a descrição pode estar nula
	- RESUMO DOS DADOS:
		 -> Usuário abre a revisão de dados -> mostrar valores dos arquivos e artigos
		 -> Durante a votação cega, devem ser exibidos apenas os indicadores
		 gerais permitidos, como total de artigos, arquivos importados e pendentes.
	- Membros da revisão: 
		 -> Somente membro que estão na revisão devem aparecer na lista.
		 -> Deve conter todas informações do usuário (nome, email, papel, progresso da votação (se for do papel dele)) 
		 -> Se o usuário sair da revisão ele deve sair tambem da lista de membros
	- Convite:
		 -> somente proprietário pode adicionar um membro
		 -> o convite só pode ser aceito se a revisão não estiver em fase de votação
		 -> o proprietário não pode convidar membros que já estão na revisão ou ele mesmo
		 -> O convite só pode ser feito usuários que já tem login no sistema

