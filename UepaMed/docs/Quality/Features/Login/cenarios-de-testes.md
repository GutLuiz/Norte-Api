# Login de usuário - Cenários de teste 
# CENARIO -> RESULTADO ESPERADO

- Login realizado com sucesso:
	-> Informar e-mail e senha de um usuário cadastrado → Login concluído, mensagem de sucesso e 
	informações não sensíveis retornadas.
- Validações do e-mail:
	-> Realizar login sem informar o e-mail → Login rejeitado.
	-> Informar e-mail nulo → Login rejeitado.
	-> Informar e-mail em formato inválido → Login rejeitado.
	-> Informar e-mail não cadastrado → Login rejeitado com a mensagem “E-mail ou senha inválidos”.
	-> Informar e-mail cadastrado utilizando letras maiúsculas → Login realizado normalmente.
- Validações da senha:
	-> Realizar login sem informar a senha → Login rejeitado.
	-> Informar senha nula → Login rejeitado.
	-> Informar senha incorreta → Login rejeitado com a mensagem “E-mail ou senha inválidos”.
- Validações das credenciais:
	-> Informar e-mail não cadastrado e uma senha qualquer → Login rejeitado com a 
	mensagem genérica “E-mail ou senha inválidos”.
	-> Informar e-mail cadastrado e senha incorreta → Login rejeitado com a mesma 
	mensagem genérica “E-mail ou senha inválidos”.
	-> Examinar uma tentativa malsucedida → Nenhum token ou cookie de autenticação é criado.
	- Validações do token e cookie:
	-> Realizar login com credenciais válidas → Token válido enviado no 
	cookie de autenticação pelo cabeçalho Set-Cookie.
	-> Examinar o cookie de autenticação → Cookie contém os atributos HttpOnly, 
	Secure, SameSite e tempo de expiração.
	-> Utilizar o cookie em um endpoint protegido → Acesso autorizado.
	-> Acessar um endpoint protegido sem o cookie → Acesso rejeitado.
    -> Utilizar um token inválido ou alterado → Acesso rejeitado.
    -> Utilizar o cookie após sua expiração → Acesso rejeitado.
- Validações de segurança:
	-> Examinar o corpo da resposta do login → Resposta não contém token, senha nem hash.
	-> Examinar as informações do usuário retornadas → Resposta contém somente informações não sensíveis.
	-> Comparar as respostas para e-mail inexistente e senha incorreta → Ambas retornam a mesma mensagem genérica.
	-> Examinar os logs durante o login → Logs não contêm senha, hash nem token.