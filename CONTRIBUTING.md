# Guia de Contribui��o � Projeto Syki

Ol�! ??  
Ficamos felizes pelo seu interesse em contribuir com o Syki � um sistema open-source para gest�o acad�mica, feito com carinho para facilitar a vida de gestores, professores e alunos. ??

## ? Como contribuir

Siga os passos abaixo para colaborar de forma eficiente com o projeto:

1. **Fork o reposit�rio**
   - Clique em "Fork" no topo da p�gina.
   - Clone seu fork para sua m�quina:
     ```bash
     git clone https://github.com/seu-usuario/syki.git
     cd syki
     ```

2. **Crie uma branch**
   - Use um nome descritivo para a sua branch:
     ```bash
     git checkout -b minha-feature-legal
     ```

3. **Fa�a suas altera��es**
   - Adicione sua feature, corre��o ou melhoria.
   - Certifique-se de seguir o padr�o de organiza��o do c�digo.

4. **Escreva commits claros**
   - Exemplo:
     ```bash
     git commit -m "feat: adiciona exporta��o de boletins em PDF"
     ```

5. **Teste suas altera��es**
   - O projeto possui testes automatizados. Sempre que poss�vel, execute:
     ```bash
     dotnet test
     ```

6. **Abra um Pull Request**
   - Depois de fazer push da sua branch, v� at� o GitHub e abra um PR.
   - Descreva claramente o que sua contribui��o faz.
   - Marque o @ZaqueuCavalcante ou mantenedores caso precise de revis�o.

---

## ?? Dicas

- Prefira altera��es pequenas e bem definidas.
- Se for algo maior, abra uma **issue antes** para discutirmos juntos.
- Use `docker-compose up` para rodar o projeto localmente.

---

## ?? Licen�a

Ao enviar contribui��es, voc� concorda com os termos da [LICENSE](./LICENSE) do projeto.

---

**Obrigado por fazer parte do projeto Syki!** ??  
Juntos constru�mos ferramentas melhores para a educa��o.
