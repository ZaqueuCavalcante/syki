# 🟢 Usuários ativos em tempo real 👨🏻‍💻

Adicionei mais uma feature ao projeto open-source que estou desenvolvendo ([Syki](https://github.com/ZaqueuCavalcante/syki)).

Agora é possível verificar quais usuários estão ativos e quantas conexões cada um está estabelecendo com o servidor, tudo isso em tempo real!

O funcionamento é simples: uma conexão WebSocket é aberta toda vez que o usuário acessa o sistema via navegador.

Um mesmo usuário pode estabelecer diversas conexões simultâneas com o servidor, pois pode acessar o sistema em diversas abas, navegadores e depositivos ao mesmo tempo.

No backend, o servidor possui um dicionário em memória que armazena, para cada usuário online, uma lista com suas conexões abertas.

Quando um usuário abre/fecha uma aba, o servidor atualiza sua lista de conexões. Se a lista de um usuário fica vazia, ele é marcado como offline.

---

Acompanhe abaixo como o Adm do sistema pode facilmente verficar quais usuários estão online, bem como quantas conexões cada um está estabelecendo com o servidor.

* Essa abordagem funciona para um único servidor. Em uma configuração com 2 ou mais servidores, seria preciso utilizar algum mecanismo de cache distribuído para armazenar esses dados.

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/009RealTimeUsersSignalR/online_users.gif?raw=true" style="display: block; margin: 0 auto" />
</p>

> Você pode ver o código aqui (https://github.com/ZaqueuCavalcante/syki) e a aplicação rodando aqui (https://app.syki.com.br). Venho postando no LinkedIn todas as atualizações de desenvolvimento do projeto (https://www.linkedin.com/in/zaqueu-cavalcante).
