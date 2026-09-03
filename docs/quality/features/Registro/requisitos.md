# Registro de usuário - Requisitos:

- Requisitos Funcionais:
	= o usuário deve conseguir criar uma conta informando nome, email e senha.
	= Nome, e-mail e senha são campos obrigatórios.
	= O sistema deve aceitar somente um e-mail em formato válido.
	= O e-mail não pode estar vinculado a outra conta.
	= A senha deve possuir no mínimo 6 caracteres.
	= Ao concluir o registro, o sistema deve informar se a conta foi criada com sucesso.
	= Quando o registro não puder ser concluído, o sistema deve apresentar uma mensagem de erro adequada.
	= O Nome não deve conter numeros ou caracteres especiais
	= O nome deve possuir entre 2 e 50 caracteres.
	= O e-mail deve ser único independentemente de letras maiúsculas e minúsculas.
- Requisitos de segurança:
	= A senha nunca deve ser armazenada em texto puro.
	= A senha deve ser armazenada no banco utilizando um algoritmo seguro de hash.
	= A senha não deve aparecer em respostas da API, mensagens de erro ou logs.
