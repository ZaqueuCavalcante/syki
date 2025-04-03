# Azure and Terraform

Subi toda a infra do Syki no Azure usando o Terraform!


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







