# Registro de usuário - Cenários de teste 
# CENARIO -> RESULTADO ESPERADO

- Cadastro com sucesso:
	-> Registrar usuário com nome, email e senha validos -> conta criada e mensagem de sucesso 
- Validações do nome:
	-> Registrar sem informar o nome -> Cadastro rejeitado e mensagem de erro 
	-> informar nome com 1 caractere -> Cadastro rejeitado e mensagem de erro especifico no campo nome 
	-> informar nome com 2 caractere -> Cadastro rejeitado e mensagem de erro especifico no campo nome 
	-> informar nome com 51 caracteres -> cadastro rejeitado e mensagem de erro especifico no campo nome
	-> informar nome contendo numero -> cadastro rejeitado com mensagem de erro especifico no campo
- Validações do email:
	-> Registrar sem informar o email -> Cadastro rejeitado e erro no campo do email
	-> Informar email em formato invalido -> cadastro rejeitado
	-> informar email ja cadastrado -> cadastro rejeitado com mensagem especifica 
	-> Informar email existrente com letras maiusculas -> cadastro rejeitadao como email duplicado
- Validações de senha:
	-> Registrar sem informar a senha -> cadastro rejeitado no campo senha
	-> Informar senha com 5 caracteres -> Cadastro rejeitado
- validações de segurança:
	-> Consultar o usuário criado no banco -> senha armazenada somente com hash
	-> Examinar a resposta do cadastro -> Resposta não contem senha nem hash
	-> Examinar os logs durante o cadastro -> log não contem senha nem hash

