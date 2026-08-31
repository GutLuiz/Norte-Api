# Norte-Api


# SOLUÇÃO RESUMO:
	 = Sistema que atende alunos que buscam uma organização e modernidade para criação de um revisão (por enquanto so revisão)
	 = API feita esta usando essas tecnologias e ferramentas:
		- ASP.NET para criação dos endpoints web
		- Arquitetura DDD (Domain-Driven Design) 
		- Entity framework para conexão e querys com banco
		- Banco de dados POSTGRES 
		- Postman para testes de enpoints 

## Arquitetura:
	= A solução foi feita com essa arquitetura pq no meu pensamento ela é melhor para escalar para mais usuarios
	= ela divide a aplicaçãom, separando responsabilidades:
		- UEPAMED: pasta onde esta a solução, onde é feito os controllers, o arquivo do program e o readme.
		- UEPAMED.APLICATION: pasta onde está as regras de negocio, dtos e as interfaces que são tipos contratos para se ligar com a infra (entity e banco)
		- UEPAMED.DOMAIN: pasta onde vai ter todos os dominos, enums. Dominios sao essencias pq alem de a maoria deles vao para banco, sao feito para ter o corpo de cada funcionalidade
		- UEPAMED.INFRASTRUCTURE: pasta onde vai ter todas as buscas e inserts do banco, alem de ter a pasta onde é feita as migrations do banco

## Navegando nas pastas:
	= Todas as pastas da arquitetura tem outras pastas para organização, vamos navegar por elas:
		- UEPAMED.CONTROLLERS: Aqui tem todos os endpoints da nossa aplicação
		- UEPAMED.APPLICATION:
			-- DTOS: Aqui vai ter todos os corpos necessarios para alguma função de regra de negocio.
			-- SERVICES: Aqui vai ter todas as regras de negocio da nossa aplicação
			-- INTERFACES: Aqui é um tipo de contrato que faz a ligação do repositorio que faz toda a ação com o banco com a regra de negocio
		- UEPAMED.DOMAIN:
			-- ENTITIES: Aqui tem todas as entidades da nossa aplicação, não necessariamente mas a maioria são importantes para fazer as tabelas do banco
			-- ENUMS: Aqui tem algums conjuntos de constantes fixas.
		- UEPAMED.INFRASTRUCTURE: 
			-- DATA: Aqui é o arquivo onde tem as relações dos bancos com as entidades, alem disso algumas regras de negocio com o banco
			-- IMPORTERS: Aqui é o arquivo onde a gente modela o tipo de arquivo das nossas importações
			-- MIGRATIONS: Aqui são todas as migrações que a gente fez com o banco
			-- Repositories: Aqui esta as relações que eu fiz com banco, crud, fazendo uma relação com as interfaces
		
