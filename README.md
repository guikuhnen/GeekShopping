# GeekShopping
### Repositório dedicado ao desenvolvimento do Curso: [Arquitetura de Microsserviços do 0 com ASP.NET, .NET 6 e C#](https://www.udemy.com/course/microservices-do-0-a-gcp-com-dot-net-6-kubernetes-e-docker/)

<br/>

## Tecnologias e frameworks utilizados

- .Net 7
- MySQL Community Server 8.0.35 
- Oauth2
- OpenID
- Duende Identity Server
- RabbitMQ
- Ocelot

<br/>

## Executando

- Necessário criação das bases de dados de cada serviço (TODO: criação automática).
- Inicialize um container docker contendo a imagem do RabbitMQ com as portas 5672 e 15672.
```
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
- Inicialize todos os serviços (Multiple Startup) exceto as "class library" (9 serviços + 2 class library).

<br/>

