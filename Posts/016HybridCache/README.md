# 💾 𝐇𝐲𝐛𝐫𝐢𝐝 𝐂𝐚𝐜𝐡𝐞 ⚡

Adicionei 𝐜𝐚𝐜𝐡𝐞 no projeto open-source que estou desenvolvendo!

O 𝐒𝐲𝐤𝐢 (https://github.com/ZaqueuCavalcante/syki) é um sistema de gerenciamento de instituições de ensino que pode ser usado por gestores, professores e alunos.

Ele possui alguns dados que mudam pouco, como cadastros de campus, cursos e disciplinas. Logo, faz muito sentido guardá-los em cache para economizar recursos e aumentar a performance da aplicação como um todo.

Pensando nisso, implementei uma camada de cache em memória utilizando a lib 𝐇𝐲𝐛𝐫𝐢𝐝𝐂𝐚𝐜𝐡𝐞, desenvolvida pelo próprio time da Microsoft. Ela é bem simples de configurar e utilizar, bastando informar um par chave-valor para salvar os items em cache. A expiração pode ser automática (após certo tempo) ou manual (quando os dados mudam e precisam ser atualizados).

Essa biblioteca ainda resolve o problema crítico de 𝐜𝐚𝐜𝐡𝐞 𝐬𝐭𝐚𝐦𝐩𝐞𝐝𝐞: quando o cache expira, se várias requisições tentarem buscar os mesmos dados em paralelo, apenas uma delas vai de fato no banco de dados e insere os registros no cache. As demais leem direto do cache, evitando consultas desnecessárias ao banco de dados.

Abaixo temos o endpoint que retorna todos os cursos de uma instituição de ensino: primeiro sempre pegando os dados do banco e depois pegando do cache. Utilizei o 𝐊𝟔 para realizar testes de carga nos dois cenários, onde o primeiro conseguiu processar até 𝟑.𝟑𝐤 req/s e o segundo 𝟖.𝟓𝐤 req/s (𝟐.𝟓𝟕𝐱 mais performático).

#cache #api #aspnet #postgres #opensource
