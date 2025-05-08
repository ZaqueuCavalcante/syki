# SYKI - Sistema Acadêmico na Azure

Este é um guia completo para implantar o sistema educacional SYKI na nuvem Azure. O projeto configura automaticamente a infraestrutura necessária para executar os três componentes da aplicação: frontend, backend e serviço daemon, além de um banco de dados e monitoramento.

O projeto automaticamente configura:

- Cluster Kubernetes na Azure (AKS)

- Banco de dados PostgreSQL

- Sistema de monitoramento (Prometheus e Grafana)

- Registro de contêineres (ACR) para armazenar as imagens da aplicação

## Pré-requisitos

Antes de começar, você precisará:

- Conta na Azure: Para hospedar todos os recursos na nuvem.

- Azure CLI: Ferramenta de linha de comando para interagir com a Azure.

- Terraform: Ferramenta para criar a infraestrutura de forma automatizada.

- Docker: Para construir as imagens da aplicação.

- kubectl: Ferramenta para gerenciar o cluster Kubernetes.


## Guia passo a passo

### 1. Preparando o ambiente

```bash
az login
```

### 2. Implante a infraestrutura

```bash
terraform init

terraform plan

terraform apply
```

Quando solicitado, digite yes para confirmar a criação dos recursos. Este processo levará aproximadamente 15-20 minutos.

### 3. Envie as imagens da aplicação

Após a conclusão da implantação, você precisará enviar as imagens do sistema para o registro de contêineres:

```bash
# Obtenha as informações do registro de contêineres
ACR_SERVER=$(terraform output -raw container_registry_login_server)
ACR_USER=$(terraform output -raw container_registry_admin_username)
ACR_PASS=$(terraform output -raw container_registry_admin_password)

# Faça login no registro de contêineres
docker login $ACR_SERVER -u $ACR_USER -p $ACR_PASS

cd Docker

docker build -t $ACR_SERVER/syki-front:latest -f Dockerfile.front .
docker push $ACR_SERVER/syki-front:latest

docker build -t $ACR_SERVER/syki-back:latest -f Dockerfile.back .
docker push $ACR_SERVER/syki-back:latest

docker build -t $ACR_SERVER/syki-daemon:latest -f Dockerfile.daemon .
docker push $ACR_SERVER/syki-daemon:latest

cd ..
```

### 4. Configure o acesso ao cluster

```bash
# Configure o kubectl para acessar seu cluster
$(terraform output -raw kubernetes_command)

# Verifique se os pods estão em execução
kubectl get pods -n syki
```

Aguarde alguns minutos até que todos os pods estejam no estado "Running".

### 5. Acessando o sistema

#### Frontend

```bash
# Obtenha o endereço IP do frontend
FRONT_IP=$(terraform output -raw front_service_ip)
echo "Acesse o sistema em: http://$FRONT_IP"
```

#### Backend

O backend da aplicação SYKI não é exposto diretamente para a internet por padrão. A forma mais simples de acessa-lo seria por acesso local(port-forward).

```bash
# Configurar o kubectl
$(terraform output -raw kubernetes_command)

# Criar um port-forward do serviço backend para sua máquina local
kubectl port-forward -n syki svc/back 8080:80
```

Agora você pode acessar o backend em:

http://localhost:8080

Caso queira verificar logs, talvez prefira conectar ao pod diretamente.

```bash
# Obter o nome do pod do backend
kubectl get pods -n syki -l app=back

# Ver os logs do backend
kubectl logs -n syki -l app=back -f

# Acessar o console/terminal do contêiner do backend
kubectl exec -it -n syki $(kubectl get pods -n syki -l app=back -o jsonpath='{.items[0].metadata.name}') -- /bin/sh

```

#### PostgreSQL

O banco de dados está executando dentro do cluster Kubernetes.

```bash
# Encontrar o nome do pod do Postgres
kubectl get pods -n syki -l app=postgres

# Criar port-forward do Postgres para sua máquina local
kubectl port-forward -n syki $(kubectl get pods -n syki -l app=postgres -o jsonpath='{.items[0].metadata.name}') 5432:5432

# Para obter a senha
terraform output -raw postgresql_password
```

Agora você pode conectar-se ao banco de dados usando qualquer cliente PostgreSQL:

- Host: localhost

- Porta: 5432

- Usuário: postgres (ou o definido na sua configuração)

- Senha: (Obtida nos outputs do Terraform)

- Banco de dados: syki (ou o nome configurado)


#### Grafana

```bash
# Obtenha o endereço IP do Grafana
kubectl get svc -n monitoring prometheus-grafana -o jsonpath='{.status.loadBalancer.ingress[0].ip}'

# Obtenha a senha do admin
GRAFANA_PASS=$(terraform output -raw grafana_admin_password)
echo "Usuário: admin"
echo "Senha: $GRAFANA_PASS"
```

Acesse o dashboard Grafana usando o IP obtido e as credenciais mostradas.

### 6. Encerrando o sistema

Quando terminar de usar o sistema, você pode destruir toda a infraestrutura para evitar cobranças desnecessárias:

```bash
terraform destroy

# Excluir o grupo de recursos inteiro caso o destroy falhe
az group delete --name syki-rg --yes

# Verificar se o grupo de recursos ainda existe
az group show --name syki-rg

# Você deve receber uma mensagem indicando que o recurso não foi encontrado
# Se o grupo ainda existir, tente forçar a exclusão
az group delete --name syki-rg --yes --no-wait

# Limpar contexto kubectl
kubectl config get-contexts
kubectl config delete-context [nome-do-contexto-aks]
```

Quando solicitado, digite yes para confirmar a exclusão de todos os recursos.

## Explicação dos componentes

- Kubernetes (AKS): Sistema para executar aplicações em contêineres de forma escalável e gerenciável

- PostgreSQL: Banco de dados para armazenar os dados do sistema acadêmico

- Terraform: Ferramenta que automatiza a criação de toda a infraestrutura

- Docker: Tecnologia para empacotar as aplicações em contêineres

- Prometheus e Grafana: Ferramentas para monitorar a saúde e o desempenho do sistema

## Resolução de problemas comuns

Os pods não estão iniciando?

```bash
# Verifique o status dos pods
kubectl get pods -n syki

# Verifique os logs de um pod específico
kubectl logs -n syki [nome-do-pod] -f

# Verifique eventos do namespace
kubectl get events -n syki --sort-by='.lastTimestamp'
```

Não consegue acessar o frontend?

```bash
# Verifique se o serviço está disponível
kubectl get svc -n syki front

# Verifique os logs do pod do frontend
kubectl logs -n syki -l app=front -f
```

As imagens não estão sendo baixadas pelo Kubernetes?

```bash
# Verifique se as imagens existem no registro
az acr repository list --name $(echo $ACR_SERVER | cut -d'.' -f1) --output table

# Verifique os detalhes do erro de pull
kubectl describe pod -n syki [nome-do-pod] | grep -A10 "Events:"
```
