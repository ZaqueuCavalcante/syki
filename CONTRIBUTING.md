# Guia de Contribuição

Ficamos felizes pelo seu interesse em contribuir com o Syki!

## Como contribuir

Siga os passos abaixo para colaborar de forma eficiente com o projeto:

1. **Fork o repositório**

- Clique em "Fork" no topo da página.
- Clone seu fork para sua máquina:

    ```bash
    git clone https://github.com/seu-usuario/syki.git
    cd syki
    ```

2. **Crie uma branch**

- Use um nome descritivo para a sua branch:

    ```bash
    git checkout -b minha-feature-legal
    ```

3. **Faça suas alterações**

- Adicione sua feature, correção ou melhoria.
- Certifique-se de seguir o padrão de organização do código.

4. **Escreva commits claros**

- Exemplo:

    ```bash
    git commit -m "feat: adiciona exportação de boletins em PDF"
    ```

5. **Teste suas alterações**

- O projeto possui testes automatizados. Sempre que possível, execute:

    ```bash
    dotnet test
    ```

6. **Abra um Pull Request**

- Depois de fazer push da sua branch, vá até o GitHub e abra um PR.
- Descreva claramente o que sua contribuição faz.
- Marque o @ZaqueuCavalcante ou mantenedores caso precise de revisão.

---

## Dicas

- Prefira alterações pequenas e bem definidas.
- Se for algo maior, abra uma **issue antes** para discutirmos juntos.
- Use `docker-compose up` para rodar o projeto localmente.

---

## Licença

Ao enviar contribuições, você concorda com os termos da [licença](./LICENSE) do projeto.

---

**Obrigado por fazer parte do projeto Syki!**

Juntos construímos ferramentas melhores para a educação.
