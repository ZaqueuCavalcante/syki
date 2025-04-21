# Azure and Terraform

## Manual

- Criar Subscription na Azure
    - "Azure for Students"

- Criar Resource Group na Azure
    - "syki-rg"

- Criar App Service
    - Select a subscription to manage deployed resources and costs.
    - Use resource groups like folders to organize and manage all your resources.
    - Name: "syki-api"
    - Publish: Code or Container
    - Region: Brazil South
    - Pricing plans
        - Name = ASP-sykirg-870e
        - Operating System = Linux
        - Region = Brazil South
        - SKU = Basic
        - Size = Small
        - ACU = 100 total ACU
        - Memory = 1.75 GB memory
    -------------------
    - Pra criar um banco, preciso de VNet e SubNet
        - Virtual network -> vnet-eftihcpw (10.0.0.0/16)
        - Outbound subnet -> subnet-uxrumtfu (10.0.1.0/24)
    - ServerName = syki-db-server
    - DatabaseName = syki-db-database
    - User = ayvnlwkjgc
    - Password = F443TUsaaPYaR$qT
    - https://learn.microsoft.com/pt-br/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=copilot&pivots=azure-portal
    -------------------
    - Application Insights
    - Tags
    -------------------
    - 


















https://dev.to/willvelida/deploying-to-azure-with-terraform-and-github-actions-5191






Subi toda a infra do Syki no Azure usando o Terraform + GitHub Actions!


- O que é IaC? Vantagens?
- Por quê usar o Terraform?
    - Multi-cloud
    - Provider-agnostic
- State?
- Backend?
- Como o Terraform funciona?
- O que são módulos?
- Como nomear cada coisa?
    - Recursos
    - Variáveis
    - Configurações
- Autenticação
    - Secrets










## 0 - Criar Subscription no Azure

Azure for Students, $100 credits

- Criar Service Principal pro Syki
- Adicionar a role de Contributor pro Syki

- Configurar os secrets como variáveis de ambiente
    - client_id
    - client_secret
    - tenant_id
    - subscription_id

- Fazer login na Azure CLI dentro do tenant criado


## Componentes

- Management Group
    - Subscription
        - Resource Group
            - Resource

- Virtual Networks (VNETs)
    - Security (NSG)
        - Filtro de pacotes
    - IP Addresses
    - Subnets







