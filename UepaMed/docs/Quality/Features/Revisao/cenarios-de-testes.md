# Revisão - Cenários de Testes
# CENARIO -> RESULTADO ESPERADO

- Premissas

Existem os papéis: Proprietário, Revisor, Avaliador e Colaborador.

Somente o Proprietário pode excluir uma revisão.

Revisor, Avaliador e Colaborador podem sair da revisão a qualquer momento.

Título, domínio e tipo de revisão são obrigatórios.

Descrição é opcional.

todos esses cenários o usuário deve estar autenticado:

- Cadastro da Revisão com sucesso:
	-> Registrar uma revisão com título, domínio e tipo válidos -> Deve ser criado com sucesso
- Validações do Registro:
	-> O Título não pode ser nulo -> mensagem "Título não pode ser nulo" 
	-> O Título não deve conter somente números -> mensagem "título não pode ser composto
	com somente números"
	-> O Título não pode ter caracteres especiais -> mensagem "título não aceita caracteres
	especiais"
	-> O Título deve conter menos de 10 caracteres -> mensagem "título no minímo 10 caracteres"
- Listar Revisões com sucesso:
	-> O sistema deve retornar a lista de revisões que aquele usuário participa
	independente do papel.
- Validações de edição:
	-> O usuário com papel propietário -> pode editar normalmente a revisão
	-> O usuário com papel diferente de propietário -> não pode editar a revisão
- Validações de exclusão:
	-> O usuário com papel propíetário -> deve excluir toda a revisão
	-> O usuário com papel diferente do proprietário -> deve somente sair da revisão e 
	a revisão deve continuar existindo
- Validação de Autenticação:
	-> Dado que não exista token válido na requisição
    Quando o usuário tentar criar, listar, editar, excluir ou sair de uma revisão
	a API deve negar o acesso.


	
