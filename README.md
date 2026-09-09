# Norte API

A Norte API é uma aplicação para apoiar a organização de revisões, centralizando funcionalidades como gerenciamento de revisões, membros, importação de artigos, detecção de duplicidades e votações.

## Tecnologias utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- Postman

## Arquitetura

A solução segue uma organização inspirada em DDD, separando as responsabilidades da aplicação.

```text
UepaMed
├── README.md
├── UepaMed.sln
├── UepaMed
├── UepaMed.Application
├── UepaMed.Domain
└── UepaMed.Infrastructure
```

- **UepaMed**: projeto web da API. Contém controllers, configurações, `Program.cs`, documentação e testes.
- **UepaMed.Application**: regras de negócio, services, DTOs e interfaces.
- **UepaMed.Domain**: entidades e enums do domínio.
- **UepaMed.Infrastructure**: acesso ao banco de dados, repositórios, migrations e importadores de arquivos.

## Funcionalidades

- Criação e gerenciamento de revisões.
- Gerenciamento de membros e papéis.
- Registro e autenticação de usuários.
- Importação de artigos nos formatos `.nbib` e `.ris`.
- Detecção de possíveis artigos duplicados.
- Votações e controle de acesso conforme o papel do membro.

## Regras de negócio

- Apenas proprietário e revisor podem importar arquivos.
- Apenas o proprietário pode detectar duplicidades.
- O proprietário pode iniciar uma votação.
- Quando existem revisores na revisão, deve existir pelo menos um avaliador para tratar possíveis empates causados por abstenções.

## Documentação

A documentação fica dentro do projeto web, na pasta `docs`.

```text
UepaMed
└── docs
    └── Qualities
        └── Features
            ├── Geral
            ├── Login
            ├── Registro
            └── Revisão
```

A pasta `Qualities` concentra documentos relacionados à qualidade das funcionalidades.

Dentro dela, `Features` organiza a documentação por área do sistema:

- **Geral**: informações e comportamentos gerais.
- **Login**: fluxo de autenticação.
- **Registro**: cadastro de usuários.
- **Revisão**: funcionalidades relacionadas à criação e gerenciamento de revisões.

Cada funcionalidade pode conter documentos em Markdown com:

- Descrição da funcionalidade.
- Requisitos.
- Cenários de teste.
- Regras de negócio.
- Comportamentos esperados.
- Validações e possíveis erros.

## Testes

Os materiais de teste ficam fora da pasta `docs`, na pasta `Tests`.

```text
UepaMed
└── Tests
    └── Postman
        └── colecao.json
```

A pasta `Postman` contém a coleção de endpoints da API. Ela pode ser importada no Postman para facilitar os testes manuais das rotas disponíveis.

Os cenários de teste documentados devem verificar tanto os fluxos válidos quanto as validações da aplicação, incluindo permissões de cada papel, importação de arquivos, autenticação e regras das revisões.