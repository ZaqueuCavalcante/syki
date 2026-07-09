# 🏷️ Você usa UUID pra tudo?

Primitive Obsession é um anti-pattern dentro do Domain Driven Design, sendo caracterizado pelo uso excessivo de tipos primitivos (string, int, uuid...) para representar conceitos de domínio.

Isso deixa o domínio menos expressivo, dificultando seu entendimento.

Vamos para um caso de uso mais completo, retirado do Estud (https://github.com/ZaqueuCavalcante/estud), um sistema de gerenciamento de instituições de ensino open-source.

Na imagem do post temos o conceito de Comando, que se relaciona com outras entidades do sistema:

- Um Comando pertence à uma Instituição de Ensino
- Um Comando pode ser gerado por um Evento de Domínio
- Um Comando pode ser gerado por outro Comando
- Um Comando pode estar agrupado dentro de um Lote de Comandos

Na parte supeior da imagem, todos esses relacionamentos são feitos através de UUIDs, logo é preciso muita atenção do desenvolvedor para não passar o id de um evento no lugar do de um comando/lote por exemplo.

Pensando nisso, podemos utilizar ids fortemente tipados para evitar esses erros, como mostrado na parte inferior da imagem. Dessa forma, todos os lugares que referenciem o id de um comando precisam possuir a tipagem correta (CommandId) ao invés da tipagem mais genérica (Guid).

Na implementação utilizei a biblioteca StronglyTypedId (https://github.com/andrewlock/StronglyTypedId), criada pelo @Andrew Lock.

Para funcionar junto com o Entity Framework, basta criar um ValueConverter do novo Id para Guid.

#uuid #api #aspnet #postgres #opensource #ddd
