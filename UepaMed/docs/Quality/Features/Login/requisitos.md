# Login do usuário - Requisitos:

- Requisitos funcionais:
	- o usuário deve conseguir logar em uma conta com email e senha ja cadastrados na base de dados
	- email e senha são campos obrigatórios
	- o sistema deve aceitar somente email que tenha cadastro
	- Ao concluir o login, o sistema deve informar que o login foi realizado com sucesso.
	- Quando o login não puder ser concluído, o sitema deve mostrar um erro generico "E-mail ou senha inválidos".
	- Resposta não contém senha nem hash.
	- Após autenticação bem-sucedida, o backend deve enviar um token válido
    no cookie de autenticação, por meio do cabeçalho Set-Cookie.
    - O cookie de autenticação deve possuir os atributos HttpOnly, Secure,
    SameSite e tempo de expiração.
    - O corpo da resposta deve conter somente informações não sensíveis
    do usuário.
    - O corpo da resposta não deve conter token, senha ou hash da senha.
	- O logout deve remover os cookies access_token e refresh_token do navegador.
    - O logout deve invalidar o refresh token associado ao usuário no banco de dados.
    - Após o logout, o usuário não deve conseguir acessar endpoints protegidos.
    - Após o logout, não deve ser possível gerar novos tokens usando o refresh token anterior.